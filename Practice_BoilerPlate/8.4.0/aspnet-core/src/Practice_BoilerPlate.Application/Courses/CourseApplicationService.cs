using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Courses.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Courses
{
    public class CourseApplicationService : ApplicationService, ICourseApplicationService
    {
     private  readonly  IRepository<Course> _coursesRepository;
        public CourseApplicationService(IRepository<Course> courserepository)
        {
            _coursesRepository=courserepository;
        }

        public async System.Threading.Tasks.Task CreateAsync(CreateCourseDto input)
        {
            var course = new Course
           {
                TenantId = (int)AbpSession.TenantId,
                Name = input.Name,
                Description = input.Description,
                Duration = input.Duration,
                Fees = input.Fees


            };
            await _coursesRepository.InsertAsync(course);

        }

        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _coursesRepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<GetCourseDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _coursesRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(c =>
                    c.Name.Contains(input.Keyword) ||
                    c.Description.Contains(input.Keyword));
            }

            var courses = await query.ToListAsync();

            var result = courses.Select(course => new GetCourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Duration = course.Duration,
                Fees = course.Fees
            }).ToList();

            return new PagedResultDto<GetCourseDto>(
                result.Count,
                result
            );
        }


        public async System.Threading.Tasks.Task UpdateAsync(UpdateCourseDto input)
        {
            var course = await _coursesRepository.GetAsync(input.Id);
            course.Name = input.Name;
            course.Description = input.Description;
            course.Duration = input.Duration;
            course.Fees = input.Fees;
            await _coursesRepository.UpdateAsync(course);
          
        }
    }
}
