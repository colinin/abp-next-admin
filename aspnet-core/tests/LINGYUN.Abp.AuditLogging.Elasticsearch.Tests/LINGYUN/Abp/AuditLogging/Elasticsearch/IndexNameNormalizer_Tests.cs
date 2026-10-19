using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Text;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Xunit;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

public class IndexNameNormalizer_Tests : AbpAuditLoggingElasticsearchTestBase
{
    public class FakeClock : IClock
    {
        public DateTime Now { get; set; }
        public DateTimeKind Kind => DateTimeKind.Utc;
        public bool SupportsMultipleTimezone => true;

        public FakeClock(DateTime now)
        {
            Now = now;
        }

        public DateTime Normalize(DateTime dateTime) => dateTime;

        public DateTime ConvertToUserTime(DateTime utcDateTime) => utcDateTime;

        public DateTimeOffset ConvertToUserTime(DateTimeOffset dateTimeOffset) => dateTimeOffset;

        public DateTime ConvertToUtc(DateTime dateTime) => dateTime;
    }

    public class FakeCurrentTenant : ICurrentTenant
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public bool IsAvailable => Id.HasValue;

        public FakeCurrentTenant(Guid? id = null, string name = null)
        {
            Id = id;
            Name = name;
        }

        public IDisposable Change(Guid? id, string name = null)
        {
            var previousId = Id;
            var previousName = Name;

            Id = id;
            Name = name;

            return new DisposeAction(() =>
            {
                Id = previousId;
                Name = previousName;
            });
        }

        private sealed class DisposeAction : IDisposable
        {
            private readonly Action _action;

            public DisposeAction(Action action)
            {
                _action = action;
            }

            public void Dispose() => _action();
        }
    }

    private static readonly DateTime FixedNow =
        new DateTime(2026, 9, 24, 10, 30, 0, DateTimeKind.Utc);
    private IndexNameNormalizer CreateNormalizer(
        RollingInterval interval = RollingInterval.Day,
        string indexPrefix = null,
        Guid? tenantId = null,
        DateTime? now = null)
    {
        var options = Options.Create(new AbpAuditLoggingElasticsearchOptions
        {
            IndexPrefix = indexPrefix,
            RollingInterval = interval,
        });

        var clock = new FakeClock(now ?? FixedNow);
        var currentTenant = new FakeCurrentTenant(tenantId);

        return new IndexNameNormalizer(
            clock,
            currentTenant,
            options);
    }

    [Theory]
    [InlineData("AuditLog", "auditlog")]
    [InlineData("audit/log", "audit-log")]
    [InlineData("audit log", "audit-log")]
    [InlineData("audit#log", "audit-log")]
    [InlineData("audit\\log", "audit-log")]
    [InlineData("audit,log", "audit-log")]
    [InlineData("-audit", "audit")]
    [InlineData("_audit", "audit")]
    [InlineData("+audit", "audit")]
    public void NormalizeIndex_Should_Normalize_Input(string input, string expectedPrefix)
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndex(input);

        result.ShouldStartWith(expectedPrefix);
    }

    [Fact]
    public void NormalizeIndex_Should_Return_Fallback_When_Input_Is_All_Invalid()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndex("---");

        result.ShouldNotBeNullOrEmpty();
        result.ShouldNotStartWith("-");
        result.ShouldNotStartWith("_");
        result.ShouldNotStartWith("+");
    }

    [Fact]
    public void NormalizeIndex_Should_Not_Exceed_MaxByteLength()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndex(new string('a', 500));

        Encoding.UTF8.GetByteCount(result)
            .ShouldBeLessThanOrEqualTo(IndexNameNormalizer.MaxByteLength);
    }

    [Fact]
    public void NormalizeIndex_Should_Be_Lowercase()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndex("AUDIT-LOG");

        result.ShouldBe(result.ToLowerInvariant());
    }

    [Fact]
    public void NormalizeIndex_Should_Not_Contain_Invalid_Chars()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndex("audit log");

        result.ShouldNotContain(" ");
        result.ShouldNotContain("/");
        result.ShouldNotContain("\\");
        result.ShouldNotContain("*");
        result.ShouldNotContain("?");
        result.ShouldNotContain("\"");
        result.ShouldNotContain("<");
        result.ShouldNotContain(">");
        result.ShouldNotContain("|");
        result.ShouldNotContain(",");
        result.ShouldNotContain("#");
    }

    [Fact]
    public void NormalizeIndex_Should_Not_Be_Dot()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndex(".");

        result.ShouldNotBe(".");
        result.ShouldNotBe("..");
    }

    [Fact]
    public void NormalizeIndex_Should_Include_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var normalizer = CreateNormalizer(tenantId: tenantId);

        var result = normalizer.NormalizeIndex("audit-log");

        result.ShouldContain(tenantId.ToString("N"));
    }

    [Fact]
    public void NormalizeIndex_Should_Not_Include_Tenant_When_Host()
    {
        var normalizer = CreateNormalizer(tenantId: null);

        var result = normalizer.NormalizeIndex("audit-log");

        // 无租户时不应包含 32 位 hex 形态的 GUID
        result.ShouldNotMatch(@"-[0-9a-f]{32}");
    }

    [Theory]
    [InlineData(RollingInterval.Infinite, "audit-log")]
    [InlineData(RollingInterval.Year, "audit-log-2026")]
    [InlineData(RollingInterval.Month, "audit-log-202609")]
    [InlineData(RollingInterval.Day, "audit-log-20260924")]
    [InlineData(RollingInterval.Hour, "audit-log-2026092410")]
    [InlineData(RollingInterval.Minute, "audit-log-202609241030")]
    public void NormalizeIndex_Should_Append_Correct_Suffix(
        RollingInterval interval,
        string expected)
    {
        var normalizer = CreateNormalizer(interval, now: FixedNow);

        var result = normalizer.NormalizeIndex("audit-log");

        result.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeIndex_Should_Include_Prefix()
    {
        var normalizer = CreateNormalizer(indexPrefix: "abp.dev.auditing");

        var result = normalizer.NormalizeIndex("audit-log");

        result.ShouldStartWith("abp.dev.auditing-audit-log");
    }

    [Fact]
    public void NormalizeIndex_Should_Normalize_Prefix_Too()
    {
        var normalizer = CreateNormalizer(indexPrefix: "ABP DEV");

        var result = normalizer.NormalizeIndex("audit-log");

        result.ShouldNotContain(" ");
        result.ShouldBe(result.ToLowerInvariant());
    }

    [Fact]
    public void NormalizeIndex_Should_Handle_Empty_Prefix()
    {
        var normalizer = CreateNormalizer(indexPrefix: "");

        var result = normalizer.NormalizeIndex("audit-log");

        result.ShouldStartWith("audit-log");
    }

    [Fact]
    public void NormalizeIndex_Should_Handle_Whitespace_Prefix()
    {
        var normalizer = CreateNormalizer(indexPrefix: "   ");

        var result = normalizer.NormalizeIndex("audit-log");

        result.ShouldStartWith("audit-log");
    }

    [Fact]
    public void NormalizeIndex_Should_Handle_CJK()
    {
        var normalizer = CreateNormalizer();

        // 100 个汉字 = 300 字节，应截断到不超过 255 字节
        var result = normalizer.NormalizeIndex(new string('中', 100));

        Encoding.UTF8.GetByteCount(result)
            .ShouldBeLessThanOrEqualTo(IndexNameNormalizer.MaxByteLength);

        // 不应截断到半个字符
        result.ShouldBe(new string('中', result.Length));
    }

    [Fact]
    public void NormalizeIndex_Should_Not_Break_Surrogate_Pair()
    {
        var normalizer = CreateNormalizer();

        // 254 个 ASCII + 1 个 emoji（4 字节代理对），总 258 字节
        var result = normalizer.NormalizeIndex(new string('a', 254) + "😀");

        Encoding.UTF8.GetByteCount(result)
            .ShouldBeLessThanOrEqualTo(IndexNameNormalizer.MaxByteLength);

        // 不应以孤立的高代理项结尾
        char.IsHighSurrogate(result[result.Length - 1]).ShouldBeFalse();
    }

    [Fact]
    public void NormalizeIndexPattern_Should_End_With_Wildcard()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndexPattern("audit-log");

        result.ShouldEndWith("*");
    }

    [Fact]
    public void NormalizeIndexPattern_Should_Not_Contain_Time_Suffix()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndexPattern("audit-log");

        result.ShouldNotMatch(@"\d{4}");
    }

    [Fact]
    public void NormalizeIndexPattern_Should_Not_Exceed_MaxByteLength()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndexPattern(new string('a', 500));

        Encoding.UTF8.GetByteCount(result)
            .ShouldBeLessThanOrEqualTo(IndexNameNormalizer.MaxByteLength);

        result.ShouldEndWith("*");
    }

    [Fact]
    public void NormalizeIndexPattern_Should_Keep_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var normalizer = CreateNormalizer(tenantId: tenantId);

        var result = normalizer.NormalizeIndexPattern("audit-log");

        result.ShouldContain(tenantId.ToString("N"));
        result.ShouldEndWith("*");
    }

    [Fact]
    public void NormalizeIndexPattern_Should_Normalize_Input()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndexPattern("Audit Log");

        result.ShouldStartWith("audit-log");
        result.ShouldEndWith("*");
    }

    [Fact]
    public void NormalizeIndexPrefix_Should_Not_Contain_Wildcard()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndexPrefix("audit-log");

        result.ShouldNotContain("*");
    }

    [Fact]
    public void NormalizeIndexPrefix_Should_Not_Contain_Time_Suffix()
    {
        var normalizer = CreateNormalizer();

        var result = normalizer.NormalizeIndexPrefix("audit-log");

        result.ShouldNotMatch(@"\d{4}");
    }

    [Fact]
    public void NormalizeIndexPrefix_Should_Keep_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var normalizer = CreateNormalizer(tenantId: tenantId);

        var result = normalizer.NormalizeIndexPrefix("audit-log");

        result.ShouldContain(tenantId.ToString("N"));
        result.ShouldNotContain("*");
    }

    [Fact]
    public void Pattern_Should_Equal_Prefix_Plus_Wildcard()
    {
        var normalizer = CreateNormalizer();

        var prefix = normalizer.NormalizeIndexPrefix("audit-log");
        var pattern = normalizer.NormalizeIndexPattern("audit-log");

        pattern.ShouldBe(prefix + "*");
    }

    [Fact]
    public void Index_And_Pattern_Should_Share_TenantPart()
    {
        var tenantId = Guid.NewGuid();
        var normalizer = CreateNormalizer(tenantId: tenantId);
        var tenantPart = tenantId.ToString("N");

        var index = normalizer.NormalizeIndex("audit-log");
        var pattern = normalizer.NormalizeIndexPattern("audit-log");

        index.ShouldContain(tenantPart);
        pattern.ShouldContain(tenantPart);
    }

    [Fact]
    public void Index_And_Pattern_Should_Share_Prefix()
    {
        var normalizer = CreateNormalizer(indexPrefix: "abp.dev.auditing");

        var index = normalizer.NormalizeIndex("audit-log");
        var pattern = normalizer.NormalizeIndexPattern("audit-log");

        index.ShouldStartWith("abp.dev.auditing");
        pattern.ShouldStartWith("abp.dev.auditing");
    }

    [Fact]
    public void Index_Should_Start_With_Prefix()
    {
        var normalizer = CreateNormalizer();

        var index = normalizer.NormalizeIndex("audit-log");
        var prefix = normalizer.NormalizeIndexPrefix("audit-log");

        index.ShouldStartWith(prefix);
    }

    [Fact]
    public void NormalizeIndexPattern_Should_Be_Idempotent()
    {
        var normalizer = CreateNormalizer();

        var once = normalizer.NormalizeIndexPattern("Audit Log");
        var prefix = normalizer.NormalizeIndexPrefix("Audit Log");

        once.ShouldBe(prefix + "*");
    }

    [Fact]
    public void NormalizeIndex_Should_Not_Throw_On_Any_Input()
    {
        var normalizer = CreateNormalizer();

        var inputs = new[]
        {
            "", " ", "-", "_", "+", "---",
            "audit/log", "audit log", "audit#log",
            new string('a', 1000),
            "中文索引名",
            "😀😀😀",
            ".", "..",
        };

        foreach (var input in inputs)
        {
            Should.NotThrow(() => normalizer.NormalizeIndex(input));
        }
    }

    [Fact]
    public void NormalizeIndexPattern_Should_Not_Throw_On_Any_Input()
    {
        var normalizer = CreateNormalizer();

        var inputs = new[]
        {
            "", " ", "-", "_", "+", "---",
            "audit/log", "audit log", "audit#log",
            new string('a', 1000),
            "中文索引名",
            "😀😀😀",
        };

        foreach (var input in inputs)
        {
            Should.NotThrow(() => normalizer.NormalizeIndexPattern(input));
        }
    }

    [Fact]
    public void NormalizeIndex_Should_Fallback_When_RollingInterval_Unknown()
    {
        var normalizer = CreateNormalizer((RollingInterval)999, now: FixedNow);

        var result = normalizer.NormalizeIndex("audit-log");

        // 未知枚举应回退为无时间后缀
        result.ShouldBe("audit-log");
    }

    [Fact]
    public void NormalizeIndex_Should_Combine_Prefix_Tenant_And_Suffix()
    {
        var tenantId = Guid.NewGuid();
        var normalizer = CreateNormalizer(
            interval: RollingInterval.Day,
            indexPrefix: "abp.dev",
            tenantId: tenantId,
            now: FixedNow);

        var result = normalizer.NormalizeIndex("audit-log");

        var expected = $"abp.dev-audit-log-{tenantId:N}-20260924";
        result.ShouldBe(expected);
    }

    [Fact]
    public void NormalizeIndexPattern_Should_Combine_Prefix_And_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var normalizer = CreateNormalizer(
            interval: RollingInterval.Day,
            indexPrefix: "abp.dev",
            tenantId: tenantId,
            now: FixedNow);

        var result = normalizer.NormalizeIndexPattern("audit-log");

        var expected = $"abp.dev-audit-log-{tenantId:N}*";
        result.ShouldBe(expected);
    }
}
