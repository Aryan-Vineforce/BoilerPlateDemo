using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Departments;
using Practice_BoilerPlate.Employee.Dto;
using Practice_BoilerPlate.Employees;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Practice_BoilerPlate.Students

{

    public class StudentAppService : ApplicationService, IStudentAppService
    {
        private readonly IRepository<Student> _repositoryStudent;
        public StudentAppService(IRepository<Student> repositoryStudent)//dependency injection ho rahi hai. ABP khud se Student entity ka repository inject karega.
        {
            _repositoryStudent = repositoryStudent;
        }

        public async System.Threading.Tasks.Task CreateAsync(CreateStudentDto input)
        {
            try
            {
                var student = new Student
                {
                    TenantId = (int)AbpSession.TenantId,
                    Name = input.Name,//input ka data utha ke ek Student entity me daal raha ha
                    RollNumber = input.RollNumber,
                    Age = input.Age,
                    DOB = input.DOB,
                    Gender = input.Gender,
                    Email = input.Email,
                    PhoneNumber = input.PhoneNumber,
                    IsActive = input.IsActive

                };

                await _repositoryStudent.InsertAsync(student);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //public async Task<List<GetStudentDto>> GetAll()
        //{
        //    var students = await _repositoryStudent.GetAllListAsync();

        //    var result = students.Select(student => new GetStudentDto
        //    {
        //       Id=student.Id,
        //        Name = student.Name,
        //        RollNumber=student.RollNumber,
        //        Age=student.Age,
        //        DOB=student.DOB,
        //        Gender=student.Gender,
        //        Email=student.Email,
        //        IsActive=student.IsActive



        //    }).ToList();

        //    return result;
        //}

        public async Task<PagedResultDto<GetStudentDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _repositoryStudent.GetAll();
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(s =>
                s.Name.Contains(input.Keyword) ||
                s.RollNumber.Contains(input.Keyword) ||
                s.Email.Contains(input.Keyword) ||
                s.PhoneNumber.Contains(input.Keyword)
            );
            }
            // Sorting - default to Name if no Sorting is specified
            query = !string.IsNullOrWhiteSpace(input.Sorting)
                ? query.OrderBy(input.Sorting)
                : query.OrderBy(d => d.Name);
            var students = await query.ToListAsync();




            var result = students.Select(student => new GetStudentDto
            {
                Id = student.Id,
                Name = student.Name,
                RollNumber = student.RollNumber,
                Age = student.Age,
                DOB = student.DOB,
                Gender = student.Gender,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,
                IsActive = student.IsActive
            }).ToList();
            var totalCount = result.Count();
            //var dbList = await result.ToListAsync();
            //var results = new List<GetStudentDto>().ToList();
            //foreach (var o in dbList)
            //{

            //}

            return new PagedResultDto<GetStudentDto>(
          totalCount,
          result);
        }

        public async System.Threading.Tasks.Task UpdateAsync(UpdateStudentDto input)
        {
            var student = await _repositoryStudent.GetAsync(input.Id);
            student.Id = input.Id;
            student.Name = input.Name;
            student.RollNumber = input.RollNumber;
            student.Age = input.Age;
            student.DOB = input.DOB;
            student.Gender = input.Gender;
            student.Email = input.Email;
            student.PhoneNumber = input.Email;
            student.IsActive = input.IsActive;

            await _repositoryStudent.UpdateAsync(student);
        }
        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _repositoryStudent.DeleteAsync(input.Id);
        }


    }
}
//public async Task<PagedResultDto<GetEmployeeDto>> GetAll(GetAllAccountsInput getAllAccountsInput)
//{
//    var students = await _employeeRepository.GetAllListAsync();

//    var result = students.Select(student => new GetStudentDto
//    {
//        Id = student.Id,
//        Name = student.Name,
//        RollNumber = student.RollNumber,
//        Age = student.Age,
//        DOB = student.DOB,
//        Gender = student.Gender,
//        Email = student.Email,
//        IsActive = student.IsActive
//    }).ToList();
//    var totalCount = result.Count();
//    //var dbList = await result.ToListAsync();
//    //var results = new List<GetStudentDto>().ToList();
//    //foreach (var o in dbList)
//    //{

//    //}

//    return new PagedResultDto<GetStudentDto>(
//  totalCount,
//  result);
//}
