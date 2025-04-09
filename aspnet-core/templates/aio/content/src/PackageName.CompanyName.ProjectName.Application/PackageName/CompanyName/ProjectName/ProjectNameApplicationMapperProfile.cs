using AutoMapper;
using PackageName.CompanyName.ProjectName.Users;
using PackageName.CompanyName.ProjectName.Users.Dtos;

namespace PackageName.CompanyName.ProjectName;

public class ProjectNameApplicationMapperProfile : Profile
{
    public ProjectNameApplicationMapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.IsActive, o => o.Ignore())
            .ForMember(d => d.RoleNames, o => o.Ignore());
        CreateMap<User, UserItemDto>()
            .ForMember(d => d.IsActive, o => o.Ignore())
            .ForMember(d => d.RoleNames, o => o.Ignore());
        CreateMap<CreateUpdateUserDto, User>(MemberList.None);

    }
}
