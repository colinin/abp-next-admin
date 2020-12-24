using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientCloneDto
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        [Required]
        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientIdMaxLength))]
        public string ClientId { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        [Required]
        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientNameMaxLength))]
        public string ClientName { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.DescriptionMaxLength))]
        public string Description { get; set; }
        /// <summary>
        /// 复制客户端授权类型
        /// </summary>
        public bool CopyAllowedGrantType { get; set; }
        /// <summary>
        /// 复制客户端重定向 Uri
        /// </summary>
        public bool CopyRedirectUri { get; set; }
        /// <summary>
        /// 复制客户端作用域
        /// </summary>
        public bool CopyAllowedScope { get; set; }
        /// <summary>
        /// 复制客户端声明
        /// </summary>
        public bool CopyClaim { get; set; }
        /// <summary>
        /// 复制客户端密钥
        /// </summary>
        public bool CopySecret { get; set; }
        /// <summary>
        /// 复制客户端跨域来源
        /// </summary>
        public bool CopyAllowedCorsOrigin { get; set; }
        /// <summary>
        /// 复制客户端注销重定向 Uri
        /// </summary>
        public bool CopyPostLogoutRedirectUri { get; set; }
        /// <summary>
        /// 复制客户端属性
        /// </summary>
        public bool CopyPropertie { get; set; }
        /// <summary>
        /// 复制客户端 IdP 限制
        /// </summary>
        public bool CopyIdentityProviderRestriction { get; set; }
        public ClientCloneDto()
        {
            CopyAllowedCorsOrigin = true;
            CopyAllowedGrantType = true;
            CopyAllowedScope = true;
            CopyClaim = true;
            CopyIdentityProviderRestriction = true;
            CopyPostLogoutRedirectUri = true;
            CopyPropertie = true;
            CopyRedirectUri = true;
            CopySecret = true;
        }
    }
}
