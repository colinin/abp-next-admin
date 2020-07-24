using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Versions
{
    /// <summary>
    /// 应用版本号
    /// </summary>
    public class AppVersion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; protected set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public virtual string Version { get; protected set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 重要级别
        /// </summary>
        public virtual ImportantLevel Level { get; set; }
        /// <summary>
        /// 版本文件列表
        /// </summary>
        public virtual ICollection<VersionFile> Files { get; protected set; }

        protected AppVersion()
        {
            Files = new List<VersionFile>();
        }

        public AppVersion(Guid id, string title, string version, Guid? tenantId = null)
        {
            Id = id;
            Title = title;
            Version = version;
            TenantId = tenantId;
            Level = ImportantLevel.Low;
        }

        public void AppendFile(string name, string version, long size, string sha256, FileType fileType = FileType.Stream)
        {
            if (!FileExists(name))
            {
                Files.Add(new VersionFile(name, version, size, sha256, fileType, TenantId));
            }
        }

        public void RemoveFile(string name)
        {
            Files.RemoveAll(x => x.Name.Equals(name));
        }

        public void RemoveAllFile()
        {
            Files.Clear();
        }

        public void ChangeFileVersion(string name, string version, long size, string sha256)
        {
            if (FileExists(name))
            {
                var file = FindFile(name);
                file.ChangeVersion(version, size, sha256);
            }
        }

        public VersionFile FindFile(string name)
        {
            return Files.Where(x => x.Name.Equals(name)).FirstOrDefault();
        }

        public bool FileExists(string name)
        {
            // TODO: Windows file system ?
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    return Files.Any(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            //}
            return Files.Any(x => x.Name.Equals(name));
        }
    }
}
