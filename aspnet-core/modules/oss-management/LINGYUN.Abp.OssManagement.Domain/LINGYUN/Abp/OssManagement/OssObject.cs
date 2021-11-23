using System;
using System.Collections.Generic;
using System.IO;

namespace LINGYUN.Abp.OssManagement
{
    /// <summary>
    /// 描述了一个对象的状态信息
    /// </summary>
    public class OssObject
    {
        private Stream _content;

        public bool IsFolder { get; }
        public string Name { get; }
        public string FullName { get; set; }
        public string Prefix { get; }
        public string MD5{ get; }
        public long Size { get; }
        public Stream Content => _content;
        public DateTime? CreationDate { get; }
        public DateTime? LastModifiedDate { get; }
        public IDictionary<string, string> Metadata { get; }
        public OssObject(
           string name,
           string prefix,
           string md5,
           DateTime? creationDate = null,
           long size = 0,
           DateTime? lastModifiedDate = null,
           IDictionary<string, string> metadata = null,
           bool isFolder = false)
        {
            Name = name;
            Prefix = prefix;
            MD5 = md5;
            CreationDate = creationDate;
            LastModifiedDate = lastModifiedDate;
            Size = size;
            IsFolder = isFolder;
            Metadata = metadata ?? new Dictionary<string, string>();
        }

        public void SetContent(Stream stream)
        {
            _content = stream;
            if (!_content.IsNullOrEmpty())
            {
                _content.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
