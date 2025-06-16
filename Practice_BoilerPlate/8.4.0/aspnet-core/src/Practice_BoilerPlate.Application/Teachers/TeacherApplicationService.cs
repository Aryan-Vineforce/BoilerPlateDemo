using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Courses.Dto;
using Practice_BoilerPlate.Students.Dto;
using Practice_BoilerPlate.Teachers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Teachers
{
    public class TeacherApplicationService:ApplicationService,ITeacherApplicationService
    {
        private readonly IRepository<Teacher> _repositoryteacher;
        public TeacherApplicationService(IRepository<Teacher> repository)
        {
            _repositoryteacher = repository;
        }

        public async System.Threading.Tasks.Task CreateAsync(TeacherCreateDto input)
        {
            int lastNumber = 1000;

            var lastTeacher = await _repositoryteacher.GetAll()
                    .IgnoreQueryFilters()
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            if (lastTeacher != null &&
                !string.IsNullOrEmpty(lastTeacher.EmployeeID) &&
                lastTeacher.EmployeeID.StartsWith("EMP"))
            {
                string numberPart = lastTeacher.EmployeeID.Substring(3);
                if (int.TryParse(numberPart, out int parsedNumber))
                {
                    lastNumber = parsedNumber;
                }
            }

            string newEmployeeID  = "EMP" +  (lastNumber + 1).ToString();

            var teacher = new Teacher
            {
                TenantId = (int)AbpSession.TenantId,
                Name = input.Name,
                Email = input.Email,
                EmployeeID = newEmployeeID
            };

            await _repositoryteacher.InsertAsync(teacher);
        }


        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _repositoryteacher.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<TeacherGetDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _repositoryteacher.GetAll();

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(c =>
                    c.Name.Contains(input.Keyword) ||
                    c.Email.Contains(input.Keyword));
            }

            var courses = await query.ToListAsync();

            var result = courses.Select(course => new TeacherGetDto
            {
                Id = course.Id,
                Name = course.Name,
                EmployeeID = course.EmployeeID,
                Email= course.Email,
               
            }).ToList();

            return new PagedResultDto<TeacherGetDto>(
                result.Count,
                result
            );
        }


        public async System.Threading.Tasks.Task UpdateAsync(TeacherUpdateDto input)
        {
            var teacher = await _repositoryteacher.GetAsync(input.Id);
            teacher.Name = input.Name;
           
            teacher.Email = input.Email;
           
            await _repositoryteacher.UpdateAsync(teacher);
        }
    }
}
