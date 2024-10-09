namespace LINGYUN.Abp.Demo;
public static class DemoErrorCodes
{
    public const string Namespace = "Demo";

    public static class Author
    {
        public const string Prefix = Namespace + ":00";
        /// <summary>
        /// 作者 {Name} 已经存在!
        /// </summary>
        public const string AuthorAlreadyExists = Prefix + "001";
    }
}
