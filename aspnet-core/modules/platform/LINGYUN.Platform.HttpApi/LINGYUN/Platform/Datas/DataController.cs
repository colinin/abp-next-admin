using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Datas
{
    [RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
    [Area("platform")]
    [Route("api/platform/datas")]
    public class DataController : AbpController, IDataAppService
    {
        protected IDataAppService DataAppService { get; }

        public DataController(
            IDataAppService dataAppService)
        {
            DataAppService = dataAppService;
        }

        [HttpPost]
        public virtual async Task<DataDto> CreateAsync(DataCreateDto input)
        {
            return await DataAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("{id}/items")]
        public virtual async Task CreateItemAsync(Guid id, DataItemCreateDto input)
        {
            await DataAppService.CreateItemAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await DataAppService.DeleteAsync(id);
        }

        [HttpDelete]
        [Route("{id}/items/{name}")]
        public virtual async Task DeleteItemAsync(Guid id, string name)
        {
            await DataAppService.DeleteItemAsync(id, name);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<DataDto> GetAsync(Guid id)
        {
            return await DataAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("all")]
        public virtual async Task<ListResultDto<DataDto>> GetAllAsync()
        {
            return await DataAppService.GetAllAsync();
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<DataDto>> GetListAsync(GetDataListInput input)
        {
            return await DataAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<DataDto> UpdateAsync(Guid id, DataUpdateDto input)
        {
            return await DataAppService.UpdateAsync(id, input);
        }

        [HttpPut]
        [Route("{id}/items/{name}")]
        public virtual async Task UpdateItemAsync(Guid id, string name, DataItemUpdateDto input)
        {
            await DataAppService.UpdateItemAsync(id, name, input);
        }
    }
}
