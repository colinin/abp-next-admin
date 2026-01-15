using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Gdpr;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class GdprRequestToGdprRequestDtoMapper : MapperBase<GdprRequest, GdprRequestDto>
{
    public override partial GdprRequestDto Map(GdprRequest source);
    public override partial void Map(GdprRequest source, GdprRequestDto destination);
}
