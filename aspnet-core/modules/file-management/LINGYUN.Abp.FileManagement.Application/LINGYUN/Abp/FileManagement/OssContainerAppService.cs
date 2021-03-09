using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.FileManagement
{
    public class OssContainerAppService : FileManagementApplicationServiceBase, IOssContainerAppService
    {
        protected IOssContainerFactory OssContainerFactory { get; }

        public OssContainerAppService(
            IOssContainerFactory ossContainerFactory)
        {
            OssContainerFactory = ossContainerFactory;
        }

        public virtual async Task<OssContainerDto> CreateAsync(string name)
        {
            var oss = CreateOssContainer();
            var container = await oss.CreateAsync(name);

            return ObjectMapper.Map<OssContainer, OssContainerDto>(container);
        }

        public virtual async Task DeleteAsync(string name)
        {
            var oss = CreateOssContainer();

            await oss.DeleteAsync(name);
        }

        public virtual async Task<OssContainerDto> GetAsync(string name)
        {
            var oss = CreateOssContainer();
            var container = await oss.GetAsync(name);

            return ObjectMapper.Map<OssContainer, OssContainerDto>(container);
        }

        public virtual async Task<OssContainersResultDto> GetListAsync(GetOssContainersInput input)
        {
            var oss = CreateOssContainer();

            var containerResponse = await oss.GetListAsync(
                input.Prefix, input.Marker, input.MaxResultCount);

            return ObjectMapper.Map<GetOssContainersResponse, OssContainersResultDto>(containerResponse);
        }

        public virtual async Task<OssObjectsResultDto> GetObjectListAsync(GetOssObjectsInput input)
        {
            var oss = CreateOssContainer();

            var ossObjectResponse = await oss.GetObjectsAsync(
                input.Bucket, input.Prefix, input.Marker,
                input.Delimiter, input.EncodingType,
                input.MaxResultCount);

            return ObjectMapper.Map<GetOssObjectsResponse, OssObjectsResultDto>(ossObjectResponse);
        }

        protected virtual IOssContainer CreateOssContainer()
        {
            return OssContainerFactory.Create();
        }
    }
}
