using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Groups;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.EntityFrameworkCore
{
    [ConnectionStringName(AbpMessageServiceDbProperties.ConnectionStringName)]
    public class MessageServiceDbContext : AbpDbContext<MessageServiceDbContext>, IMessageServiceDbContext
    {
        public DbSet<UserChatCard> UserChatCards { get; set; }
        public DbSet<UserGroupCard> UserGroupCards { get; set; }

        public MessageServiceDbContext(DbContextOptions<MessageServiceDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMessageService(options =>
            {
                options.TablePrefix = AbpMessageServiceDbProperties.DefaultTablePrefix;
                options.Schema = AbpMessageServiceDbProperties.DefaultSchema;
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
