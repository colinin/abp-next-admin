using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantDto : ExtensibleEntityDto<Guid>
    {
        public string Name { get; set; }
    }
}