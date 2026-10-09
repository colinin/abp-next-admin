using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Security.Claims;

namespace LINGYUN.Abp.AspNetCore.Authentication;

public class AbpClaimsMapClaimsTransformation : AbpClaimsTransformation
{
    public AbpClaimsMapClaimsTransformation(IOptions<AbpClaimsMapOptions> abpClaimsMapOptions)
        : base(abpClaimsMapOptions)
    {
    }

    public override Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var maps = AbpClaimsMapOptions.Value.Maps;
        if (maps.Count == 0)
        {
            return Task.FromResult(principal);
        }

        var mappedClaims = new List<Claim>();

        foreach (var claim in principal.Claims)
        {
            if (!maps.TryGetValue(claim.Type, out var mapClaimType))
            {
                continue;
            }

            var mappedClaimType = mapClaimType();
            if (string.IsNullOrWhiteSpace(mappedClaimType))
            {
                continue;
            }

            if (string.Equals(mappedClaimType, claim.Type, StringComparison.Ordinal))
            {
                continue;
            }

            if (principal.HasClaim(mappedClaimType, claim.Value))
            {
                continue;
            }

            mappedClaims.Add(new Claim(
                mappedClaimType,
                claim.Value,
                claim.ValueType,
                claim.Issuer,
                claim.OriginalIssuer));
        }

        if (mappedClaims.Count == 0)
        {
            return Task.FromResult(principal);
        }

        var identity = principal.Identities.FirstOrDefault(x => x.IsAuthenticated) ?? principal.Identities.FirstOrDefault();
        if (identity is ClaimsIdentity claimsIdentity)
        {
            try
            {
                foreach (var mappedClaim in mappedClaims)
                {
                    claimsIdentity.AddClaim(mappedClaim);
                }

                return Task.FromResult(principal);
            }
            catch (InvalidOperationException)
            {
                // ignore
            }
        }

        principal.AddIdentity(new ClaimsIdentity(mappedClaims));

        return Task.FromResult(principal);
    }
}
