using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService
{
    public interface IMessageDataSeeder
    {
        Task SeedAsync(Guid? tenantId = null);
    }
}
