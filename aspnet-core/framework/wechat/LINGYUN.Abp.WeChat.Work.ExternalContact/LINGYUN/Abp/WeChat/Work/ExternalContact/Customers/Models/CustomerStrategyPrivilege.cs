using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 规则组权限
/// </summary>
public class CustomerStrategyPrivilege
{
    /// <summary>
    /// 查看客户列表，基础权限，不可取消
    /// </summary>
    [NotNull]
    [JsonProperty("view_customer_list")]
    [JsonPropertyName("view_customer_list")]
    public bool ViewCustomerList { get; set; }
    /// <summary>
    /// 查看客户统计数据，基础权限，不可取消
    /// </summary>
    [NotNull]
    [JsonProperty("view_customer_data")]
    [JsonPropertyName("view_customer_data")]
    public bool ViewCustomerData { get; set; }
    /// <summary>
    /// 查看群聊列表，基础权限，不可取消
    /// </summary>
    [NotNull]
    [JsonProperty("view_room_list")]
    [JsonPropertyName("view_room_list")]
    public bool ViewRoomList { get; set; }
    /// <summary>
    /// 可使用联系我，基础权限，不可取消
    /// </summary>
    [NotNull]
    [JsonProperty("contact_me")]
    [JsonPropertyName("contact_me")]
    public bool ContactMe { get; set; }
    /// <summary>
    /// 可加入群聊，基础权限，不可取消
    /// </summary>
    [NotNull]
    [JsonProperty("join_room")]
    [JsonPropertyName("join_room")]
    public bool JoinRoom { get; set; }
    /// <summary>
    /// 允许分享客户给其他成员，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("share_customer")]
    [JsonPropertyName("share_customer")]
    public bool ShareCustomer { get; set; }
    /// <summary>
    /// 允许分配离职成员客户，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("oper_resign_customer")]
    [JsonPropertyName("oper_resign_customer")]
    public bool OperResignCustomer { get; set; }
    /// <summary>
    /// 允许分配离职成员客户群，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("oper_resign_group")]
    [JsonPropertyName("oper_resign_group")]
    public bool OperResignGroup { get; set; }
    /// <summary>
    /// 允许给企业客户发送消息，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("send_customer_msg")]
    [JsonPropertyName("send_customer_msg")]
    public bool SendCustomerMsg { get; set; }
    /// <summary>
    /// 允许配置欢迎语，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("edit_welcome_msg")]
    [JsonPropertyName("edit_welcome_msg")]
    public bool EditWelcomeMsg { get; set; }
    /// <summary>
    /// 允许查看成员联系客户统计，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("view_behavior_data")]
    [JsonPropertyName("view_behavior_data")]
    public bool ViewBehaviorData { get; set; }
    /// <summary>
    /// 	允许查看群聊数据统计，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("view_room_data")]
    [JsonPropertyName("view_room_data")]
    public bool ViewRoomData { get; set; }
    /// <summary>
    /// 允许发送消息到企业的客户群，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("send_group_msg")]
    [JsonPropertyName("send_group_msg")]
    public bool SendGroupMsg { get; set; }
    /// <summary>
    /// 允许对企业客户群进行去重，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("room_deduplication")]
    [JsonPropertyName("room_deduplication")]
    public bool RoomDeduplication { get; set; }
    /// <summary>
    /// 配置快捷回复，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("rapid_reply")]
    [JsonPropertyName("rapid_reply")]
    public bool RapidReply { get; set; }
    /// <summary>
    /// 转接在职成员的客户，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("onjob_customer_transfer")]
    [JsonPropertyName("onjob_customer_transfer")]
    public bool OnjobCustomerTransfer { get; set; }
    /// <summary>
    /// 编辑企业成员防骚扰规则，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("edit_anti_spam_rule")]
    [JsonPropertyName("edit_anti_spam_rule")]
    public bool EditAntiSpamRule { get; set; }
    /// <summary>
    /// 导出客户列表，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("export_customer_list")]
    [JsonPropertyName("export_customer_list")]
    public bool ExportCustomerList { get; set; }
    /// <summary>
    /// 导出成员客户统计，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("export_customer_data")]
    [JsonPropertyName("export_customer_data")]
    public bool ExportCustomerData { get; set; }
    /// <summary>
    /// 导出客户群列表，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("export_customer_group_list")]
    [JsonPropertyName("export_customer_group_list")]
    public bool ExportCustomerGroupList { get; set; }
    /// <summary>
    /// 配置企业客户标签，默认为true
    /// </summary>
    [NotNull]
    [JsonProperty("manage_customer_tag")]
    [JsonPropertyName("manage_customer_tag")]
    public bool ManageCustomerTag { get; set; }

    public static CustomerStrategyPrivilege Default()
    {
        return new CustomerStrategyPrivilege
        {
            ViewCustomerList = true,
            ViewCustomerData = true,
            ViewRoomList = true,
            ContactMe = true,
            JoinRoom = true,
            ShareCustomer = true,
            EditWelcomeMsg = true,
            ViewBehaviorData = true,
            ViewRoomData = true,
            SendGroupMsg = true,
            RoomDeduplication = true,
            RapidReply = true,
            OnjobCustomerTransfer = true,
            EditAntiSpamRule = true,
            ExportCustomerList = true,
            ExportCustomerData = true,
            ExportCustomerGroupList = true,
            ManageCustomerTag = true,
        };
    }
}
