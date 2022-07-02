using AutoMapper;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhooksManagementApplicationMapperProfile : Profile
{
    public WebhooksManagementApplicationMapperProfile()
    {
        CreateMap<WebhookEventRecord, WebhookEventRecordDto>();
        CreateMap<WebhookSendRecord, WebhookSendRecordDto>()
            .ForMember(dto => dto.RequestHeaders, map => map.MapFrom((src, dto) =>
            {
                var result = new Dictionary<string, string>();

                if (src.RequestHeaders.IsNullOrWhiteSpace())
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject< Dictionary<string, string>>(src.RequestHeaders);
                    }
                    catch { }
                }

                return result;
            }))
            .ForMember(dto => dto.ResponseHeaders, map => map.MapFrom((src, dto) =>
            {
                var result = new Dictionary<string, string>();

                if (src.ResponseHeaders.IsNullOrWhiteSpace())
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<Dictionary<string, string>>(src.ResponseHeaders);
                    }
                    catch { }
                }

                return result;
            }));
    }
}
