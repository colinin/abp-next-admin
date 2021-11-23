using System;
using System.IO;
using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    public class CreateOssObjectInput
    {
        public string Bucket { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public bool Overwrite { get; set; }

        [DisableAuditing]
        [DisableValidation]
        public IRemoteStreamContent File { get; set; }

        public TimeSpan? ExpirationTime { get; set; }

        public void SetContent(Stream content)
        {
            content.Seek(0, SeekOrigin.Begin);
            File = new RemoteStreamContent(content);
        }
    }
}
