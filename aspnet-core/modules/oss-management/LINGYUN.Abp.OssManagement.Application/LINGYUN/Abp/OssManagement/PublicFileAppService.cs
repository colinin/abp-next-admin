namespace LINGYUN.Abp.OssManagement
{
    public class PublicFileAppService : FileAppServiceBase, IPublicFileAppService
    {
        public PublicFileAppService(
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory)
            : base(fileValidater, ossContainerFactory)
        {
        }
        protected override string GetCurrentBucket()
        {
            return "public";
        }
    }
}
