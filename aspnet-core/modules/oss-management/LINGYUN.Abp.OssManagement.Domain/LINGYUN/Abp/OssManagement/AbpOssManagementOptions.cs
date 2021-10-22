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
                "public",
                "users",
                "system",
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
