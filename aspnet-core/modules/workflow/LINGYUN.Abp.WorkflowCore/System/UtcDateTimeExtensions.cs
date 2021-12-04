namespace System
{
    public static class UtcDateTimeExtensions
    {
        public static DateTime? ToNullableUtcDateTime(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToUtcDateTime();
            }
            return null;
        }

        public static DateTime ToUtcDateTime(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}
