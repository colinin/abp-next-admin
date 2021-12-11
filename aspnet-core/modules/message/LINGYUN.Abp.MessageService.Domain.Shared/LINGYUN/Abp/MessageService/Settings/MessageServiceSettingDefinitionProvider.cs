using LINGYUN.Abp.MessageService.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.MessageService.Settings
{
    public class MessageServiceSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                 new SettingDefinition(
                     MessageServiceSettingNames.Messages.RecallExpirationTime,
                     "2",
                     L("DisplayName:RecallExpirationTime"),
                     L("Description:RecallExpirationTime"),
                     isVisibleToClients: false,
                     isEncrypted: false)
                 .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
             );
        }

        protected ILocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
