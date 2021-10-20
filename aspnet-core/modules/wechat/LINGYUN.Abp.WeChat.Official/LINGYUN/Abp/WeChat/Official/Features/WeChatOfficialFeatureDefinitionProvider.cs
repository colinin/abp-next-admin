using LINGYUN.Abp.WeChat.Features;
using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.WeChat.Official.Features
{
    public class WeChatOfficialFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var group = context.GetGroupOrNull(WeChatFeatures.GroupName);

            var official = group.AddFeature(
                name: WeChatOfficialFeatures.GroupName,
                displayName: L("Features:WeChat.Official"),
                description: L("Features:WeChat.OfficialDesc"));

            official.CreateChild(
                name: WeChatOfficialFeatures.Enable,
                defaultValue: true.ToString(),
                displayName: L("Features:WeChat.Official.Enable"),
                description: L("Features:WeChat.Official.EnableDesc"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));
            official.CreateChild(
                name: WeChatOfficialFeatures.EnableAuthorization,
                defaultValue: true.ToString(),
                displayName: L("Features:WeChat.Official.EnableAuthorization"),
                description: L("Features:WeChat.Official.EnableAuthorizationDesc"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatResource>(name);
        }
    }
}
