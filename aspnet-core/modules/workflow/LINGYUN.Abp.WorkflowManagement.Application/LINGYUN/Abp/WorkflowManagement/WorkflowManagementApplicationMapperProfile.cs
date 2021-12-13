using AutoMapper;
using LINGYUN.Abp.WorkflowManagement.Activitys;
using LINGYUN.Abp.WorkflowManagement.Workflows;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowManagement
{
    public class WorkflowManagementApplicationMapperProfile : Profile
    {
        public WorkflowManagementApplicationMapperProfile()
        {
            CreateMap<ExecutionPointer, ExecutionPointerDto>()
                .ForMember(dto => dto.Status, map => map.MapFrom(src => src.Status.ToString()));
            CreateMap<WorkflowInstance, WorkflowInstanceDto>()
                .ForMember(dto => dto.Status, map => map.MapFrom(src => src.Status.ToString()));
            CreateMap<PendingActivity, PendingActivityDto>();

            CreateMap<StepNode, StepNodeDto>();
            CreateMap<CompensateNode, StepNodeDto>();
            CreateMap<Workflow, WorkflowDto>()
                .ForMember(dto => dto.Steps, map => map.Ignore())
                .ForMember(dto => dto.CompensateNodes, map => map.Ignore());
        }
    }
}
