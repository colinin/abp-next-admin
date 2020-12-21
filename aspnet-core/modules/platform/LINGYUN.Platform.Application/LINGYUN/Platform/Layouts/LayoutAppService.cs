using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Permissions;
using LINGYUN.Platform.Utils;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Layouts
{
    [Authorize(PlatformPermissions.Layout.Default)]
    public class LayoutAppService : PlatformApplicationServiceBase, ILayoutAppService
    {
        protected IDataRepository DataRepository { get; }
        protected ILayoutRepository LayoutRepository { get; }

        public LayoutAppService(
            IDataRepository dataRepository,
            ILayoutRepository layoutRepository)
        {
            DataRepository = dataRepository;
            LayoutRepository = layoutRepository;
        }

        [Authorize(PlatformPermissions.Layout.Create)]
        public virtual async Task<LayoutDto> CreateAsync(LayoutCreateDto input)
        {
            var data = DataRepository.FindAsync(input.DataId);
            if (data == null)
            {
                throw new UserFriendlyException($"数据字典 {input.DataId} 的不存在或已删除!");
            }

            var layout = await LayoutRepository.FindByNameAsync(input.Name);
            if (layout != null)
            {
                throw new UserFriendlyException($"已经存在名为 {input.Name} 的布局!");
            }

            var lastLayout = await LayoutRepository.GetLastOrNullAsync();
            var code = lastLayout != null
                ? CodeNumberGenerator.CalculateNextCode(lastLayout.Code)
                : CodeNumberGenerator.CreateCode(1);

            layout = new Layout(
                GuidGenerator.Create(),
                input.Path,
                input.Name,
                code,
                input.DisplayName,
                input.DataId,
                input.PlatformType,
                input.Redirect,
                input.Description,
                CurrentTenant.Id);

            layout = await LayoutRepository.InsertAsync(layout);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Layout, LayoutDto>(layout);
        }

        [Authorize(PlatformPermissions.Layout.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var layout = await LayoutRepository.GetAsync(id);

            //if (await LayoutRepository.AnyMenuAsync(layout.Id))
            //{
            //    throw new UserFriendlyException($"不能删除存在菜单的布局!");
            //}

            await LayoutRepository.DeleteAsync(layout);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<LayoutDto> GetAsync(Guid id)
        {
            var layout = await LayoutRepository.GetAsync(id);

            return ObjectMapper.Map<Layout, LayoutDto>(layout);
        }

        public virtual async Task<PagedResultDto<LayoutDto>> GetListAsync(GetLayoutListInput input)
        {
            var count = await LayoutRepository.GetCountAsync(input.PlatformType, input.Filter);

            var layouts = await LayoutRepository.GetListAsync(
                input.PlatformType, input.Filter,
                input.Sorting, input.Reverse, false,
                input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<LayoutDto>(count,
                ObjectMapper.Map<List<Layout>, List<LayoutDto>>(layouts));
        }

        [Authorize(PlatformPermissions.Layout.Update)]
        public virtual async Task<LayoutDto> UpdateAsync(Guid id, LayoutUpdateDto input)
        {
            var layout = await LayoutRepository.GetAsync(id);

            if (!string.Equals(layout.Name, input.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                layout.Name = input.Name;
            }
            if (!string.Equals(layout.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                layout.DisplayName = input.DisplayName;
            }
            if (!string.Equals(layout.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                layout.Description = input.Description;
            }
            if (!string.Equals(layout.Path, input.Path, StringComparison.InvariantCultureIgnoreCase))
            {
                layout.Path = input.Path;
            }
            if (!string.Equals(layout.Redirect, input.Redirect, StringComparison.InvariantCultureIgnoreCase))
            {
                layout.Redirect = input.Redirect;
            }
            layout = await LayoutRepository.UpdateAsync(layout);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Layout, LayoutDto>(layout);
        }
    }
}
