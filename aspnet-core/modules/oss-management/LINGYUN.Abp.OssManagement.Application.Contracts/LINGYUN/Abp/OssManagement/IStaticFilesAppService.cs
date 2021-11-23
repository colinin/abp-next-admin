using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement
{
    public interface IStaticFilesAppService: IApplicationService
    {
        Task<IRemoteStreamContent> GetAsync(GetStaticFileInput input);
    }
}
