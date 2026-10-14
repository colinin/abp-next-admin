using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Volo.Abp.Http;

namespace Microsoft.IdentityModel.Tokens;

/// <summary>
/// Copy from: https://github.com/maliming/Owl.TokenWildcardIssuerValidator
/// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Tokens/Validators.cs#L207
/// </summary>
public static class TokenWildcardIssuerValidator
{
    private const string IDX10204 = "IDX10204: Unable to validate issuer. validationParameters.ValidIssuer is null or whitespace AND validationParameters.ValidIssuers is null.";
    private const string IDX10205 = "IDX10205: Issuer validation failed. Issuer: '{0}'. Did not match: validationParameters.ValidIssuer: '{1}' or validationParameters.ValidIssuers: '{2}'.";
    private const string IDX10211 = "IDX10211: Unable to validate issuer. The 'issuer' parameter is null or whitespace";
    private const string IDX10235 = "IDX10235: ValidateIssuer property on ValidationParameters is set to false. Exiting without validating the issuer.";
    private const string IDX10236 = "IDX10236: Issuer Validated.Issuer: '{0}'";
    private const string IDX10262 = "IDX10262: One of the issuers in TokenValidationParameters.ValidIssuers was null or an empty string. See https://aka.ms/wilson/tokenvalidation for details.";

    public readonly static IssuerValidatorUsingConfiguration IssuerValidatorUsingConfiguration = (issuer, securityToken, validationParameters, configuration) =>
    {
        if (validationParameters == null)
        {
            throw LogHelper.LogArgumentNullException(nameof(validationParameters));
        }

        if (!validationParameters.ValidateIssuer)
        {
            LogHelper.LogWarning(IDX10235);
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
        if (string.IsNullOrWhiteSpace(validationParameters.ValidIssuer)
            && validationParameters.ValidIssuers.IsNullOrEmpty()
            && string.IsNullOrWhiteSpace(configuration?.Issuer))
        {
            throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(IDX10204)
            {
                InvalidIssuer = issuer
            });
        }

        if (configuration != null)
        {
            if (string.Equals(configuration.Issuer, issuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                {
                    LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                }

                return issuer;
            }

            if (CheckWildcardDomain(issuer, configuration.Issuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                {
                    LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                }

                return issuer;
            }
        }

        if (string.Equals(validationParameters.ValidIssuer, issuer))
        {
            if (LogHelper.IsEnabled(EventLogLevel.Informational))
            {
                LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
            }

            return issuer;
        }

        if (!string.IsNullOrWhiteSpace(validationParameters.ValidIssuer))
        {
            if (CheckWildcardDomain(issuer, validationParameters.ValidIssuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                {
                    LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                }

                return issuer;
            }
        }

        if (validationParameters.ValidIssuers != null)
        {
            foreach (var str in validationParameters.ValidIssuers)
            {
                if (string.IsNullOrEmpty(str))
                {
                    LogHelper.LogInformation(IDX10262);
                    continue;
                }

                if (string.Equals(str, issuer))
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    {
                        LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                    }

                    return issuer;
                }

                if (CheckWildcardDomain(issuer, str))
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    {
                        LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                    }

                    return issuer;
                }
            }
        }

        var ex = new SecurityTokenInvalidIssuerException(
                LogHelper.FormatInvariant(IDX10205,
                    LogHelper.MarkAsNonPII(issuer),
                    LogHelper.MarkAsNonPII(validationParameters.ValidIssuer ?? "null"),
                    LogHelper.MarkAsNonPII(
                        SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers)),
                    LogHelper.MarkAsNonPII(configuration?.Issuer)))
        {
            InvalidIssuer = issuer
        };

        if (!validationParameters.LogValidationExceptions)
        {
            throw ex;
        }

        throw LogHelper.LogExceptionMessage(ex);
    };


    public readonly static IssuerValidator IssuerValidator = (issuer, token, validationParameters) =>
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
        if (string.IsNullOrWhiteSpace(validationParameters.ValidIssuer)
            && validationParameters.ValidIssuers.IsNullOrEmpty())
        {
            throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(IDX10204)
            { 
                InvalidIssuer = issuer
            });
        }

        if (string.Equals(validationParameters.ValidIssuer, issuer))
        {
            if (LogHelper.IsEnabled(EventLogLevel.Informational))
            {
                LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
            }

            return issuer;
        }

        if (!string.IsNullOrWhiteSpace(validationParameters.ValidIssuer))
        {
            if (CheckWildcardDomain(issuer, validationParameters.ValidIssuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                {
                    LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                }

                return issuer;
            }
        }

        if (validationParameters.ValidIssuers != null)
        {
            foreach (var str in validationParameters.ValidIssuers)
            {
                if (string.IsNullOrEmpty(str))
                {
                    LogHelper.LogInformation(IDX10262);
                    continue;
                }

                if (string.Equals(str, issuer))
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    {
                        LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                    }

                    return issuer;
                }

                if (CheckWildcardDomain(issuer, str))
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    {
                        LogHelper.LogInformation(IDX10236, LogHelper.MarkAsNonPII(issuer));
                    }

                    return issuer;
                }
            }
        }

        var ex = new SecurityTokenInvalidIssuerException(
            LogHelper.FormatInvariant(IDX10205,
                LogHelper.MarkAsNonPII(issuer),
                LogHelper.MarkAsNonPII(validationParameters.ValidIssuer ?? "null"),
                LogHelper.MarkAsNonPII(SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers))))
        {
            InvalidIssuer = issuer
        };

        if (!validationParameters.LogValidationExceptions)
        {
            throw ex;
        }

        throw LogHelper.LogExceptionMessage(ex);
    };

    private static bool CheckWildcardDomain(string url, params string[] domainFormats)
    {
        return !domainFormats.IsNullOrEmpty() && domainFormats
            .Select(domainFormat => domainFormat.Replace("{0}", "*"))
            .Any(domain => UrlHelpers.IsSubdomainOf(url, domain));
    }

    private static string SerializeAsSingleCommaDelimitedString(IEnumerable<string>? strings)
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

    private static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }
}
