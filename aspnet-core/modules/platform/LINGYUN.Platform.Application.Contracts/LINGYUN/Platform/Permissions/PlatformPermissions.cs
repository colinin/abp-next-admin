﻿using Volo.Abp.Reflection;

namespace LINGYUN.Platform.Permissions;

public static class PlatformPermissions
{
    public const string GroupName = "Platform";

    public static class DataDictionary
    {
        public const string Default = GroupName + ".DataDictionary";

        public const string Create = Default + ".Create";

        public const string Update = Default + ".Update";

        public const string Move = Default + ".Move";

        public const string Delete = Default + ".Delete";

        public const string ManageItems = Default + ".ManageItems";
    }

    public static class Layout
    {
        public const string Default = GroupName + ".Layout";

        public const string Create = Default + ".Create";

        public const string Update = Default + ".Update";

        public const string Delete = Default + ".Delete";
    }

    public static class Menu
    {
        public const string Default = GroupName + ".Menu";

        public const string Create = Default + ".Create";

        public const string Update = Default + ".Update";

        public const string Delete = Default + ".Delete";

        public const string ManageRoles = Default + ".ManageRoles";

        public const string ManageUsers = Default + ".ManageUsers";

        public const string ManageUserFavorites = Default + ".ManageUserFavorites";
    }

    // 如果abp后期提供对象存储的目录管理接口,则启用此权限
    /// <summary>
    /// 文件系统
    /// </summary>
    public static class FileSystem
    {
        public const string Default = GroupName + ".FileSystem";

        public const string Create = Default + ".Create";

        public const string Delete = Default + ".Delete";

        public const string Rename = Default + ".Rename";

        public const string Copy = Default + ".Copy";

        public const string Move = Default + ".Move";

        public static class FileManager
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

    public static class Package
    {
        public const string Default = GroupName + ".Package";

        public const string Create = Default + ".Create";

        public const string Delete = Default + ".Delete";

        public const string Update = Default + ".Update";

        public const string ManageBlobs = Default + ".ManageBlobs";
    }

    public static class Feedback
    {
        public const string Default = GroupName + ".Feedback";

        public const string Create = Default + ".Create";

        public const string Update = Default + ".Update";

        public const string Delete = Default + ".Delete";

        public const string ManageComments = Default + ".ManageComments";

        public const string ManageAttachments = Default + ".ManageAttachments";
    }

    public static class Enterprise
    {
        public const string Default = GroupName + ".Enterprise";

        public const string Create = Default + ".Create";

        public const string Delete = Default + ".Delete";

        public const string Update = Default + ".Update";
    }

    public static class EmailMessage
    {
        public const string Default = GroupName + ".EmailMessage";

        public const string Delete = Default + ".Delete";

        public const string SendMessage = Default + ".SendMessage";
    }

    public static class SmsMessage
    {
        public const string Default = GroupName + ".SmsMessage";

        public const string Delete = Default + ".Delete";

        public const string SendMessage = Default + ".SendMessage";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PlatformPermissions));
    }
}
