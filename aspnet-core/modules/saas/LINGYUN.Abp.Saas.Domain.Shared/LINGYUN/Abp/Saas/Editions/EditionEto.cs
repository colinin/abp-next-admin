using System;

namespace LINGYUN.Abp.Saas.Editions;

[Serializable]
public class EditionEto
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; }
}
