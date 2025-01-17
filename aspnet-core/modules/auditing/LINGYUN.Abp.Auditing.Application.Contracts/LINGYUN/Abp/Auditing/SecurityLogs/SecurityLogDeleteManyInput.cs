using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Auditing.SecurityLogs;
public class SecurityLogDeleteManyInput
{
    [Required]
    public List<Guid> Ids { get; set; }
}
