using System.Text.RegularExpressions;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.Tencent
{
    public class TencentBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
    {
        /// <summary>
        /// 腾讯云对象命名规范
        /// https://cloud.tencent.com/document/product/436/13324
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public virtual string NormalizeBlobName(string blobName)
        {
            // 不允许以正斜线/或者反斜线\开头。
            blobName = Regex.Replace(blobName, "^/", string.Empty);
            blobName = blobName.StartsWith("\\") ? blobName.Substring(1) : blobName;

            // 对象键中不支持 ASCII 控制字符中的
            // 字符上(↑)，字符下(↓)，字符右(→)，字符左(←)，
            // 分别对应 CAN(24)，EM(25)，SUB(26)，ESC(27)。
            blobName = blobName.Replace("↑", "");
            blobName = blobName.Replace("↓", "");
            blobName = blobName.Replace("←", "");
            blobName = blobName.Replace("→", "");
            
            // TODO: 要求还真多...其他暂时不写了

            return blobName;
        }

        /// <summary>
        /// 腾讯云BucketName命名规范
        /// https://cloud.tencent.com/document/product/436/13312
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public virtual string NormalizeContainerName(string containerName)
        {
            // 仅支持小写英文字母和数字，即[a-z，0-9]、中划线“-”及其组合。
            containerName = containerName.ToLower();
            containerName = Regex.Replace(containerName, "[^a-z0-9-]", string.Empty);

            // 不能以短划线（-）开头
            containerName = Regex.Replace(containerName, "^-", string.Empty);
            // 不能以短划线（-）结尾
            containerName = Regex.Replace(containerName, "-$", string.Empty);

            // 存储桶名称的最大允许字符受到 地域简称 和 APPID 的字符数影响，组成的完整请求域名字符数总计最多60个字符。
            // 例如请求域名123456789012345678901-1250000000.cos.ap-beijing.myqcloud.com总和为60个字符。
            if (containerName.Length > 60)
            {
                containerName = containerName.Substring(0, 60);
            }

            return containerName;
        }
    }
}
