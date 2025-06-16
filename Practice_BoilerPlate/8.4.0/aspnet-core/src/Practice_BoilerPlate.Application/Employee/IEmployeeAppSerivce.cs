using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Employee.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Employee
{
    public interface IEmployeeAppSerivce:IApplicationService
    {
        Task<PagedResultDto<GetEmployeeDto>> GetAll(GetAllAccountsInput getAllAccountsInput);
        System.Threading.Tasks.Task CreateAsync(CreateUpdateEmployeDto input);
        System.Threading.Tasks.Task UpdateAsync(UpdateEmployeeDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input);
    }
}
