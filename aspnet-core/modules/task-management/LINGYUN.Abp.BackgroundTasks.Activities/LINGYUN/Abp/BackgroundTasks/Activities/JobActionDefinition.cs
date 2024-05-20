using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp.Collections;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

public class JobActionDefinition
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 类型
    /// </summary>
    public JobActionType Type { get; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public ILocalizableString DisplayName { get; }
    /// <summary>
    /// 描述
    /// </summary>
    public ILocalizableString Description { get; }
    /// <summary>
    /// 参数列表
    /// </summary>
    public IList<JobActionParamter> Paramters { get; }
    /// <summary>
    /// 通知提供者
    /// </summary>
    public ITypeList<IJobExecutedProvider> Providers { get; }
    /// <summary>
    /// 额外属性
    /// </summary>
    public Dictionary<string, object> Properties { get; }
    public JobActionDefinition(
        [NotNull] string name,
        [NotNull] JobActionType type,
        [NotNull] ILocalizableString displayName,
        [NotNull] IList<JobActionParamter> paramters,
        ILocalizableString description = null)
    {
        Name = name;
        Type = type;
        DisplayName = displayName;
        Paramters = paramters;
        Description = description;

        Providers = new TypeList<IJobExecutedProvider>();
        Properties = new Dictionary<string, object>();
    }

    public virtual JobActionDefinition WithProvider<TProvider>()
        where TProvider : IJobExecutedProvider
    {
        Providers.Add(typeof(TProvider));

        return this;
    }

    public virtual JobActionDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[{nameof(JobActionDefinition)} {Name}]";
    }
}
