using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Courses.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Courses
{
    public interface ICourseApplicationService:IApplicationService
    {
        Task<PagedResultDto<GetCourseDto>> GetAll(GetAllAccountsInput getAllAccountsInput);
        System.Threading.Tasks.Task CreateAsync(CreateCourseDto input);
        System.Threading.Tasks.Task UpdateAsync(UpdateCourseDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input); //input is just an object that holds an int Id.
    }
}
