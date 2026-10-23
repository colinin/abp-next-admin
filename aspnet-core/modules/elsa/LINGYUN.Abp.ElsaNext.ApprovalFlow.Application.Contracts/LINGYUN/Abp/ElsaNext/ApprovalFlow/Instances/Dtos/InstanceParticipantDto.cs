using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceParticipantDto
{
    [Required]
    public string Id { get; set; } = "";

    public string? DisplayName { get; set; }

    public string Provider { get; set; } = ParticipantConsts.DefaultProvider;

    /// <summary>User or Group.</summary>
    public string Type { get; set; } = ParticipantConsts.UserType;
}
