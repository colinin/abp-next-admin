using AutoMapper;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhooksManagementApplicationMapperProfile : Profile
{
    public WebhooksManagementApplicationMapperProfile()
    {
        CreateMap<WebhookEventRecord, WebhookEventRecordDto>();
        CreateMap<WebhookSendRecord, WebhookSendRecordDto>();
    }
}
