using LINGYUN.Abp.WeChat.Features;
using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram.Features
{
    public class WeChatMiniProgramFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var wechatGroup = context.GetGroupOrNull(WeChatFeatures.GroupName);
            if (wechatGroup != null)
            {
                var weappFeature = wechatGroup
                    .AddFeature(
                        WeChatMiniProgramFeatures.GroupName, 
                        true.ToString(),
                        L("Features:MiniProgram"),
                        L("Features:MiniProgramDescription"),
                        new ToggleStringValueType(new BooleanValueValidator()));


                var weappNofitication = weappFeature
                    .CreateChild(
                        WeChatMiniProgramFeatures.Notifications.Default,
                        true.ToString(),
                        L("Features:Notifications"),
                        L("Features:Notifications"),
                        new ToggleStringValueType(new BooleanValueValidator()));
                weappNofitication
                    .CreateChild(
                        WeChatMiniProgramFeatures.Notifications.PublishLimit,
                        WeChatMiniProgramFeatures.Notifications.DefaultPublishLimit.ToString(),
                        L("Features:PublishLimit"),
                        L("Features:PublishLimitDescription"),
                        new ToggleStringValueType(new NumericValueValidator(0, 100000)));
                weappNofitication
                    .CreateChild(
                        WeChatMiniProgramFeatures.Notifications.PublishLimitInterval,
                        WeChatMiniProgramFeatures.Notifications.DefaultPublishLimitInterval.ToString(),
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
