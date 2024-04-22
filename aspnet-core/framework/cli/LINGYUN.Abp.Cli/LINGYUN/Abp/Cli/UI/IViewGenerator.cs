using System.Threading.Tasks;

namespace LINGYUN.Abp.Cli.UI;
public interface IViewGenerator
{
    Task GenerateAsync(GenerateViewArgs args);
}
