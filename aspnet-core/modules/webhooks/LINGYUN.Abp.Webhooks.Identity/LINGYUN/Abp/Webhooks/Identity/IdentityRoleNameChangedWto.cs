using System;

namespace LINGYUN.Abp.Webhooks.Identity;

public class IdentityRoleNameChangedWto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string OldName { get; set; }
}
