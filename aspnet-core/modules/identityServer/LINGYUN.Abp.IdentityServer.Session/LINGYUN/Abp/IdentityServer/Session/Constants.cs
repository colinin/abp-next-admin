using IdentityModel;
using System.Collections.Generic;

namespace LINGYUN.Abp.IdentityServer.Session;
internal static class Constants
{
    public static Dictionary<string, int> ProtectedResourceErrorStatusCodes = new Dictionary<string, int>
    {
        { OidcConstants.ProtectedResourceErrors.InvalidToken,      401 },
        { OidcConstants.ProtectedResourceErrors.ExpiredToken,      401 },
        { OidcConstants.ProtectedResourceErrors.InvalidRequest,    400 },
        { OidcConstants.ProtectedResourceErrors.InsufficientScope, 403 }
    };

    public static class ProtectedResourceErrors
    {
        public const string ExpiredToken = "expired_token";
    }

    public class Filters
    {
        public static readonly string[] ProtocolClaimsFilter = {
            JwtClaimTypes.AccessTokenHash,
            JwtClaimTypes.Audience,
            JwtClaimTypes.AuthorizedParty,
            JwtClaimTypes.AuthorizationCodeHash,
            JwtClaimTypes.ClientId,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.IssuedAt,
            JwtClaimTypes.Issuer,
            JwtClaimTypes.JwtId,
            JwtClaimTypes.Nonce,
            JwtClaimTypes.NotBefore,
            JwtClaimTypes.ReferenceTokenId,
            JwtClaimTypes.SessionId,
            JwtClaimTypes.Scope
        };
    }
}
