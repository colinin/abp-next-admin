using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;

public abstract class CreateOrUpdateCalendar
{
    /// <summary>
    /// 日历标题。1 ~ 128 字符
    /// </summary>
    [NotNull]
    [JsonProperty("summary")]
    [JsonPropertyName("summary")]
    public string Summary { get; }
    /// <summary>
    /// 日历在终端上显示的颜色，RGB颜色编码16进制表示，例如："#0000FF" 表示纯蓝色
    /// </summary>
    [NotNull]
    [JsonProperty("color")]
    [JsonPropertyName("color")]
    public string Color { get; }
    /// <summary>
    /// 日历描述。0 ~ 512 字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    /// <summary>
    /// 日历的管理员userid列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("admins")]
    [JsonPropertyName("admins")]
    public string[]? Admins { get; set; }
    /// <summary>
    /// 是否将该日历设置为access_token所对应应用的默认日历。
    /// </summary>
    [NotNull]
    [JsonProperty("set_as_default")]
    [JsonPropertyName("set_as_default")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool SetAsDefault { get; set; }
    /// <summary>
    /// 是否公共日历。0-否；1-是
    /// </summary>
    [NotNull]
    [JsonProperty("is_public")]
    [JsonPropertyName("is_public")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsPublic { get; private set; }
    /// <summary>
    /// 公开范围。仅当是公共日历时有效
    /// </summary>
    [CanBeNull]
    [JsonProperty("public_range")]
    [JsonPropertyName("public_range")]
    public CalendarPublicRange? PublicRange { get; private set; }
    /// <summary>
    /// 是否全员日历。0-否；1-是
    /// </summary>
    [NotNull]
    [JsonProperty("is_corp_calendar")]
    [JsonPropertyName("is_corp_calendar")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsCorpCalendar { get; private set; }
    /// <summary>
    /// 日历通知范围成员列表。最多2000人
    /// </summary>
    [CanBeNull]
    [JsonProperty("shares")]
    [JsonPropertyName("shares")]
    public CalendarShare[]? Shares { get; private set; }
    protected CreateOrUpdateCalendar(string summary, string color, string? description)
    {
        Summary = Check.NotNullOrWhiteSpace(summary, nameof(summary), 128, 1);
        Color = Check.NotNullOrWhiteSpace(color, nameof(color));
        Description = Check.Length(description, nameof(description), 512);
    }
    /// <summary>
    /// 设置公开范围
    /// </summary>
    /// <param name="publicRange"></param>
    public void WithPublic(CalendarPublicRange publicRange)
    {
        IsPublic = true;
        PublicRange = publicRange;
    }
    /// <summary>
    /// 设置全员日历
    /// </summary>
    /// <param name="shares"></param>
    public void WithCorpCalendar(CalendarShare[] shares)
    {
        IsCorpCalendar = true;
        Shares = shares;
    }
    /// <summary>
    /// 验证输入
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Validate()
    {
        Check.Length(Description, nameof(Description), 512);

        if (IsPublic && PublicRange == null)
        {
            throw new ArgumentException("The public range of the public calendar cannot be null!", nameof(PublicRange));
        }
        if (IsCorpCalendar)
        {
            if (Shares == null)
            {
                throw new ArgumentException("The shares of the corp calendar cannot be null!", nameof(IsCorpCalendar));
            }
            if (Shares.Length > 2000)
            {
                throw new ArgumentException("Calendar shares member list. Up to 2,000!", nameof(Shares));
            }
        }
    }
}
