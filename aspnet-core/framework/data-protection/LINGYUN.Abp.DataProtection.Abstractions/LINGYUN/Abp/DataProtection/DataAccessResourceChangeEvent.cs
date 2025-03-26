using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.DataProtection;

[Serializable]
[EventName("abp.data_protection.resource_changed")]
public class DataAccessResourceChangeEvent
{
    public DataAccessResource Resource { get; set; }
    public DataAccessResourceChangeEvent()
    {

    }

    public DataAccessResourceChangeEvent(DataAccessResource resource)
    {
        Resource = resource;
    }
}
