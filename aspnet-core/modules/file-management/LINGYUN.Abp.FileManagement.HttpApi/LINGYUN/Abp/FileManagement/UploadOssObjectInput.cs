using Microsoft.AspNetCore.Http;

namespace LINGYUN.Abp.FileManagement
{
    public class UploadOssObjectInput 
    {
        public string Bucket { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
