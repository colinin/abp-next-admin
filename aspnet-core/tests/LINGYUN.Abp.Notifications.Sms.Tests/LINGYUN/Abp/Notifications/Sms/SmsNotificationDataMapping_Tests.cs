using Microsoft.Extensions.Options;
using Shouldly;
using System;
using Xunit;

namespace LINGYUN.Abp.Notifications.Sms
{
    public class SmsNotificationDataMapping_Tests : AbpNotificationsSmsTestsBase
    {
        private readonly NotificationData _notificationData;
        protected AbpNotificationOptions NotificationOptions { get; }
        protected AbpNotificationsSmsOptions NotificationSmsOptions { get; }
        public SmsNotificationDataMapping_Tests()
        {
            NotificationOptions = GetRequiredService<IOptions<AbpNotificationOptions>>().Value;
            NotificationSmsOptions = GetRequiredService<IOptions<AbpNotificationsSmsOptions>>().Value;

            _notificationData = new NotificationData();
            InitNotificationData(_notificationData);
        }

        private void InitNotificationData(NotificationData data)
        {
            data.WriteStandardData("title", "message", DateTime.Now, "formUser", "description");
            data.WriteStandardData(NotificationSmsOptions.TemplateParamsPrefix, "phoneNumber", "13800138000");
            data.WriteStandardData(NotificationSmsOptions.TemplateParamsPrefix, "template", "SM_202011250901");
            data.TrySetData("otherDataKey", "otherDataValue");
        }

        [Fact]
        public void Mapping_Sms_Notification_Data_Test()
        {
            var mappingSmsItem = NotificationOptions
                .NotificationDataMappings
                .GetMapItemOrDefault(SmsNotificationPublishProvider.ProviderName, NotificationsTestsNames.Test1);

            mappingSmsItem.ShouldNotBeNull();

            var mappingSmsData = mappingSmsItem.MappingFunc(_notificationData);
            mappingSmsData.TryGetData("phoneNumber").ShouldNotBeNull();
            mappingSmsData.TryGetData("phoneNumber").ToString().ShouldBe("13800138000");

            mappingSmsData.TryGetData("template").ShouldNotBeNull();
            mappingSmsData.TryGetData("template").ToString().ShouldBe("SM_202011250901");

            // 按照预定义规则,这条数据被丢弃
            mappingSmsData.TryGetData("otherDataKey").ShouldBeNull();
        }

        [Fact]
        public void Mapping_Standard_Notification_Data_Test()
        {
            var mappingStandardItem = NotificationOptions
                .NotificationDataMappings
                .GetMapItemOrDefault(SmsNotificationPublishProvider.ProviderName, NotificationsTestsNames.Test2);

            var mappingStandardData = mappingStandardItem.MappingFunc(_notificationData);

            // 按照自定义规则,其他数据被丢弃
            mappingStandardData.TryGetData("phoneNumber").ShouldBeNull();
            mappingStandardData.TryGetData("template").ShouldBeNull();
            mappingStandardData.TryGetData("otherDataKey").ShouldBeNull();
            mappingStandardData.ExtraProperties.Count.ShouldBe(6);
        }

        [Fact]
        public void Mapping_Origin_Notification_Data_Test()
        {
            var mappingOriginItem = NotificationOptions
                .NotificationDataMappings
                .GetMapItemOrDefault(SmsNotificationPublishProvider.ProviderName, NotificationsTestsNames.Test3);

            var mappingOriginData = mappingOriginItem.MappingFunc(_notificationData);

            // 按照自定义规则,所有数据被保留
            mappingOriginData.TryGetData(NotificationSmsOptions.TemplateParamsPrefix + "phoneNumber").ShouldNotBeNull();
            mappingOriginData.TryGetData(NotificationSmsOptions.TemplateParamsPrefix + "template").ShouldNotBeNull();
            mappingOriginData.TryGetData("otherDataKey").ShouldNotBeNull();
            mappingOriginData.ExtraProperties.Count.ShouldBe(9);
        }
    }
}
