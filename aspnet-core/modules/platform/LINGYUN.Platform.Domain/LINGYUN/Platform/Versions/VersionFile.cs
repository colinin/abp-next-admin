using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.IO;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Versions
{
    public class VersionFile : AuditedEntity<int>, IMultiTenant
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// 文件版本
        /// </summary>
        public virtual string Version { get; protected set; }
        /// <summary>
        /// 文件大小
        /// 单位b
        /// </summary>
        public virtual long Size { get; protected set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual FileType FileType { get; protected set; }
        /// <summary>
        /// 文件SHA256编码
        /// </summary>
        public virtual string SHA256 { get; protected set; }
        /// <summary>
        /// 下载次数
        /// </summary>
        public virtual int DownloadCount { get; protected set; }
        /// <summary>
        /// 应用版本标识
        /// </summary>
        public virtual Guid AppVersionId { get; protected set; }
        /// <summary>
        /// 所属应用版本号
        /// </summary>
        public virtual AppVersion AppVersion { get; protected set; }
        protected VersionFile()
        {

        }

        public VersionFile(string name, string version, long size, string sha256, FileType fileType = FileType.Stream, Guid? tenantId = null)
        {
            Name = name;
            FileType = fileType;
            TenantId = tenantId;
            ChangeVersion(version, size, sha256);
        }

        public void ChangeVersion(string version, long size, string sha256)
        {
            Size = size;
            SHA256 = sha256;
            Version = version;
        }

        public void Download()
        {
            DownloadCount += 1;
        }

        public static string NormalizeBlobName(string appVersion, string fileName, string fileVersion)
        {
            var fileNameWithNotExten = fileName;
            // 取出文件扩展名
            var fileExten = FileHelper.GetExtension(fileName);
            if (!fileExten.IsNullOrWhiteSpace())
            {
                // 取出不带扩展名的文件名
                fileNameWithNotExten = fileName.Replace(fileExten, "");
                // 去掉最后一位扩展名符号
                fileNameWithNotExten = fileNameWithNotExten.Remove(fileNameWithNotExten.Length - 1);
            }
            // 转换不受支持的符号
            fileNameWithNotExten = fileNameWithNotExten.Replace(".", "-");
            // 最终文件名为 应用版本号/文件名(不带扩展名)/文件版本/文件名
            // 例: 1.0.0.0/test-upload-text-file/1.0.0.0/test-upload-text-file.text
            return $"{appVersion}/{fileNameWithNotExten}/{fileVersion}/{fileName}";
        }
    }
}
