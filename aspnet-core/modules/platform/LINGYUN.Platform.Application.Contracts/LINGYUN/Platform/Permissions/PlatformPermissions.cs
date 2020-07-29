using Volo.Abp.Reflection;

namespace LINGYUN.Platform.Permissions
{
    public class PlatformPermissions
    {
        public const string GroupName = "Platform";

        // 如果abp后期提供对象存储的目录管理接口,则启用此权限
        /// <summary>
        /// 文件系统
        /// </summary>
        public class FileSystem
        {
            public const string Default = GroupName + ".FileSystem";

            public const string Create = Default + ".Create";

            public const string Delete = Default + ".Delete";

            public const string Rename = Default + ".Rename";

            public const string Copy = Default + ".Copy";

            public const string Move = Default + ".Move";

            public class FileManager
            {
                public const string Default = FileSystem.Default + ".FileManager";

                public const string Create = Default + ".Create";

                public const string Copy = Default + ".Copy";

                public const string Delete = Default + ".Delete";

                public const string Update = Default + ".Update";

                public const string Move = Default + ".Move";

                public const string Download = Default + ".Download";
            }
        }

        public class AppVersion
        {
            public const string Default = GroupName + ".AppVersion";

            public const string Create = Default + ".Create";

            public const string Delete = Default + ".Delete";

            public class FileManager
            {
                public const string Default = AppVersion.Default + ".FileManager";

                public const string Create = Default + ".Create";

                public const string Delete = Default + ".Delete";

                public const string Download = Default + ".Download";
            }
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(PlatformPermissions));
        }
    }
}
