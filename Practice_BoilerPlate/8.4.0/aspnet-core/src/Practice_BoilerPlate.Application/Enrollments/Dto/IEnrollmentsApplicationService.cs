using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Students.Dto;
using System.Threading.Tasks;


namespace Practice_BoilerPlate.Enrollments.Dto
{
    public interface IEnrollmentsApplicationService:IApplicationService
    {
        Task<PagedResultDto<EnrollmentGetDto>> GetAll(GetAllAccountsInput getAllAccountInput);
        System.Threading.Tasks.Task CreateAsync(EnrollmentCreateUpdateDto input);
        System.Threading.Tasks.Task UpdateAsync(EnrollmentCreateUpdateDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input);
    }
}
