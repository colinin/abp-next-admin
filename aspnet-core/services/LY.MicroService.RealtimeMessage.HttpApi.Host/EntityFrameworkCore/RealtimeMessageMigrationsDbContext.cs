using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.RealtimeMessage.EntityFrameworkCore
{
    public class RealtimeMessageMigrationsDbContext : AbpDbContext<RealtimeMessageMigrationsDbContext>
    {
        public RealtimeMessageMigrationsDbContext(DbContextOptions<RealtimeMessageMigrationsDbContext> options)
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
