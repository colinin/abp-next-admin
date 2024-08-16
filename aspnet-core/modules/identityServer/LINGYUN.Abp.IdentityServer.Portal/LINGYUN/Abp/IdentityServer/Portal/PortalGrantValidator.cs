using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Platform.Portal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using static IdentityModel.OidcConstants;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.Portal;
public class PortalGrantValidator : IExtensionGrantValidator
{
    public const string ProviderName = "portal";
    public string GrantType => ProviderName;

    private readonly ILogger<PortalGrantValidator> _logger;

    private readonly IdentityServerOptions _options;

    private readonly IEventService _events;
    private readonly IResourceOwnerPasswordValidator _resourceOwnerValidator;
    private readonly IdentitySecurityLogManager _identitySecurityLogManager;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly ICurrentTenant _currentTenant;
    private readonly IEnterpriseRepository _enterpriseRepository;

    private readonly AbpAspNetCoreMultiTenancyOptions _multiTenancyOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PortalGrantValidator(
        ILogger<PortalGrantValidator> logger,
        IOptions<IdentityServerOptions> options,
        IEventService events,
        IResourceOwnerPasswordValidator resourceOwnerValidator,
        IdentitySecurityLogManager identitySecurityLogManager,
        UserManager<IdentityUser> userManager,
        ICurrentTenant currentTenant,
        IEnterpriseRepository enterpriseRepository,
        IOptions<AbpAspNetCoreMultiTenancyOptions> multiTenancyOptions,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _options = options.Value;
        _events = events;
        _resourceOwnerValidator = resourceOwnerValidator;
        _identitySecurityLogManager = identitySecurityLogManager;
        _userManager = userManager;
        _currentTenant = currentTenant;
        _enterpriseRepository = enterpriseRepository;
        _multiTenancyOptions = multiTenancyOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    [UnitOfWork]
    public async virtual Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        /*  ************************************
         * 
         * 启用平台登录后，需要管理员在宿主端定义企业信息, 并关联到某个租户
         * 
         *  1、用户使用protal发起登录请求
         *  2、检查是否携带企业标识字段 EnterpriseId
         *  2.1、未携带 EnterpriseId 字段, 检索关联了租户信息的企业列表, 通过自定义标头反馈给用户端
         *  2.2、检索关联 EnterpriseId 的租户信息, 切换指定租户, 通过 password 方式来进行登录验证
         *  3、登录成功返回token
         * 
         *  *************************************/

        var parameters = context.Request.Raw;

        Guid? tenantId = null;
        using (_currentTenant.Change(null))
        {
            var enterprise = parameters.Get("enterpriseId") ?? parameters.Get("EnterpriseId");
            if (enterprise.IsNullOrWhiteSpace() || !Guid.TryParse(enterprise, out var enterpriseId))
            {
                // TODO: configurabled
                var enterprises = await _enterpriseRepository.GetEnterprisesInTenantListAsync(25);

                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    customResponse: new Dictionary<string, object>
                    {
                        // 是否可直接选择的模式
                        { "Enterprises", JsonConvert.SerializeObject(enterprises.Select(x => new { Id = x.Id, Name = x.Name, Logo = x.Logo })) },
                    });
                return;
            }

            tenantId = await _enterpriseRepository.GetEnterpriseInTenantAsync(enterpriseId);
        }

        using (_currentTenant.Change(tenantId))
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                AbpMultiTenancyCookieHelper.SetTenantCookie(
                    _httpContextAccessor.HttpContext,
                    tenantId,
                    _multiTenancyOptions.TenantKey);
            }

            var validatedRequest = new ValidatedTokenRequest
            {
                Raw = parameters ?? throw new ArgumentNullException(nameof(parameters)),
                Options = _options
            };

            var userName = parameters.Get(OidcConstants.TokenRequest.UserName);
            var password = parameters.Get(OidcConstants.TokenRequest.Password);

            if (userName.IsNullOrWhiteSpace())
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            if (password.IsNullOrWhiteSpace())
            {
                password = "";
            }

            if (userName.Length > _options.InputLengthRestrictions.UserName ||
                password.Length > _options.InputLengthRestrictions.Password)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            var resourceOwnerContext = new ResourceOwnerPasswordValidationContext
            {
                UserName = userName,
                Password = password,
                Request = validatedRequest
            };
            await _resourceOwnerValidator.ValidateAsync(resourceOwnerContext);

            if (resourceOwnerContext.Result.IsError)
            {
                // protect against bad validator implementations
                resourceOwnerContext.Result.Error ??= OidcConstants.TokenErrors.InvalidGrant;

                if (resourceOwnerContext.Result.Error == OidcConstants.TokenErrors.UnsupportedGrantType)
                {
                    _logger.LogError("Resource owner password credential grant type not supported");

                    await RaiseFailedResourceOwnerAuthenticationEventAsync(userName, "password grant type not supported", resourceOwnerContext.Request.Client.ClientId);

                    context.Result = new GrantValidationResult(TokenRequestErrors.UnsupportedGrantType, customResponse: resourceOwnerContext.Result.CustomResponse);
                    return;
                }

                var errorDescription = "invalid_username_or_password";

                if (!resourceOwnerContext.Result.ErrorDescription.IsNullOrWhiteSpace())
                {
                    errorDescription = resourceOwnerContext.Result.ErrorDescription;
                }

                _logger.LogInformation("User authentication failed: {0}", errorDescription ?? resourceOwnerContext.Result.Error);
                await RaiseFailedResourceOwnerAuthenticationEventAsync(userName, errorDescription, context.Request.Client.ClientId);

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription, resourceOwnerContext.Result.CustomResponse);

                return;
            }

            if (resourceOwnerContext.Result.Subject == null)
            {
                var error = "User authentication failed: no principal returned";
                _logger.LogError(error);

                await RaiseFailedResourceOwnerAuthenticationEventAsync(userName, error, context.Request.Client.ClientId);

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

                return;
            }

            var currentUser = await _userManager.GetUserAsync(resourceOwnerContext.Result.Subject);

            await _events.RaiseAsync(new UserLoginSuccessEvent(userName, currentUser.Id.ToString(), currentUser.Name, clientId: resourceOwnerContext.Request.ClientId));

            await SetSuccessResultAsync(context, currentUser);
        }
    }

    protected async virtual Task SetSuccessResultAsync(ExtensionGrantValidationContext context, IdentityUser user)
    {
        var sub = await _userManager.GetUserIdAsync(user);

        _logger.LogInformation("Credentials validated for username: {username}", user.UserName);

        var additionalClaims = new List<Claim>();

        await AddCustomClaimsAsync(additionalClaims, user, context);

        context.Result = new GrantValidationResult(
            sub,
            AuthenticationMethods.ConfirmationBySms,
            additionalClaims.ToArray()
        );

        await SaveSecurityLogAsync(
            context,
            user,
            IdentityServerSecurityLogActionConsts.LoginSucceeded);
    }

    protected async virtual Task SaveSecurityLogAsync(
        ExtensionGrantValidationContext context,
        IdentityUser user,
        string action)
    {
        var logContext = new IdentitySecurityLogContext
        {
            Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
            Action = action,
            UserName = user.UserName,
            ClientId = await FindClientIdAsync(context)
        };
        logContext.WithProperty("GrantType", GrantType);

        await _identitySecurityLogManager.SaveAsync(logContext);
    }

    protected virtual Task<string> FindClientIdAsync(ExtensionGrantValidationContext context)
    {
        return Task.FromResult(context.Request?.Client?.ClientId);
    }

    protected virtual Task AddCustomClaimsAsync(
            List<Claim> customClaims,
            IdentityUser user,
            ExtensionGrantValidationContext context)
    {
        if (user.TenantId.HasValue)
        {
            customClaims.Add(
                new Claim(
                    AbpClaimTypes.TenantId,
                    user.TenantId?.ToString()
                )
            );
        }

        return Task.CompletedTask;
    }

    private Task RaiseFailedResourceOwnerAuthenticationEventAsync(string userName, string error, string clientId)
    {
        return _events.RaiseAsync(new UserLoginFailureEvent(userName, error, interactive: false, clientId: clientId));
    }
}
