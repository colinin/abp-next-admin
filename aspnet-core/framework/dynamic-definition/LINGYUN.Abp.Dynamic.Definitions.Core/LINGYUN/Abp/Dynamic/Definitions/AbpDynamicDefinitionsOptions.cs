using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Dynamic.Definitions;

public class AbpDynamicDefinitionsOptions
{
    public DynamicDefinitionStrategy DefaultStrategy { get; set; }
    public Dictionary<Type, DynamicDefinitionStrategy> DefinitionStrategy { get; set; }
    public AbpDynamicDefinitionsOptions()
    {
        DefaultStrategy = DynamicDefinitionStrategy.Static;
        DefinitionStrategy = new Dictionary<Type, DynamicDefinitionStrategy>();
    }

    public void MapStrategy<T>(DynamicDefinitionStrategy strategy = DynamicDefinitionStrategy.Static)
    {
        DefinitionStrategy[typeof(T)] = strategy;
    }

    public DynamicDefinitionStrategy GetStrategy<T>()
    {
        return GetStrategy(typeof(T));
    }

    public DynamicDefinitionStrategy GetStrategy(Type type)
    {
        if (DefinitionStrategy.TryGetValue(type, out var strategy))
        {
            return strategy;
        }

        return DefaultStrategy;
    }
}
