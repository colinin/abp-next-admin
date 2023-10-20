import { isDate, isNumber } from "./is";

export function sorter(a: Recordable<any>, b: Recordable<any>, field: string) : number {
  if (!a[field] && !b[field]) {
    return 0;
  }
  if (a[field] && !b[field]) {
    return 1;
  }
  if (b[field] && !a[field]) {
    return -1;
  }
  if (isDate(a[field])) {
    return a[field] - b[field];
  }
  if (isNumber(a[field])) {
    return a[field] - b[field];
  }
  if (Array.isArray(a[field])) {
    return a[field].length - b[field].length;
  }
  return String(a[field]).localeCompare(String(b[field]));
}
