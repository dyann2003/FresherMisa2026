using FresherMisa2026.Application.Interfaces;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FresherMisa2026.Application.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(
            IBaseRepository<Employee> baseRepository,
            IEmployeeRepository employeeRepository
            ) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetEmployeeByCodeAsync(string code)
        {
            var employee = await _employeeRepository.GetEmployeeByCode(code);
            if (employee == null)
                throw new Exception("Employee not found");

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(Guid departmentId)
        {
            return await _employeeRepository.GetEmployeesByDepartmentId(departmentId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByPositionIdAsync(Guid positionId)
        {
            return await _employeeRepository.GetEmployeesByPositionId(positionId);
        }

        protected override List<ValidationError> ValidateCustom(Employee employee)
        {
            var errors = new List<ValidationError>();

            // Check length
            if (!string.IsNullOrEmpty(employee.EmployeeCode) && employee.EmployeeCode.Length > 20)
            {
                errors.Add(new ValidationError("EmployeeCode", "Mã nhân viên không được vượt quá 20 ký tự"));
            }

            // Check duplicate EmployeeCode
            var existEmployee = _employeeRepository.GetEmployeeByCode(employee.EmployeeCode).Result;
            if (existEmployee != null && existEmployee.EmployeeID != employee.EmployeeID)
            {
                errors.Add(new ValidationError("EmployeeCode", "Mã nhân viên đã tồn tại"));
            }

            // Validate Email
            if (!string.IsNullOrEmpty(employee.Email))
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(employee.Email))
                {
                    errors.Add(new ValidationError("Email", "Email không đúng định dạng"));
                }
            }

            // Validate Phone
            if (!string.IsNullOrEmpty(employee.PhoneNumber))
            {
                var phoneRegex = new Regex(@"^(0|\+84)[0-9]{9}$");
                if (!phoneRegex.IsMatch(employee.PhoneNumber))
                {
                    errors.Add(new ValidationError("PhoneNumber", "Số điện thoại không đúng định dạng"));
                }
            }

            // Validate DateOfBirth
            if (employee.DateOfBirth.HasValue)
            {
                if (employee.DateOfBirth.Value >= DateTime.Now)
                {
                    errors.Add(new ValidationError("DateOfBirth", "Ngày sinh phải nhỏ hơn ngày hiện tại"));
                }
            }

            return errors;

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

            if (pageSize <= 0)
                throw new ArgumentException("PageSize phải > 0");

            if (pageIndex <= 0)
                throw new ArgumentException("PageIndex phải > 0");

            if (departmentId.HasValue && departmentId == Guid.Empty)
            {
                throw new ArgumentException("DepartmentId không hợp lệ");
            }

            if (positionId.HasValue && positionId == Guid.Empty)
            {
                throw new ArgumentException("PositionId không hợp lệ");
            }

            if (salaryFrom.HasValue && salaryFrom < 0)
            {
                throw new ArgumentException("SalaryFrom phải >= 0");
            }

            if (salaryTo.HasValue && salaryTo < 0)
            {
                throw new ArgumentException("SalaryTo phải >= 0");
            }

            if (salaryFrom.HasValue && salaryTo.HasValue && salaryFrom > salaryTo)
            {
                throw new ArgumentException("Mức lương từ phải nhỏ hơn mức lương đến");
            }

            if (hireDateFrom.HasValue && hireDateTo.HasValue && hireDateFrom > hireDateTo)
            {
                throw new ArgumentException("Ngày tuyển dụng từ phải nhỏ hơn ngày tuyển dụng đến");
            }

            if (hireDateTo.HasValue && hireDateTo > DateTime.Now)
            {
                throw new ArgumentException("Ngày tuyển dụng không được lớn hơn hiện tại");
            }

            return await _employeeRepository.FilterEmployees(
                departmentId,
                positionId,
                salaryFrom,
                salaryTo,
                gender,
                hireDateFrom,
                hireDateTo,
                pageSize,
                pageIndex
            );
        }
    }
}