using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.TextTemplating;
public class TextTemplateDefinitionUpdateDto : TextTemplateDefinitionCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
