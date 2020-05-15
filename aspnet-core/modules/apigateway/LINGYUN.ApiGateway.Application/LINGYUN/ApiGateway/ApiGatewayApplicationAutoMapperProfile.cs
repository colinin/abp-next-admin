using AutoMapper;
using LINGYUN.ApiGateway.Ocelot;
using LINGYUN.ApiGateway.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.ApiGateway
{
    public class ApiGatewayApplicationAutoMapperProfile : Profile, ISingletonDependency
    {
        public ApiGatewayApplicationAutoMapperProfile(ISnowflakeIdGenerator snowflakeIdGenerator)
        {
            //Configure your AutoMapper mapping configuration here...

            CreateMap<CacheOptions, CacheOptionsDto>();

            CreateMap<QoSOptions, QosOptionsDto>();

            CreateMap<LoadBalancerOptions, LoadBalancerOptionsDto>();

            CreateMap<RateLimitOptions, RateLimitOptionsDto>();

            CreateMap<ServiceDiscoveryProvider, ServiceDiscoveryProviderDto>();

            CreateMap<RateLimitRule, RateLimitRuleDto>()
                .ForMember(dto => dto.ClientWhitelist, map => map.MapFrom((m, n) =>
                {
                    if (!m.ClientWhitelist.IsNullOrWhiteSpace())
                    {
                        return m.ClientWhitelist.Split(',').ToList();
                    }
                    return new List<string>();
                }));

            CreateMap<AuthenticationOptions, AuthenticationOptionsDto>()
                .ForMember(dto => dto.AllowedScopes, map => map.MapFrom(m => !string.IsNullOrWhiteSpace(m.AllowedScopes) 
                ? m.AllowedScopes.Split(',').ToList() 
                : new List<string>()));

            CreateMap<HttpHandlerOptions, HttpHandlerOptionsDto>();

            CreateMap<HostAndPort, HostAndPortDto>();

            CreateMap<SecurityOptions, SecurityOptionsDto>()
                .ForMember(dto => dto.IPAllowedList, map => map.MapFrom((m, n) =>
                {
                    if (!m.IPAllowedList.IsNullOrWhiteSpace())
                    {
                        return m.IPAllowedList.Split(',').ToList();
                    }
                    return new List<string>();
                }))
                .ForMember(dto => dto.IPBlockedList, map => map.MapFrom((m, n) =>
                {
                    if (!m.IPBlockedList.IsNullOrWhiteSpace())
                    {
                        return m.IPBlockedList.Split(',').ToList();
                    }
                    return new List<string>();
                }));

            CreateMap<AggregateReRouteConfig, AggregateReRouteConfigDto>();

            CreateMap<AggregateReRoute, AggregateReRouteDto>()
                .ForMember(dto => dto.ReRouteKeys, map => map.MapFrom(m => !m.ReRouteKeys.IsNullOrWhiteSpace() && m.ReRouteKeys.Contains(",") 
                ? m.ReRouteKeys.Split(',').ToList()
                : new List<string>()))
                .ForMember(dto => dto.UpstreamHttpMethod, map => map.MapFrom(m => !m.UpstreamHttpMethod.IsNullOrWhiteSpace() && m.UpstreamHttpMethod.Contains(",")
                ? m.UpstreamHttpMethod.Split(',').ToList()
                : new List<string>()));

            CreateMap<GlobalConfiguration, GlobalConfigurationDto>();

            CreateMap<DynamicReRoute, DynamicReRouteDto>();

            CreateMap<ReRoute, ReRouteDto>()
                .ForMember(frr => frr.AddClaimsToRequest, map => map.MapFrom(m => MapperDictionary(m.AddClaimsToRequest)))
                .ForMember(frr => frr.AddHeadersToRequest, map => map.MapFrom(m => MapperDictionary(m.AddHeadersToRequest)))
                .ForMember(frr => frr.AddQueriesToRequest, map => map.MapFrom(m => MapperDictionary(m.AddQueriesToRequest)))
                .ForMember(frr => frr.DelegatingHandlers, map => map.MapFrom(m => MapperList(m.DelegatingHandlers)))
                .ForMember(frr => frr.DownstreamHeaderTransform, map => map.MapFrom(m => MapperDictionary(m.DownstreamHeaderTransform)))
                .ForMember(frr => frr.RouteClaimsRequirement, map => map.MapFrom(m => MapperDictionary(m.RouteClaimsRequirement)))
                .ForMember(frr => frr.UpstreamHeaderTransform, map => map.MapFrom(m => MapperDictionary(m.UpstreamHeaderTransform)))
                .ForMember(frr => frr.ChangeDownstreamPathTemplate, map => map.MapFrom(m => MapperDictionary(m.ChangeDownstreamPathTemplate)))
                .ForMember(frr => frr.UpstreamHttpMethod, map => map.MapFrom(m => MapperList(m.UpstreamHttpMethod)))
                .ForMember(frr => frr.DownstreamHostAndPorts, map => map.MapFrom(m => MapperHostAndPortList(m.DownstreamHostAndPorts)));

            CreateMap<ReRouteCreateDto, ReRoute>()
                .ForCtorParam("rerouteId", x => x.MapFrom(m => snowflakeIdGenerator.NextId()))
                .ForCtorParam("routeName", x => x.MapFrom(m => m.ReRouteName))
                .ForCtorParam("appId", x => x.MapFrom(m => m.AppId))
                .ForMember(s => s.DelegatingHandlers, d => d.MapFrom((dto, src) => 
                {
                    src.SetDelegatingHandler(dto.DelegatingHandlers);
                    return src.DelegatingHandlers;
                }))
                .ForMember(s => s.DownstreamHeaderTransform, d => d.MapFrom((dto, src) =>
                {
                    src.SetDownstreamHeader(dto.DownstreamHeaderTransform);
                    return src.DownstreamHeaderTransform;
                }))
                .ForMember(s => s.AddQueriesToRequest, d => d.MapFrom((dto, src) =>
                {
                    src.SetQueriesParamter(dto.AddQueriesToRequest);
                    return src.AddQueriesToRequest;
                }))
                .ForMember(s => s.AddClaimsToRequest, d => d.MapFrom((dto, src) =>
                {
                    src.SetRequestClaims(dto.AddClaimsToRequest);
                    return src.AddClaimsToRequest;
                }))
                .ForMember(s => s.AddHeadersToRequest, d => d.MapFrom((dto, src) =>
                {
                    src.SetRequestHeader(dto.AddHeadersToRequest);
                    return src.AddHeadersToRequest;
                }))
                .ForMember(s => s.RouteClaimsRequirement, d => d.MapFrom((dto, src) =>
                {
                    src.SetRouteClaims(dto.RouteClaimsRequirement);
                    return src.RouteClaimsRequirement;
                }))
                .ForMember(s => s.UpstreamHeaderTransform, d => d.MapFrom((dto, src) =>
                {
                    src.SetUpstreamHeader(dto.UpstreamHeaderTransform);
                    return src.UpstreamHeaderTransform;
                }))
                .ForMember(s => s.ChangeDownstreamPathTemplate, d => d.MapFrom((dto, src) =>
                {
                    src.SetChangeDownstreamPath(dto.ChangeDownstreamPathTemplate);
                    return src.ChangeDownstreamPathTemplate;
                }))
                .ForMember(map => map.QoSOptions, dto => dto.Ignore())
                .ForMember(map => map.CacheOptions, dto => dto.Ignore())
                .ForMember(map => map.LoadBalancerOptions, dto => dto.Ignore())
                .ForMember(map => map.RateLimitOptions, dto => dto.Ignore())
                .ForMember(map => map.AuthenticationOptions, dto => dto.Ignore())
                .ForMember(map => map.HttpHandlerOptions, dto => dto.Ignore())
                .ForMember(map => map.SecurityOptions, dto => dto.Ignore());

            CreateMap<RouteGroup, RouteGroupDto>();
        }

        private Dictionary<string, string> MapperDictionary(string sourceString)
        {
            var dictionary = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(sourceString))
            {
                var headers = sourceString.Split(',').ToList();

                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        var current = header.Split(':');
                        if (current != null && current.Length == 2)
                        {
                            dictionary.Add(current[0], current[1]);
                        }
                    }
                }
            }
            return dictionary;
        }

        private List<string> MapperList(string sourceString)
        {
            var list = new List<string>();
            if (!string.IsNullOrWhiteSpace(sourceString))
            {
                var srcList = sourceString.Split(',');
                foreach (var src in srcList)
                {
                    if (!src.IsNullOrWhiteSpace())
                    {
                        list.Add(src);
                    }
                }
            }
            return list;
        }

        private List<HostAndPortDto> MapperHostAndPortList(string sourceString)
        {
            var list = new List<HostAndPortDto>();
            if (!string.IsNullOrWhiteSpace(sourceString))
            {
                var sourceList = sourceString.Split(',').ToList();
                foreach (var source in sourceList)
                {
                    var current = source.Split(':');
                    if (current != null && current.Length == 2)
                    {
                        var hostAndPort = new HostAndPortDto();
                        hostAndPort.Host = current[0];
                        hostAndPort.Port = int.Parse(current[1]);
                        list.Add(hostAndPort);
                    }
                }
            }
            return list;
        }
    }
}
