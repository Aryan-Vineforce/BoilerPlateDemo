using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Students.Dto;
using Practice_BoilerPlate.Subjects.Dto;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Subjects
{
    public interface ISubjectApplicationService:IApplicationService
    {
        Task<PagedResultDto<GetSubjectDto>> GetAll(GetAllAccountsInput getAllAccountsInput);
        System.Threading.Tasks.Task CreateAsync(CreateSubjectDto input);
        System.Threading.Tasks.Task UpdateAsync(UpdateSubjectDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input);

    }
}
