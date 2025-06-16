using Abp.Application.Services;
using Abp.Application.Services.Dto;

using Practice_BoilerPlate.Students.Dto;
using Practice_BoilerPlate.Teachers.Dto;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Teachers
{
    public interface ITeacherApplicationService:IApplicationService
    {
        Task<PagedResultDto<TeacherGetDto>> GetAll(GetAllAccountsInput getAllAccountsInput);
        System.Threading.Tasks.Task CreateAsync(TeacherCreateDto input);
        System.Threading.Tasks.Task UpdateAsync(TeacherUpdateDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input); //input is just an object that holds an int Id.
    }
}
