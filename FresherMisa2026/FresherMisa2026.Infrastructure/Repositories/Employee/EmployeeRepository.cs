using Dapper;
using FresherMisa2026.Application.Extensions;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration, IMemoryCache cache) : base(configuration, cache)
        {
        }

        public async Task<Employee> GetEmployeeByCode(string code)
        {
            using var connection = CreateConnection();
            string query = SQLExtension.GetQuery("Employee.GetByCode");
            var param = new Dictionary<string, object>
            {
                {"@EmployeeCode", code }
            };
            return await connection.QueryFirstOrDefaultAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentId(Guid departmentId)
        {
            using var connection = CreateConnection();
            string query = SQLExtension.GetQuery("Employee.GetByDepartmentId");
            var param = new Dictionary<string, object>
            {
                {"@DepartmentID", departmentId }
            };
            return await connection.QueryAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByPositionId(Guid positionId)
        {
            using var connection = CreateConnection();
            string query = SQLExtension.GetQuery("Employee.GetByPositionId");
            var param = new Dictionary<string, object>
            {
                {"@PositionID", positionId }
            };
            return await connection.QueryAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<PagingResponse<Employee>> FilterEmployees(
            Guid? departmentId,
            Guid? positionId,
            decimal? salaryFrom,
            decimal? salaryTo,
            int? gender,
            DateTime? hireDateFrom,
            DateTime? hireDateTo,
            int pageSize,
            int pageIndex)
        {
            using var connection = CreateConnection();
            var where = new StringBuilder(" WHERE 1=1 ");
            var param = new DynamicParameters();

            if (departmentId.HasValue)
            {
                where.Append(" AND DepartmentID = @DepartmentId");
                param.Add("@DepartmentId", departmentId);
            }

            if (positionId.HasValue)
            {
                where.Append(" AND PositionID = @PositionId");
                param.Add("@PositionId", positionId);
            }

            if (salaryFrom.HasValue)
            {
                where.Append(" AND Salary >= @SalaryFrom");
                param.Add("@SalaryFrom", salaryFrom);
            }

            if (salaryTo.HasValue)
            {
                where.Append(" AND Salary <= @SalaryTo");
                param.Add("@SalaryTo", salaryTo);
            }

            if (gender.HasValue)
            {
                where.Append(" AND Gender = @Gender");
                param.Add("@Gender", gender);
            }

            if (hireDateFrom.HasValue)
            {
                where.Append(" AND HireDate >= @HireDateFrom");
                param.Add("@HireDateFrom", hireDateFrom);
            }

            if (hireDateTo.HasValue)
            {
                where.Append(" AND HireDate <= @HireDateTo");
                param.Add("@HireDateTo", hireDateTo);
            }

            // Total
            var totalQuery = $"SELECT COUNT(*) FROM employee {where}";
            var total = await connection.ExecuteScalarAsync<int>(totalQuery, param);

            // Data
            var offset = (pageIndex - 1) * pageSize;

            var dataQuery = $@"
                SELECT * 
                FROM employee
                {where}
                ORDER BY CreatedDate DESC
                LIMIT {offset}, {pageSize}";

            var data = await connection.QueryAsync<Employee>(dataQuery, param);

            return new PagingResponse<Employee>
            {
                Total = total,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Data = data
            };
        }
    }
}