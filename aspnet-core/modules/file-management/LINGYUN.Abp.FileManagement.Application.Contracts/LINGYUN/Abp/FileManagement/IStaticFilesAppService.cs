using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.FileManagement
{
    public interface IStaticFilesAppService: IApplicationService
    {
        Task<Stream> GetAsync(GetStaticFileInput input);
    }
}
