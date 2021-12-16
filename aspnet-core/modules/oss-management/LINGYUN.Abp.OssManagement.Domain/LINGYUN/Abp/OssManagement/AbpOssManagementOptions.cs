using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.OssManagement
{
    public class AbpOssManagementOptions
    {
        /// <summary>
        /// 静态容器
        /// 不允许被删除
        /// </summary>
        public List<string> StaticBuckets { get; }

        public AbpOssManagementOptions()
        {
            StaticBuckets = new List<string>
            {
                // 公共目录
                "public",
                // 用户私有目录
                "users",
                // 系统目录
                "system",
                // 工作流
                "workflow",
                // 图标
                "icons"
            };
        }

        public void AddStaticBucket(string bucket)
        {
            Check.NotNullOrWhiteSpace(bucket, nameof(bucket));

            StaticBuckets.AddIfNotContains(bucket);
        }

        public bool CheckStaticBucket(string bucket)
        {
            return StaticBuckets.Any(bck => bck.Equals(bucket));
        }
    }
}
