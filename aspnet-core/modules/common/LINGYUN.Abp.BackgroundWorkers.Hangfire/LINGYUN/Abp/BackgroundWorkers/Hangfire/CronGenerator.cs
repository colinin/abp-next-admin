using System;

namespace LINGYUN.Abp.BackgroundWorkers.Hangfire
{
    public class CronGenerator
    {
        public const long MilliSecondsOfYear = 315_3600_0000L;
        public const long MilliSecondsOfMonth = 26_7840_0000L;
        public const int MilliSecondsOfWeek = 6_0480_0000;
        public const int MilliSecondsOfDays = 8640_0000;
        public const int MilliSecondsOfHours = 360_0000;
        public const int MilliSecondsOfMinutes = 60000;

        /// <summary>
        /// 从毫秒计算Cron表达式
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static string FormMilliseconds(long milliseconds = 1000)
        {
            if (milliseconds <= 1000)
            {
                return Seconds(0, 1);
            }

            if (milliseconds >= MilliSecondsOfYear)
            {
                return Year(1, 1, 0, 0, 2001, (int)(milliseconds / MilliSecondsOfYear));
            }

            if (milliseconds >= MilliSecondsOfMonth)
            {
                return Month(1, 0, 0, 1, (int)(milliseconds / MilliSecondsOfMonth));
            }

            // TODO: 以周为单位的任务Cron
            if (milliseconds >= MilliSecondsOfWeek)
            {
                return Day(0, 0, 1, (int)(milliseconds / MilliSecondsOfWeek));
            }

            if (milliseconds >= MilliSecondsOfDays)
            {
                return Day(0, 0, 1, (int)(milliseconds / MilliSecondsOfDays));
            }

            if (milliseconds >= MilliSecondsOfHours)
            {
                return Hour(0, 0, (int)(milliseconds / MilliSecondsOfHours));
            }

            if (milliseconds >= MilliSecondsOfMinutes)
            {
                return Minute(0, 0, (int)(milliseconds / MilliSecondsOfMinutes));
            }

            return Seconds(0, (int)(milliseconds / 1000));
        }
        /// <summary>
        /// 周期性为秒钟的任务
        /// </summary>
        /// <param name="second">第几秒开始，默认为第0秒</param>
        /// <param name="interval">执行周期的间隔，默认为每5秒一次</param>
        /// <returns></returns>
        public static string Seconds(int second = 0, int interval = 5)
        {
            Check.Range(second, nameof(second), 0, 59);

            return $"{second}/{interval} * * * * ? ";
        }

        /// <summary>
        /// 周期性为分钟的任务
        /// </summary>
        /// <param name="second">第几秒开始，默认为第0秒</param>
        /// <param name="minute">第几分钟开始，默认为第0分钟</param>
        /// <param name="interval">执行周期的间隔，默认为每分钟一次</param>
        /// <returns></returns>
        public static string Minute(int second = 0, int minute = 0, int interval = 1)
        {
            Check.Range(second, nameof(second), 0, 59);
            Check.Range(minute, nameof(minute), 0, 59);

            return $"{second} {minute}/{interval} * * * ? ";
        }

        /// <summary>
        /// 周期性为小时的任务
        /// </summary>
        /// <param name="minute">第几分钟开始，默认为第0分钟</param>
        /// <param name="hour">第几小时开始，默认为0点</param>
        /// <param name="interval">执行周期的间隔，默认为每小时一次</param>
        /// <returns></returns>
        public static string Hour(int minute = 0, int hour = 0, int interval = 1)
        {
            Check.Range(hour, nameof(hour), 0, 23);
            Check.Range(minute, nameof(minute), 0, 59);

            return $"0 {minute} {hour}/{interval} * * ? ";
        }

        /// <summary>
        /// 周期性为天的任务
        /// </summary>
        /// <param name="hour">第几小时开始，默认从0点开始</param>
        /// <param name="minute">第几分钟开始，默认从第0分钟开始</param>
        /// <param name="day">第几天开始，默认从第1天开始</param>
        /// <param name="interval">执行周期的间隔，默认为每天一次</param>
        /// <returns></returns>
        public static string Day(int hour = 0, int minute = 0, int day = 1, int interval = 1)
        {
            Check.Range(hour, nameof(hour), 0, 23);
            Check.Range(minute, nameof(minute), 0, 59);
            Check.Range(day, nameof(day), 1, 31);

            return $"0 {minute} {hour} {day}/{interval} * ? ";
        }

        /// <summary>
        /// 周期性为周的任务
        /// </summary>
        /// <param name="dayOfWeek">星期几开始，默认从星期一点开始</param>
        /// <param name="hour">第几小时开始，默认从0点开始</param>
        /// <param name="minute">第几分钟开始，默认从第0分钟开始</param>
        /// <returns></returns>
        public static string Week(DayOfWeek dayOfWeek = DayOfWeek.Monday, int hour = 0, int minute = 0)
        {
            Check.Range(hour, nameof(hour), 0, 23);
            Check.Range(minute, nameof(minute), 0, 59);

            return $"{minute} {hour} * * {dayOfWeek.ToString().Substring(0, 3)}";
        }

        /// <summary>
        /// 周期性为月的任务
        /// </summary>
        /// <param name="day">几号开始，默认从一号开始</param>
        /// <param name="hour">第几小时开始，默认从0点开始</param>
        /// <param name="minute">第几分钟开始，默认从第0分钟开始</param>
        /// <param name="month">第几月开始，默认从第1月开始</param>
        /// <param name="interval">月份间隔</param>
        /// <returns></returns>
        public static string Month(int day = 1, int hour = 0, int minute = 0, int month = 1, int interval = 1)
        {
            Check.Range(hour, nameof(hour), 0, 23);
            Check.Range(minute, nameof(minute), 0, 59);
            Check.Range(day, nameof(day), 1, 31);

            return $"0 {minute} {hour} {day} {month}/{interval} ?";
        }

        /// <summary>
        /// 周期性为年的任务
        /// </summary>
        /// <param name="month">几月开始，默认从一月开始</param>
        /// <param name="day">几号开始，默认从一号开始</param>
        /// <param name="hour">第几小时开始，默认从0点开始</param>
        /// <param name="year">第几年开始，默认从2001年开始</param>
        /// <param name="minute">第几分钟开始，默认从第0分钟开始</param>
        /// <returns></returns>
        public static string Year(int month = 1, int day = 1, int hour = 0, int minute = 0, int year = 2001, int interval = 1)
        {
            Check.Range(hour, nameof(hour), 0, 23);
            Check.Range(minute, nameof(minute), 0, 59);
            Check.Range(day, nameof(day), 1, 31);
            Check.Range(month, nameof(month), 1, 12);

            return $"0 {minute} {hour} {day} {month} ? {year}/{interval}";
        }
    }
}
