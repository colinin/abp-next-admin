using System;
using System.Collections.Generic;
using System.Text;

namespace LINGYUN.Abp.FileManagement
{
    public static class FileManagementErrorCodes
    {
        public const string Namespace = "Abp.FileManagement";

        public const string ContainerDeleteWithNotEmpty = Namespace + ":010001";
        public const string ContainerAlreadyExists = Namespace + ":010402";
        public const string ContainerNotFound = Namespace + ":010404";

        public const string ObjectDeleteWithNotEmpty = Namespace + ":020001";
        public const string ObjectAlreadyExists = Namespace + ":020402";
        public const string ObjectNotFound = Namespace + ":020404";
    }
}
