using System;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MultiTenancy.DbFinder
{
    [Serializable]
    [IgnoreMultiTenancy]
    public class TenantConfigurationCacheItem
    {
        protected const string FormatKey = "pn:{0},k:{1}";
        public Guid Id { get; set; }
        public string Name { get; set; }
        // TODO: 是否需要加密存储?
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();

        public TenantConfigurationCacheItem() { }

        public TenantConfigurationCacheItem(Guid id, string name, ConnectionStrings connectionStrings)
        {
            Id = id;
            Name = name;
            ConnectionStrings = connectionStrings ?? ConnectionStrings;
        }
        public static string CalculateCacheKey(string key)
        {
            return string.Format(FormatKey, "tenant", key);
        }
    }
}
