using Volo.Abp.Reflection;

namespace LINGYUN.Abp.ElsaNext.Studio.Permissions;

public static class AbpElsaNextStudioPermissionsNames
{
    public const string GroupName = "Abp.ElsaNext.Studio";

    public const string View = GroupName + ".View";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AbpElsaNextStudioPermissionsNames));
    }
}
