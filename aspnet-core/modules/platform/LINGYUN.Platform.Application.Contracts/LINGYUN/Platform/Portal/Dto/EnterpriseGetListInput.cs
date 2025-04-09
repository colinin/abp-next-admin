using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Portal;

public class EnterpriseGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public DateTime? BeginRegistrationDate { get; set; }

    public DateTime? EndRegistrationDate { get; set; }

    public DateTime? BeginExpirationDate { get; set; }

    public DateTime? EndExpirationDate { get; set; }
}
