﻿using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.Saas.Tenants;

[Serializable]
public class TenantEto : IHasEntityVersion
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int EntityVersion { get; set; }
}
