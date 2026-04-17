using FresherMisa2026.Application.Interfaces;
using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Enums;
using FresherMisa2026.Entities.Extensions;
using System.Collections.Concurrent;
using System.Reflection;

namespace FresherMisa2026.Application.Services
{
    /// <summary>
    /// Service dùng chung
    /// </summary>
    /// <typeparam name="TEntity">Loại thực thể</typeparam>
    /// CREATED BY: DVHAI (11/07/2026)
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseModel
    {
        #region Declare
        protected readonly IBaseRepository<TEntity> _baseRepository;
        private readonly string _tableName;
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _cachedProperties = new();
        private const string SearchFieldSeparator = ";";
        #endregion

        #region Constructer
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _tableName = typeof(TEntity).GetTableName().ToLowerInvariant();
        }
        #endregion

        #region Protected Helpers - có thể override trong derived class
        protected static ServiceResponse CreateSuccessResponse(object? data = null) => new()
        {
            IsSuccess = true,
            Code = (int)ResponseCode.Success,
            Data = data
        };

        protected static ServiceResponse CreateErrorResponse(ResponseCode code, string devMessage, string? userMessage = null) => new()
        {
            IsSuccess = false,
            Code = (int)code,
            DevMessage = devMessage,
            Data = userMessage
        };

        private static PropertyInfo[] GetCachedProperties(Type entityType)
        {
            return _cachedProperties.GetOrAdd(entityType, type => type.GetProperties());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CREATED BY: DVHAI 11/07/2026
        public async Task<ServiceResponse> GetEntitiesAsync()
        {
            var entities = await _baseRepository.GetEntitiesAsync();
            return CreateSuccessResponse(entities.Cast<TEntity>().ToList());
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="entityId">Id của bản ghi</param>
        /// <returns>Bản ghi duy nhất</returns>
        /// CREATED BY: DVHAI (11/07/2026)
        public async Task<ServiceResponse> GetEntityByIDAsync(Guid entityId)
        {
            if (entityId == Guid.Empty)
            {
                return CreateErrorResponse(ResponseCode.BadRequest, "Id không hợp lệ");
            }

            var entity = await _baseRepository.GetEntityByIDAsync(entityId);
            return entity != null 
                ? CreateSuccessResponse(entity) 
                : CreateErrorResponse(ResponseCode.NotFound, "Không tìm thấy bản ghi");
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="entityId">Id của bản ghi</param>
        /// <returns>Số dòng bị xóa</returns>
        /// CREATED BY: DVHAI (07/07/2026)
        public async Task<ServiceResponse> DeleteByIDAsync(Guid entityId)
        {
            if (entityId == Guid.Empty)
            {
                return CreateErrorResponse(ResponseCode.BadRequest, "Id không hợp lệ");
            }

            //1. Validate xóa
            bool canDelete = await ValidateBeforeDeleteAsync(entityId);
            if (!canDelete)
            {
                return CreateErrorResponse(ResponseCode.BadRequest, "Không thể xóa bản ghi này");
            }
            
            //2. Thực hiện xóa
            int rowAffects = await _baseRepository.DeleteAsync(entityId);
            
            if (rowAffects > 0)
            {
                //3. Xóa thành công thì làm gì
                AfterDelete();
                return CreateSuccessResponse(rowAffects);
            }

            return CreateErrorResponse(ResponseCode.NotFound, "Không tìm thấy bản ghi để xóa");
        }

        /// <summary>
        /// Validate tất cả
        /// </summary>
        /// <param name="entity">Thực thể</param>
        /// <returns>Danh sách lỗi validate</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        private List<ValidationError> Validate(TEntity entity)
        {
            var errors = new List<ValidationError>();
            var properties = GetCachedProperties(entity.GetType());

            foreach (var property in properties)
            {
                //1.1 Kiểm tra xem có attribute cần phải validate không
                if (property.IsDefined(typeof(IRequired), false))
                {
                    var error = ValidateRequired(entity, property);
                    if (error != null)
                    {
                        errors.Add(error);
                    }
                }
            }

            //2. Validate tùy chỉnh từng màn hình
            var customErrors = ValidateCustom(entity);
            errors.AddRange(customErrors);

            return errors;
        }

        /// <summary>
        /// Validate bắt buộc nhập
        /// </summary>
        /// <param name="entity">Thực thể</param>
        /// <param name="propertyInfo">Thuộc tính của thực thể</param>
        /// <returns>Lỗi validate hoặc null nếu hợp lệ</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        private ValidationError? ValidateRequired(TEntity entity, PropertyInfo propertyInfo)
        {
            //1. Tên trường
            var propertyName = propertyInfo.Name;

            //2. Giá trị
            var propertyValue = propertyInfo.GetValue(entity);

            //3. Tên hiển thị
            var propertyDisplayName = typeof(TEntity).GetColumnDisplayName(propertyName);

            if (propertyValue == null || string.IsNullOrEmpty(propertyValue.ToString()))
            {
                return new ValidationError(propertyName, $"Trường {propertyDisplayName} bắt buộc nhập");
            }

            return null;
        }

        /// <summary>
        /// Validate từng màn hình
        /// </summary>
        /// <param name="entity">Thực thể</param>
        /// <returns>Danh sách lỗi tùy chỉnh</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        protected virtual List<ValidationError> ValidateCustom(TEntity entity)
        {
            return new List<ValidationError>();
        }


        /// <summary>
        /// Thêm một thực thể
        /// </summary>
        /// <param name="entity">Thực thể cần thêm</param>
        /// <returns>ServiceResponse chứa kết quả</returns>
        /// CREATED BY: DVHAI (11/07/2021)
        public async Task<ServiceResponse> InsertAsync(TEntity entity)
        {
            entity.State = ModelSate.Add;

            //1. Validate tất cả các trường nếu được gắn thẻ
            var errors = Validate(entity);

            //2. Sử lí lỗi tương ứng
            if (errors.Count == 0)
            {
                var result = await _baseRepository.InsertAsync(entity);
                return CreateSuccessResponse(result);
            }

            return CreateErrorResponse(
                ResponseCode.BadRequest, 
                "Validate thất bại", 
                string.Join("; ", errors.Select(e => e.Message))
            );
        }

        /// <summary>
        /// Cập nhập thông tin bản ghi 
        /// </summary>
        /// <param name="entityId">Id bản ghi</param>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>ServiceResponse chứa kết quả</returns>
        /// CREATED BY: DVHAI (11/07/2021)
        public async Task<ServiceResponse> UpdateAsync(Guid entityId, TEntity entity)
        {
            if (entityId == Guid.Empty)
            {
                return CreateErrorResponse(ResponseCode.BadRequest, "Id không hợp lệ");
            }

            //1. Trạng thái
            entity.State = ModelSate.Update;

            //2. Validate tất cả các trường nếu được gắn thẻ
            var errors = Validate(entity);
            
            if (errors.Count == 0)
            {
                int rowAffects = await _baseRepository.UpdateAsync(entityId, entity);
                if (rowAffects > 0)
                {
                    return CreateSuccessResponse(rowAffects);
                }
                return CreateErrorResponse(ResponseCode.NotFound, "Không tìm thấy bản ghi để cập nhật");
            }

            //3. Validate fail - trả về BadRequest
            return CreateErrorResponse(
                ResponseCode.BadRequest,
                "Validate thất bại",
                string.Join("; ", errors.Select(e => e.Message))
            );
        }

        /// <summary>
        /// Lấy danh sách thực thể paging
        /// </summary>
        /// <param name="pagingRequest">Thông tin phân trang</param>
        /// <returns>Danh sách thực thể phân trang</returns>
        /// CREATED BY: DVHAI (07/07/2026)
        public async Task<ServiceResponse> GetFilterPagingAsync(PagingRequest pagingRequest)
        {
            var fields = string.IsNullOrEmpty(pagingRequest.SearchFields)
                ? new List<string>()
                : pagingRequest.SearchFields.Split(SearchFieldSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();

            var (total, data) = await _baseRepository.GetFilterPagingAsync(
                pagingRequest.PageSize, 
                pagingRequest.PageIndex, 
                pagingRequest.Search,
                fields, 
                pagingRequest.Sort
            );

            var response = new PagingResponse<TEntity>
            {
                Total = total,
                Data = data.ToList()
            };

            return CreateSuccessResponse(response);
        }
        #endregion

        #region Virtual method - Lifecycle hooks
        /// <summary>
        /// Trước khi lấy danh sách entity
        /// </summary>
        protected virtual void OnBeforeGetEntities() { }

        /// <summary>
        /// Sau khi lấy danh sách entity
        /// </summary>
        /// <param name="entities">Danh sách entity</param>
        protected virtual void OnAfterGetEntities(IEnumerable<TEntity> entities) { }

        /// <summary>
        /// Trước khi lấy entity theo Id
        /// </summary>
        /// <param name="entityId">Id entity</param>
        protected virtual void OnBeforeGetById(Guid entityId) { }

        /// <summary>
        /// Sau khi lấy entity theo Id
        /// </summary>
        /// <param name="entity">Entity lấy được</param>
        protected virtual void OnAfterGetById(TEntity? entity) { }

        /// <summary>
        /// Trước khi thêm mới entity
        /// </summary>
        /// <param name="entity">Entity cần thêm</param>
        protected virtual void OnBeforeInsert(TEntity entity) { }

        /// <summary>
        /// Sau khi thêm mới entity
        /// </summary>
        /// <param name="entity">Entity đã thêm</param>
        /// <param name="result">Kết quả</param>
        protected virtual void OnAfterInsert(TEntity entity, int result) { }

        /// <summary>
        /// Trước khi cập nhật entity
        /// </summary>
        /// <param name="entityId">Id entity</param>
        /// <param name="entity">Entity cập nhật</param>
        protected virtual void OnBeforeUpdate(Guid entityId, TEntity entity) { }

        /// <summary>
        /// Sau khi cập nhật entity
        /// </summary>
        /// <param name="entityId">Id entity</param>
        /// <param name="entity">Entity đã cập nhật</param>
        /// <param name="result">Kết quả</param>
        protected virtual void OnAfterUpdate(Guid entityId, TEntity entity, int result) { }

        /// <summary>
        /// Trước khi xóa entity
        /// </summary>
        /// <param name="entityId">Id entity</param>
        protected virtual void OnBeforeDelete(Guid entityId) { }

        /// <summary>
        /// Sau khi xóa entity
        /// </summary>
        /// <param name="entityId">Id entity</param>
        /// <param name="result">Kết quả</param>
        protected virtual void OnAfterDelete(Guid entityId, int result) { }

        /// <summary>
        /// Trước khi lấy danh sách phân trang
        /// </summary>
        /// <param name="pagingRequest">Thông tin phân trang</param>
        protected virtual void OnBeforeGetFilterPaging(PagingRequest pagingRequest) { }

        /// <summary>
        /// Sau khi lấy danh sách phân trang
        /// </summary>
        /// <param name="response">Kết quả phân trang</param>
        protected virtual void OnAfterGetFilterPaging(PagingResponse<TEntity> response) { }

        /// <summary>
        /// Khi validation thất bại
        /// </summary>
        /// <param name="errors">Danh sách lỗi</param>
        protected virtual void OnValidationFailed(List<ValidationError> errors) { }

        #endregion

        #region Virtual method - Override methods
        /// <summary>
        /// Xóa thành công
        /// </summary>
        protected virtual void AfterDelete()
        {
        }

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        /// <param name="entityId">Id bản ghi cần xóa</param>
        /// <returns>Có thể xóa hay không</returns>
        protected virtual Task<bool> ValidateBeforeDeleteAsync(Guid entityId)
        {
            return Task.FromResult(true);
        }
        #endregion
    }

    /// <summary>
    /// Lỗi validate
    /// </summary>
    /// <param name="Field">Tên trường</param>
    /// <param name="Message">Thông báo lỗi</param>
    public record ValidationError(string Field, string Message);
}
