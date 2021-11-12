using Microsoft.Extensions.Options;
using Shouldly;
using System;
using Xunit;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram
{
    public class WeChatMiniProgramNotificationDataMapping_Tests : AbpNotificationsWeChatMiniProgramTestsBase
    {
        private readonly NotificationData _notificationData;
        protected AbpNotificationOptions NotificationOptions { get; }
        protected AbpNotificationsWeChatMiniProgramOptions NotificationWeChatMiniProgramOptions { get; }
        public WeChatMiniProgramNotificationDataMapping_Tests()
        {
            NotificationOptions = GetRequiredService<IOptions<AbpNotificationOptions>>().Value;
            NotificationWeChatMiniProgramOptions = GetRequiredService<IOptions<AbpNotificationsWeChatMiniProgramOptions>>().Value;

            _notificationData = new NotificationData();
            InitNotificationData(_notificationData);
        }

        private void InitNotificationData(NotificationData data)
        {
            data.WriteStandardData("title", "message", DateTime.Now, "formUser", "description");
            data.WriteStandardData(NotificationWeChatMiniProgramOptions.DefaultMsgPrefix, "openid", "TEST");
            data.TrySetData("otherDataKey", "otherDataValue");
        }


        [Fact]
        public void Mapping_WeChatMiniProgram_Notification_Data_Test()
        {
            var mappingOpenIdItem = NotificationOptions
                .NotificationDataMappings
                .GetMapItemOrDefault(WeChatMiniProgramNotificationPublishProvider.ProviderName, NotificationsTestsNames.Test1);

            mappingOpenIdItem.ShouldNotBeNull();

            var mappingOpenIdData = mappingOpenIdItem.MappingFunc(_notificationData);
            mappingOpenIdData.TryGetData("openid").ShouldNotBeNull();
            mappingOpenIdData.TryGetData("openid").ToString().ShouldBe("TEST");

            // 按照预定义规则,这条数据被丢弃
            mappingOpenIdData.TryGetData("otherDataKey").ShouldBeNull();
        }

        [Fact]
        public void Mapping_Standard_Notification_Data_Test()
        {
            var mappingStandardItem = NotificationOptions
                .NotificationDataMappings
                .GetMapItemOrDefault(WeChatMiniProgramNotificationPublishProvider.ProviderName, NotificationsTestsNames.Test2);

            var mappingStandardData = mappingStandardItem.MappingFunc(_notificationData);

            // 按照自定义规则,其他数据被丢弃
            mappingStandardData.TryGetData("openid").ShouldBeNull();
            mappingStandardData.TryGetData("otherDataKey").ShouldBeNull();
            mappingStandardData.ExtraProperties.Count.ShouldBe(6);
        }

        [Fact]
        public void Mapping_Origin_Notification_Data_Test()
        {
            var mappingOriginItem = NotificationOptions
                .NotificationDataMappings
                .GetMapItemOrDefault(WeChatMiniProgramNotificationPublishProvider.ProviderName, NotificationsTestsNames.Test3);

            var mappingOriginData = mappingOriginItem.MappingFunc(_notificationData);

            // 按照自定义规则,所有数据被保留
            mappingOriginData.TryGetData(NotificationWeChatMiniProgramOptions.DefaultMsgPrefix + "openid").ShouldNotBeNull();
            mappingOriginData.TryGetData("otherDataKey").ShouldNotBeNull();
            mappingOriginData.ExtraProperties.Count.ShouldBe(8);
        }
    }
}
