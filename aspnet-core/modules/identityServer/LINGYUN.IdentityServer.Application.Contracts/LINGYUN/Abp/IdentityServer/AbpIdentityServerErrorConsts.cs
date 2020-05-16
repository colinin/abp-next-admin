namespace LINGYUN.Abp.IdentityServer
{
    public class AbpIdentityServerErrorConsts
    {
        /// <summary>
        /// 客户端标识已经存在
        /// </summary>
        public const string ClientIdExisted = "ClientIdExisted";

        /// <summary>
        /// 客户端声明不存在
        /// </summary>
        public const string ClientClaimNotFound = "ClientClaimNotFound";

        /// <summary>
        /// 客户端密钥不存在
        /// </summary>
        public const string ClientSecretNotFound = "ClientSecretNotFound";

        /// <summary>
        /// 客户端属性不存在
        /// </summary>
        public const string ClientPropertyNotFound = "ClientPropertyNotFound";
    }
}
