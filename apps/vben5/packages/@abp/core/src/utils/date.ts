import { formatDate, isNumber } from '@vben/utils';

/**
 * Independent time operation tool to facilitate subsequent switch to dayjs
 */
import dayjs from 'dayjs';

const DATE_TIME_FORMAT: Format = 'YYYY-MM-DD HH:mm:ss';
const DATE_FORMAT: Format = 'YYYY-MM-DD';

type FormatDate = Date | dayjs.Dayjs | number | string;
type Format =
  | 'HH'
  | 'HH:mm'
  | 'HH:mm:ss'
  | 'YYYY'
  | 'YYYY-MM'
  | 'YYYY-MM-DD'
  | 'YYYY-MM-DD HH'
  | 'YYYY-MM-DD HH:mm'
  | 'YYYY-MM-DD HH:mm:ss'
  | (string & {});

/**
 * @zh_CN 格式化时间
 * @param date 需要格式化的时间
 * @param format 格式化字符串,参考dayJs文档
 * @returns 返回格式化后的时间字符串
 */
export function formatToDateTime(
  date?: FormatDate,
  format: Format = DATE_TIME_FORMAT,
): string {
  return formatDate(date, format);
}

/**
 * @zh_CN 格式化日期
 * @param date 需要格式化的日期
 * @param format 格式化字符串,参考dayJs文档
 * @returns 返回格式化后的日期字符串
 */
export function formatToDate(
  date?: FormatDate,
  format: Format = DATE_FORMAT,
): string {
  if (isNumber(date)) {
    date *= 1000;
  }
  return formatDate(date, format);
}
/**
 * @zh_CN 获取指定日期
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
 * @zh_CN 获取本周第一天
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
 * @zh_CN 获取本周最后一天
 * @returns 返回本周最后一天
 */
export function lastDayOfWeek(): dayjs.Dayjs {
  return firstDayOfWeek().add(6, 'day');
}

/**
 * @zh_CN 获取当月第一天
 * @returns 返回当月第一天
 */
export function firstDayOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth(), 1));
}

/**
 * @zh_CN 获取当月最后一天00:00:00
 * @returns 返回当月最后一天00:00:00
 */
export function lastDayOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth() + 1, 0));
}

/**
 * @zh_CN 获取当月最后一天23:59:59
 * @returns 返回当月最后一天23:59:59
 */
export function lastDateOfMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(
    new Date(now.getFullYear(), now.getMonth() + 1, 0, 23, 59, 59, 999),
  );
}

/**
 * @zh_CN 获取上个月第一天
 * @returns 返回上个月第一天
 */
export function firstDayOfLastMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth() - 1, 1));
}

/**
 * @zh_CN 获取上个月最后一天23:59:59
 * @returns 返回上个月最后一天23:59:59
 */
export function lastDateOfLastMonth(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(
    new Date(now.getFullYear(), now.getMonth(), 0, 23, 59, 59, 999), // 当月第0天 = 上月最后一天
  );
}

/**
 * @zh_CN 获取本年第一天
 * @returns 返回本年第一天
 */
export function firstDayOfYear(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), 0, 1));
}

/**
 * @zh_CN 获取本年最后一天
 * @returns 返回本年最后一天
 */
export function lastDayOfYear(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), 11, 31));
}

/**
 * @zh_CN 获取最近半年第一天
 * @returns 返回最近半年第一天
 */
export function firstDayOfLastHalfYear(): dayjs.Dayjs {
  const now = new Date();
  return dayjs(new Date(now.getFullYear(), now.getMonth() - 6, 1));
}

/**
 * @zh_CN 获取本季度第一天
 * @returns 返回本季度第一天
 */
export function firstDayOfQuarter(): dayjs.Dayjs {
  const now = new Date();
  const quarter = Math.floor(now.getMonth() / 3) + 1;
  return dayjs(new Date(now.getFullYear(), (quarter - 1) * 3, 1));
}

/**
 * @zh_CN 获取本季度最后一天
 * @returns 返回本季度最后一天
 */
export function lastDayOfQuarter(): dayjs.Dayjs {
  const now = new Date();
  const quarter = Math.floor(now.getMonth() / 3) + 1;
  return dayjs(new Date(now.getFullYear(), quarter * 3, 0));
}

export const dateUtil = dayjs;
