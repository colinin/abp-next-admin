using AutoMapper;
using System;
using System.Linq;

namespace LINGYUN.Abp.DataProtectionManagement;
public class DataProtectionManagementApplicationMappingProfile :Profile
{
    public DataProtectionManagementApplicationMappingProfile()
    {
        CreateMap<EntityEnumInfo, EntityEnumInfoDto>();
        CreateMap<EntityPropertyInfo, EntityPropertyInfoDto>();
        CreateMap<EntityTypeInfo, EntityTypeInfoDto>();
        CreateMap<RoleEntityRule, RoleEntityRuleDto>()
            .ForMember(dto => dto.AccessedProperties, map => map.MapFrom(src => MapToArray(src.AccessedProperties)));
        CreateMap<OrganizationUnitEntityRule, OrganizationUnitEntityRuleDto>()
            .ForMember(dto => dto.AccessedProperties, map => map.MapFrom(src => MapToArray(src.AccessedProperties)));

        CreateMap<SubjectStrategy, SubjectStrategyDto>();
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
