using AutoMapper;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    public class AbpTenantManagementApplicationAutoMapperProfile : Profile
    {
        public AbpTenantManagementApplicationAutoMapperProfile()
        {
            CreateMap<TenantConnectionString, TenantConnectionStringDto>();

            CreateMap<Tenant, TenantDto>()
                .MapExtraProperties();
        }
    }
}
