/**
 * Independent time operation tool to facilitate subsequent switch to dayjs
 */
import dayjs from 'dayjs';

const DATE_TIME_FORMAT = 'YYYY-MM-DD HH:mm:ss';
const DATE_FORMAT = 'YYYY-MM-DD';

export function formatToDateTime(
  date?: dayjs.ConfigType,
  format = DATE_TIME_FORMAT,
): string {
  return dayjs(date).format(format);
}

export function formatToDate(
  date?: dayjs.ConfigType,
  format = DATE_FORMAT,
): string {
  if (typeof date === 'number') date *= 1000;
  return dayjs(date).format(format);
}
/**
 * 获取指定日期
 * @param days 天数
 * @returns 返回指定天数之后的日期
 */
export function getAppointDate(days: number): dayjs.Dayjs {
  const today = new Date(); // 获取当前日期
  const tomorrow = new Date(
    today.getFullYear(),
    today.getMonth(),
    today.getDate() + days,
  );
  return dayjs(tomorrow);
}

export function lastDayOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth() + 1, 0));
}

export function lastDateOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(
    new Date(now.getFullYear(), now.getMonth() + 1, 0, 23, 59, 59, 999),
  );
}

export const dateUtil = dayjs;
