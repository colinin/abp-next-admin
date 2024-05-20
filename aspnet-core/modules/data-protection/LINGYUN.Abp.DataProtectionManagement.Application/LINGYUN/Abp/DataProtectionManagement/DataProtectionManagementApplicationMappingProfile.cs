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
