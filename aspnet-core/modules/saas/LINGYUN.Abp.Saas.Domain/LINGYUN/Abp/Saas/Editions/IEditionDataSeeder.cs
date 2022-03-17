using System.Threading.Tasks;

namespace LINGYUN.Abp.Saas.Editions;

public interface IEditionDataSeeder
{
    Task SeedDefaultEditionsAsync();
}
