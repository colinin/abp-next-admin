using System.ComponentModel.DataAnnotations;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantGetByNameInputDto
    {
        [Required]
        [StringLength(TenantConsts.MaxNameLength)]
        public string Name { get; set; }

        public TenantGetByNameInputDto() { }
        public TenantGetByNameInputDto(string name)
        {
            Name = name;
        }
    }
}
