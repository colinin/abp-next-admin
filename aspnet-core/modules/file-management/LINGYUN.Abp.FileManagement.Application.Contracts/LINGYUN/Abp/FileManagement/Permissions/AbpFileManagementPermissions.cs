namespace LINGYUN.Abp.FileManagement.Permissions
{
    public class AbpFileManagementPermissions
    {
        public const string GroupName = "AbpFileManagement";

        /// <summary>
        /// 文件系统
        /// </summary>
        public class FileSystem
        {
            public const string Default = GroupName + ".FileSystem";

            public const string Create = Default + ".Create";

            public const string Delete = Default + ".Delete";

            public const string Update = Default + ".Update";

            public const string Copy = Default + ".Copy";

            public const string Move = Default + ".Move";
            /// <summary>
            /// 文件管理
            /// </summary>
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
    }
}
