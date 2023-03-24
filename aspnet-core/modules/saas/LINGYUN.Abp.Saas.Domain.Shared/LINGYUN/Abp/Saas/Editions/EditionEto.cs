using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.Saas.Editions;

[Serializable]
public class EditionEto : IHasEntityVersion
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; }

    public int EntityVersion { get; set; }
}
