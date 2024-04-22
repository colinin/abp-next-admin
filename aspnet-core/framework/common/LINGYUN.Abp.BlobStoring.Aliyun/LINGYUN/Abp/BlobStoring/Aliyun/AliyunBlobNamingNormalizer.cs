using System.Text.RegularExpressions;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public class AliyunBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
    {
        /// <summary>
        /// 阿里云对象命名规范
        /// https://help.aliyun.com/document_detail/31827.html?spm=a2c4g.11186623.6.607.37b332eaM3NKzY
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public virtual string NormalizeBlobName(string blobName)
        {
            return blobName;
        }

        /// <summary>
        /// 阿里云BucketName命名规范
        /// https://help.aliyun.com/document_detail/31885.html?spm=a2c4g.11186623.6.583.56081c62w6meOR
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public virtual string NormalizeContainerName(string containerName)
        {
            // 小写字母、数字和短划线（-）
            containerName = containerName.ToLower();
            containerName = Regex.Replace(containerName, "[^a-z0-9-]", string.Empty);

            // 不能以短划线（-）开头
            containerName = Regex.Replace(containerName, "^-", string.Empty);
            // 不能以短划线（-）结尾
            containerName = Regex.Replace(containerName, "-$", string.Empty);

            // 长度必须在3-63之间
            if (containerName.Length < 3)
            {
                var length = containerName.Length;
                for (var i = 0; i < 3 - length; i++)
                {
                    containerName += "0";
                }
            }

            if (containerName.Length > 63)
            {
                containerName = containerName.Substring(0, 63);
            }

            return containerName;
        }
    }
}
