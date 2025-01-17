using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Auditing.AuditLogs;
public class AuditLogDeleteManyInput
{
    [Required]
    public List<Guid> Ids { get; set; }
}
