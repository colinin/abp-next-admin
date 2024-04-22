using Volo.Abp.Collections;

namespace LINGYUN.Abp.SettingManagement;
public class SettingManagementMergeOptions
{
    public ITypeList<IUserSettingAppService> UserSettingProviders { get; }
    public ITypeList<IReadonlySettingAppService> GlobalSettingProviders { get; }
    public SettingManagementMergeOptions()
    {
        UserSettingProviders = new TypeList<IUserSettingAppService>();
        GlobalSettingProviders = new TypeList<IReadonlySettingAppService>();
    }
}
