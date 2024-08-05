using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IdentityServer.Session;
public class AbpIdentitySessionUserInfoRequestValidator : IUserInfoRequestValidator
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenValidator _tokenValidator;
    private readonly IProfileService _profile;
    private readonly ILogger _logger;

    public AbpIdentitySessionUserInfoRequestValidator(
        IHttpContextAccessor httpContextAccessor,
        ITokenValidator tokenValidator,
        IProfileService profile,
        ILogger<AbpIdentitySessionUserInfoRequestValidator> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenValidator = tokenValidator;
        _profile = profile;
        _logger = logger;
    }

    /// <summary>
    /// Validates a userinfo request.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public async Task<UserInfoRequestValidationResult> ValidateRequestAsync(string accessToken)
    {
        if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == false)
        {
            _logger.LogError("User session has expired.");

            return new UserInfoRequestValidationResult
            {
                IsError = true,
                Error = Constants.ProtectedResourceErrors.ExpiredToken,
            };
        }

        // the access token needs to be valid and have at least the openid scope
        var tokenResult = await _tokenValidator.ValidateAccessTokenAsync(
            accessToken,
            IdentityServerConstants.StandardScopes.OpenId);

        if (tokenResult.IsError)
        {
            return new UserInfoRequestValidationResult
            {
                IsError = true,
                Error = tokenResult.Error
            };
        }

        // the token must have a one sub claim
        var subClaim = tokenResult.Claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Subject);
        if (subClaim == null)
        {
            _logger.LogError("Token contains no sub claim");

            return new UserInfoRequestValidationResult
            {
                IsError = true,
                Error = OidcConstants.ProtectedResourceErrors.InvalidToken
            };
        }

        // create subject from incoming access token
        var claims = tokenResult.Claims.Where(x => !Constants.Filters.ProtocolClaimsFilter.Contains(x.Type));
        var subject = Principal.Create("UserInfo", claims.ToArray());

        // make sure user is still active
        var isActiveContext = new IsActiveContext(subject, tokenResult.Client, IdentityServerConstants.ProfileIsActiveCallers.UserInfoRequestValidation);
        await _profile.IsActiveAsync(isActiveContext);

        if (isActiveContext.IsActive == false)
        {
            _logger.LogError("User is not active: {sub}", subject.GetSubjectId());

            return new UserInfoRequestValidationResult
            {
                IsError = true,
                Error = OidcConstants.ProtectedResourceErrors.InvalidToken
            };
        }

        return new UserInfoRequestValidationResult
        {
            IsError = false,
            TokenValidationResult = tokenResult,
            Subject = subject
        };
    }

}
