using System;
using Volo.Abp.Data;

namespace LINGYUN.Abp.MultiTenancy.DbFinder
{
    public class TenantConfigurationCacheItem
    {
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
            return "p:tenant" + ",k:" + key;
        }
    }
}
