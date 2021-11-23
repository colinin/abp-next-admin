using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Content;
using Volo.Abp.Features;

namespace LINGYUN.Abp.OssManagement
{
    public class StaticFilesAppService : OssManagementApplicationServiceBase, IStaticFilesAppService
    {
        protected IOssContainerFactory OssContainerFactory { get; }

        public StaticFilesAppService(
            IOssContainerFactory ossContainerFactory)
        {
            OssContainerFactory = ossContainerFactory;
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.DownloadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.DownloadLimit,
            AbpOssManagementFeatureNames.OssObject.DownloadInterval,
            LimitPolicy.Month)]
        public virtual async Task<IRemoteStreamContent> GetAsync(GetStaticFileInput input)
        {
            var ossObjectRequest = new GetOssObjectRequest(
                HttpUtility.UrlDecode(input.Bucket), // 需要处理特殊字符
                HttpUtility.UrlDecode(input.Name),
                HttpUtility.UrlDecode(input.Path),
                HttpUtility.UrlDecode(input.Process))
            {
                MD5 = true,
            };

            var ossContainer = OssContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            return new RemoteStreamContent(ossObject.Content, ossObject.Name);
        }
    }
}
