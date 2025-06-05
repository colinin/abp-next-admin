namespace LINGYUN.Abp.Account.Web.Bundling;

public static class AccountBundles
{
    public static class Scripts
    {
        public const string Global = "Abp.Account";

        public const string ChangePassword = Global + ".ChangePassword";
    }

    public static class Styles
    {
        public const string Global = "Abp.Account";

        public const string UserLoginLink = Global + ".UserLoginLink";
    }
}
