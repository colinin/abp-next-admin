using LINGYUN.Abp.WeChat.Features;
using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.WeChat.MiniProgram.Features
{
    public class WeChatMiniProgramFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var group = context.GetGroupOrNull(WeChatFeatures.GroupName);

            var miniProgram = group.AddFeature(
                name: WeChatMiniProgramFeatures.GroupName,
                displayName: L("Features:WeChat.MiniProgram"),
                description: L("Features:WeChat.MiniProgramDesc"));

            miniProgram.CreateChild(
                name: WeChatMiniProgramFeatures.Enable,
                defaultValue: true.ToString(),
                displayName: L("Features:WeChat.MiniProgram.Enable"),
                description: L("Features:WeChat.MiniProgram.EnableDesc"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));
            miniProgram.CreateChild(
                name: WeChatMiniProgramFeatures.EnableAuthorization,
                defaultValue: true.ToString(),
                displayName: L("Features:WeChat.MiniProgram.EnableAuthorization"),
                description: L("Features:WeChat.MiniProgram.EnableAuthorizationDesc"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));


            var message = miniProgram.CreateChild(
                name: WeChatMiniProgramFeatures.Messages.Default,
                displayName: L("Features:WeChat.MiniProgram.Messages"),
                description: L("Features:WeChat.MiniProgram.MessagesDesc"));

            message.CreateChild(
                name: WeChatMiniProgramFeatures.Messages.Enable,
                defaultValue: true.ToString(),
                displayName: L("Features:WeChat.MiniProgram.EnableMessages"),
                description: L("Features:WeChat.MiniProgram.EnableMessagesDesc"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));
            message.CreateChild(
                name: WeChatMiniProgramFeatures.Messages.SendLimit,
                defaultValue: WeChatMiniProgramFeatures.Messages.DefaultSendLimit.ToString(),
                displayName: L("Features:WeChat.MiniProgram.SendLimit"),
                description: L("Features:WeChat.MiniProgram.SendLimitDesc"),
                valueType: new FreeTextStringValueType(new NumericValueValidator(1, 100_0000)));
            message.CreateChild(
                name: WeChatMiniProgramFeatures.Messages.SendLimitInterval,
                defaultValue: WeChatMiniProgramFeatures.Messages.DefaultSendLimitInterval.ToString(),
                displayName: L("Features:WeChat.MiniProgram.SendLimitInterval"),
                description: L("Features:WeChat.MiniProgram.SendLimitIntervalDesc"),
                valueType: new FreeTextStringValueType(new NumericValueValidator(1, 100_0000)));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatResource>(name);
        }
    }
}
