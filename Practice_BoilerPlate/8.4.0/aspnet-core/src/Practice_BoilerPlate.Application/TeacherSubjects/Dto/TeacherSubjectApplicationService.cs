using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Students.Dto;
using Practice_BoilerPlate.Subjects.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.TeacherSubjects.Dto
{
    public class TeacherSubjectApplicationService:ApplicationService,ITeacherSubjectApplicationService
    {
        private readonly IRepository<TeacherSubject> _repositoryTeacherSubject;
        public TeacherSubjectApplicationService(IRepository<TeacherSubject> repositoryTeacehrSubject)
        {
            _repositoryTeacherSubject = repositoryTeacehrSubject;
        }

        public async System.Threading.Tasks.Task CreateAsync(TeacherSubjectCreateUpdateDto input)
        {
            var teachsub = new TeacherSubject
            {
                TenantId = (int)AbpSession.TenantId,
                SubjectId = input.SubjectId,
                TeacherId=input.TeacherId
            };
            await _repositoryTeacherSubject.InsertAsync(teachsub);

        }

        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _repositoryTeacherSubject.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<TeacherSubjectGetDto>> GetAll(GetAllAccountsInput input)
        {
           
            var query = _repositoryTeacherSubject

        .GetAllIncluding(e => e.Teacher, e => e.Subject);
        //.Where(e => e.Teacher != null && e.Subject !=null);
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(s =>
                    s.Teacher.Name.Contains(input.Keyword) ||
                    s.Subject.Name.Contains(input.Keyword));
            }
            var totalCount = await query.CountAsync(); // Count before paging
            var subjects = await query
           .Skip(input.SkipCount)
           .Take(input.MaxResultCount)
           .ToListAsync();
            var result = subjects.Select(subject => new TeacherSubjectGetDto
            {
                Id = subject.Id,
                TeacherId = subject.TeacherId,
                TeacherName = subject.Teacher?.Name,
                SubjectId=subject.SubjectId,
                SubjectName=subject.Subject?.Name

            }).ToList();
            return new PagedResultDto<TeacherSubjectGetDto>(
                totalCount,
                result
                );
        }


        public async System.Threading.Tasks.Task UpdateAsync(TeacherSubjectCreateUpdateDto input)
        {
            var teacherSubject = await _repositoryTeacherSubject.GetAsync(input.Id);

            teacherSubject.TeacherId = input.TeacherId;
            teacherSubject.SubjectId = input.SubjectId;

            await _repositoryTeacherSubject.UpdateAsync(teacherSubject);
        }
    }
}
