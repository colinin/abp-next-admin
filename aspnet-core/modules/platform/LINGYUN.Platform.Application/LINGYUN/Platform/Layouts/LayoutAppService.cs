using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Layouts;

[Authorize(PlatformPermissions.Layout.Default)]
public class LayoutAppService : PlatformApplicationServiceBase, ILayoutAppService
{
    protected ILayoutRepository LayoutRepository { get; }

    public LayoutAppService(
        ILayoutRepository layoutRepository)
    {
        LayoutRepository = layoutRepository;
    }

    [Authorize(PlatformPermissions.Layout.Create)]
    public async virtual Task<LayoutDto> CreateAsync(LayoutCreateDto input)
    {
        var layout = await LayoutRepository.FindByNameAsync(input.Name);
        if (layout != null)
        {
            throw new UserFriendlyException(L["DuplicateLayout", input.Name]);
        }

        layout = new Layout(
            GuidGenerator.Create(),
            input.Path,
            input.Name,
            input.DisplayName,
            input.DataId,
            input.Framework,
            input.Redirect,
            input.Description,
            CurrentTenant.Id);

        layout = await LayoutRepository.InsertAsync(layout);
        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Layout, LayoutDto>(layout);
    }

    [Authorize(PlatformPermissions.Layout.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var layout = await LayoutRepository.GetAsync(id);

        //if (await LayoutRepository.AnyMenuAsync(layout.Id))
        //{
        //    throw new UserFriendlyException($"不能删除存在菜单的布局!");
        //}

        await LayoutRepository.DeleteAsync(layout);
        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<LayoutDto> GetAsync(Guid id)
    {
        var layout = await LayoutRepository.GetAsync(id);

        return ObjectMapper.Map<Layout, LayoutDto>(layout);
    }

    public async virtual Task<ListResultDto<LayoutDto>> GetAllListAsync()
    {
        var layouts = await LayoutRepository.GetListAsync();

        return new ListResultDto<LayoutDto>(
            ObjectMapper.Map<List<Layout>, List<LayoutDto>>(layouts));
    }

    public async virtual Task<PagedResultDto<LayoutDto>> GetListAsync(GetLayoutListInput input)
    {
        var count = await LayoutRepository.GetCountAsync(input.Framework, input.Filter);

        var layouts = await LayoutRepository.GetPagedListAsync(
            input.Framework, input.Filter,
            input.Sorting, false,
            input.SkipCount, input.MaxResultCount);

        return new PagedResultDto<LayoutDto>(count,
            ObjectMapper.Map<List<Layout>, List<LayoutDto>>(layouts));
    }

    [Authorize(PlatformPermissions.Layout.Update)]
    public async virtual Task<LayoutDto> UpdateAsync(Guid id, LayoutUpdateDto input)
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
