using System;

namespace LINGYUN.Abp.FileManagement
{
    public class FileSystemDto
    {
        public FileSystemType Type { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public long? Size { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
