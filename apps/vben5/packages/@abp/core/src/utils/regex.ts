/* eslint-disable regexp/no-unused-capturing-group */
export function isRegMatch(reg: RegExp, val: string) {
  return reg.test(val);
}

export function isEmail(val: string) {
  const reg = /^\w+((-\w+)|(\.\w+))*@[A-Z0-9]+((\.|-)[A-Z0-9]+)*\.[A-Z0-9]+$/i;
  return isRegMatch(reg, val);
}

export function isPhone(val: string) {
  const reg = /^(13\d|14[5|7]|15\d|17\d|18\d|19\d)\d{8}$/;
  return isRegMatch(reg, val);
}

export function isDigit(val: string) {
  return [...val].some((element) => _isDigit(element));
}

export function isLower(val: string) {
  return [...val].some((element) => _isLower(element));
}

export function isUpper(val: string) {
  return [...val].some((element) => _isUpper(element));
}

export function isLetterOrDigit(val: string) {
  const arr = [...val];
  return !arr.some((element) => _isLetterOrDigit(element));
}

function _isDigit(char: string) {
  return isRegMatch(/\d/g, char);
}

function _isLower(char: string) {
  return isRegMatch(/[a-z]/g, char);
}

function _isUpper(char: string) {
  return isRegMatch(/[A-Z]/g, char);
}

function _isLetterOrDigit(char: string) {
  return isRegMatch(/[^ \w]/g, char);
}
