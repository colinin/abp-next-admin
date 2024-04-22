using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Tencent.Features
{
    public class TencentCloudFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var group = context.AddGroup(TencentCloudFeatures.GroupName, L("Features:TencentCloud"));

            var smsEnableFeature = group.AddFeature(
                name: TencentCloudFeatures.Sms.Enable,
                defaultValue: true.ToString(),
                displayName: L("Features:TencentSmsEnable"),
                description: L("Features:TencentSmsEnable.Desc"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));

            var blobStoringEnableFeature = group.AddFeature(
                name: TencentCloudFeatures.BlobStoring.Enable,
                defaultValue: true.ToString(),
                displayName: L("Features:TencentBlobStoringEnable"),
                description: L("Features:TencentBlobStoringEnable.Desc"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));
            blobStoringEnableFeature.CreateChild(
                name: TencentCloudFeatures.BlobStoring.MaximumStreamSize,
                defaultValue: "0",
                displayName: L("Features:TencentBlobStoringMaximumStreamSize"),
                description: L("Features:TencentBlobStoringMaximumStreamSize.Desc"),
                valueType: new FreeTextStringValueType(new NumericValueValidator(0)));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<TencentCloudResource>(name);
        }
    }
}
