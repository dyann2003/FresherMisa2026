using Dapper;
using FresherMisa2026.Application.Extensions;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Entities.Employee;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Employee> GetEmployeeByCode(string code)
        {
            string query = SQLExtension.GetQuery("Employee.GetByCode");
            var param = new Dictionary<string, object>
            {
                {"@EmployeeCode", code }
            };
            return await _dbConnection.QueryFirstOrDefaultAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentId(Guid departmentId)
        {
            string query = SQLExtension.GetQuery("Employee.GetByDepartmentId");
            var param = new Dictionary<string, object>
            {
                {"@DepartmentID", departmentId }
            };
            return await _dbConnection.QueryAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByPositionId(Guid positionId)
        {
            string query = SQLExtension.GetQuery("Employee.GetByPositionId");
            var param = new Dictionary<string, object>
            {
                {"@PositionID", positionId }
            };
            return await _dbConnection.QueryAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> FilterEmployees(
            Guid? departmentId,
            Guid? positionId,
            decimal? salaryFrom,
            decimal? salaryTo,
            int? gender,
            DateTime? hireDateFrom,
            DateTime? hireDateTo)
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT * FROM employee WHERE 1=1 ");

            var param = new DynamicParameters();

            if (departmentId.HasValue)
            {
                sql.Append(" AND DepartmentID = @DepartmentId");
                param.Add("@DepartmentId", departmentId);
            }

            if (positionId.HasValue)
            {
                sql.Append(" AND PositionID = @PositionId");
                param.Add("@PositionId", positionId);
            }

            if (salaryFrom.HasValue)
            {
                sql.Append(" AND Salary >= @SalaryFrom");
                param.Add("@SalaryFrom", salaryFrom);
            }

            if (salaryTo.HasValue)
            {
                sql.Append(" AND Salary <= @SalaryTo");
                param.Add("@SalaryTo", salaryTo);
            }

            if (gender.HasValue)
            {
                sql.Append(" AND Gender = @Gender");
                param.Add("@Gender", gender);
            }

            if (hireDateFrom.HasValue)
            {
                sql.Append(" AND HireDate >= @HireDateFrom");
                param.Add("@HireDateFrom", hireDateFrom);
            }

            if (hireDateTo.HasValue)
            {
                sql.Append(" AND HireDate <= @HireDateTo");
                param.Add("@HireDateTo", hireDateTo);
            }

            return await _dbConnection.QueryAsync<Employee>(sql.ToString(), param);
        }
    }
}