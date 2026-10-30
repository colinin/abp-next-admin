using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class NodeTaskMapper : MapperBase<ApprovalNodeTask, NodeTaskDto>
{
    public override partial NodeTaskDto Map(ApprovalNodeTask source);

    public override partial void Map(ApprovalNodeTask source, NodeTaskDto destination);
}
