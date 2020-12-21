using LINGYUN.Platform.Permissions;
using LINGYUN.Platform.Utils;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Datas
{
    [Authorize(PlatformPermissions.DataDictionary.Default)]
    public class DataAppService : PlatformApplicationServiceBase, IDataAppService
    {
        protected IDataRepository DataRepository { get; }

        public DataAppService(
            IDataRepository dataRepository)
        {
            DataRepository = dataRepository;
        }

        [Authorize(PlatformPermissions.DataDictionary.Create)]
        public virtual async Task<DataDto> CreateAsync(DataCreateDto input)
        {
            var data = await DataRepository.FindByNameAsync(input.Name);
            if (data != null)
            {
                throw new UserFriendlyException("指定名称的数据字典已经存在!");
            }

            string code = string.Empty;
            var children = await DataRepository.GetChildrenAsync(input.ParentId);
            if (children.Any())
            {
                var lastChildren = children.OrderBy(x => x.Code).FirstOrDefault();
                code = CodeNumberGenerator.CalculateNextCode(lastChildren.Code);
            }
            else
            {
                var parentData = input.ParentId != null
                ? await DataRepository.GetAsync(input.ParentId.Value)
                : null;

                code = CodeNumberGenerator.AppendCode(parentData?.Code, CodeNumberGenerator.CreateCode(1));
            }

            data = new Data(
                GuidGenerator.Create(),
                input.Name,
                code,
                input.DisplayName,
                input.Description,
                input.ParentId,
                CurrentTenant.Id
                );

            data = await DataRepository.InsertAsync(data);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Data, DataDto>(data);
        }

        [Authorize(PlatformPermissions.DataDictionary.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var data = await DataRepository.GetAsync(id);

            var children = await DataRepository.GetChildrenAsync(data.Id);
            if (children.Any())
            {
                throw new UserFriendlyException("当前数据字典存在子节点,无法删除!");
            }

            await DataRepository.DeleteAsync(data);
        }

        public virtual async Task<DataDto> GetAsync(Guid id)
        {
            var data = await DataRepository.GetAsync(id);

            return ObjectMapper.Map<Data, DataDto>(data);
        }

        public virtual async Task<ListResultDto<DataDto>> GetAllAsync()
        {
            var datas = await DataRepository.GetListAsync(includeDetails: false);

            return new ListResultDto<DataDto>(
                ObjectMapper.Map<List<Data>, List<DataDto>>(datas));
        }

        public virtual async Task<PagedResultDto<DataDto>> GetListAsync(GetDataListInput input)
        {
            var count = await DataRepository.GetCountAsync(input.Filter);

            var datas = await DataRepository.GetPagedListAsync(
                input.Filter, input.Sorting,
                false, input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<DataDto>(count,
                ObjectMapper.Map<List<Data>, List<DataDto>>(datas));
        }

        [Authorize(PlatformPermissions.DataDictionary.Update)]
        public virtual async Task<DataDto> UpdateAsync(Guid id, DataUpdateDto input)
        {
            var data = await DataRepository.GetAsync(id);

            if (!string.Equals(data.Name, input.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                data.Name = input.Name;
            }
            if (!string.Equals(data.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                data.DisplayName = input.DisplayName;
            }
            if (!string.Equals(data.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                data.Description = input.Description;
            }

            data = await DataRepository.UpdateAsync(data);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Data, DataDto>(data);
        }

        [Authorize(PlatformPermissions.DataDictionary.ManageItems)]
        public virtual async Task UpdateItemAsync(Guid id, string name, DataItemUpdateDto input)
        {
            var data = await DataRepository.GetAsync(id);
            var dataItem = data.FindItem(name);
            if (dataItem == null)
            {
                throw new UserFriendlyException($"不存在名为 {name} 的数据字典项!");
            }

            if (!string.Equals(dataItem.DefaultValue, input.DefaultValue, StringComparison.InvariantCultureIgnoreCase))
            {
                dataItem.DefaultValue = input.DefaultValue;
            }
            if (!string.Equals(dataItem.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                dataItem.DisplayName = input.DisplayName;
            }
            if (!string.Equals(dataItem.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                dataItem.Description = input.Description;
            }
            dataItem.AllowBeNull = input.AllowBeNull;

            data = await DataRepository.UpdateAsync(data);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(PlatformPermissions.DataDictionary.ManageItems)]
        public virtual async Task CreateItemAsync(Guid id, DataItemCreateDto input)
        {
            var data = await DataRepository.GetAsync(id);
            var dataItem = data.FindItem(input.Name);
            if (dataItem != null)
            {
                throw new UserFriendlyException($"已经存在名为 {input.Name} 的数据字典项!");
            }

            data.AddItem(
                GuidGenerator,
                input.Name,
                input.DisplayName,
                input.DefaultValue,
                input.ValueType,
                input.Description,
                input.AllowBeNull);

            await DataRepository.UpdateAsync(data);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(PlatformPermissions.DataDictionary.ManageItems)]
        public virtual async Task DeleteItemAsync(Guid id, string name)
        {
            var data = await DataRepository.GetAsync(id);
            data.RemoveItem(name);

            await DataRepository.UpdateAsync(data);
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
