using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantConnectionStringCreateOrUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(TenantConnectionStringConsts), nameof(TenantConnectionStringConsts.MaxNameLength))]
        public string Name { get; set; }

        [Required]
        [DynamicStringLength(typeof(TenantConnectionStringConsts), nameof(TenantConnectionStringConsts.MaxValueLength))]
        public string Value { get; set; }
    }
}
