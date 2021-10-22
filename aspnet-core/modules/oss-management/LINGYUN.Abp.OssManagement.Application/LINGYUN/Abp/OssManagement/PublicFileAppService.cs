using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.Users;

namespace LINGYUN.Abp.OssManagement
{
    /// <summary>
    /// 所有登录用户公开访问文件服务接口
    /// bucket限制在users
    /// path限制在用户id
    /// </summary>
    [Authorize]
    // 不对外公开，仅通过控制器调用
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class PublicFileAppService : OssManagementApplicationServiceBase, IPublicFileAppService
    {
        private readonly IFileValidater _fileValidater;
        private readonly IOssContainerFactory _ossContainerFactory;

        public PublicFileAppService(
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory)
        {
            _fileValidater = fileValidater;
            _ossContainerFactory = ossContainerFactory;
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.UploadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.UploadLimit,
            AbpOssManagementFeatureNames.OssObject.UploadInterval,
            LimitPolicy.Month)]
        public virtual async Task<OssObjectDto> UploadAsync(UploadPublicFileInput input)
        {
            await _fileValidater.ValidationAsync(new UploadFile
            {
                TotalSize = input.Content.Length,
                FileName = input.Object
            });

            var oss = _ossContainerFactory.Create();

            var createOssObjectRequest = new CreateOssObjectRequest(
                 "users",
                 HttpUtility.UrlDecode(input.Object),
                 input.Content,
                 GetCurrentUserPath(HttpUtility.UrlDecode(input.Path)))
            {
                Overwrite = input.Overwrite
            };

            var ossObject = await oss.CreateObjectAsync(createOssObjectRequest);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.DownloadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.DownloadLimit,
            AbpOssManagementFeatureNames.OssObject.DownloadInterval,
            LimitPolicy.Month)]
        public virtual async Task<Stream> GetAsync(GetPublicFileInput input)
        {
            var ossObjectRequest = new GetOssObjectRequest(
                "users", // 需要处理特殊字符
                HttpUtility.UrlDecode(input.Name),
                GetCurrentUserPath(HttpUtility.UrlDecode(input.Path)),
                HttpUtility.UrlDecode(input.Process));

            var ossContainer = _ossContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            return ossObject.Content;
        }

        private string GetCurrentUserPath(string path)
        {
            path = path.StartsWith("/") ? path.Substring(1) : path;
            var userId = CurrentUser.GetId().ToString();
            return path.StartsWith(userId) ? path : $"{userId}/{path}";
        }
    }
}
