using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WebhooksManagement;
public class WebhookSendRecordResendManyInput
{
    public List<Guid> RecordIds { get; set; } = new List<Guid>();
}
