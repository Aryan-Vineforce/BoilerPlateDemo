using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Courses;
using Practice_BoilerPlate.Students.Dto;
using Practice_BoilerPlate.TeacherSubjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Enrollments.Dto
{
    public class EnrollmentApplicationService:ApplicationService,IEnrollmentsApplicationService
    {
        private readonly IRepository<Enrollment> _enrollmentsRepository;
        public EnrollmentApplicationService(IRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentsRepository = enrollmentRepository;
            
        }

        public async System.Threading.Tasks.Task CreateAsync(EnrollmentCreateUpdateDto input)
        {
            try
            {
                foreach (var courseId in input.CourseIds)
                {
                    var enroll = new Enrollment
                    {
                        TenantId = (int)AbpSession.TenantId,
                        StudentId = input.StudentId,
                        CourseId = courseId,
                    
                    };

                    await _enrollmentsRepository.InsertAsync(enroll);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while creating enrollments", ex);
                throw;
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _enrollmentsRepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<EnrollmentGetDto>> GetAll(GetAllAccountsInput input)
        {

            var query = _enrollmentsRepository

        .GetAllIncluding(e => e.Student, e => e.Course);
            //.Where(e => e.Teacher != null && e.Subject !=null);
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(s =>
                    s.Student.Name.Contains(input.Keyword) ||
                    s.Course.Name.Contains(input.Keyword));
            }
            var totalCount = await query.CountAsync(); // Count before paging
            var enrollment = await query
           .Skip(input.SkipCount)
           .Take(input.MaxResultCount)
           .ToListAsync();
            var result = enrollment.Select(enrollment => new EnrollmentGetDto
            {
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                StudentName = enrollment.Student?.Name,
                CourseId = enrollment.CourseId,
                CourseName = enrollment.Course?.Name

            }).ToList();
            return new PagedResultDto<EnrollmentGetDto>(
                totalCount,
                result
                );
        }

        public async System.Threading.Tasks.Task UpdateAsync(EnrollmentCreateUpdateDto input)
        {
            try
            {
                // Step 1: Delete existing enrollments for this student
                var existingEnrollments = await _enrollmentsRepository
                    .GetAll()
                    .Where(e => e.StudentId == input.StudentId)
                    .ToListAsync();

                foreach (var enrollment in existingEnrollments)
                {
                    await _enrollmentsRepository.DeleteAsync(enrollment);
                }

                // Step 2: Insert updated course enrollments
                foreach (var courseId in input.CourseIds)
                {
                    var newEnrollment = new Enrollment
                    {
                        TenantId = (int)AbpSession.TenantId,
                        StudentId = input.StudentId,
                        CourseId = courseId
                    };

                    await _enrollmentsRepository.InsertAsync(newEnrollment);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while updating enrollments", ex);
                throw;
            }
        }
    }
}
