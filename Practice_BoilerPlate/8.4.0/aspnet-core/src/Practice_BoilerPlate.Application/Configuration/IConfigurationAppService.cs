using System.Threading.Tasks;
using Practice_BoilerPlate.Configuration.Dto;

namespace Practice_BoilerPlate.Configuration
{
    public interface IConfigurationAppService
    {
        System.Threading.Tasks.Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
