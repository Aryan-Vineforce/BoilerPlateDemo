using Abp.Application.Features;
using Abp.Localization;
using Abp.UI.Inputs;

namespace Practice_BoilerPlate.Features
{
    public class AppFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            var maxUsers = context.Create(
                AppFeatures.MaxUsers,
                defaultValue: "5",
                displayName: L("Maximum Users")
            );
            maxUsers.InputType = new SingleLineStringInputType();

            var maxRoles = context.Create(
                AppFeatures.MaxRoles,
                defaultValue: "5",
                displayName: L("Maximum Roles")
            );
            maxRoles.InputType = new SingleLineStringInputType();

            var chatEnabled = context.Create(
                AppFeatures.ChatEnabled,
                defaultValue: "false",
                displayName: L("Chat Enabled")
            );
            chatEnabled.InputType = new CheckboxInputType();

            var currencyEnabled = context.Create(
                AppFeatures.CurrencyExchangeEnabled,
                defaultValue: "false",
                displayName: L("Currency Exchange Enabled")
            );
            currencyEnabled.InputType = new CheckboxInputType();
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, Practice_BoilerPlateConsts.LocalizationSourceName);
        }
    }
}
