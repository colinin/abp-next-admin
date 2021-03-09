using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.FileManagement
{
    public class OssObjectDto
    {
        public bool IsFolder { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
