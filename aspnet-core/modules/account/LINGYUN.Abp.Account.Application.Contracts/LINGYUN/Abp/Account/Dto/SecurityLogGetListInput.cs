using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Account;

public class SecurityLogGetListInput : PagedAndSortedResultRequestDto
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string ApplicationName { get; set; }
    public string Identity { get; set; }
    public string ActionName { get; set; }
    public string ClientId { get; set; }
    public string CorrelationId { get; set; }
}