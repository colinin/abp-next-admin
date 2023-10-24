namespace LINGYUN.Abp.AspNetCore.Mvc.Client
{
    public class AbpAspNetCoreMvcClientCacheOptions
    {
        /// <summary>
        /// 用户缓存过期时间, 单位: 秒
        /// 默认: 300
        /// </summary>
        public int UserCacheExpirationSeconds { get; set; } = 300;
        /// <summary>
        /// 匿名用户缓存过期时间, 单位: 秒
        /// 默认: 300
        /// </summary>
        public int AnonymousCacheExpirationSeconds { get; set; } = 300;
    }
}
