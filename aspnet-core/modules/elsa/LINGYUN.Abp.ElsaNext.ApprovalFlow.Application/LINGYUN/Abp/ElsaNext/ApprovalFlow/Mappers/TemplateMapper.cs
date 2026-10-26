using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TemplateMapper : MapperBase<ApprovalTemplate, TemplateDto>
{
    public override partial TemplateDto Map(ApprovalTemplate source);

    public override partial void Map(ApprovalTemplate source, TemplateDto destination);
}
