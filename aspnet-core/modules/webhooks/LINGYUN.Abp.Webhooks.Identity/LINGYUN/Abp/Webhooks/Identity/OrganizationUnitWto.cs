using System;

namespace LINGYUN.Abp.Webhooks.Identity;

public class OrganizationUnitWto
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string DisplayName { get; set; }
}
