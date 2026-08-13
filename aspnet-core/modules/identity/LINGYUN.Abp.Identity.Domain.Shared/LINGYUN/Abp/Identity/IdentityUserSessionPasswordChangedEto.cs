using System;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

[Serializable]
public class IdentityUserSessionPasswordChangedEto : IdentityUserPasswordChangedEto
{
    public string SessionId { get; set; }
}
