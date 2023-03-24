using LINGYUN.Abp.WebhooksManagement.Definitions.Dto;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;
public class WebhookDefinitionUpdateDto : WebhookDefinitionCreateOrUpdateDto, IHasConcurrencyStamp
{
    [StringLength(40)]
    public string ConcurrencyStamp { get; set; }
}
