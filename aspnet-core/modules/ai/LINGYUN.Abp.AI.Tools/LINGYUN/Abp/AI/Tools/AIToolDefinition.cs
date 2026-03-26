using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI.Tools;

public class AIToolDefinition : IHasSimpleStateCheckers<AIToolDefinition>
{
    /// <summary>
    /// 名称
    /// </summary>
    [NotNull]
    public string Name { get; }
    /// <summary>
    /// 提供者
    /// </summary>
    [NotNull]
    public string Provider { get; }
    /// <summary>
    /// 描述
    /// </summary>
    public ILocalizableString? Description { get; set; }
    /// <summary>
    /// 属性
    /// </summary>
    [NotNull]
    public Dictionary<string, object?> Properties { get; }

    public List<ISimpleStateChecker<AIToolDefinition>> StateCheckers { get; }

    public AIToolDefinition(
        [NotNull] string name,
        [NotNull] string provider, 
        ILocalizableString? description = null)
    {
        Name = name;
        Provider = provider;
        Description = description;

        Properties = new Dictionary<string, object?>();
        StateCheckers = new List<ISimpleStateChecker<AIToolDefinition>>();
    }

    public virtual AIToolDefinition WithProperty(string key, object? value)
    {
        Properties[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[{nameof(AIToolDefinition)} {Name}]";
    }
}
