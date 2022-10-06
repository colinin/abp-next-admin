namespace LINGYUN.Abp.OpenIddict;

public static class OpenIddictApplicationErrorCodes
{
    private const string Namespace = "OpenIddict:";

    public static class Applications
    {
        public const string ClientIdExisted = Namespace + "Applications:0001";
    }

    public static class Authorizations
    {

    }

    public static class Scopes
    {
        public const string NameExisted = Namespace + "Scopes:0001";
    }

    public static class Tokens
    {

    }
}
