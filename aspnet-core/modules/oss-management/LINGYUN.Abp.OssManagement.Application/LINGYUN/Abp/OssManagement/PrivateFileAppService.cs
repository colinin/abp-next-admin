using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;

namespace LINGYUN.Abp.OssManagement
{
    /// <summary>
    /// 所有登录用户访问私有文件服务接口
    /// bucket限制在users
    /// path限制在用户id
    /// </summary>
    [Authorize]
    // 不对外公开，仅通过控制器调用
    //[RemoteService(IsMetadataEnabled = false)]
    public class PrivateFileAppService : FileAppServiceBase, IPrivateFileAppService
    {
        public PrivateFileAppService(
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory)
            : base(fileValidater, ossContainerFactory)
        {
        }
        protected override string GetCurrentBucket()
        {
            return "users";
        }

        protected override string GetCurrentPath(string path)
        {
            path = base.GetCurrentPath(path);
            var userId = CurrentUser.GetId().ToString("N");
            return path.StartsWith(userId) ? path : $"{userId}/{path}";
        }
    }
}
