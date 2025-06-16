using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BoilerPlate_New.Beds.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate_New.Beds
{
    public interface IBedApplicationService:IApplicationService 
    {
        Task<PagedResultDto<GetBedDto>> GetAll(GetAllAccountsInput input);
        Task CreateAsync(CreateUpdateDto input);
        Task UpdateAsync(CreateUpdateDto input);
        Task DeleteAsync(EntityDto<int> input);
    }
}
