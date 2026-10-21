using Shouldly;
using System;
using Xunit;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

public class OpenObserveTimestampHelper_Tests
{
    [Fact]
    public void Should_Convert_To_Microseconds()
    {
        var time = new DateTime(2026, 10, 12, 8, 0, 0, DateTimeKind.Utc);

        OpenObserveTimestampHelper.ToMicroseconds(time)
            .ShouldBe(new DateTimeOffset(time).ToUnixTimeMilliseconds() * 1000L);
    }

    [Fact]
    public void Should_Use_Log_Time_Within_Allowed_Window()
    {
        var now = new DateTime(2026, 10, 12, 12, 0, 0, DateTimeKind.Utc);
        var executionTime = now.AddHours(-2);

        var timestamp = OpenObserveTimestampHelper.TryGetTimestamp(executionTime, now, 5);

        timestamp.ShouldNotBeNull();
        timestamp!.Value.ShouldBe(OpenObserveTimestampHelper.ToMicroseconds(executionTime));
    }

    [Fact]
    public void Should_Fallback_When_Log_Time_Is_Too_Old()
    {
        var now = new DateTime(2026, 10, 12, 12, 0, 0, DateTimeKind.Utc);

        // 超出 ZO_INGEST_ALLOWED_UPTO 窗口的数据会被 OpenObserve 丢弃, 因此不覆盖 _timestamp
        OpenObserveTimestampHelper.TryGetTimestamp(now.AddDays(-3), now, 5)
            .ShouldBeNull();
    }

    [Fact]
    public void Should_Fallback_When_Log_Time_Is_Default()
    {
        var now = new DateTime(2026, 10, 12, 12, 0, 0, DateTimeKind.Utc);

        OpenObserveTimestampHelper.TryGetTimestamp(default, now, 5)
            .ShouldBeNull();
    }
}
