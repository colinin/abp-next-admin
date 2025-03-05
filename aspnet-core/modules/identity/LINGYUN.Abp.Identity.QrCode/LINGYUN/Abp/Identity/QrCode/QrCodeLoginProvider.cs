using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;

namespace LINGYUN.Abp.Identity.QrCode;

public class QrCodeLoginProvider : IQrCodeLoginProvider, ITransientDependency
{
    protected IdentityUserManager UserManager { get; }
    protected IStringLocalizer<IdentityResource> L { get; }
    protected IDistributedCache<QrCodeCacheItem> QrCodeCache { get; }

    public QrCodeLoginProvider(
        IStringLocalizer<IdentityResource> stringLocalizer,
        IDistributedCache<QrCodeCacheItem> qrCodeCache,
        IdentityUserManager userManager)
    {
        L = stringLocalizer;
        QrCodeCache = qrCodeCache;
        UserManager = userManager;
    }

    public async virtual Task<QrCodeInfo> GenerateAsync()
    {
        var key = Guid.NewGuid().ToString("n");

        var cacheItem = new QrCodeCacheItem(key);

        await QrCodeCache.SetAsync(key, cacheItem,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(180),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
            });

        return cacheItem.GetQrCodeInfo();
    }

    public async virtual Task<QrCodeInfo> GetCodeAsync(string key)
    {
        var cacheItem = await QrCodeCache.GetAsync(key);
        if (cacheItem == null)
        {
            var qrCodeInfo = new QrCodeInfo(key);
            qrCodeInfo.SetStatus(QrCodeStatus.Invalid);

            return qrCodeInfo;
        }

        return cacheItem.GetQrCodeInfo();
    }

    public async virtual Task<QrCodeInfo> ScanCodeAsync(string key, QrCodeScanParams @params)
    {
        var cacheItem = await QrCodeCache.GetAsync(key);
        if (cacheItem == null)
        {
            var qrCodeInfo = new QrCodeInfo(key);
            qrCodeInfo.SetStatus(QrCodeStatus.Invalid);

            return qrCodeInfo;
        }

        if (cacheItem.Status == QrCodeStatus.Scaned)
        {
            return cacheItem.GetQrCodeInfo();
        }

        cacheItem.UserId = @params.UserId;
        cacheItem.UserName = @params.UserName;
        cacheItem.Picture = @params.Picture;
        cacheItem.TenantId = @params.TenantId;
        cacheItem.Status = QrCodeStatus.Scaned;

        await QrCodeCache.SetAsync(key, cacheItem,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(180),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
            });

        return cacheItem.GetQrCodeInfo();
    }

    public async virtual Task<QrCodeInfo> ConfirmCodeAsync(string key)
    {
        var cacheItem = await QrCodeCache.GetAsync(key);
        if (cacheItem == null)
        {
            var qrCodeInfo = new QrCodeInfo(key);
            qrCodeInfo.SetStatus(QrCodeStatus.Invalid);

            return qrCodeInfo;
        }

        if (cacheItem.Status == QrCodeStatus.Confirmed)
        {
            return cacheItem.GetQrCodeInfo();
        }

        if (cacheItem.UserId.IsNullOrWhiteSpace())
        {
            throw new UserFriendlyException(L["QrCode:NotScaned"]);
        }

        var token = await GenerateConfirmToken(cacheItem.UserId);

        cacheItem.Token = token;
        cacheItem.Status = QrCodeStatus.Confirmed;

        await QrCodeCache.SetAsync(key, cacheItem,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(180),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
            });

        return cacheItem.GetQrCodeInfo();
    }

    public async virtual Task RemoveAsync(string key)
    {
        var cacheItem = await QrCodeCache.GetAsync(key);
        if (cacheItem != null)
        {
            await QrCodeCache.RemoveAsync(key);
        }
    }

    protected async virtual Task<string> GenerateConfirmToken(string userId)
    {
        var user = await UserManager.FindByIdAsync(userId);

        return await UserManager.GenerateUserTokenAsync(user,
            QrCodeLoginProviderConsts.Name,
            QrCodeLoginProviderConsts.Purpose);
    }}
