using System.Threading.Tasks;

namespace LINGYUN.Abp.Localization.Persistence;

public interface IStaticLocalizationSaver
{
    Task SaveAsync();
}
