using AutoMapper;
using Elsa.Models;
using Elsa.Server.Api.Endpoints.WorkflowDefinitions;
using Elsa.Server.Api.Endpoints.WorkflowInstances;
using Elsa.Server.Api.Endpoints.WorkflowRegistry;
using Elsa.Server.Api.Mapping;
using Elsa.Services.Models;
using Volo.Abp.AutoMapper;

namespace LINGYUN.Abp.Elsa;
public class AbpElsaAutoMapperProfile : Profile
{
    public AbpElsaAutoMapperProfile()
    {
        CreateMap<IWorkflowBlueprint, WorkflowBlueprintModel>()
            .ForMember(x => x.IsEnabled, y => y.MapFrom(map => !map.IsDisabled))
            .Ignore(x => x.InputProperties)
            .Ignore(x => x.OutputProperties);
        CreateMap<IWorkflowBlueprint, WorkflowBlueprintSummaryModel>();
        CreateMap<IActivityBlueprint, ActivityBlueprintModel>().ConvertUsing<ActivityBlueprintConverter>();
        CreateMap<ICompositeActivityBlueprint, CompositeActivityBlueprintModel>()
            .Ignore(x => x.InputProperties)
            .Ignore(x => x.OutputProperties);
        CreateMap<IConnection, ConnectionModel>().ConvertUsing<ConnectionConverter>();
        CreateMap<WorkflowInstance, WorkflowInstanceSummaryModel>();
        CreateMap<WorkflowDefinition, WorkflowDefinitionSummaryModel>();
        CreateMap<WorkflowDefinition, WorkflowDefinitionVersionModel>();
    }
}
