using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Deals.Dto;
using Practice_BoilerPlate.Students.Dto;
using Practice_BoilerPlate.Task.Dto;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Deals
{
    public interface IDealWithTasksAppService :IApplicationService
    {
        Task<DealDto> CreateDealWithTasksAsync(CreateUpdateDealDto input);
        Task<PagedResultDto<DealDto>> GetDealWithTasksAsync(GetAllAccountsInput getAllAccountInput);
        Task<DealDto> UpdateDealWithTasksAsync(int id, CreateUpdateDealDto input);
        Task<DealDto> DeleteAsync( EntityDto<int> input);
        Task <TaskDto> DeleteTaskAsync(EntityDto<int> input);
    }
}
