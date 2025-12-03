using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
/// <summary>
/// 会议室使用范围
/// </summary>
public class MeetingRoomRange
{
    /// <summary>
    /// 会议室使用范围的userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("user_list")]
    [JsonPropertyName("user_list")]
    public string[]? UserList { get; }
    /// <summary>
    /// 会议室使用范围的部门id列表
    /// </summary>
    [NotNull]
    [JsonProperty("department_list")]
    [JsonPropertyName("department_list")]
    public int[]? DepartmentList { get; }
    public MeetingRoomRange(string[]? userList = null, int[]? departmentList = null)
    {
        UserList = userList;
        DepartmentList = departmentList;
    }
}
