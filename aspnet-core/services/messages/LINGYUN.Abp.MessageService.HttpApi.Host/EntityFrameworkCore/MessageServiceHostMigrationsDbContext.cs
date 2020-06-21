using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.EntityFrameworkCore
{
    public class MessageServiceHostMigrationsDbContext : AbpDbContext<MessageServiceHostMigrationsDbContext>
    {
        public MessageServiceHostMigrationsDbContext(DbContextOptions<MessageServiceHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureMessageService();
        }
    }
}
