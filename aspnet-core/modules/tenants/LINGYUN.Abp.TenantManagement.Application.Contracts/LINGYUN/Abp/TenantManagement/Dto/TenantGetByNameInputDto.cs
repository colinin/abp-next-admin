using System.ComponentModel.DataAnnotations;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantGetByNameInputDto
    {
        [Required]
        [DynamicStringLength(typeof(TenantConsts), nameof(TenantConsts.MaxNameLength))]
        public string Name { get; set; }

        public TenantGetByNameInputDto() { }
        public TenantGetByNameInputDto(string name)
        {
            Name = name;
        }
    }
}
