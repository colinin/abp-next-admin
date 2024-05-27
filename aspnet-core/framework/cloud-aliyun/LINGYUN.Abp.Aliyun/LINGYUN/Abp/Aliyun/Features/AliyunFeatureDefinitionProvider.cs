using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Aliyun.Features;
public class AliyunFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var featureGroup = context.AddGroup(
                name: AliyunFeatureNames.GroupName,
                displayName: L("Features:AlibabaCloud"));

        featureGroup.AddFeature(
            name: AliyunFeatureNames.Enable,
            defaultValue: false.ToString(),
            displayName: L("Features:AlibabaCloud:IsEnabled"),
            description: L("Features:AlibabaCloud:IsEnabledDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        var smsFeature = featureGroup.AddFeature(
            name: AliyunFeatureNames.Sms.Enable,
            defaultValue: false.ToString(),
            displayName: L("Features:AlibabaCloud:Sms"),
            description: L("Features:AlibabaCloud:SmsDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        smsFeature.CreateChild(
            name: AliyunFeatureNames.Sms.SendLimit,
            defaultValue: AliyunFeatureNames.Sms.DefaultSendLimit.ToString(),
            displayName: L("Features:AlibabaCloud:Sms.SendLimit"),
            description: L("Features:AlibabaCloud:Sms.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 100_0000)));
        smsFeature.CreateChild(
            name: AliyunFeatureNames.Sms.SendLimitInterval,
            defaultValue: AliyunFeatureNames.Sms.DefaultSendLimitInterval.ToString(),
            displayName: L("Features:AlibabaCloud:Sms.SendLimitInterval"),
            description: L("Features:AlibabaCloud:Sms.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 100_0000)));

        var blobFeature = featureGroup.AddFeature(
            name: AliyunFeatureNames.BlobStoring.Enable,
            defaultValue: false.ToString(),
            displayName: L("Features:AlibabaCloud:Oss"),
            description: L("Features:AlibabaCloud:OssDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<AliyunResource>(name);
    }
}
