using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Addresses.Dto
{
    public interface IAddressApplicationService:IApplicationService
    {
        Task<PagedResultDto<GetAddressDto>> GetAll(GetAllAccountsInput getAllAccountsInput);
        System.Threading.Tasks.Task CreateAsync(CreateAddressDto input);
        System.Threading.Tasks.Task UpdateAsync(UpdateAddressDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input);
    }
}
