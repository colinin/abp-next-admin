﻿using AutoMapper;
using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Feedbacks;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Messages;
using LINGYUN.Platform.Packages;
using LINGYUN.Platform.Portal;

namespace LINGYUN.Platform;

public class PlatformApplicationMappingProfile : Profile
{
    public PlatformApplicationMappingProfile()
    {
        CreateMap<PackageBlob, PackageBlobDto>();
        CreateMap<Package, PackageDto>();

        CreateMap<DataItem, DataItemDto>();
        CreateMap<Data, DataDto>();
        CreateMap<Menu, MenuDto>()
            .ForMember(dto => dto.Meta, map => map.MapFrom(src => src.ExtraProperties))
            .ForMember(dto => dto.Startup, map => map.Ignore());
        CreateMap<Layout, LayoutDto>()
            .ForMember(dto => dto.Meta, map => map.MapFrom(src => src.ExtraProperties));
        CreateMap<UserFavoriteMenu, UserFavoriteMenuDto>();

        CreateMap<Feedback, FeedbackDto>();
        CreateMap<FeedbackComment, FeedbackCommentDto>();
        CreateMap<FeedbackAttachment, FeedbackAttachmentDto>();

        CreateMap<EmailMessageAttachment, EmailMessageAttachmentDto>();
        CreateMap<EmailMessageHeader, EmailMessageHeaderDto>();
        CreateMap<EmailMessage, EmailMessageDto>();
        CreateMap<SmsMessage, SmsMessageDto>();

        CreateMap<Enterprise, EnterpriseDto>();
    }
}
