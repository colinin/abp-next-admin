using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class RecordMapper : MapperBase<ApprovalRecord, RecordDto>
{
    [MapperIgnoreTarget(nameof(RecordDto.FirstViewedAt))]
    [MapperIgnoreTarget(nameof(RecordDto.CompletedAt))]
    public override partial RecordDto Map(ApprovalRecord source);

    [MapperIgnoreTarget(nameof(RecordDto.FirstViewedAt))]
    [MapperIgnoreTarget(nameof(RecordDto.CompletedAt))]
    public override partial void Map(ApprovalRecord source, RecordDto destination);
}
