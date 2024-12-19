import { computed, unref } from 'vue';

import {
  getUnique,
  isDigit,
  isLetterOrDigit,
  isLower,
  isNullOrWhiteSpace,
  isUpper,
  useLocalization,
  useSettings,
  ValidationEnum,
} from '@abp/core';

export function usePasswordValidator() {
  const { getNumber, isTrue } = useSettings();
  const { L } = useLocalization(['AbpIdentity', 'AbpUi']);

  const passwordSetting = computed(() => {
    return {
      requiredDigit: isTrue('Abp.Identity.Password.RequireDigit'),
      requiredLength: getNumber('Abp.Identity.Password.RequiredLength'),
      requiredLowercase: isTrue('Abp.Identity.Password.RequireLowercase'),
      requiredUniqueChars: getNumber(
        'Abp.Identity.Password.RequiredUniqueChars',
      ),
      requireNonAlphanumeric: isTrue(
        'Abp.Identity.Password.RequireNonAlphanumeric',
      ),
      requireUppercase: isTrue('Abp.Identity.Password.RequireUppercase'),
    };
  });

  function validate(password: string): Promise<void> {
    return new Promise((resolve, reject) => {
      if (isNullOrWhiteSpace(password)) {
        return reject(
          L(ValidationEnum.FieldRequired, [L('DisplayName:Password')]),
        );
      }
      const setting = unref(passwordSetting);
      if (
        setting.requiredLength > 0 &&
        password.length < setting.requiredLength
      ) {
        return reject(
          L('Volo.Abp.Identity:PasswordTooShort', [setting.requiredLength]),
        );
      }
      if (setting.requireNonAlphanumeric && isLetterOrDigit(password)) {
        return reject(L('Volo.Abp.Identity:PasswordRequiresNonAlphanumeric'));
      }
      if (setting.requiredDigit && !isDigit(password)) {
        return reject(L('Volo.Abp.Identity:PasswordRequiresDigit'));
      }
      if (setting.requiredLowercase && !isLower(password)) {
        return reject(L('Volo.Abp.Identity:PasswordRequiresLower'));
      }
      if (setting.requireUppercase && !isUpper(password)) {
        return reject(L('Volo.Abp.Identity:PasswordRequiresUpper'));
      }
      if (
        setting.requiredUniqueChars >= 1 &&
        getUnique(password).length < setting.requiredUniqueChars
      ) {
        return reject(
          L('Volo.Abp.Identity:PasswordRequiredUniqueChars', [
            setting.requiredUniqueChars,
          ]),
        );
      }
      return resolve();
    });
  }

  return {
    validate,
  };
}
