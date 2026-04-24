using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account.Web.OpenIddict.Pages.Account;

[Authorize]
public class SelectAccountModel : AccountPageModel
{
    private const string LastLoginTimeFieldName = "LastLoginTime";
    private const string AllowedTenantsFieldName = "AllowedTenants";
    public const string DefaultDateFormat = "yyyy-MM-dd HH:mm:ss";
    private OriginalRequestInfo _originalRequest;

    [BindProperty(SupportsGet = true)]
    public string RedirectUri { get; set; }

    public string ClientName { get; set; }

    public string UserName { get; set; }

    [BindProperty]
    public SelectAccountInput Input { get; set; }

    public List<UserAccountInfo> AvailableAccounts { get; set; } = new();

    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache => LazyServiceProvider.LazyGetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();

    protected IOpenIddictApplicationManager ApplicationManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictApplicationManager>();

    protected ITenantStore TenantStore => LazyServiceProvider.LazyGetRequiredService<ITenantStore>();

    public SelectAccountModel()
    {
        LocalizationResourceType = typeof(AbpOpenIddictResource);
    }

    public async virtual Task<IActionResult> OnGetAsync()
    {
        // 检查用户是否已登录
        if (!User.Identity.IsAuthenticated)
        {
            // 未登录，重定向到登录页面
            return RedirectToPage("/Account/Login", new
            {
                ReturnUrl = Url.Page("/Account/SelectAccount", new { RedirectUri }),
                Prompt = "select_account"
            });
        }

        _originalRequest = await ParseOriginalRequestFromRedirectUriAsync();
        if (_originalRequest == null)
        {
            Alerts.Warning(L["InvalidSelectAccountRequest"]);
            return Page();
        }

        Input = new SelectAccountInput
        {
            RedirectUri = _originalRequest.RedirectUri,
            ClientId = _originalRequest.ClientId,
        };

        var application = await ApplicationManager.FindByClientIdAsync(_originalRequest.ClientId);
        ClientName = await ApplicationManager.GetLocalizedDisplayNameAsync(application) ?? _originalRequest.ClientId;

        var currentUser = await UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            await SignInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new 
            {
                ReturnUrl = _originalRequest.RedirectUri
            });
        }

        UserName = currentUser.UserName;

        AvailableAccounts = await DiscoverUserAccountsAsync(currentUser, _originalRequest.ClientId);

        if (AvailableAccounts.Count == 0)
        {
            Alerts.Warning(L["NoAvailableAccounts"]);
        }

        // 仅关联一个账户时直接登录
        if (AvailableAccounts.Count == 1)
        {
            return await HandleLoginAsync(currentUser, Input.RememberMe, Input.RememberSelection);
        }

        return Page();
    }

    public async virtual Task<IActionResult> OnPostAsync()
    {
        try
        {
            ValidateModel();

            await IdentityOptions.SetAsync();

            var tenantUser = ParseSelectedAccountId(Input.SelectedAccountId);
            if (tenantUser == null)
            {
                Alerts.Warning(L["InvalidSelectAccount"]);
                return Page();
            }

            var user = await ValidateSelectedAccountAsync(tenantUser.UserId, tenantUser.TenantId);
            if (user == null)
            {
                Alerts.Warning(L["InvalidSelectAccount"]);
                return Page();
            }

            using (CurrentTenant.Change(tenantUser.TenantId))
            {
                return await HandleLoginAsync(user, Input.RememberMe, Input.RememberSelection);
            }
        }
        catch (AbpIdentityResultException e)
        {
            if (!string.IsNullOrWhiteSpace(e.Message))
            {
                Alerts.Warning(GetLocalizeExceptionMessage(e));
                return Page();
            }

            throw;
        }
        catch (AbpValidationException)
        {
            return Page();
        }
    }

    protected async virtual Task<IActionResult> HandleLoginAsync(IdentityUser user, bool rememberMe = false, bool rememberSelection = false)
    {
        // 检查是否已经是当前用户
        var currentUser = await UserManager.GetUserAsync(User);
        if (currentUser == null || currentUser.Id == user.Id)
        {
            // 使用选择的账户登录
            // TODO: 实现 RememberMe
            await SignInManager.SignInAsync(user, rememberMe);

            // TODO: date format
            user.SetProperty(LastLoginTimeFieldName, Clock.Now.ToString(DefaultDateFormat));

            // TODO: 实现 RememberSelection
            if (rememberSelection)
            {
                await SaveAccountSelectionAsync(Input.ClientId, user);
            }

            var logContext = new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.LoginSucceeded,
                UserName = user.UserName,
                ClientId = Input.ClientId,
            };
            logContext.WithProperty("prompt", "select_account");

            await IdentitySecurityLogManager.SaveAsync(logContext);

            // Clear the dynamic claims cache.
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
        }

        // 重定向回原始授权请求
        var originalAuthorizeUrl = await ReconstructOriginalAuthorizeUrlAsync();

        return Redirect(originalAuthorizeUrl);
    }

    protected async virtual Task<string> ReconstructOriginalAuthorizeUrlAsync()
    {
        if (_originalRequest == null)
        {
            return Input.RedirectUri;
        }
        // 构建完整的授权请求 URL
        var authorizeUrl = "/connect/authorize";
        var parameters = new Dictionary<string, string>
        {
            ["client_id"] = _originalRequest.ClientId,
            ["redirect_uri"] = _originalRequest.RedirectUri,
            ["response_type"] = _originalRequest.ResponseType ?? "code",
            ["scope"] = _originalRequest.Scope,
            ["state"] = _originalRequest.State,
            ["code_challenge"] = _originalRequest.CodeChallenge,
            ["code_challenge_method"] = _originalRequest.CodeChallengeMethod,
            ["prompt"] = "none" // 用户已认证，不需要再次提示
        };

        // 添加可选的 nonce 参数
        if (!string.IsNullOrEmpty(_originalRequest.Nonce))
        {
            parameters["nonce"] = _originalRequest.Nonce;
        }

        return QueryHelpers.AddQueryString(authorizeUrl, parameters);
    }

    protected virtual Task<OriginalRequestInfo> ParseOriginalRequestFromRedirectUriAsync()
    {
        if (string.IsNullOrWhiteSpace(RedirectUri))
        {
            return Task.FromResult<OriginalRequestInfo>(null);
        }

        try
        {
            var info = new OriginalRequestInfo();
            string queryString = null;

            if (RedirectUri.StartsWith("/"))
            {
                int queryIndex = RedirectUri.IndexOf('?');
                if (queryIndex >= 0)
                {
                    queryString = RedirectUri.Substring(queryIndex + 1);
                }
                else
                {
                    // 没有查询参数，尝试直接解析整个字符串
                    queryString = RedirectUri;
                }
            }
            else if (RedirectUri.Contains("://"))
            {
                try
                {
                    var uri = new Uri(RedirectUri);
                    queryString = uri.Query;
                    if (queryString.StartsWith("?"))
                    {
                        queryString = queryString.Substring(1);
                    }
                }
                catch (UriFormatException)
                {
                    queryString = RedirectUri;
                }
            }
            else
            {
                queryString = RedirectUri;
            }

            // 解析查询参数
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                var query = QueryHelpers.ParseQuery(queryString);

                info.ClientId = GetQueryValue(query, "client_id");
                info.RedirectUri = GetQueryValue(query, "redirect_uri");
                info.ResponseType = GetQueryValue(query, "response_type");
                info.Scope = GetQueryValue(query, "scope");
                info.State = GetQueryValue(query, "state");
                info.Nonce = GetQueryValue(query, "nonce");
                info.CodeChallenge = GetQueryValue(query, "code_challenge");
                info.CodeChallengeMethod = GetQueryValue(query, "code_challenge_method");
                info.Prompt = GetQueryValue(query, "prompt");
            }

            return Task.FromResult(info);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Parse the error of the RedirectUri parameter: {message}", ex.Message);
            return Task.FromResult<OriginalRequestInfo>(null);
        }
    }

    protected async virtual Task<List<UserAccountInfo>> DiscoverUserAccountsAsync(
        IdentityUser currentUser,
        string clientId)
    {
        var accounts = new List<UserAccountInfo>
        {
            new UserAccountInfo
            {
                IsCurrentAccount = true,
                UserId = currentUser.Id.ToString(),
                TenantId = CurrentTenant.Id,
                TenantName = CurrentTenant.Name,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                LastLoginTime = currentUser.GetProperty<DateTime?>(LastLoginTimeFieldName),
            }
        };

        // 获取客户端允许的租户
        var allowedTenants = await GetAllowedTenantsForClientAsync(clientId);

        foreach (var tenant in allowedTenants)
        {
            var tenantUserAccount = await GetTenantUserAccountInfoAsync(currentUser.UserName, tenant);
            if (tenantUserAccount != null)
            {
                accounts.Add(tenantUserAccount);
            }
        }

        // 按最后登录时间排序，当前账户排第一
        return accounts
            .OrderByDescending(a => a.IsCurrentAccount)
            .ThenByDescending(a => a.LastLoginTime)
            .ToList();
    }

    protected async virtual Task<UserAccountInfo> GetTenantUserAccountInfoAsync(string userName, TenantInfo tenant)
    {
        using (CurrentTenant.Change(tenant.Id, tenant.Name))
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                // 检查用户是否有效
                if (user.IsActive && !await UserManager.IsLockedOutAsync(user))
                {
                    var lastLoginTime = user.GetProperty<DateTime?>(LastLoginTimeFieldName);
                    return new UserAccountInfo
                    {
                        IsCurrentAccount = false,
                        UserId = user.Id.ToString(),
                        TenantId = tenant.Id,
                        TenantName = tenant.Name,
                        UserName = user.UserName,
                        Email = user.Email,
                        LastLoginTime = lastLoginTime,
                    };
                }
            }
            return null;
        }
    }

    protected async virtual Task<List<TenantInfo>> GetAllowedTenantsForClientAsync(string clientId)
    {
        var tenants = new List<TenantInfo>();

        if (string.IsNullOrWhiteSpace(clientId))
        {
            return tenants;
        }

        var application = await ApplicationManager.FindByClientIdAsync(clientId);
        if (application == null)
        {
            return tenants;
        }

        var properties = await ApplicationManager.GetPropertiesAsync(application);
        if (properties.TryGetValue(AllowedTenantsFieldName, out var allowedTenantsValue))
        {
            var tenantIds = allowedTenantsValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var tenantIdString in tenantIds)
            {
                if (Guid.TryParse(tenantIdString.Trim(), out var tenantId))
                {
                    var tenant = await TenantStore.FindAsync(tenantId);
                    if (tenant != null && tenant.IsActive)
                    {
                        tenants.Add(new TenantInfo
                        {
                            Id = tenant.Id,
                            Name = tenant.Name
                        });
                    }
                }
            }
        }

        return tenants;
    }

    protected virtual TenantUser ParseSelectedAccountId(string selectedAccountId)
    {
        // 解析账户格式为: UserId@TenantId
        if (string.IsNullOrWhiteSpace(selectedAccountId))
        {
            return new TenantUser();
        }

        var parts = selectedAccountId.Split('@', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 1)
        {
            return new TenantUser();
        }

        if (Guid.TryParse(parts[0], out var userId))
        {
            if (parts.Length > 1 &&
                Guid.TryParse(parts[1], out var tenantId))
            {
                return new TenantUser(tenantId, userId);
            }
            return new TenantUser(null, userId);
        }

        return new TenantUser();
    }

    protected virtual string GetQueryValue(Dictionary<string, StringValues> query, string key)
    {
        return query.TryGetValue(key, out var value) ? value.ToString() : null;
    }

    protected async virtual Task<IdentityUser> ValidateSelectedAccountAsync(Guid userId, Guid? tenantId)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            if (await UserManager.IsLockedOutAsync(user))
            {
                return null;
            }

            return user;
        }
    }

    protected virtual Task SaveAccountSelectionAsync(string clientId, IdentityUser user)
    {
        // TODO: 保存用户当前选择账户, 下次选择账户时默认选择此账户
        return Task.CompletedTask;
    }

    public class OriginalRequestInfo
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public string Nonce { get; set; }
        public string ResponseType { get; set; }
        public string CodeChallenge { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string Prompt { get; set; }
    }

    public class SelectAccountInput
    {
        [Required]
        public string SelectedAccountId { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string RedirectUri { get; set; }

        public bool RememberSelection { get; set; } = true;

        public bool RememberMe { get; set; } = true;
    }

    public class UserAccountInfo
    {
        public string UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string TenantName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool IsCurrentAccount { get; set; }
    }

    public class TenantInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class TenantUser
    {
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public TenantUser()
        {

        }
        public TenantUser(Guid? tenantId, Guid userId)
        {
            TenantId = tenantId;
            UserId = userId;
        }
    }
}
