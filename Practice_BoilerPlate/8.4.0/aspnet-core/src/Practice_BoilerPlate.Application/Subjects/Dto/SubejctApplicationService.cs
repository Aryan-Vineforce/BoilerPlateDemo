using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Students.Dto;
using System.Linq;
using System.Threading.Tasks;
namespace Practice_BoilerPlate.Subjects.Dto
{
    public class SubejctApplicationService : ApplicationService, ISubjectApplicationService
    {
        private readonly IRepository<Subject> _repositorysubject;
        public SubejctApplicationService(IRepository<Subject> repositorysubject)
        {
            _repositorysubject = repositorysubject;
        }

        public async System.Threading.Tasks.Task CreateAsync(CreateSubjectDto input)
        {
            var subejct = new Subject
            {
                TenantId = (int)AbpSession.TenantId,
                Name = input.Name,
                Code = input.Code,
                Credits = input.Credits,

                CourseId = input.CourseId
            };
            await _repositorysubject.InsertAsync(subejct);
        }


        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _repositorysubject.DeleteAsync(input.Id);
        }
        public async Task<PagedResultDto<GetSubjectDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _repositorysubject
        .GetAllIncluding(e => e.Course)
        .Where(e => e.Course != null);
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(s =>
                    s.Name.Contains(input.Keyword) ||
                    s.Code.Contains(input.Keyword));
            }
            var totalCount = await query.CountAsync(); // Count before paging
                 var subjects = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();
            var result = subjects.Select(subject => new GetSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Code = subject.Code,
                Credits = subject.Credits,
                CourseId = subject.CourseId,
                CourseName = subject.Course?.Name
            }).ToList();
            return new PagedResultDto<GetSubjectDto>(
                totalCount,
                result
                );
        }
        public async System.Threading.Tasks.Task UpdateAsync(UpdateSubjectDto input)
        {
            var employees = await _repositorysubject
     .GetAllIncluding(e => e.Course)
     .Where(e => e.Course != null)
     .ToListAsync();
            var subject = await _repositorysubject.FirstOrDefaultAsync(input.Id);
            if (subject == null)
            {
                throw new UserFriendlyException("Employee not found");
            }


            subject.Name = input.Name;
            subject.Code = input.Code;
            subject.Credits = input.Credits;
            subject.CourseId = input.CourseId;

            subject.Course.Name = subject.Course.Name;
            await _repositorysubject.UpdateAsync(subject);
        }
    }
}