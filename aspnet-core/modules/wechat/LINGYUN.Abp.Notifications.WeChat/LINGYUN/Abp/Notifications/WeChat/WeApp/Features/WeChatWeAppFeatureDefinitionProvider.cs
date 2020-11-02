using LINGYUN.Abp.WeChat.Features;
using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp.Features
{
    public class WeChatWeAppFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var wechatGroup = context.GetGroupOrNull(WeChatFeatures.GroupName);
            if (wechatGroup != null)
            {
                var weappFeature = wechatGroup
                    .AddFeature(
                        WeChatWeAppFeatures.GroupName, 
                        true.ToString(),
                        L("Features:WeApp"),
                        L("Features:WeAppDescription"),
                        new ToggleStringValueType(new BooleanValueValidator()));


                var weappNofitication = weappFeature
                    .CreateChild(
                        WeChatWeAppFeatures.Notifications.Default,
                        true.ToString(),
                        L("Features:Notifications"),
                        L("Features:Notifications"),
                        new ToggleStringValueType(new BooleanValueValidator()));
                weappNofitication
                    .CreateChild(
                        WeChatWeAppFeatures.Notifications.PublishLimit,
                        WeChatWeAppFeatures.Notifications.DefaultPublishLimit.ToString(),
                        L("Features:PublishLimit"),
                        L("Features:PublishLimitDescription"),
                        new ToggleStringValueType(new NumericValueValidator(0, 100000)));
                weappNofitication
                    .CreateChild(
                        WeChatWeAppFeatures.Notifications.PublishLimitInterval,
                        WeChatWeAppFeatures.Notifications.DefaultPublishLimitInterval.ToString(),
                        L("Features:PublishLimitInterval"),
                        L("Features:PublishLimitIntervalDescription"),
                        new ToggleStringValueType(new NumericValueValidator(1, 12)));
            }
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatResource>(name);
        }
    }
}
