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
        /// 适应平台
        /// </summary>
        public virtual PlatformType PlatformType { get; protected set; }
        /// <summary>
        /// 版本文件列表
        /// </summary>
        public virtual ICollection<VersionFile> Files { get; protected set; }

        protected AppVersion()
        {
            Files = new List<VersionFile>();
        }

        public AppVersion(Guid id, string title, string version, PlatformType platformType = PlatformType.None, Guid? tenantId = null)
        {
            Id = id;
            Title = title;
            Version = version;
            TenantId = tenantId;
            PlatformType = platformType;
            Level = ImportantLevel.Low;
        }

        public void AppendFile(string name, string version, long size, string sha256, 
            string filePath = "", FileType fileType = FileType.Stream)
        {
            if (!FileExists(name))
            {
                var versionFile = new VersionFile(name, version, size, sha256, fileType, TenantId)
                {
                    Path = filePath
                };
                Files.Add(versionFile);
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

        public VersionFile FindFile(string path, string name)
        {
            return Files.Where(x => x.Path.Equals(path) && x.Name.Equals(name)).FirstOrDefault();
        }

        public VersionFile FindFile(string path, string name, string version)
        {
            return Files.Where(x => x.Path.Equals(path) && x.Name.Equals(name) && x.Version.Equals(version))
                .FirstOrDefault();
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
