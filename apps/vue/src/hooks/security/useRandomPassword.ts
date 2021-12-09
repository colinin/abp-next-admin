import { useSettings } from '/@/hooks/abp/useSettings';
/**
 * 摘自 https://www.html5tricks.com/demo/js-passwd-generator/index.html
 * 侵权请联系删除
 */
export function useRandomPassword() {
  const randomFunc = {
    lower: getRandomLower,
    upper: getRandomUpper,
    number: getRandomNumber,
    symbol: getRandomSymbol,
    defaultNumber: getRandomNumber,
  };

  function getRandomLower() {
    return String.fromCharCode(Math.floor(Math.random() * 26) + 97);
  }

  function getRandomUpper() {
    return String.fromCharCode(Math.floor(Math.random() * 26) + 65);
  }

  function getRandomNumber() {
    return String.fromCharCode(Math.floor(Math.random() * 10) + 48);
  }

  function getRandomSymbol() {
    const symbols = '~!@#$%^&*()_+{}":?><;.,';
    return symbols[Math.floor(Math.random() * symbols.length)];
  }

  function generatePassword() {
    const { settingProvider } = useSettings();
    // 根据配置项生成随机密码
    // 密码长度
    const length = settingProvider.getNumber('Abp.Identity.Password.RequiredLength');
    // 需要小写字母
    const lower = settingProvider.isTrue('Abp.Identity.Password.RequireLowercase');
    // 需要大写字母
    const upper = settingProvider.isTrue('Abp.Identity.Password.RequireUppercase');
    // 需要数字
    const number = settingProvider.isTrue('Abp.Identity.Password.RequireDigit');
    // 需要符号
    const symbol = settingProvider.isTrue('Abp.Identity.Password.RequireNonAlphanumeric');
    // 默认生成数字
    const defaultNumber = !lower && !upper && !number && !symbol;

    let generatedPassword = '';
    const typesArr = [{ lower }, { upper }, { number }, { symbol }, { defaultNumber }].filter(
      (item) => Object.values(item)[0],
    );
    for (let i = 0; i < length; i++) {
      typesArr.forEach((type) => {
        const funcName = Object.keys(type)[0];
        generatedPassword += randomFunc[funcName]();
      });
    }
    return generatedPassword.slice(0, length);
  }

  return { generatePassword };
}
