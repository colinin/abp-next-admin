using AutoMapper;
using System;
using System.Linq;

namespace LINGYUN.Abp.DataProtectionManagement;
public class DataProtectionManagementApplicationMappingProfile :Profile
{
    public DataProtectionManagementApplicationMappingProfile()
    {
        CreateMap<EntityPropertyInfo, EntityPropertyInfoDto>()
            .ForMember(dto => dto.ValueRange, map => map.MapFrom(src => MapToArray(src.ValueRange)));
        CreateMap<EntityTypeInfo, EntityTypeInfoDto>();
        CreateMap<RoleEntityRule, RoleEntityRuleDto>()
            .ForMember(dto => dto.AllowProperties, map => map.MapFrom(src => MapToArray(src.AllowProperties)));
        CreateMap<OrganizationUnitEntityRule, OrganizationUnitEntityRuleDto>()
            .ForMember(dto => dto.AllowProperties, map => map.MapFrom(src => MapToArray(src.AllowProperties)));
    }

    private string[] MapToArray(string val)
    {
        if (val.IsNullOrWhiteSpace())
        {
            return Array.Empty<string>();
        }
        return val.Split(',').ToArray();
    }
}
