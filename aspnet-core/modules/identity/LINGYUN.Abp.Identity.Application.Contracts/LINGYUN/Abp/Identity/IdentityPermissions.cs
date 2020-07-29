using Volo.Abp.Reflection;

namespace LINGYUN.Abp.Identity
{
    public class IdentityPermissions
    {
        public static class OrganizationUnits
        {
            public const string Default = Volo.Abp.Identity.IdentityPermissions.GroupName + ".OrganizationUnits";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageUsers = Default + ".ManageUsers";
            public const string ManageRoles = Default + ".ManageRoles";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissions));
        }
    }
}
