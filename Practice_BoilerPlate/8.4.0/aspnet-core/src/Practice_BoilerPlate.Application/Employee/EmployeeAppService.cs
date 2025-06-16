using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Employee.Dto;
using Practice_BoilerPlate.Employees;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Employee
{
    public class EmployeeAppService : ApplicationService, IEmployeeAppSerivce
    {
        private readonly IRepository<Employeee> _employeeRepository;
        public EmployeeAppService(IRepository<Employeee> repositoryEmployee)
        {
            _employeeRepository = repositoryEmployee;
        }
        public async System.Threading.Tasks.Task CreateAsync(CreateUpdateEmployeDto input)
            {
            var employee = new Employeee
            {
                TenantId = (int)AbpSession.TenantId,
               FirstName=input.FirstName,
               LastName=input.LastName,
               Email=input.Email,
               PhoneNumber=input.PhoneNumber,
               DateOfBirth=input.DateOfBirth,
               HireDate=input.HireDate,
               Position=input.Position,
               Salary=input.Salary,
                DepartmentId = input.DepartmentId,
                AddressId = input.AddressId,
                };
            await _employeeRepository.InsertAsync(employee);
        }
        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _employeeRepository.DeleteAsync(input.Id);
        }
        public async Task<PagedResultDto<GetEmployeeDto>> GetAll(GetAllAccountsInput input)
        {
            // Get all employees with related Department and Address
            var query = _employeeRepository.GetAllIncluding(e => e.Department, e => e.Address);

            // If there’s a search keyword, filter employees based on the keyword
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(input.Keyword) ||
                    s.LastName.Contains(input.Keyword) ||
                    s.Email.Contains(input.Keyword) ||
                    s.PhoneNumber.Contains(input.Keyword));
            }

            // Get total count of employees after filtering (for pagination)
            var totalCount = await query.CountAsync();

            // Fetch the list of employees based on pagination parameters
            var employees = await query
                .OrderBy(e => e.FirstName) // Sort by first name
                .Skip(input.SkipCount)     // Skip records based on pagination
                .Take(input.MaxResultCount) // Take the desired number of records
                .ToListAsync();

            // Debug: Log the employee data (you can also log this to a file or use a logging framework)
            foreach (var employee in employees)
            {
                // Check if Address is null for any employee
                Console.WriteLine($"Employee ID: {employee.Id}, Address: {employee.Address?.Address1 ?? "No Address"}");
            }

            // Map the employees to DTOs (Data Transfer Objects)
            var result = employees.Select(employee => new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                Position = employee.Position,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? "No Department", // Handle null Department
                AddressId = employee.AddressId,
                AddressAddress1 = employee.Address?.Address1 ?? "No Address" // Handle null Address
            }).ToList();

            // Return the paged result
            return new PagedResultDto<GetEmployeeDto>(totalCount, result);
        }



        public async System.Threading.Tasks.Task UpdateAsync(UpdateEmployeeDto input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(input.Id);
            if (employee == null)
            {
                throw new UserFriendlyException("Employee not found");
            }

            employee.FirstName = input.FirstName;
            employee.LastName = input.LastName;
            employee.Email = input.Email;
            employee.PhoneNumber = input.PhoneNumber;
            employee.DateOfBirth = input.DateOfBirth;
            employee.HireDate = input.HireDate;
            employee.Position = input.Position;
            employee.Salary = input.Salary;
            employee.DepartmentId = input.DepartmentId;
            employee.AddressId = input.AddressId;

            await _employeeRepository.UpdateAsync(employee);
        }

    }
}
