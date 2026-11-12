namespace LINGYUN.Abp.PrintDesigner;

public static class PrintDesignerErrorCodes
{
    public const string Namespace = "PrintDesigner";

    public static class CustomElement
    {
        public const string Prefix = Namespace + ":001";
        /// <summary>
        /// 已经存在名为 {Name} 的自定义元素定义!
        /// </summary>
        public const string NameAlreadyExists = Prefix + "001";
    }

    public static class Template
    {
        public const string Prefix = Namespace + ":002";
        /// <summary>
        /// 已经存在名为 {Name} 的模板定义!
        /// </summary>
        public const string NameAlreadyExists = Prefix + "001";
        /// <summary>
        /// {Name} 模板不存在!
        /// </summary>
        public const string NameNotFound = Prefix + "002";
    }
}
