namespace LINGYUN.Abp.LocalizationManagement;

public class AbpLocalizationManagementOptions
{
    /// <summary>
    /// 保存本地化文本到数据库
    /// </summary>
    public bool SaveStaticLocalizationsToDatabase { get; set; }

    public AbpLocalizationManagementOptions()
    {
    }
}
