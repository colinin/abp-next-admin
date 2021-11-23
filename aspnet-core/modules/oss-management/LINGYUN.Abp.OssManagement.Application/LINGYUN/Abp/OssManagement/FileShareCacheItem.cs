using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.OssManagement
{
    [Serializable]
    public class MyFileShareCacheItem
    {
        private const string CacheKeyFormat = "ui:{0}";

        public List<FileShareCacheItem> Items { get; set; }
        public MyFileShareCacheItem() { }
        public MyFileShareCacheItem(List<FileShareCacheItem> items)
        {
            Items = items ?? new List<FileShareCacheItem>();
        }

        public static string CalculateCacheKey(Guid userId)
        {
            return string.Format(CacheKeyFormat, userId.ToString("N"));
        }

        public DateTime? GetLastExpirationTime()
        {
            if (!Items.Any())
            {
                return null;
            }

            return Items
                .OrderByDescending(item => item.ExpirationTime)
                .Select(item => item.ExpirationTime)
                .FirstOrDefault();
        }
    }

    [Serializable]
    public class FileShareCacheItem
    {
        private const string CacheKeyFormat = "url:{0}";

        public string Bucket { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string[] Roles { get; set; }

        public string[] Users { get; set; }

        public string MD5 { get; set; }

        public string Url { get; set; }

        public int AccessCount { get; set; }

        public int MaxAccessCount { get; set; }

        public DateTime ExpirationTime { get; set; }

        public Guid UserId { get; set; }

        public FileShareCacheItem() { }

        public FileShareCacheItem(
            Guid userId,
            string bucket,
            string name,
            string path,
            string md5,
            string url,
            DateTime expirationTime,
            string[] roles = null,
            string[] users = null,
            int maxAccessCount = 0)
        {
            UserId = userId;
            Bucket = bucket;
            Name = name;
            Path = path;
            MD5 = md5;
            Url = url;
            ExpirationTime = expirationTime;
            Roles = roles ?? new string[0];
            Users = users ?? new string[0];
            MaxAccessCount = maxAccessCount;
        }

        public static string CalculateCacheKey(string shareUrl)
        {
            return string.Format(CacheKeyFormat, shareUrl);
        }
    }
}
