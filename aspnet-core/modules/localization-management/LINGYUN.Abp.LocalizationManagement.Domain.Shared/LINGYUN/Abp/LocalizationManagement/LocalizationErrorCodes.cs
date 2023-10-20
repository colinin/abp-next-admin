namespace LINGYUN.Abp.LocalizationManagement;
public static class LocalizationErrorCodes
{
    public const string Namespace = "Localization";

    /// <summary>
    /// 语言代码002
    /// </summary>
    public static class Language
    {
        public const string Prefix = Namespace + ":001";
        /// <summary>
        /// 语言 {CultureName} 已经存在
        /// </summary>
        public const string NameAlreadyExists = Prefix + "100";
        /// <summary>
        /// 没有找到名为 {CultureName} 的语言名称或内置语言不允许操作!
        /// </summary>
        public const string NameNotFoundOrStaticNotAllowed = Prefix + "400";
    }

    /// <summary>
    /// 资源代码002
    /// </summary>
    public static class Resource
    {
        public const string Prefix = Namespace + ":002";
        /// <summary>
        /// 资源 {Name} 已经存在
        /// </summary>
        public const string NameAlreadyExists = Prefix + "100";
        /// <summary>
        /// 没有找到名为 {Name} 的资源名称或内置资源不允许操作!
        /// </summary>
        public const string NameNotFoundOrStaticNotAllowed = Prefix + "400";
    }
}
