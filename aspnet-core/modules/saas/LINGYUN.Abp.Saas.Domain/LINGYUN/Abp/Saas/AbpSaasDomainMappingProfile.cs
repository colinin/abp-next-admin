using AutoMapper;
using LINGYUN.Abp.MultiTenancy.Editions;
using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.Tenants;
using System;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Saas;

public class AbpSaasDomainMappingProfile : Profile
{
    public AbpSaasDomainMappingProfile()
    {
        CreateMap<Edition, EditionInfo>();
        CreateMap<Edition, EditionEto>();

        CreateMap<Tenant, TenantConfiguration>()
            .ForMember(ti => ti.ConnectionStrings, opts =>
            {
                opts.MapFrom((tenant, ti) =>
                {
                    var connStrings = new ConnectionStrings();

                    foreach (var connectionString in tenant.ConnectionStrings)
                    {
                        connStrings[connectionString.Name] = connectionString.Value;
                    }

                    return connStrings;
                });
            })
            .ForMember(ti => ti.IsActive, opts =>
            {
                opts.MapFrom((tenant, ti) =>
                {
                    if (!tenant.IsActive)
                    {
                        return false;
                    }
                    // Injection IClock ?
                    if (tenant.EnableTime.HasValue && DateTime.Now < tenant.EnableTime)
                    {
                        return false;
                    }

                    if(tenant.DisableTime.HasValue && DateTime.Now > tenant.DisableTime)
                    {
                        return false;
                    }

                    return true;
                });
            });

        CreateMap<Tenant, TenantEto>();
    }
}
