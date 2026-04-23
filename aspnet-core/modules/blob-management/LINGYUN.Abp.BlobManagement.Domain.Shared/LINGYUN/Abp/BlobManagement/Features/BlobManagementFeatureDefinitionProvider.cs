using LINGYUN.Abp.BlobManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.BlobManagement.Features;

public class BlobManagementFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var featureGroup = context.AddGroup(
            name: BlobManagementFeatureNames.GroupName,
            displayName: L("Features:BlobManagement"));

        featureGroup.AddFeature(
            name: BlobManagementFeatureNames.Enable,
            defaultValue: true.ToString(),
            displayName: L("Features:DisplayName:EnableBlobManagement"),
            description: L("Features:Description:EnableBlobManagement"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        featureGroup.AddFeature(
            name: BlobManagementFeatureNames.PublicAccess,
            defaultValue: false.ToString(),
            displayName: L("Features:DisplayName:PublicAccess"),
            description: L("Features:Description:PublicAccess"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        var blobDefaultFeature = featureGroup.AddFeature(
            name: BlobManagementFeatureNames.Blob.Enable,
            defaultValue: true.ToString(),
            displayName: L("Features:DisplayName:EnableBlob"),
            description: L("Features:Description:EnableBlob"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        blobDefaultFeature.CreateChild(
            name: BlobManagementFeatureNames.Blob.DownloadFile,
            defaultValue: true.ToString(),
            displayName: L("Features:DisplayName:DownloadFile"),
            description: L("Features:Description:DownloadFile"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        blobDefaultFeature.CreateChild(
            name: BlobManagementFeatureNames.Blob.DownloadLimit,
            defaultValue: "1000",
            displayName: L("Features:DisplayName:DownloadLimit"),
            description: L("Features:Description:DownloadLimit"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(0, 100_0000))); // 上限100万次调用
        blobDefaultFeature.CreateChild(
            name: BlobManagementFeatureNames.Blob.DownloadInterval,
            defaultValue: "1",
            displayName: L("Features:DisplayName:DownloadInterval"),
            description: L("Features:Description:DownloadInterval"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 12))); // 上限12月

        blobDefaultFeature.CreateChild(
            name: BlobManagementFeatureNames.Blob.UploadFile,
            defaultValue: true.ToString(),
            displayName: L("Features:DisplayName:UploadFile"),
            description: L("Features:Description:UploadFile"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        blobDefaultFeature.CreateChild(
            name: BlobManagementFeatureNames.Blob.UploadLimit,
            defaultValue: "1000",
            displayName: L("Features:DisplayName:UploadLimit"),
            description: L("Features:Description:UploadLimit"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(0, 100_0000))); // 上限100万次调用
        blobDefaultFeature.CreateChild(
            name: BlobManagementFeatureNames.Blob.UploadInterval,
            defaultValue: "1",
            displayName: L("Features:DisplayName:UploadInterval"),
            description: L("Features:Description:UploadInterval"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 12))); // 上限12月
    }

    protected ILocalizableString L(string name)
    {
        return LocalizableString.Create<BlobManagementResource>(name);
    }
}
