using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Volo.Abp.Text.Formatting;

namespace LY.MicroService.AuthServer;

/// <summary>
/// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Tokens/Validators.cs#L207
/// </summary>
public static class TokenWildcardIssuerValidator
{
    private const string IDX10204 = "IDX10204: Unable to validate issuer. validationParameters.ValidIssuer is null or whitespace AND validationParameters.ValidIssuers is null.";
    private const string IDX10205 = "IDX10205: Issuer validation failed. Issuer: '{0}'. Did not match: validationParameters.ValidIssuer: '{1}' or validationParameters.ValidIssuers: '{2}'.";
    private const string IDX10211 = "IDX10211: Unable to validate issuer. The 'issuer' parameter is null or whitespace";
    private const string IDX10235 = "IDX10235: ValidateIssuer property on ValidationParameters is set to false. Exiting without validating the issuer.";
    private const string IDX10236 = "IDX10236: Issuer Validated.Issuer: '{0}'";

    public static readonly IssuerValidator IssuerValidator = (issuer, token, validationParameters) =>
    {
        if (validationParameters == null)
        {
            throw LogHelper.LogArgumentNullException(nameof(validationParameters));
        }

        if (!validationParameters.ValidateIssuer)
        {
            LogHelper.LogInformation(IDX10235);
            return issuer;
        }

        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(IDX10211)
            {
                InvalidIssuer = issuer
            });
        }

        // Throw if all possible places to validate against are null or empty
        if (string.IsNullOrWhiteSpace(validationParameters.ValidIssuer) &&
            validationParameters.ValidIssuers == null)
        {
            throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(IDX10204)
            {
                InvalidIssuer = issuer
            });
        }

        if (string.Equals(validationParameters.ValidIssuer, issuer, StringComparison.Ordinal))
        {
            LogHelper.LogInformation(IDX10236, issuer);
            return issuer;
        }

        if (!string.IsNullOrWhiteSpace(validationParameters.ValidIssuer))
        {
            var extractResult = FormattedStringValueExtracter.Extract(issuer, validationParameters.ValidIssuer, ignoreCase: true);
            if (extractResult.IsMatch &&
                extractResult.Matches.Aggregate(validationParameters.ValidIssuer, (current, nameValue) => current.Replace($"{{{nameValue.Name}}}", nameValue.Value))
                    .IndexOf(issuer, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return issuer;
            }
        }

        if (null != validationParameters.ValidIssuers)
        {
            foreach (var str in validationParameters.ValidIssuers)
            {
                if (string.IsNullOrEmpty(str))
                {
                    LogHelper.LogInformation(IDX10235);
                    continue;
                }

                if (string.Equals(str, issuer, StringComparison.Ordinal))
                {
                    LogHelper.LogInformation(IDX10236, issuer);
                    return issuer;
                }

                var extractResult = FormattedStringValueExtracter.Extract(issuer, str, ignoreCase: true);
                if (extractResult.IsMatch &&
                    extractResult.Matches.Aggregate(str, (current, nameValue) => current.Replace($"{{{nameValue.Name}}}", nameValue.Value))
                        .IndexOf(issuer, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return issuer;
                }
            }
        }

        throw LogHelper.LogExceptionMessage(
            new SecurityTokenInvalidIssuerException(LogHelper.FormatInvariant(IDX10205, issuer,
                (validationParameters.ValidIssuer ?? "null"),
                SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers)))
            {
                InvalidIssuer = issuer
            });
    };

    private static string SerializeAsSingleCommaDelimitedString(IEnumerable<string> strings)
    {
        if (strings == null)
        {
            return Utility.Null;
        }

        var sb = new StringBuilder();
        var first = true;
        foreach (var str in strings)
        {
            if (first)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", str ?? Utility.Null);
                first = false;
            }
            else
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, ", {0}", str ?? Utility.Null);
            }
        }

        return first ? Utility.Empty : sb.ToString();
    }
}
