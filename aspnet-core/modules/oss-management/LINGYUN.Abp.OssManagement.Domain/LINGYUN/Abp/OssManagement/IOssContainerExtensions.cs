using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement
{
    public static class IOssContainerExtensions
    {
        /// <summary>
        /// 如果不存在容器则创建
        /// </summary>
        /// <param name="ossContainer"></param>
        /// <param name="name"></param>
        /// <returns>返回容器信息</returns>
        public static async Task<OssContainer> CreateIfNotExistsAsync(
            this IOssContainer ossContainer,
            string name)
        {
            if (! await ossContainer.ExistsAsync(name))
            {
                await ossContainer.CreateAsync(name);
            }

            return await ossContainer.GetAsync(name);
        }

        public static async Task DeleteObjectAsync(
            this IOssContainer ossContainer,
            string bucket,
            string @object,
            string path = "")
        {
            await ossContainer.DeleteObjectAsync(
                new GetOssObjectRequest(bucket, @object, path));
        }

        public static async Task BulkDeleteObjectsAsync(
            this IOssContainer ossContainer,
            string bucketName,
            ICollection<string> objectNames,
            string path = "")
        {
            await ossContainer.BulkDeleteObjectsAsync(
                new BulkDeleteObjectRequest(bucketName, objectNames, path));
        }

        public static async Task<GetOssContainersResponse> GetListAsync(
            this IOssContainer ossContainer,
            string prefix = null,
            string marker = null,
            int skipCount = 0,
            int maxResultCount = 10)
        {
            return await ossContainer.GetListAsync(
                new GetOssContainersRequest(prefix, marker, skipCount, maxResultCount));
        }

        public static async Task<OssObject> GetObjectAsync(
            this IOssContainer ossContainer,
            string bucket,
            string @object,
            string path = "",
            bool md5 = false,
            bool createPathIsNotExists = false)
        {
            return await ossContainer.GetObjectAsync(
                new GetOssObjectRequest(bucket, @object, path)
                {
                    MD5 = md5,
                    CreatePathIsNotExists = createPathIsNotExists
                });
        }

        public static async Task<GetOssObjectsResponse> GetObjectsAsync(
            this IOssContainer ossContainer,
            string name, 
            string prefix = null, 
            string marker = null, 
            string delimiter = null, 
            string encodingType = null,
            bool md5 = false,
            int skipCount = 0,
            int maxResultCount = 10,
            bool createPathIsNotExists = false)
        {
            return await ossContainer.GetObjectsAsync(
                new GetOssObjectsRequest(name, prefix, marker, delimiter, encodingType, skipCount, maxResultCount)
                {
                    MD5 = md5,
                    CreatePathIsNotExists = createPathIsNotExists,
                });
        }
    }
}
