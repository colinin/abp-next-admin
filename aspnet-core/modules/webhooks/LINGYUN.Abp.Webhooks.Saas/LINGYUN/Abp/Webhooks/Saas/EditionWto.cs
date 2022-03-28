using System;

namespace LINGYUN.Abp.Webhooks.Saas;

[Serializable]
public class EditionWto
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; }
}
