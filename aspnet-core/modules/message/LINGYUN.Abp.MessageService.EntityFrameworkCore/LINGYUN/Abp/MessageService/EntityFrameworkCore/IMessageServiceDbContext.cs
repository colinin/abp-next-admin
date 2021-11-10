using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Groups;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.EntityFrameworkCore
{
    [ConnectionStringName(AbpMessageServiceDbProperties.ConnectionStringName)]
    public interface IMessageServiceDbContext : IEfCoreDbContext
    {
        DbSet<UserChatCard> UserChatCards { get; set; }
        DbSet<UserGroupCard> UserGroupCards { get; set; }
    }
}
