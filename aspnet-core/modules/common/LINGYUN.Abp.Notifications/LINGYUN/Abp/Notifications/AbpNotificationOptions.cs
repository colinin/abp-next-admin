using Volo.Abp.Collections;

namespace LINGYUN.Abp.Notifications
{
    public class AbpNotificationOptions
    {
        public ITypeList<INotificationDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<INotificationPublishProvider> PublishProviders { get; }

        public AbpNotificationOptions()
        {
            PublishProviders = new TypeList<INotificationPublishProvider>();
            DefinitionProviders = new TypeList<INotificationDefinitionProvider>();
        }
    }
}
