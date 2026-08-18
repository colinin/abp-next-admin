namespace LINGYUN.Abp.WeChat.Work.JsSdk.Dtos;
public class AgentConfigDto
{
    public string AgentId { get; set; }
    public string CorpId { get; set; }
    public AgentConfigDto(string agentId, string corpId)
    {
        AgentId = agentId;
        CorpId = corpId;
    }
}
