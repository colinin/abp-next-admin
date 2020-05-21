using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantConnectionStringCreateOrUpdateDto
    {
        [Required]
        [StringLength(TenantConnectionStringConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(TenantConnectionStringConsts.MaxValueLength)]
        public string Value { get; set; }
    }
}
