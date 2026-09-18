using System;
using System.Text;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace LINGYUN.Abp.LazyCaptcha;

public class LazyCaptchaNormalizer(
    ICurrentClient _currentClient,
    ICurrentUser _currentUser,
    ICurrentTenant _currentTenant) : ILazyCaptchaNormalizer, ISingletonDependency
{
    protected const char SeparatorChar = ';';
    public virtual string NormalizeCaptchaId(string captchaId)
    {
        return captchaId;
    }

    public virtual string NormalizeRateLimitKey(string captchaId)
    {
        var rateLimitKeyBuilder = new StringBuilder();
        if (_currentTenant.Id.HasValue)
        {
            rateLimitKeyBuilder.Append("t:");
            rateLimitKeyBuilder.Append(_currentTenant.Id.Value.ToString("N"));
            rateLimitKeyBuilder.Append(SeparatorChar);
        }

        if (!_currentClient.Id.IsNullOrWhiteSpace())
        {
            rateLimitKeyBuilder.Append("c:");
            rateLimitKeyBuilder.Append(_currentClient.Id);
            rateLimitKeyBuilder.Append(SeparatorChar);
        }

        if (_currentUser.Id.HasValue)
        {
            rateLimitKeyBuilder.Append("u:");
            rateLimitKeyBuilder.Append(_currentUser.Id.Value.ToString("N"));
            rateLimitKeyBuilder.Append(SeparatorChar);
        }

        return rateLimitKeyBuilder.ToString();
    }
}
