using AutoMapper;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class LocalizationManagementDomainMapperProfile : Profile
    {
        public LocalizationManagementDomainMapperProfile()
        {
            CreateMap<Text, TextEto>();
            CreateMap<Resource, ResourceEto>();
            CreateMap<Language, LanguageEto>();
        }
    }
}
