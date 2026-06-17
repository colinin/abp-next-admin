namespace LINGYUN.Abp.LocalizationManagement;

public class AbpLocalizationManagementOptions
{
    /// <summary>
    /// 保存本地化文本到数据库
    /// </summary>
    public bool SaveStaticLocalizationsToDatabase { get; set; } = true;
    /// <summary>
    /// 是否动态本地化文本初始化节点
    /// </summary>
    public bool IsDynamicLocalizationInitializerHost { get; set; } = false;

    public AbpLocalizationManagementOptions()
    {
    }
}
