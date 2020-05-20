using AutoMapper;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    public class AbpTenantManagementApplicationAutoMapperProfile : Profile
    {
        public AbpTenantManagementApplicationAutoMapperProfile()
        {
            CreateMap<Tenant, TenantDto>()
                .MapExtraProperties();
        }
    }
}
