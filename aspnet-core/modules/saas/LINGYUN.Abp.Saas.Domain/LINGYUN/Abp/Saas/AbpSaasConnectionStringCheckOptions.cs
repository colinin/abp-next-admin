using LINGYUN.Abp.Saas.Tenants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Saas;

public class AbpSaasConnectionStringCheckOptions
{
    public IDictionary<string, IDataBaseConnectionStringChecker> ConnectionStringCheckers { get; }

    public AbpSaasConnectionStringCheckOptions()
    {
        ConnectionStringCheckers = new Dictionary<string, IDataBaseConnectionStringChecker>(StringComparer.InvariantCultureIgnoreCase);
    }
}
