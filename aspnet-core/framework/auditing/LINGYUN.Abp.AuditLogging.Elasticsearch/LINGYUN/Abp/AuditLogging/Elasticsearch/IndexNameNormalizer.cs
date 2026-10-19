using Microsoft.Extensions.Options;
using System;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

public class IndexNameNormalizer : IIndexNameNormalizer, ISingletonDependency
{
    /// <summary>
    /// ES 索引名不允许的字符集合
    /// 参考: https://www.elastic.co/docs/api/doc/elasticsearch/operation/operation-indices-create
    /// </summary>
    private readonly static char[] InvalidChars =
    [
        '\\', '/', '*', '?', '"', '<', '>', '|', ' ', ',', '#'
    ];
    /// <summary>
    /// 索引名不允许作为开头的字符集合
    /// 参考: https://www.elastic.co/docs/api/doc/elasticsearch/operation/operation-indices-create
    /// </summary>
    private readonly static char[] InvalidLeadingChars = ['-', '_', '+'];
    /// <summary>
    /// ES 索引名最大字节数
    /// 参考: https://www.elastic.co/docs/api/doc/elasticsearch/operation/operation-indices-create
    /// </summary>
    public const int MaxByteLength = 255;
    /// <summary>
    /// 通配符占用的字节数（用于 pattern 预留）。
    /// </summary>
    private const int WildcardByteLength = 1;

    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly AbpAuditLoggingElasticsearchOptions _options;

    public IndexNameNormalizer(
        IClock clock,
        ICurrentTenant currentTenant, 
        IOptions<AbpAuditLoggingElasticsearchOptions> options)
    {
        _clock = clock;
        _currentTenant = currentTenant;
        _options = options.Value;
    }

    public virtual string NormalizeIndex(string index)
    {
        return BuildIndexName(index, GetSuffix(_clock.Now), reserveBytes: 0);
    }

    public virtual string NormalizeIndexPrefix(string index)
    {
        return BuildIndexName(index, suffix: string.Empty, reserveBytes: 0);
    }

    public virtual string NormalizeIndexPattern(string index)
    {
        var prefix = BuildIndexName(index, suffix: string.Empty, reserveBytes: WildcardByteLength);
        return prefix + "*";
    }

    protected virtual string BuildIndexName(string index, string suffix, int reserveBytes)
    {
        var maxBytes = MaxByteLength - reserveBytes;

        var normalizedIndex = NormalizeSegment(index);
        var essentialPart = normalizedIndex + GetTenantPart() + suffix;
        var essentialBytes = Encoding.UTF8.GetByteCount(essentialPart);

        if (essentialBytes >= maxBytes)
        {
            return TruncateToByteLength(normalizedIndex, maxBytes);
        }

        var prefix = GetPrefix();
        var remaining = maxBytes - essentialBytes;
        var safePrefix = TruncateToByteLength(prefix, remaining);

        var fullName = safePrefix + essentialPart;

        return NormalizeToValidIndexName(fullName);
    }

    protected virtual string GetPrefix()
    {
        return string.IsNullOrWhiteSpace(_options.IndexPrefix)
            ? string.Empty
            : _options.IndexPrefix + "-";
    }

    protected virtual string GetTenantPart()
    {
        return _currentTenant.Id.HasValue
            ? "-" + _currentTenant.Id.Value.ToString("N")
            : string.Empty;
    }

    protected virtual string GetSuffix(DateTime now)
    {
        var format = _options.RollingInterval switch
        {
            RollingInterval.Infinite => null,
            RollingInterval.Year => "yyyy",
            RollingInterval.Month => "yyyyMM",
            RollingInterval.Day => "yyyyMMdd",
            RollingInterval.Hour => "yyyyMMddHH",
            RollingInterval.Minute => "yyyyMMddHHmm",
            _ => null,
        };

        return format is null ? string.Empty : $"-{now.ToString(format)}";
    }

    protected virtual string NormalizeSegment(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var chars = value.ToLowerInvariant().ToCharArray();

        for (var i = 0; i < chars.Length; i++)
        {
            if (Array.IndexOf(InvalidChars, chars[i]) >= 0)
            {
                chars[i] = '-';
            }
        }

        return new string(chars).TrimStart(InvalidLeadingChars);
    }

    protected virtual string TruncateToByteLength(string value, int maxBytes)
    {
        if (string.IsNullOrEmpty(value) || maxBytes <= 0)
        {
            return string.Empty;
        }

        var byteCount = 0;
        var charCount = 0;

        for (var i = 0; i < value.Length; i++)
        {
            var ch = value[i];

            int charBytes;
            var charUtf16Length = 1;

            if (char.IsHighSurrogate(ch) && i + 1 < value.Length && char.IsLowSurrogate(value[i + 1]))
            {
                charBytes = 4;
                charUtf16Length = 2;
            }
            else
            {
                if (ch <= '\u007F')
                {
                    charBytes = 1;
                }
                else if (ch <= '\u07FF')
                {
                    charBytes = 2;
                }
                else
                {
                    charBytes = 3;
                }
            }

            if (byteCount + charBytes > maxBytes)
            {
                break;
            }

            byteCount += charBytes;
            charCount += charUtf16Length;
            i += charUtf16Length - 1;
        }

        return value.Substring(0, charCount);
    }

    protected virtual string NormalizeToValidIndexName(string name)
    {
        var result = name.ToLowerInvariant();

        var chars = result.ToCharArray();
        for (var i = 0; i < chars.Length; i++)
        {
            if (Array.IndexOf(InvalidChars, chars[i]) >= 0)
            {
                chars[i] = '-';
            }
        }

        result = new string(chars).TrimStart(InvalidLeadingChars);

        if (Encoding.UTF8.GetByteCount(result) > MaxByteLength)
        {
            var truncated = TruncateToByteLength(result, MaxByteLength);

            result = truncated;
        }

        return result;
    }
}
