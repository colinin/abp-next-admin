using LINGYUN.Abp.LocalizationManagement.Features;
using LINGYUN.Abp.LocalizationManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;

namespace LINGYUN.Abp.LocalizationManagement;

[RequiresFeature(LocalizationManagementFeatures.Enable)]
[Authorize(LocalizationManagementPermissions.Text.Default)]
public class TextAppService : LocalizationAppServiceBase, ITextAppService
{
    private readonly ITextRepository _textRepository;
    public TextAppService(ITextRepository repository)
    {
        _textRepository = repository;
    }

    public async virtual Task<TextDto> GetByKeyAsync(TextGetByKeyInput input)
    {
        var text = await _textRepository.GetByCultureKeyAsync(input.ResourceName, input.CultureName, input.Key);

        return new TextDto
        {
            Key = input.Key,
            CultureName = input.CultureName,
            ResourceName = input.ResourceName,
            Value = text?.Value
        };
    }

    public async virtual Task<ListResultDto<TextDifferenceDto>> GetDifferencesAsync(TextDifferenceGetListInput input)
    {
        Expression<Func<Text, bool>> basicPredicate = _ => true;
        if (!input.ResourceName.IsNullOrWhiteSpace())
        {
            basicPredicate = basicPredicate.And(x => x.ResourceName == input.ResourceName);
        }
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            basicPredicate = basicPredicate.And(x => x.ResourceName.Contains(input.Filter) || x.Key.Contains(input.Filter));
        }

        var queryable = await _textRepository.GetQueryableAsync();

        var sourceQueryable = queryable.Where(basicPredicate.And(x => x.CultureName == input.CultureName));
        var targetQueryable = queryable.Where(basicPredicate.And(x => x.CultureName == input.TargetCultureName));

        var query = from source in sourceQueryable
                    join target in targetQueryable
                        on new { source.ResourceName, source.Key }
                        equals new { target.ResourceName, target.Key } into joined
                    from target in joined.DefaultIfEmpty()
                    select new
                    {
                        Source = source,
                        Target = target,
                        IsMissingOrEmpty = target == null || string.IsNullOrEmpty(target.Value)
                    };

        if (input.OnlyNull == true)
        {
            query = query.Where(x => x.IsMissingOrEmpty);
        }

        var texts = await AsyncExecuter.ToListAsync(query
            .Select(x => new TextDifferenceDto
            {
                ResourceName = x.Source.ResourceName,
                CultureName = x.Source.CultureName,
                Key = x.Source.Key,
                Value = x.Source.Value,
                TargetCultureName = input.TargetCultureName,
                TargetValue = x.Target != null ? x.Target.Value : null,
            }));

        return new ListResultDto<TextDifferenceDto>(texts);
    }

    public async virtual Task SetTextAsync(SetTextInput input)
    {
        var text = await _textRepository.GetByCultureKeyAsync(input.ResourceName, input.CultureName, input.Key);
        if (text == null)
        {
            await AuthorizationService.CheckAsync(LocalizationManagementPermissions.Text.Create);

            text = new Text(
                input.ResourceName,
                input.CultureName,
                input.Key,
                input.Value);

            await _textRepository.InsertAsync(text);
        }
        else
        {
            await AuthorizationService.CheckAsync(LocalizationManagementPermissions.Text.Update);

            text.SetValue(input.Value);

            await _textRepository.UpdateAsync(text);
        }

        await PublishDynamicLocalizationRefreshEvent(new DynamicTextRefreshEventData(text.ResourceName, text.CultureName));

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(LocalizationManagementPermissions.Text.Delete)]
    public async virtual Task DeleteAsync(TextDeleteInput input)
    {
        var text = await _textRepository.GetByCultureKeyAsync(input.ResourceName, input.CultureName, input.Key);
        if (text != null)
        {
            await _textRepository.DeleteAsync(text);

            await PublishDynamicLocalizationRefreshEvent(new DynamicTextRefreshEventData(text.ResourceName, text.CultureName));

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    [Authorize(LocalizationManagementPermissions.Text.Delete)]
    [Obsolete("This interface will be removed in the next version. Please use DeleteAsync instead.")]
    public async virtual Task RestoreToDefaultAsync(RestoreDefaultTextInput input)
    {
        var text = await _textRepository.GetByCultureKeyAsync(input.ResourceName, input.CultureName, input.Key);
        if (text != null)
        {
            await _textRepository.DeleteAsync(text);

            await PublishDynamicLocalizationRefreshEvent(new DynamicTextRefreshEventData(text.ResourceName, text.CultureName));

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
