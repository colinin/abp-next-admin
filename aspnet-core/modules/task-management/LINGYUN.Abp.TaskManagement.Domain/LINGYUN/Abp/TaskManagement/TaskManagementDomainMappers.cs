using LINGYUN.Abp.BackgroundTasks;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.TaskManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BackgroundJobInfoToJobInfoMapper : MapperBase<BackgroundJobInfo, JobInfo>
{
    [MapperIgnoreTarget(nameof(JobInfo.Args))]
    [MapperIgnoreSource(nameof(BackgroundJobInfo.Args))]
    public override partial JobInfo Map(BackgroundJobInfo source);

    [MapperIgnoreTarget(nameof(JobInfo.Args))]
    [MapperIgnoreSource(nameof(BackgroundJobInfo.Args))]
    public override partial void Map(BackgroundJobInfo source, JobInfo destination);

    public override void AfterMap(BackgroundJobInfo source, JobInfo destination)
    {
        destination.Args = source.Args;
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BackgroundJobInfoToBackgroundJobEtoMapper : MapperBase<BackgroundJobInfo, BackgroundJobEto>
{
    public override partial BackgroundJobEto Map(BackgroundJobInfo source);
    public override partial void Map(BackgroundJobInfo source, BackgroundJobEto destination);
}
