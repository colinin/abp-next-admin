import { isDate, isNumber } from './is';

export function sorter(
  a: Record<string, any>,
  b: Record<string, any>,
  field: string,
): number {
  if (!a[field] && !b[field]) {
    return 0;
  }
  if (a[field] && !b[field]) {
    return 1;
  }
  if (b[field] && !a[field]) {
    return -1;
  }
  const va = a[field];
  const vb = b[field];
  if (isDate(va) && isDate(vb)) {
    return va.getTime() - vb.getTime();
  }
  if (isNumber(va) && isNumber(vb)) {
    return va - vb;
  }
  if (Array.isArray(va) && Array.isArray(vb)) {
    return va.length - vb.length;
  }
  return String(va).localeCompare(String(vb));
}

export { default as sortby } from 'lodash.sortby';
