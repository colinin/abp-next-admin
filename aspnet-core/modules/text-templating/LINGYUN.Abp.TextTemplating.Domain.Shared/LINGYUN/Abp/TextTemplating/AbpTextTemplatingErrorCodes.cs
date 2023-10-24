namespace LINGYUN.Abp.TextTemplating;

public static class AbpTextTemplatingErrorCodes
{
    public const string Namespace = "TextTemplating";
    public static class TextTemplateDefinition
    {
        private const string Prefix = Namespace + ":001";

        public const string StaticTemplateNotAllowedChanged = Prefix + "010";
        /// <summary>
        /// 模板 {Name} 已经存在
        /// </summary>
        public const string NameAlreadyExists = Prefix + "100";
        /// <summary>
        /// 模板 {Name} 不存在!
        /// </summary>
        public const string TemplateNotFound = Prefix + "404";
    }
}
