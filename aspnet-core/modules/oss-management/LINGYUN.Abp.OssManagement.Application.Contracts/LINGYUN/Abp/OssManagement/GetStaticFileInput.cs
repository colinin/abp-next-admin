using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.OssManagement
{
    public class GetStaticFileInput
    {
        [Required]
        public string Name { get; set; }

        public string Path { get; set; }

        public string Bucket { get; set; }

        public string Process { get; set; }
        /// <summary>
        /// 解决通过路由传递租户标识时,abp写入cookies
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
