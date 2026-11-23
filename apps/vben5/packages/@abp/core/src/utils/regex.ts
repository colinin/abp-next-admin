export function isRegMatch(reg: RegExp, val: string) {
  reg.lastIndex = 0;
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
  return /\d/.test(val);
}

export function isLower(val: string) {
  return /[a-z]/.test(val);
}

export function isUpper(val: string) {
  return /[A-Z]/.test(val);
}

export function isLetterOrDigit(val: string) {
  return /^[A-Za-z0-9]+$/.test(val);
}
