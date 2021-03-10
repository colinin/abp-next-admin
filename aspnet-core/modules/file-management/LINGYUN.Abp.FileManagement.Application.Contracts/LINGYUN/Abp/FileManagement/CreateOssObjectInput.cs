using System;
using System.IO;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FileManagement
{
    public class CreateOssObjectInput
    {
        public string Bucket { get; set; }
        public string Path { get; set; }
        public string Object { get; set; }

        [DisableAuditing]
        [DisableValidation]
        public Stream Content { get; set; }
        public TimeSpan? ExpirationTime { get; set; }

        public void SetContent(Stream content)
        {
            Content = content;
            Content?.Seek(0, SeekOrigin.Begin);
        }
    }
}
