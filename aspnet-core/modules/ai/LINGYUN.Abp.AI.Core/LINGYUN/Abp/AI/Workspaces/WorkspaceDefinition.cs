using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI.Workspaces;
/// <summary>
/// 工作区定义
/// </summary>
public class WorkspaceDefinition : IHasSimpleStateCheckers<WorkspaceDefinition>
{
    /// <summary>
    /// 名称
    /// </summary>
    [NotNull]
    public string Name { get; }
    /// <summary>
    /// AI提供者名称
    /// </summary>
    [NotNull]
    public string Provider { get; }
    /// <summary>
    /// 模型名称
    /// </summary>
    [NotNull]
    public string ModelName { get; }
    /// <summary>
    /// 显示名称
    /// </summary>
    [NotNull]
    public ILocalizableString DisplayName {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }
    private ILocalizableString _displayName = default!;
    /// <summary>
    /// 描述
    /// </summary>
    public ILocalizableString? Description { get; set; }
    /// <summary>
    /// API 身份验证密钥
    /// </summary>
    public string? ApiKey { get; set; }
    /// <summary>
    /// 自定义端点 URL
    /// </summary>
    public string? ApiBaseUrl { get; set; }
    /// <summary>
    /// 系统提示词
    /// </summary>
    public string? SystemPrompt { get; set; }
    /// <summary>
    /// 启用/禁用工作区
    /// </summary>
    public bool IsEnabled { get; set; }

    [NotNull]
    public Dictionary<string, object> Properties { get; }

    public List<ISimpleStateChecker<WorkspaceDefinition>> StateCheckers { get; }

    public WorkspaceDefinition(
        string name, 
        string provider,
        string modelName, 
        ILocalizableString displayName, 
        ILocalizableString? description = null)
    {
        Name = name;
        Provider = provider;
        ModelName = modelName;
        _displayName = displayName;
        _displayName = displayName;
        Description = description;

        IsEnabled = true;
        Properties = new Dictionary<string, object>();
        StateCheckers = new List<ISimpleStateChecker<WorkspaceDefinition>>();
    }

    public virtual WorkspaceDefinition WithApiBaseUrl(string apiBaseUrl)
    {
        ApiBaseUrl = apiBaseUrl;
        return this;
    }

    public virtual WorkspaceDefinition WithApiKey(string apiKey)
    {
        ApiKey = apiKey;
        return this;
    }

    public virtual WorkspaceDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[{nameof(WorkspaceDefinition)} {Name}]";
    }
}
