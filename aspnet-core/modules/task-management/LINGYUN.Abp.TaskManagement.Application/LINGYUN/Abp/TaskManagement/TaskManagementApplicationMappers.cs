using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.TaskManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BackgroundJobInfoToBackgroundJobInfoDtoMapper : MapperBase<BackgroundJobInfo, BackgroundJobInfoDto>
{
    public override partial BackgroundJobInfoDto Map(BackgroundJobInfo source);
    public override partial void Map(BackgroundJobInfo source, BackgroundJobInfoDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BackgroundJobLogToBackgroundJobLogDtoMapper : MapperBase<BackgroundJobLog, BackgroundJobLogDto>
{
    public override partial BackgroundJobLogDto Map(BackgroundJobLog source);
    public override partial void Map(BackgroundJobLog source, BackgroundJobLogDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BackgroundJobActionToBackgroundJobActionDtoMapper : MapperBase<BackgroundJobAction, BackgroundJobActionDto>
{
    public override partial BackgroundJobActionDto Map(BackgroundJobAction source);
    public override partial void Map(BackgroundJobAction source, BackgroundJobActionDto destination);
}
