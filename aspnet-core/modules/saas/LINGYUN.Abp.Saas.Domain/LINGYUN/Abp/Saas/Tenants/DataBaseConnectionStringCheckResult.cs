using System;
using Volo.Abp.Data;

namespace LINGYUN.Abp.Saas.Tenants;

public class DataBaseConnectionStringCheckResult : AbpConnectionStringCheckResult
{
    public Exception Error { get; set; }
}
