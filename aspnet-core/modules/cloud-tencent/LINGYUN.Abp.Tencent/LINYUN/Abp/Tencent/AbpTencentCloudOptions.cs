using System;
using System.Collections.Generic;
using TencentCloud.Common;
using TencentCloud.Common.Profile;

namespace LINGYUN.Abp.Tencent;

public class AbpTencentCloudOptions
{
    public IDictionary<Type, Func<Credential, string, ClientProfile, AbstractClient>> ClientProxies { get; }

    public AbpTencentCloudOptions()
    {
        ClientProxies = new Dictionary<Type, Func<Credential, string, ClientProfile, AbstractClient>>();
    }
}
