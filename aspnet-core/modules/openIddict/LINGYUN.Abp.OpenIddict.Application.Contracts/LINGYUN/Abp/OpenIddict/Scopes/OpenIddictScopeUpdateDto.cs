using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.OpenIddict.Scopes;

[Serializable]
public class OpenIddictScopeUpdateDto : OpenIddictScopeCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
