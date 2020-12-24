using System.Collections.Generic;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceCreateOrUpdateDto
    {
        [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.DisplayNameMaxLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public string AllowedAccessTokenSigningAlgorithms { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<ApiResourceSecretCreateOrUpdateDto> Secrets { get; set; }

        public List<ApiResourceScopeDto> Scopes { get; set; }

        public List<ApiResourceClaimDto> UserClaims { get; set; }

        public List<ApiResourcePropertyDto> Properties { get; set; }

        protected ApiResourceCreateOrUpdateDto()
        {
            UserClaims = new List<ApiResourceClaimDto>();
            Scopes = new List<ApiResourceScopeDto>();
            Secrets = new List<ApiResourceSecretCreateOrUpdateDto>();
            Properties = new List<ApiResourcePropertyDto>();
        }
    }
}
