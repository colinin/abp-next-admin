using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement
{
    public interface ITextAppService : IApplicationService
    {
        Task SetTextAsync(SetTextInput input);

        Task RestoreToDefaultAsync(RestoreDefaultTextInput input);
    }
}
