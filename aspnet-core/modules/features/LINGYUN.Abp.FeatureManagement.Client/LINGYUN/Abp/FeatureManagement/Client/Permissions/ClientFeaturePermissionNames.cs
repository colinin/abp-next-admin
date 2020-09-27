namespace LINGYUN.Abp.FeatureManagement.Client.Permissions
{
    public class ClientFeaturePermissionNames
    {
        public const string GroupName = "IdentityServer";

        public static class Clients
        {
            public const string Default = GroupName + ".Clients";
            /// <summary>
            /// 管理功能权限
            /// </summary>
            public const string ManageFeatures = Default + ".ManageFeatures";
        }
    }
}
