using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.LocalizationManagement;
public class LanguageDto : AuditedEntityDto<Guid>
{
    public string CultureName { get; set; } = default!;
    public string UiCultureName { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string TwoLetterISOLanguageName { get; set; } = default!;
}
