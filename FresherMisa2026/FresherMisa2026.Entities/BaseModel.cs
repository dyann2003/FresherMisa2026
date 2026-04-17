using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Entities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IRequired : Attribute
    {

    }
    public class BaseModel
    {
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreateDate { get; set; }
        
        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Trạng thái thêm sửa xóa, không lưu database
        /// </summary>
        public ModelSate State { get; set; }

        /// <summary>
        /// Có xóa mềm hay không
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
