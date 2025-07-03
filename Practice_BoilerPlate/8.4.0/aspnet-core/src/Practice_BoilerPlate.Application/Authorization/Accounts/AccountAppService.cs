using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.UI;
using Abp.Zero.Configuration;
using Practice_BoilerPlate.Authorization.Accounts.Dto;
using Practice_BoilerPlate.Authorization.Users;
using Practice_BoilerPlate.Features;
using Abp.Collections;
using Abp;
using Stripe;

namespace Practice_BoilerPlate.Authorization.Accounts
{
    public class AccountAppService : Practice_BoilerPlateAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager)
        {
            _userRegistrationManager = userRegistrationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            // ✅ Create user
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Email Confirmed
            );

            // ✅ Get current tenant id
            var tenantId = AbpSession.TenantId;
            if (!tenantId.HasValue)
            {
                throw new UserFriendlyException("Tenant ID could not be resolved.");
            }

            var tenant = await TenantManager.GetByIdAsync(tenantId.Value);

            // ✅ Determine features from selected plan
            var selectedPlan = input.Plan?.ToLower();
            var featureValues = new Dictionary<string, string>();

            if (selectedPlan == "trial")
            {
                featureValues = new()
        {
            { AppFeatures.MaxUsers, "5" },
            { AppFeatures.MaxRoles, "5" },
            { AppFeatures.ChatEnabled, "false" },
            { AppFeatures.CurrencyExchangeEnabled, "false" }
        };
            }
            else if (selectedPlan == "standard")
            {
                featureValues = new()
        {
            { AppFeatures.MaxUsers, "50" },
            { AppFeatures.MaxRoles, "50" },
            { AppFeatures.ChatEnabled, "true" },
            { AppFeatures.CurrencyExchangeEnabled, "true" }
        };
            }
            else if (selectedPlan == "premium")
            {
                featureValues = new()
        {
            { AppFeatures.MaxUsers, int.MaxValue.ToString() },
            { AppFeatures.MaxRoles, int.MaxValue.ToString() },
            { AppFeatures.ChatEnabled, "true" },
            { AppFeatures.CurrencyExchangeEnabled, "true" }
        };
            }

            // ✅ Set feature values
            var nameValueList = new List<NameValue>();
            foreach (var item in featureValues)
            {
                nameValueList.Add(new NameValue(item.Key, item.Value));
            }

            await TenantManager.SetFeatureValuesAsync(tenant.Id, nameValueList.ToArray());

            // ✅ Return output
            var isEmailConfirmationRequiredForLogin = await SettingManager
                .GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }
    }
}
