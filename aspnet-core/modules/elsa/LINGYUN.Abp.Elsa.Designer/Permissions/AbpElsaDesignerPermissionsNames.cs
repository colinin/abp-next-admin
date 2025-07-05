using Volo.Abp.Reflection;

namespace LINGYUN.Abp.Elsa.Designer.Permissions;

public static class AbpElsaDesignerPermissionsNames
{
    public const string GroupName = "Abp.Elsa.Designer";

    public const string View = GroupName + ".View";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AbpElsaDesignerPermissionsNames));
    }
}
