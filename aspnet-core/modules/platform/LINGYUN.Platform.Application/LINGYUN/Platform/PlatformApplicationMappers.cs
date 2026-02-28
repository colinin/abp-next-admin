using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Feedbacks;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Messages;
using LINGYUN.Platform.Packages;
using LINGYUN.Platform.Portal;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Platform;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PackageBlobTPackageBlobDtoMapper : MapperBase<PackageBlob, PackageBlobDto>
{
    public override partial PackageBlobDto Map(PackageBlob source);
    public override partial void Map(PackageBlob source, PackageBlobDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PackageTPackageDtoMapper : MapperBase<Package, PackageDto>
{
    public override partial PackageDto Map(Package source);
    public override partial void Map(Package source, PackageDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DataItemTDataItemDtoMapper : MapperBase<DataItem, DataItemDto>
{
    public override partial DataItemDto Map(DataItem source);
    public override partial void Map(DataItem source, DataItemDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DataTDataDtoMapper : MapperBase<Data, DataDto>
{
    public override partial DataDto Map(Data source);
    public override partial void Map(Data source, DataDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MenuTMenuDtoMapper : MapperBase<Menu, MenuDto>
{
    [MapperIgnoreTarget(nameof(MenuDto.Startup))]
    [MapProperty(nameof(Menu.ExtraProperties), nameof(MenuDto.Meta))]
    public override partial MenuDto Map(Menu source);

    [MapperIgnoreTarget(nameof(MenuDto.Startup))]
    [MapProperty(nameof(Menu.ExtraProperties), nameof(MenuDto.Meta))]
    public override partial void Map(Menu source, MenuDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LayoutTLayoutDtoMapper : MapperBase<Layout, LayoutDto>
{
    [MapProperty(nameof(Layout.ExtraProperties), nameof(LayoutDto.Meta))]
    public override partial LayoutDto Map(Layout source);

    [MapProperty(nameof(Layout.ExtraProperties), nameof(LayoutDto.Meta))]
    public override partial void Map(Layout source, LayoutDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class UserFavoriteMenuTUserFavoriteMenuDtoMapper : MapperBase<UserFavoriteMenu, UserFavoriteMenuDto>
{
    public override partial UserFavoriteMenuDto Map(UserFavoriteMenu source);
    public override partial void Map(UserFavoriteMenu source, UserFavoriteMenuDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FeedbackTFeedbackDtoMapper : MapperBase<Feedback, FeedbackDto>
{
    public override partial FeedbackDto Map(Feedback source);
    public override partial void Map(Feedback source, FeedbackDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FeedbackCommentTFeedbackCommentDtoMapper : MapperBase<FeedbackComment, FeedbackCommentDto>
{
    public override partial FeedbackCommentDto Map(FeedbackComment source);
    public override partial void Map(FeedbackComment source, FeedbackCommentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FeedbackAttachmentTFeedbackAttachmentDtoMapper : MapperBase<FeedbackAttachment, FeedbackAttachmentDto>
{
    public override partial FeedbackAttachmentDto Map(FeedbackAttachment source);
    public override partial void Map(FeedbackAttachment source, FeedbackAttachmentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EmailMessageAttachmentTEmailMessageAttachmentDtoMapper : MapperBase<EmailMessageAttachment, EmailMessageAttachmentDto>
{
    public override partial EmailMessageAttachmentDto Map(EmailMessageAttachment source);
    public override partial void Map(EmailMessageAttachment source, EmailMessageAttachmentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EmailMessageHeaderTEmailMessageHeaderDtoMapper : MapperBase<EmailMessageHeader, EmailMessageHeaderDto>
{
    public override partial EmailMessageHeaderDto Map(EmailMessageHeader source);
    public override partial void Map(EmailMessageHeader source, EmailMessageHeaderDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EmailMessageTEmailMessageDtoMapper : MapperBase<EmailMessage, EmailMessageDto>
{
    public override partial EmailMessageDto Map(EmailMessage source);
    public override partial void Map(EmailMessage source, EmailMessageDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SmsMessageTSmsMessageDtoMapper : MapperBase<SmsMessage, SmsMessageDto>
{
    public override partial SmsMessageDto Map(SmsMessage source);
    public override partial void Map(SmsMessage source, SmsMessageDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EnterpriseTEnterpriseDtoMapper : MapperBase<Enterprise, EnterpriseDto>
{
    public override partial EnterpriseDto Map(Enterprise source);
    public override partial void Map(Enterprise source, EnterpriseDto destination);
}

