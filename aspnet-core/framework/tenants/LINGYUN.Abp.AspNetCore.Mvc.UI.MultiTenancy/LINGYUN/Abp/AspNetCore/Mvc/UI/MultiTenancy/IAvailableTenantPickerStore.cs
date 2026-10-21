using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AspNetCore.Mvc.UI.MultiTenancy;

public interface IAvailableTenantPickerStore
{
    Task<List<NameValue<string>>> GetAvailableTenantsAsync();
}

