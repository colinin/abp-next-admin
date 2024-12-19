import { useSettings } from '@abp/core';

/**
 * 摘自 https://www.html5tricks.com/demo/js-passwd-generator/index.html
 * 侵权请联系删除
 */
export function useRandomPassword() {
  const randomFunc: { [key: string]: () => string } = {
    defaultNumber: getRandomNumber,
    lower: getRandomLower,
    number: getRandomNumber,
    symbol: getRandomSymbol,
    upper: getRandomUpper,
  };

  function getRandomLower() {
    return String.fromCodePoint(Math.floor(Math.random() * 26) + 97);
  }

  function getRandomUpper() {
    return String.fromCodePoint(Math.floor(Math.random() * 26) + 65);
  }

  function getRandomNumber() {
    return String.fromCodePoint(Math.floor(Math.random() * 10) + 48);
  }

  function getRandomSymbol() {
    const symbols = '~!@#$%^&*()_+{}":?><;.,';
    return symbols[Math.floor(Math.random() * symbols.length)] ?? '';
  }

  function generatePassword() {
    const { getNumber, isTrue } = useSettings();
    // 根据配置项生成随机密码
    // 密码长度
    const length = getNumber('Abp.Identity.Password.RequiredLength');
    // 需要小写字母
    const lower = isTrue('Abp.Identity.Password.RequireLowercase');
    // 需要大写字母
    const upper = isTrue('Abp.Identity.Password.RequireUppercase');
    // 需要数字
    const number = isTrue('Abp.Identity.Password.RequireDigit');
    // 需要符号
    const symbol = isTrue('Abp.Identity.Password.RequireNonAlphanumeric');
    // 默认生成数字
    const defaultNumber = !lower && !upper && !number && !symbol;

    let generatedPassword = '';
    const typesArr = [
      { lower },
      { upper },
      { number },
      { symbol },
      { defaultNumber },
    ].filter((item) => Object.values(item)[0]);
    for (let i = 0; i < length; i++) {
      typesArr.forEach((type) => {
        const funcName = Object.keys(type)[0];
        if (funcName && randomFunc[funcName]) {
          generatedPassword += randomFunc[funcName]();
        }
      });
    }
    return generatedPassword.slice(0, length);
  }

  return { generatePassword };
}
