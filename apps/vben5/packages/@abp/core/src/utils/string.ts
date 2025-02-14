/**
 *
 * @param str 字符串是否为空或空格
 */
export function isNullOrWhiteSpace(str?: string) {
  return str === undefined || str === null || str === '' || str === ' ';
}

/**
 * 格式化字符串
 * @param formatted 需要处理的字符串
 * @param args 参数列表，可以是数组，也可以是对象
 * @returns 返回格式化的字符串
 * @example format('Hello, {0}!', ['World'])
 * @example format('Hello, {name}!', {name: 'World'})
 */
export function format(formatted: string, args: any[] | object) {
  if (Array.isArray(args)) {
    for (const [i, arg] of args.entries()) {
      const regexp = new RegExp(String.raw`\{` + i + String.raw`\}`, 'gi');
      formatted = formatted.replace(regexp, arg);
    }
  } else if (typeof args === 'object') {
    Object.keys(args).forEach((key) => {
      const regexp = new RegExp(String.raw`\{` + key + String.raw`\}`, 'gi');
      const param = (args as any)[key];
      formatted = formatted.replace(regexp, param);
    });
  }
  return formatted;
}

export function getUnique(val: string) {
  const arr = [...val];
  const newArr = [...new Set(arr)];
  return newArr.join('');
}
