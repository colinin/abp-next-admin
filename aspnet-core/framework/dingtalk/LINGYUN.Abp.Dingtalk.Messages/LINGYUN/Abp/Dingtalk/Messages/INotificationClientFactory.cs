using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Dingtalk.Messages;

public interface INotificationClientFactory
{
    Task<AlibabaCloud.SDK.Dingtalkim_2_0.Client>
}
