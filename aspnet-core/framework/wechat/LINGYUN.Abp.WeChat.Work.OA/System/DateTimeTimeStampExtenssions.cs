namespace System;

internal static class DateTimeTimeStampExtenssions
{
    private readonly static DateTime _beginUnixTime = new DateTime(1970, 1, 1);
    /// <summary>
    /// 获取Unix时间戳（秒）
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long GetUnixTimeSeconds(this DateTime dateTime)
    {
        return (long)(dateTime - _beginUnixTime).TotalSeconds;
    }
}
