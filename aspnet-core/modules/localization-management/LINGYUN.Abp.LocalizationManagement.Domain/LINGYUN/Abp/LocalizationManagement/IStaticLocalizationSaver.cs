using System.Threading.Tasks;

namespace LINGYUN.Abp.LocalizationManagement;

public interface IStaticLocalizationSaver
{
    Task SaveAsync();
}
