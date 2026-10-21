namespace LINGYUN.Abp.Identity.Features;

public static class IdentityFeatureNames
{
    public const string GroupName = "AbpIdentity";
    public static class TwoFactor
    {
        public const string Default = GroupName + ".TwoFactor";

        public const string Behaviour = Default + ".Behaviour";
    }
}
