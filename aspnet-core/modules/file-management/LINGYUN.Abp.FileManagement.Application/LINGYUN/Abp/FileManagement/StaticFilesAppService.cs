using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace LINGYUN.Abp.FileManagement
{
    public class StaticFilesAppService : FileManagementApplicationServiceBase, IStaticFilesAppService
    {
        protected IOssContainerFactory OssContainerFactory { get; }

        public StaticFilesAppService(
            IOssContainerFactory ossContainerFactory)
        {
            OssContainerFactory = ossContainerFactory;
        }

        public virtual async Task<Stream> GetAsync(GetStaticFileInput input)
        {
            var ossObjectRequest = new GetOssObjectRequest(
                HttpUtility.UrlDecode(input.Bucket), // 需要处理特殊字符
                HttpUtility.UrlDecode(input.Name),
                HttpUtility.UrlDecode(input.Path),
                HttpUtility.UrlDecode(input.Process));

            var ossContainer = OssContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            return ossObject.Content;
        }
    }
}
