using LINGYUN.Abp.FileManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.FileManagement.Features
{
    public class AbpFileManagementFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var featureGroup = context.AddGroup(
                name: AbpFileManagementFeatureNames.GroupName,
                displayName: L("Features:FileManagement"));

            var fileSystemFeature = featureGroup.AddFeature(
                name: AbpFileManagementFeatureNames.FileSystem.Default,
                displayName: L("Features:DisplayName:FileSystem"),
                description: L("Features:Description:FileSystem"));

            fileSystemFeature.CreateChild(
                name: AbpFileManagementFeatureNames.FileSystem.DownloadFile, 
                defaultValue: false.ToString(),
                displayName: L("Features:DisplayName:DownloadFile"),
                description: L("Features:Description:DownloadFile"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));

            fileSystemFeature.CreateChild(
                name: AbpFileManagementFeatureNames.FileSystem.UploadFile,
                defaultValue: true.ToString(),
                displayName: L("Features:DisplayName:UploadFile"),
                description: L("Features:Description:UploadFile"),
                valueType: new ToggleStringValueType(new BooleanValueValidator()));

            // TODO: 此功能需要控制器协同,暂时不实现
            fileSystemFeature.CreateChild(
                name: AbpFileManagementFeatureNames.FileSystem.MaxUploadFileCount,
                defaultValue: 1.ToString(),
                displayName: L("Features:DisplayName:MaxUploadFileCount"),
                description: L("Features:Description:MaxUploadFileCount"),
                valueType: new FreeTextStringValueType(new NumericValueValidator(1, 10)));
        }

        protected ILocalizableString L(string name)
        {
            return LocalizableString.Create<AbpFileManagementResource>(name);
        }
    }
}
