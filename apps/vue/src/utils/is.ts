import { isNull, isFunction } from 'lodash-es';

export { isNull, isFunction };

export {
  isArguments,
  isArrayBuffer,
  isArrayLike,
  isArrayLikeObject,
  isBuffer,
  isBoolean,
  isDate,
  isElement,
  isEmpty,
  isEqual,
  isEqualWith,
  isError,
  isFinite,
  isLength,
  isMap,
  isMatch,
  isMatchWith,
  isNative,
  isNil,
  isNumber,
  isObjectLike,
  isPlainObject,
  isRegExp,
  isSafeInteger,
  isSet,
  isString,
  isSymbol,
  isTypedArray,
  isUndefined,
  isWeakMap,
  isWeakSet,
} from 'lodash-es';

const toString = Object.prototype.toString;

export function is(val: unknown, type: string) {
  return toString.call(val) === `[object ${type}]`;
}

export function isDef<T = unknown>(val?: T): val is T {
  return typeof val !== 'undefined';
}

export function isUnDef<T = unknown>(val?: T): val is T {
  return !isDef(val);
}

export function isObject(val: any): val is Record<any, any> {
  return val !== null && is(val, 'Object');
}

export function isRegMatch(reg: RegExp, val: string) {
  return reg.test(val);
}

export function isNullAndUnDef(val: unknown): val is null | undefined {
  return isUnDef(val) && isNull(val);
}

export function isNullOrUnDef(val: unknown): val is null | undefined {
  return isUnDef(val) || isNull(val);
}

export function isPromise<T = any>(val: unknown): val is Promise<T> {
  return is(val, 'Promise') && isObject(val) && isFunction(val.then) && isFunction(val.catch);
}

export function isArray(val: any): val is Array<any> {
  return val && Array.isArray(val);
}

export function isWindow(val: any): val is Window {
  return typeof window !== 'undefined' && is(val, 'Window');
}

export const isServer = typeof window === 'undefined';

export const isClient = !isServer;

export function isEmail(val: string) {
  const reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
  return isRegMatch(reg, val);
}

export function isPhone(val: string) {
  const reg = /^(13[0-9]|14[5|7]|15[0-9]|17[0-9]|18[0-9]|19[0-9])\d{8}$/;
  return isRegMatch(reg, val);
}

export function isUrl(path: string): boolean {
  const reg = /^http(s)?:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/;
  return isRegMatch(reg, path);
}

export function isSortUrl(path: string) {
  const reg1 = /^[a-zA-Z]+:\d{1,5}$/;
  const reg2 = /^([a-zA-Z\-\d+]+\.){1,}[a-z\-\d]+:\d{1,5}$/;
  return isRegMatch(reg1, path) || isRegMatch(reg2, path);
}

export function isIpPort(path: string): boolean {
  const reg =
    /^(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])(\.(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])){3}:([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$/;
  return isRegMatch(reg, path);
}

export function isDigit(val: string) {
  return val.split('').some(_isDigit);
}

export function isLower(val: string) {
  return val.split('').some(_isLower);
}

export function isUpper(val: string) {
  return val.split('').some(_isUpper);
}

export function isLetterOrDigit(val: string) {
  const arr = val.split('');
  return !arr.some(_isLetterOrDigit);
}

function _isDigit(char: string) {
  return isRegMatch(/[0-9]/g, char);
}

function _isLower(char: string) {
  return isRegMatch(/[a-z]/g, char);
}

function _isUpper(char: string) {
  return isRegMatch(/[A-Z]/g, char);
}

function _isLetterOrDigit(char: string) {
  return isRegMatch(/[^ A-Za-z0-9_]/g, char);
}

