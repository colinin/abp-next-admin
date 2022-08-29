using Volo.Abp.Collections;

namespace LINGYUN.Abp.Notifications
{
    public class AbpNotificationsOptions
    {
        /// <summary>
        /// 自定义通知集合
        /// </summary>
        public ITypeList<INotificationDefinitionProvider> DefinitionProviders { get; }

        public AbpNotificationsOptions()
        {
            DefinitionProviders = new TypeList<INotificationDefinitionProvider>();
        }
    }
}
