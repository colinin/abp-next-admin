using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Cli.UI;
public class AbpCliViewGeneratorOptions
{
    public IDictionary<string, Type> Generators { get; }

    public AbpCliViewGeneratorOptions()
    {
        Generators = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
    }
}
