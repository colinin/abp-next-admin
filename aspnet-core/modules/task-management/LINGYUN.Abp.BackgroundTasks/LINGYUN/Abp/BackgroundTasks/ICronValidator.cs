using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface ICronValidator
{
    Task<bool> ValidateAsync(string? cron);
}
