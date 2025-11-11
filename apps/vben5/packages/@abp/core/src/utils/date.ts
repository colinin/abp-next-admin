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

/**
 * 获取本周第一天
 * @returns 返回本周第一天
 */
export function firstDayOfWeek(): dayjs.Dayjs {
  const now = new Date();
  const today = now.getDay();
  const dayOffset = today === 0 ? -6 : 1 - today;
  const monday = new Date(now);
  monday.setDate(now.getDate() + dayOffset);
  return dayjs(
    new Date(monday.getFullYear(), monday.getMonth(), monday.getDate()),
  );
}

/**
 * 获取当月第一天
 * @returns 返回当月第一天
 */
export function firstDayOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth(), 1));
}

/**
 * 获取当月最后一天00:00:00
 * @returns 返回当月最后一天00:00:00
 */
export function lastDayOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth() + 1, 0));
}

/**
 * 获取当月最后一天23:59:59
 * @returns 返回当月最后一天23:59:59
 */
export function lastDateOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(
    new Date(now.getFullYear(), now.getMonth() + 1, 0, 23, 59, 59, 999),
  );
}

/**
 * 获取上个月第一天
 * @returns 返回上个月第一天
 */
export function firstDayOfLastMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth() - 1, 1));
}

/**
 * 获取上个月最后一天23:59:59
 * @returns 返回上个月最后一天23:59:59
 */
export function lastDateOfLastMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(
    new Date(now.getFullYear(), now.getMonth(), 0, 23, 59, 59, 999), // 当月第0天 = 上月最后一天
  );
}

/**
 * 获取本年第一天
 * @returns 返回本年第一天
 */
export function firstDayOfYear(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), 0, 1));
}

export const dateUtil = dayjs;
