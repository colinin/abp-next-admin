using System.Collections.Generic;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement;

public class GetPermissionGrantedWithProviderListResultDto
{
    public List<ProviderInfoDto> GrantedProviders { get; set; } = new List<ProviderInfoDto>();
}
