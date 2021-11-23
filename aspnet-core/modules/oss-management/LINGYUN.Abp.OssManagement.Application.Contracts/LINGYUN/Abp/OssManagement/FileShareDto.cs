using System;

namespace LINGYUN.Abp.OssManagement
{
    public class FileShareDto
    {
        public string Url { get; set; }
        public int MaxAccessCount { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }

    public class MyFileShareDto
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string[] Roles { get; set; }

        public string[] Users { get; set; }

        public string MD5 { get; set; }

        public string Url { get; set; }

        public int AccessCount { get; set; }

        public int MaxAccessCount { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
