using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.DataProtection.Models;

public class EntityTypeInfoGetModel
{
    [Required]
    public DataAccessOperation Operation { get; set; }
}
