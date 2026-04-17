using Dapper;
using FresherMisa2026.Application.Extensions;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Entities.Department;
using FresherMisa2026.Entities.Employee;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for Department entity
    /// </summary>
    /// Created By: dvhai (09/04/2026)
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Lấy department theo code
        /// </summary>
        /// <param name="code">Mã department</param>
        /// <returns>Department tìm thấy hoặc null</returns>
        /// CREATED BY: dvhai (09/04/2026)
        public async Task<Department> GetDepartmentByCode(string code)
        {
            string query = SQLExtension.GetQuery("Department.GetByCode");
            var @param = new Dictionary<string, object>
            {
                {"@DepartmentCode", code }
            };
            return await _dbConnection.QueryFirstOrDefaultAsync<Department>(query, @param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentCode(string code)
        {
            string query = SQLExtension.GetQuery("Department.GetEmployeeByDepartmentCode");
            var @param = new Dictionary<string, object>
            {
                {"@DepartmentCode", code }
            };

            return await _dbConnection.QueryAsync<Employee>(query, @param, commandType: System.Data.CommandType.Text);
        }

        public async Task<int> GetEmployeesCountByDepartmentCode(string code)
        {
            string query = SQLExtension.GetQuery("Department.GetEmployeeCountByDepartmentCode");

            var param = new Dictionary<string, object>
            {
                { "@DepartmentCode", code }
            };

            var result = await _dbConnection.QueryFirstAsync<int>(query, param);
            return result;
        }
    }
}
