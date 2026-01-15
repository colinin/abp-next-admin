using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Datas;

[Authorize(PlatformPermissions.DataDictionary.Default)]
public class GlobalDataAppService : PlatformApplicationServiceBase, IGlobalDataAppService
{
    protected IDataRepository DataRepository { get; }

    public GlobalDataAppService(
        IDataRepository dataRepository)
    {
        DataRepository = dataRepository;
    }

    public async virtual Task<DataDto> GetByNameAsync(string name)
    {
        using (DataFilter.Disable<IMultiTenant>())
        {
            var data = await DataRepository.FindByNameAsync(name) ??
                throw new EntityNotFoundException(typeof(Data), name);

            return ObjectMapper.Map<Data, DataDto>(data);
        }
    }

    public async virtual Task<DataDto> GetAsync(Guid id)
    {
        using (DataFilter.Disable<IMultiTenant>())
        {
            var data = await DataRepository.GetAsync(id);

            return ObjectMapper.Map<Data, DataDto>(data);
        }
    }

    public async virtual Task<ListResultDto<DataDto>> GetAllAsync()
    {
        using (DataFilter.Disable<IMultiTenant>())
        {
            var datas = await DataRepository.GetListAsync(includeDetails: false);

            return new ListResultDto<DataDto>(
                ObjectMapper.Map<List<Data>, List<DataDto>>(datas));
        }
    }
}
