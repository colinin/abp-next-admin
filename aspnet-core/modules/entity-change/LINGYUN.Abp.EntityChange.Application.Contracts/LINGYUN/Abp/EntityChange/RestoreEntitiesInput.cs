using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.EntityChange;

public class RestoreEntitiesInput
{
    [Required]
    public List<RestoreEntityInput> Entities { get; set; }
}
