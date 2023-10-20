namespace LINGYUN.Abp.FeatureManagement;
public static class FeatureManagementErrorCodes
{
    public const string Namespace = "FeatureManagement";

    public static class GroupDefinition
    {
        private const string Prefix = Namespace + ":001";

        public const string StaticGroupNotAllowedChanged = Prefix + "010";

        public const string AlreayNameExists = Prefix + "100";

        public const string NameNotFount = Prefix + "404";
    }

    public static class Definition
    {
        private const string Prefix = Namespace + ":002";

        public const string StaticFeatureNotAllowedChanged = Prefix + "010";

        public const string AlreayNameExists = Prefix + "100";

        public const string FailedGetGroup = Prefix + "101";

        public const string NameNotFount = Prefix + "404";
    }
}
