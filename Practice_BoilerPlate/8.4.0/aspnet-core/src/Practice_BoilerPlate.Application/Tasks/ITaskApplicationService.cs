using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Task.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Task
{
    public interface ITaskApplicationService:IApplicationService
    {
        Task<PagedResultDto<TaskDto>> GetAllAsync(PagedAndSortedResultRequestDto input);
        Task<TaskDto> GetAsync(EntityDto<int> input);
        Task_Item CreateAsync(CreateUpdateTaskDto input);
        Task_Item UpdateAsync(CreateUpdateTaskDto input);
        Task_Item DeleteAsync(EntityDto<int> input);
    }
}
