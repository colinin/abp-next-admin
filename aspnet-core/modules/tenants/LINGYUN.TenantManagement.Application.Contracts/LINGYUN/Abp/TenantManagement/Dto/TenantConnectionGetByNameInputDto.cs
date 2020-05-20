using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantConnectionGetByNameInputDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(TenantConnectionStringConsts.MaxNameLength)]
        public string Name { get; set; }
    }
}
