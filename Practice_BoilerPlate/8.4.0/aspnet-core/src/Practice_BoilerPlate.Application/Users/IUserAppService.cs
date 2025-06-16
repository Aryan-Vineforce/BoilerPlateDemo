using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Roles.Dto;
using Practice_BoilerPlate.Users.Dto;

namespace Practice_BoilerPlate.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        System.Threading.Tasks.Task DeActivate(EntityDto<long> user);
        System.Threading.Tasks.Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        System.Threading.Tasks.Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);
    }
}
