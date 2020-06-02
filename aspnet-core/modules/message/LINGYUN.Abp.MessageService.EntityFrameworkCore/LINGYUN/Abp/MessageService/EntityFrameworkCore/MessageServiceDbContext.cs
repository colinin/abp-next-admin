using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.EntityFrameworkCore
{
    [ConnectionStringName("MessageService")]
    public class MessageServiceDbContext : AbpDbContext<MessageServiceDbContext>
    {
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
