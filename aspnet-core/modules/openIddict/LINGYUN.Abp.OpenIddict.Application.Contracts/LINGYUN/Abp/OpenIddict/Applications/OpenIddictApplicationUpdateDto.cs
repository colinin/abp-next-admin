using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.OpenIddict.Applications;

[Serializable]
public class OpenIddictApplicationUpdateDto : OpenIddictApplicationCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
