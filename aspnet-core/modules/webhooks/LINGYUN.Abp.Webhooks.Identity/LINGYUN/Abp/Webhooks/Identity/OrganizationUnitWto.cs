using System;

namespace LINGYUN.Abp.Webhooks.Identity;

[Serializable]
public class OrganizationUnitWto
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string DisplayName { get; set; }
}
