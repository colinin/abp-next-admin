namespace System;

internal static class DateTimeExtensions
{
    public static long ToMicroseconds(this DateTime time)
    {
        var utc = time.Kind == DateTimeKind.Utc ? time : time.ToUniversalTime();

        return (utc.Ticks - DateTime.UnixEpoch.Ticks) / 10;
    }
}
