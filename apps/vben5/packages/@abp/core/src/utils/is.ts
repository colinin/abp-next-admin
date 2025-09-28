import isDateEx from 'lodash.isdate';
import isNumberEx from 'lodash.isnumber';

export function isNullAndUnDef(val: unknown): val is null | undefined {
  return isUnDef(val) && isNull(val);
}

export function isNullOrUnDef(val: unknown): val is null | undefined {
  return isUnDef(val) || isNull(val);
}

export function isDef<T = unknown>(val?: T): val is T {
  return val !== undefined;
}

export function isUnDef<T = unknown>(val?: T): val is T {
  return !isDef(val);
}

export function isNull(value: any): value is null {
  return value === null;
}

export function isNumber(value?: any): value is number {
  return isNumberEx(value);
}

export function isDate(value?: any): value is Date {
  return isDateEx(value);
}
