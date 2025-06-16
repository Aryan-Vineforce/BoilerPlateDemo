using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Practice_BoilerPlate.Configuration.Dto;

namespace Practice_BoilerPlate.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : Practice_BoilerPlateAppServiceBase, IConfigurationAppService
    {
        public async System.Threading.Tasks.Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
