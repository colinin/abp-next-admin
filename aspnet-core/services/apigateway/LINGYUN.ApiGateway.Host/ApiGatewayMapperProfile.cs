using AutoMapper;
using LINGYUN.ApiGateway.Ocelot;
using Ocelot.Configuration.File;

namespace LINGYUN.ApiGateway
{
    public class ApiGatewayMapperProfile : Profile
    {
        public ApiGatewayMapperProfile()
        {
            CreateMap<HostAndPortDto, FileHostAndPort>()
                .ForMember(fhp => fhp.Port, map => map.MapFrom(m => m.Port ?? 0));
            CreateMap<HttpHandlerOptionsDto, FileHttpHandlerOptions>();
            CreateMap<AuthenticationOptionsDto, FileAuthenticationOptions>();
            CreateMap<RateLimitRuleDto, FileRateLimitRule>()
                .ForMember(frl => frl.Limit, map => map.MapFrom(m => m.Limit ?? 0L))
                .ForMember(frl => frl.PeriodTimespan, map => map.MapFrom(m => m.PeriodTimespan ?? 0d));
            CreateMap<LoadBalancerOptionsDto, FileLoadBalancerOptions>()
                .ForMember(flb => flb.Expiry, map => map.MapFrom(m => m.Expiry ?? 0));
            CreateMap<QosOptionsDto, FileQoSOptions>()
                .ForMember(fqs => fqs.DurationOfBreak, map => map.MapFrom(m => m.DurationOfBreak ?? 0))
                .ForMember(fqs => fqs.ExceptionsAllowedBeforeBreaking, map => map.MapFrom(m => m.ExceptionsAllowedBeforeBreaking ?? 0))
                .ForMember(fqs => fqs.TimeoutValue, map => map.MapFrom(m => m.TimeoutValue ?? 0));
            CreateMap<CacheOptionsDto, FileCacheOptions>()
                .ForMember(fco => fco.TtlSeconds, map => map.MapFrom(m => m.TtlSeconds ?? 0));
            CreateMap<SecurityOptionsDto, FileSecurityOptions>();
            CreateMap<ServiceDiscoveryProviderDto, FileServiceDiscoveryProvider>()
                .ForMember(fsd => fsd.Port, map => map.MapFrom(m => m.Port ?? 0))
                .ForMember(fsd => fsd.PollingInterval, map => map.MapFrom(m => m.PollingInterval ?? 0));
            CreateMap<RateLimitOptionsDto, FileRateLimitOptions>()
                .ForMember(flo => flo.HttpStatusCode, map => map.MapFrom(m => m.HttpStatusCode ?? 429));

            CreateMap<ReRouteDto, FileRoute>()
                .ForMember(frr => frr.RouteIsCaseSensitive, map => map.MapFrom(m => m.ReRouteIsCaseSensitive))
                .ForMember(frr => frr.FileCacheOptions, map => map.MapFrom(m => m.FileCacheOptions))
                .ForMember(frr => frr.Priority, map => map.MapFrom(m => m.Priority ?? 0))
                .ForMember(frr => frr.Timeout, map => map.MapFrom(m => m.Timeout ?? 0))
                .ForMember(frr => frr.DownstreamHttpMethod, map => map.MapFrom(m => m.DownstreamHttpMethod));

            CreateMap<GlobalConfigurationDto, FileGlobalConfiguration>()
                .ForMember(fgc => fgc.HttpHandlerOptions, map => map.MapFrom(m => m.HttpHandlerOptions))
                .ForMember(fgc => fgc.LoadBalancerOptions, map => map.MapFrom(m => m.LoadBalancerOptions))
                .ForMember(fgc => fgc.QoSOptions, map => map.MapFrom(m => m.QoSOptions))
                .ForMember(fgc => fgc.RateLimitOptions, map => map.MapFrom(m => m.RateLimitOptions))
                .ForMember(fgc => fgc.ServiceDiscoveryProvider, map => map.MapFrom(m => m.ServiceDiscoveryProvider));

            CreateMap<DynamicReRouteDto, FileDynamicRoute>();

            CreateMap<AggregateReRouteConfigDto, AggregateRouteConfig>()
                .ForMember(arc => arc.RouteKey, map => map.MapFrom(m => m.ReRouteKey));

            CreateMap<AggregateReRouteDto, FileAggregateRoute>()
                .ForMember(far => far.RouteKeys, map => map.MapFrom(m => m.ReRouteKeys))
                .ForMember(far => far.RouteKeysConfig, map => map.MapFrom(m => m.ReRouteKeysConfig))
                .ForMember(far => far.RouteIsCaseSensitive, map => map.MapFrom(m => m.ReRouteIsCaseSensitive));
        }
    }
}
