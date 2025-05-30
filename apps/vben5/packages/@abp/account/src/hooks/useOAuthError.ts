import { $t } from '@vben/locales';

interface OAuthError {
  error: string;
  error_description?: string;
  error_uri?: string;
}

export function useOAuthError() {
  function formatError(error: OAuthError) {
    switch (error.error_description) {
      // 用户名或密码无效
      case 'Invalid username or password!': {
        return $t('abp.oauth.errors.invalidUserNameOrPassword');
      }
      // 需要二次认证
      case 'RequiresTwoFactor': {
        return $t('abp.oauth.errors.requiresTwoFactor');
      }
      // 需要更改密码
      case 'ShouldChangePasswordOnNextLogin': {
        return $t('abp.oauth.errors.shouldChangePassword');
      }
      // Token已失效
      case 'The token is no longer valid.': {
        return $t('abp.oauth.errors.tokenHasExpired');
      }
      // 用户尝试登录次数太多,用户被锁定
      case 'The user account has been locked out due to invalid login attempts. Please wait a while and try again.': {
        return $t('abp.oauth.errors.accountLockedByInvalidLoginAttempts');
      }
      // 用户未启用
      case 'You are not allowed to login! Your account is inactive.': {
        return $t('abp.oauth.errors.accountInactive');
      }
      // 其他不常用的错误信息返回原始字符串
      default: {
        return error.error_description;
      }
    }
  }

  return {
    formatError,
  };
}
