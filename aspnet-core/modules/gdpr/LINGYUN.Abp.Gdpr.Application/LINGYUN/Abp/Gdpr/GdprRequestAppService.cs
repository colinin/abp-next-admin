using LINGYUN.Abp.Exporter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Gdpr;
using Volo.Abp.Json;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Gdpr;

[Authorize]
public class GdprRequestAppService(
    IJsonSerializer jsonSerializer,
    IExporterProvider exporterProvider,
    IOptions<AbpGdprOptions> gdprOptions,
    IDistributedEventBus distributedEventBus,
    IDistributedCache<GdprRequestCacheItem> gdprRequestCache,
    IGdprRequestRepository gdprRequestRepository
) : GdprApplicationServiceBase, 
    IGdprRequestAppService
{
    public async virtual Task DeleteAsync(Guid id)
    {
        var gdprRequest = await gdprRequestRepository.GetAsync(id);

        await gdprRequestRepository.DeleteAsync(gdprRequest);
    }

    public async virtual Task DeletePersonalAccountAsync()
    {
        var userId = CurrentUser.GetId();

        await DeletePersonalDataAsync();

        await distributedEventBus.PublishAsync(
            new GdprUserAccountDeletionRequestedEto()
            {
                UserId = userId,
            });
    }

    public async virtual Task DeletePersonalDataAsync()
    {
        var userId = CurrentUser.GetId();

        var specification = new Volo.Abp.Specifications.ExpressionSpecification<GdprRequest>(x => x.UserId == userId);

        var totalCount = await gdprRequestRepository.GetCountAsync(specification);
        var gdprRequests = await gdprRequestRepository.GetListAsync(specification, maxResultCount: totalCount);

        await gdprRequestRepository.DeleteManyAsync(gdprRequests);

        await CurrentUnitOfWork!.SaveChangesAsync();

        await distributedEventBus.PublishAsync(
            new GdprUserDataDeletionRequestedEto()
            {
                UserId = userId,
            });
    }

    public async virtual Task<GdprRequestDto> GetAsync(Guid id)
    {
        var gdprRequest = await gdprRequestRepository.GetAsync(id);

        return ObjectMapper.Map<GdprRequest, GdprRequestDto>(gdprRequest);
    }

    public async virtual Task<PagedResultDto<GdprRequestDto>> GetListAsync(GdprRequestGetListInput input)
    {
        Expression<Func<GdprRequest, bool>> expression = x => x.UserId == CurrentUser.GetId();

        if (!input.CreationTime.IsNullOrWhiteSpace())
        {
            var times = input.CreationTime.Split(';');
            if (times.Length >= 1 && DateTime.TryParse(times[0], out var beginCreationTime))
            {
                expression = expression.And(x => x.CreationTime >= beginCreationTime);
            }
            if (times.Length >= 2 && DateTime.TryParse(times[1], out var endCreationTime))
            {
                expression = expression.And(x => x.CreationTime <= endCreationTime);
            }
        }
        if (!input.ReadyTime.IsNullOrWhiteSpace())
        {
            var times = input.ReadyTime.Split(';');
            if (times.Length >= 1 && DateTime.TryParse(times[0], out var beginReadyTime))
            {
                expression = expression.And(x => x.ReadyTime >= beginReadyTime);
            }
            if (times.Length >= 2 && DateTime.TryParse(times[1], out var endReadyTime))
            {
                expression = expression.And(x => x.ReadyTime <= endReadyTime);
            }
        }

        var specification = new Volo.Abp.Specifications.ExpressionSpecification<GdprRequest>(expression);

        var totalCount = await gdprRequestRepository.GetCountAsync(specification);
        var gdprRequests = await gdprRequestRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<GdprRequestDto>(totalCount,
            ObjectMapper.Map<List<GdprRequest>, List<GdprRequestDto>>(gdprRequests));
    }

    public async virtual Task<IRemoteStreamContent> DownloadPersonalDataAsync(Guid requestId)
    {
        var userId = CurrentUser.GetId();
        var cacheKey = GdprRequestCacheItem.CalculateCacheKey(userId, requestId);
        var cacheItem = await gdprRequestCache.GetAsync(cacheKey);
        if (cacheItem == null)
        {
            var gdprRequest = await gdprRequestRepository.GetAsync(requestId);
            if (Clock.Now < gdprRequest.ReadyTime)
            {
                throw new BusinessException(GdprErrorCodes.DataNotPreparedYet)
                    .WithData(nameof(GdprRequest.ReadyTime), gdprRequest.ReadyTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            cacheItem = new GdprRequestCacheItem(userId, requestId)
            {
                Infos = gdprRequest.Infos.Select(x => new GdprInfoCacheItem(x.Provider, x.Data)).ToList()
            };

            await gdprRequestCache.SetAsync(cacheKey, cacheItem, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = gdprOptions.Value.RequestTimeInterval
            });
        }

        return await DownloadUserData(cacheItem);
    }

    public async virtual Task PreparePersonalDataAsync()
    {
        var userId = CurrentUser.GetId();

        await ValidationRequestTimeInterval(userId);

        var gdprRequest = new GdprRequest(
            GuidGenerator.Create(),
            userId,
            Clock.Now.Add(gdprOptions.Value.MinutesForDataPreparation));

        await gdprRequestRepository.InsertAsync(gdprRequest);

        await CurrentUnitOfWork!.SaveChangesAsync();

        await distributedEventBus.PublishAsync(
            new GdprUserDataRequestedEto()
            {
                RequestId = gdprRequest.Id,
                UserId = gdprRequest.UserId,
            });
    }

    protected async virtual Task<IRemoteStreamContent> DownloadUserData(GdprRequestCacheItem cacheItem)
    {
        var fileName = $"{L["PersonalData"]}.xlsx";

        var emptyValue = !L["PersonalData:Empty"].ResourceNotFound ? L["PersonalData:Empty"] : "None";

        var personalData = new List<Dictionary<string, string>>();

        foreach (var gdprInfo in cacheItem.Infos)
        {
            var gdprData = new Dictionary<string, string>();
            var gdprInfoData = jsonSerializer.Deserialize<Dictionary<string, string>>(gdprInfo.Data);

            foreach (var gdprDataItem in gdprInfoData)
            {
                // 导出数据本地化
                var gdprDataKey = gdprDataItem.Key;
                var gdprDataLocalizaztionKey = L[$"PersonalData:{gdprDataKey.ToPascalCase()}"];
                if (!gdprDataLocalizaztionKey.ResourceNotFound)
                {
                    gdprDataKey = gdprDataLocalizaztionKey.Value;
                }
                gdprData[gdprDataKey] = gdprDataItem.Value.IsNullOrWhiteSpace() ? emptyValue : gdprDataItem.Value;
            }
            personalData.Add(gdprData);
        }
        var stream = await exporterProvider.ExportAsync(personalData);

        return new RemoteStreamContent(stream, fileName);
    }

    protected async Task ValidationRequestTimeInterval(Guid userId)
    {
        var latestRequestTime = await gdprRequestRepository.FindLatestRequestTimeAsync(userId);
        if (latestRequestTime.HasValue && (Clock.Now - latestRequestTime) < gdprOptions.Value.RequestTimeInterval)
        {
            var nextRequestTime = latestRequestTime.Value.Add(gdprOptions.Value.RequestTimeInterval);

            throw new BusinessException(GdprErrorCodes.PersonalDataRequestAlreadyDays)
                .WithData(nameof(AbpGdprOptions.RequestTimeInterval), nextRequestTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
