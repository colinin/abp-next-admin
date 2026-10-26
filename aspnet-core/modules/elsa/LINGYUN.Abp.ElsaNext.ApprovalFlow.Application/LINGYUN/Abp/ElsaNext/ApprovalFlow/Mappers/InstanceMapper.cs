using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class InstanceMapper : MapperBase<ApprovalInstance, InstanceDto>
{
    public override partial InstanceDto Map(ApprovalInstance source);

    public override partial void Map(ApprovalInstance source, InstanceDto destination);
}
