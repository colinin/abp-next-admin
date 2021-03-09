using System;
using System.IO;

namespace LINGYUN.Abp.FileManagement
{
    public class CreateOssObjectInput
    {
        public string Bucket { get; set; }
        public string Path { get; set; }
        public string Object { get; set; }
        public Stream Content { get; set; }
        public TimeSpan? ExpirationTime { get; set; }

        public void SetContent(Stream content)
        {
            Content = content;
        }
    }
}
