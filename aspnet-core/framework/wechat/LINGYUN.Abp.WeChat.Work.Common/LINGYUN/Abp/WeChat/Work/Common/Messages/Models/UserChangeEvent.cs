using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 成员变更事件
/// </summary>
public abstract class UserChangeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 改变类型
    /// </summary>
    [XmlElement("ChangeType")]
    public string ChangeType { get; set; }
    /// <summary>
    /// 成员UserID
    /// </summary>
    [XmlElement("UserID")]
    public string UserId { get; set; }
    /// <summary>
    /// 成员名称;代开发自建应用需要管理员授权才返回
    /// </summary>
    [XmlElement("Name")]
    public string Name { get; set; }
    /// <summary>
    /// 成员部门列表，仅返回该应用有查看权限的部门id
    /// </summary>
    [XmlElement("Department")]
    public string Department { get; set; }
    /// <summary>
    /// 主部门
    /// </summary>
    [XmlElement("MainDepartment")]
    public string MainDepartment { get; set; }
    /// <summary>
    /// 表示所在部门是否为部门负责人，
    /// 0-否，
    /// 1-是，
    /// 顺序与Department字段的部门逐一对应。
    /// 第三方通讯录应用或者授权了“组织架构信息-应用可获取企业的部门组织架构信息-部门负责人”权限的第三方应用和代开发应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用不可获取；
    /// 上游企业不可获取下游企业成员该字段
    /// </summary>
    [XmlElement("IsLeaderInDept")]
    public string IsLeaderInDept { get; set; }
    /// <summary>
    /// 直属上级UserID，最多1个。
    /// 第三方通讯录应用或者授权了“组织架构信息-应用可获取可见范围内成员组织架构信息-直属上级”权限的第三方应用和代开发应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用不可获取；
    /// 上游企业不可获取下游企业成员该字段
    /// </summary>
    [XmlElement("DirectLeader")]
    public string DirectLeader { get; set; }
    /// <summary>
    /// 职位信息。
    /// 长度为0~64个字节;代开发自建应用需要管理员授权才返回。
    /// 上游共享的应用不返回该字段
    /// </summary>
    [XmlElement("Position")]
    public string Position { get; set; }
    /// <summary>
    /// 手机号码，代开发自建应用需要管理员授权且成员oauth2授权获取；
    /// 第三方仅通讯录应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用也不可获取；
    /// 上游企业不可获取下游企业成员该字段
    /// </summary>
    [XmlElement("Mobile")]
    public string Mobile { get; set; }
    /// <summary>
    /// 性别。
    /// 0表示未定义，
    /// 1表示男性，
    /// 2表示女性。
    /// 代开发自建应用需要管理员授权且成员oauth2授权获取；
    /// 第三方仅通讯录应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用也不可获取；
    /// 上游企业不可获取下游企业成员该字段。
    /// 注：不可获取指返回值0
    /// </summary>
    [XmlElement("Gender")]
    public byte Gender { get; set; }
    /// <summary>
    /// 邮箱，
    /// 代开发自建应用需要管理员授权且成员oauth2授权获取；
    /// 第三方仅通讯录应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用也不可获取；
    /// 上游企业不可获取下游企业成员该字段
    /// </summary>
    [XmlElement("Email")]
    public string Email { get; set; }
    /// <summary>
    /// 企业邮箱，
    /// 代开发自建应用需要管理员授权且成员oauth2授权获取；
    /// 第三方仅通讯录应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用也不可获取；
    /// 上游企业不可获取下游企业成员该字段
    /// </summary>
    [XmlElement("BizMail")]
    public string BizMail { get; set; }
    /// <summary>
    /// 激活状态：
    /// 1=已激活 
    /// 2=已禁用 
    /// 4=未激活  已激活代表已激活企业微信或已关注微信插件（原企业号）
    /// 5=成员退出
    /// </summary>
    [XmlElement("Status")]
    public byte Status { get; set; }
    /// <summary>
    /// 头像url。 
    /// 注：如果要获取小图将url最后的”/0”改成”/100”即可。
    /// 代开发自建应用需要管理员授权且成员oauth2授权获取；
    /// 第三方仅通讯录应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用也不可获取；
    /// 上游企业不可获取下游企业成员该字段
    /// </summary>
    [XmlElement("Avatar")]
    public string Avatar { get; set; }
    /// <summary>
    /// 成员别名。
    /// 上游共享的应用不返回该字段
    /// </summary>
    [XmlElement("Alias")]
    public string Alias { get; set; }
    /// <summary>
    /// 座机;
    /// 代开发自建应用需要管理员授权才返回。
    /// 上游共享的应用不返回该字段
    /// </summary>
    [XmlElement("Telephone")]
    public string Telephone { get; set; }
    /// <summary>
    /// 地址。
    /// 代开发自建应用需要管理员授权且成员oauth2授权获取；
    /// 第三方仅通讯录应用可获取；
    /// 对于非第三方创建的成员，第三方通讯录应用也不可获取；
    /// 上游企业不可获取下游企业成员该字段
    /// </summary>
    [XmlElement("Address")]
    public string Address { get; set; }
    /// <summary>
    /// 扩展属性;
    /// 代开发自建应用需要管理员授权才返回。
    /// 上游共享的应用不返回该字段
    /// </summary>
    [XmlArray("ExtAttr")]
    public MemberExtendAttribute ExtendAttribute { get; set; }
}
