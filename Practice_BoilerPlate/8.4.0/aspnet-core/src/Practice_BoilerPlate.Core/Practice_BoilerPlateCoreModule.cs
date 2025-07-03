using Abp.Application.Features;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Practice_BoilerPlate.Authorization.Roles;
using Practice_BoilerPlate.Authorization.Users;
using Practice_BoilerPlate.Configuration;
using Practice_BoilerPlate.Features;
using Practice_BoilerPlate.Localization;
using Practice_BoilerPlate.MultiTenancy;
using Practice_BoilerPlate.Timing;

namespace Practice_BoilerPlate
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class Practice_BoilerPlateCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            Practice_BoilerPlateLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = Practice_BoilerPlateConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));

            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = Practice_BoilerPlateConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = Practice_BoilerPlateConsts.DefaultPassPhrase;

            // ✅ Add these two lines:
            Configuration.Features.Providers.Add<AppFeatureProvider>();
            Configuration.ReplaceService<IFeatureValueStore, FeatureValueStore>();
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Practice_BoilerPlateCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
