using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Messages;
using LINGYUN.Platform.Packages;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Platform;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LayoutToLayoutEtoMapper : MapperBase<Layout, LayoutEto>
{
    public override partial LayoutEto Map(Layout source);
    public override partial void Map(Layout source, LayoutEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MenuToMenuEtoMapper : MapperBase<Menu, MenuEto>
{
    public override partial MenuEto Map(Menu source);
    public override partial void Map(Menu source, MenuEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class UserMenuToUserMenuEtoMapper : MapperBase<UserMenu, UserMenuEto>
{
    public override partial UserMenuEto Map(UserMenu source);
    public override partial void Map(UserMenu source, UserMenuEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class RoleMenuToRoleMenuEtoMapper : MapperBase<RoleMenu, RoleMenuEto>
{
    public override partial RoleMenuEto Map(RoleMenu source);
    public override partial void Map(RoleMenu source, RoleMenuEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PackageToPackageEtoMapper : MapperBase<Package, PackageEto>
{
    public override partial PackageEto Map(Package source);
    public override partial void Map(Package source, PackageEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EmailMessageToEmailMessageEtoMapper : MapperBase<EmailMessage, EmailMessageEto>
{
    public override partial EmailMessageEto Map(EmailMessage source);
    public override partial void Map(EmailMessage source, EmailMessageEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SmsMessageToSmsMessageEtoMapper : MapperBase<SmsMessage, SmsMessageEto>
{
    public override partial SmsMessageEto Map(SmsMessage source);
    public override partial void Map(SmsMessage source, SmsMessageEto destination);
}
