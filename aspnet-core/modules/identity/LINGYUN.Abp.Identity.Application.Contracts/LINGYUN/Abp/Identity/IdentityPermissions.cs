using Volo.Abp.Reflection;

namespace LINGYUN.Abp.Identity
{
    public class IdentityPermissions
    {
        public static class Roles
        {
            public const string ManageClaims = Volo.Abp.Identity.IdentityPermissions.Roles.Default + ".ManageClaims";
            public const string ManageOrganizationUnits = Volo.Abp.Identity.IdentityPermissions.Roles.Default + ".ManageOrganizationUnits";
        }

        public static class Users
        {
            public const string ManageClaims = Volo.Abp.Identity.IdentityPermissions.Users.Default + ".ManageClaims";
            public const string ManageOrganizationUnits = Volo.Abp.Identity.IdentityPermissions.Users.Default + ".ManageOrganizationUnits";
        }

        public static class OrganizationUnits
        {
            public const string Default = Volo.Abp.Identity.IdentityPermissions.GroupName + ".OrganizationUnits";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageUsers = Default + ".ManageUsers";
            public const string ManageRoles = Default + ".ManageRoles";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class IdentityClaimType
        {
            public const string Default = Volo.Abp.Identity.IdentityPermissions.GroupName + ".IdentityClaimTypes";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissions));
        }
    }
}
