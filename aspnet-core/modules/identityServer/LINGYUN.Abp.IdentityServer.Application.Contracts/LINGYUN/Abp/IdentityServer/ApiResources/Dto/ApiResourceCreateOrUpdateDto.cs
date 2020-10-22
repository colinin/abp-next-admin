using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiResourceCreateOrUpdateDto
    {
        [StringLength(ApiResourceConsts.DisplayNameMaxLength)]
        public string DisplayName { get; set; }

        [StringLength(ApiResourceConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public List<string> UserClaims { get; set; }

        public List<ApiScopeDto> Scopes { get; set; }

        public List<ApiSecretCreateOrUpdateDto> Secrets { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        protected ApiResourceCreateOrUpdateDto()
        {
            UserClaims = new List<string>();
            Scopes = new List<ApiScopeDto>();
            Secrets = new List<ApiSecretCreateOrUpdateDto>();
            Properties = new Dictionary<string, string>();
        }
    }
}
