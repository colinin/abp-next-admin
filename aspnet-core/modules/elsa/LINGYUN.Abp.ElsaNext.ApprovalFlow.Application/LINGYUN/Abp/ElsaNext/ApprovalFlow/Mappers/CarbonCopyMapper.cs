using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CarbonCopyMapper : MapperBase<ApprovalCc, CarbonCopyDto>
{
    public override partial CarbonCopyDto Map(ApprovalCc source);

    public override partial void Map(ApprovalCc source, CarbonCopyDto destination);
}
