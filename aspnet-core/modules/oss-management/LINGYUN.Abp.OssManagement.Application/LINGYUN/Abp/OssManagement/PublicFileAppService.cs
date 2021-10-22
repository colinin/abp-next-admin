namespace LINGYUN.Abp.OssManagement
{
    public class PublicFileAppService : FileAppServiceBase, IPublicFileAppService
    {
        public PublicFileAppService(
           IFileUploader fileUploader,
           IFileValidater fileValidater,
           IOssContainerFactory ossContainerFactory)
           : base(fileUploader, fileValidater, ossContainerFactory)
        {
        }

        protected override string GetCurrentBucket()
        {
            return "public";
        }
    }
}
