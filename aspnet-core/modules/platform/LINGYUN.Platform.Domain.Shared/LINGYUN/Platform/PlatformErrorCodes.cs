namespace LINGYUN.Platform;

public static class PlatformErrorCodes
{
    private const string Namespace = "Platform";

    public const string VersionFileNotFound = Namespace + ":01404";
    /// <summary>
    /// 包版本不能降级
    /// </summary>
    public const string PackageVersionDegraded = Namespace + ":01403";
    /// <summary>
    /// 同级菜单已经存在
    /// </summary>
    public const string DuplicateMenu = Namespace + ":02001";
    /// <summary>
    /// 不能删除拥有子菜单的节点
    /// </summary>
    public const string DeleteMenuHaveChildren = Namespace + ":02002";
    /// <summary>
    /// 菜单层级已达到最大值
    /// </summary>
    public const string MenuAchieveMaxDepth = Namespace + ":02003";
    /// <summary>
    /// 菜单元数据缺少必要的元素
    /// </summary>
    public const string MenuMissingMetadata = Namespace + ":02101";
    /// <summary>
    /// 元数据格式不匹配
    /// </summary>
    public const string MetaFormatMissMatch = Namespace + ":03001";
    /// <summary>
    /// 用户重复收藏菜单
    /// </summary>
    public const string UserDuplicateFavoriteMenu = Namespace + ":04400";
    /// <summary>
    /// 用户收藏菜单未找到
    /// </summary>
    public const string UserFavoriteMenuNotFound = Namespace + ":04404";
}
