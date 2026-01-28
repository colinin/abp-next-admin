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
    public string? ApiKey { get; private set; }
    /// <summary>
    /// 自定义端点 URL
    /// </summary>
    public string? ApiBaseUrl { get; private set; }
    /// <summary>
    /// 系统提示词
    /// </summary>
    public string? SystemPrompt { get; set; }
    /// <summary>
    /// 附加系统提示词
    /// </summary>
    public string? Instructions { get; set; }
    /// <summary>
    /// 聊天回复时所依据的温度值, 为空时由模型提供者决定默认值
    /// </summary>
    /// <remarks>
    /// 范围在 0 到 2 之间, 数值越高（比如 0.8）会使输出更加随机，而数值越低（比如 0.2）则会使输出更加集中且更具确定性
    /// </remarks>
    public float? Temperature {
        get => _temperature;
        set {
            if (value.HasValue)
            {
                _temperature = Check.Range(value.Value, nameof(value), 0, 2);
            }
            else
            {
                _temperature = value;
            }
        }
    }
    private float? _temperature;
    /// <summary>
    /// 限制一次请求中模型生成 completion 的最大 token 数
    /// </summary>
    public int? MaxOutputTokens { get; set; }
    /// <summary>
    /// 介于 -2.0 和 2.0 之间的数字
    /// </summary>
    /// <remarks>
    /// 如果该值为正，那么新 token 会根据其在已有文本中的出现频率受到相应的惩罚，降低模型重复相同内容的可能性
    /// </remarks>
    public float? FrequencyPenalty {
        get => _frequencyPenalty;
        set {
            if (value.HasValue)
            {
                _frequencyPenalty = Check.Range(value.Value, nameof(value), -2, 2);
            }
            else
            {
                _frequencyPenalty = value;
            }
        }
    }
    private float? _frequencyPenalty;
    /// <summary>
    /// 介于 -2.0 和 2.0 之间的数字
    /// </summary>
    /// <remarks>
    /// 如果该值为正，那么新 token 会根据其是否已在已有文本中出现受到相应的惩罚，从而增加模型谈论新主题的可能性
    /// </remarks>
    public float? PresencePenalty {
        get => _presencePenalty;
        set {
            if (value.HasValue)
            {
                _presencePenalty = Check.Range(value.Value, nameof(value), -2, 2);
            }
            else
            {
                _presencePenalty = value;
            }
        }
    }
    private float? _presencePenalty;
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
        ILocalizableString? description = null,
        string? systemPrompt = null,
        string? instructions = null,
        float? temperature = null,
        int? maxOutputTokens = null,
        float? frequencyPenalty = null,
        float? presencePenalty = null)
    {
        Name = name;
        Provider = provider;
        ModelName = modelName;
        _displayName = displayName;
        _displayName = displayName;
        Description = description;
        SystemPrompt = systemPrompt;
        Instructions = instructions;
        Temperature = temperature;
        MaxOutputTokens = maxOutputTokens;
        FrequencyPenalty = frequencyPenalty;
        PresencePenalty = presencePenalty;

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
