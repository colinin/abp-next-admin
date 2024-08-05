using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Datas;

public interface IDataAppService : 
    ICrudAppService<
        DataDto,
        Guid,
        GetDataListInput,
        DataCreateDto,
        DataUpdateDto>
{
    Task<DataDto> GetAsync(string name);

    Task<ListResultDto<DataDto>> GetAllAsync();

    Task<DataDto> MoveAsync(Guid id, DataMoveDto input);

    Task CreateItemAsync(Guid id, DataItemCreateDto input);

    Task UpdateItemAsync(Guid id, string name, DataItemUpdateDto input);

    Task DeleteItemAsync(Guid id, string name);
}
