using AutoMapper;
using LINGYUN.Abp.BackgroundTasks;

namespace LINGYUN.Abp.TaskManagement;

public class TaskManagementDomainMapperProfile : Profile
{
    public TaskManagementDomainMapperProfile()
    {
        CreateMap<BackgroundJobInfo, JobInfo>();
    }
}
