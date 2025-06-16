using Abp.Application.Services;
using Abp.Application.Services.Dto;

using Practice_BoilerPlate.Students.Dto;

using System.Threading.Tasks;

namespace Practice_BoilerPlate.Students
{
    public interface IStudentAppService: IApplicationService
    {
        Task<PagedResultDto<GetStudentDto>> GetAll(GetAllAccountsInput getAllAccountsInput);
        System.Threading.Tasks.Task CreateAsync(CreateStudentDto input);
        System.Threading.Tasks.Task UpdateAsync(UpdateStudentDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input);

    }
}
