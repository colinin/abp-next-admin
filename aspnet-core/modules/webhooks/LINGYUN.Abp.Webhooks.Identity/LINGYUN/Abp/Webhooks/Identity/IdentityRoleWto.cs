using System;

namespace LINGYUN.Abp.Webhooks.Identity;

[Serializable]
public class IdentityRoleWto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
