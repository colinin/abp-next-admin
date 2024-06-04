import { ref, computed, unref, Ref } from 'vue';
import { useValidation } from '/@/hooks/abp/useValidation';
import { usePasswordValidator } from '/@/hooks/security/usePasswordValidator';
import { Rule } from '/@/components/Form';

export enum LoginStateEnum {
  LOGIN,
  REGISTER,
  RESET_PASSWORD,
  MOBILE,
  QR_CODE,
  MOBILE_REGISTER,
  WECHAT,
  SSO,
  TwoFactor,
  Portal,
}

interface UserLoginfo {
  userId?: string;
  userName: string;
  password: string;
  twoFactorToken?: string;
}

const currentState = ref(LoginStateEnum.LOGIN);
const userInfo = ref<UserLoginfo>({
  userName: '',
  password: '',
});

export function useLoginState() {
  function setLoginInfoState(state: UserLoginfo) {
    userInfo.value = state;
  }

  const getLoginInfoState = computed(() => userInfo.value);

  function setLoginState(state: LoginStateEnum) {
    currentState.value = state;
  }

  const getLoginState = computed(() => currentState.value);

  function handleBackLogin() {
    setLoginState(LoginStateEnum.LOGIN);
  }

  return { setLoginState, getLoginState, getLoginInfoState, setLoginInfoState, handleBackLogin };
}

export function useFormValid<T extends Object = any>(formRef: Ref<any>) {
  async function validForm() {
    const form = unref(formRef);
    if (!form) return;
    const data = await form.validate();
    return data as T;
  }

  return { validForm };
}

export function useFormFieldsValid<T extends Object = any>(formRef: Ref<any>) {
  async function validFormFields(fields?: string[]) {
    const form = unref(formRef);
    if (!form) return;
    const data = await form.validateFields(fields);
    return data as T;
  }

  return { validFormFields };
}

export function useFormRules(formData?: Recordable) {
  const { ruleCreator } = useValidation();
  const { validate } = usePasswordValidator();

  const getUserNameFormRule = computed(() =>
    ruleCreator.fieldRequired({
      name: 'UserName',
      resourceName: 'AbpAccount',
      prefix: 'DisplayName',
    }),
  );
  const getPasswordFormRule = computed(() => 
    ruleCreator.fieldRequired({
      name: 'Password',
      resourceName: 'AbpAccount',
      prefix: 'DisplayName',
    })
  );
  const getRegPasswordFormRule = computed(() =>
    ruleCreator.defineValidator({
      trigger: 'change',
      validator: (_, value: any) => {
        if (!value) {
          return Promise.resolve();
        }
        return validate(value)
          .then(() => Promise.resolve())
          .catch((error) => Promise.reject(error));
      },
    })
  );
  const getEmailFormRule = computed(() => {
    return [
      ...ruleCreator.fieldRequired({
        name: 'Email',
        resourceName: 'AbpAccount',
        prefix: 'DisplayName',
      }),
      ...ruleCreator.fieldDoNotValidEmailAddress({
        name: 'Email',
        resourceName: 'AbpAccount',
        prefix: 'DisplayName',
      }),
    ];
  });
  const getMobileFormRule = computed(() => {
    return [
      ...ruleCreator.fieldRequired({
        name: 'PhoneNumber',
        resourceName: 'AbpAccount',
        prefix: 'DisplayName',
      }),
      ...ruleCreator.fieldDoNotValidPhoneNumber({
        name: 'PhoneNumber',
        resourceName: 'AbpAccount',
        prefix: 'DisplayName',
      }),
    ];
  });
  const getMobileCodeFormRule = computed(() =>
    ruleCreator.fieldRequired({
      name: 'SmsVerifyCode',
      resourceName: 'AbpAccount',
      prefix: 'DisplayName',
    }),
  );
  const getValidateConfirmPasswordRule = computed(() => {
    return (password: string) => {
      return ruleCreator.doNotMatch({
        name: 'NewPassword',
        resourceName: 'AbpAccount',
        prefix: 'DisplayName',
        matchField: 'NewPasswordConfirm',
        matchValue: password,
        trigger: 'change',
        required: true,
      });
    };
  });

  // const validatePolicy = async (_: RuleObject, value: boolean) => {
  //   return !value ? Promise.reject(t('sys.login.policyPlaceholder')) : Promise.resolve();
  // };

  const getFormRules = computed((): { [k: string]: Rule | Rule[] } => {
    const userNameFormRule = unref(getUserNameFormRule);
    const passwordFormRule = unref(getPasswordFormRule);
    const regPasswordFormRule = unref(getRegPasswordFormRule);
    const emailFormRule = unref(getEmailFormRule);
    const mobileFormRule = unref(getMobileFormRule);
    const mobileCodeFormRule = unref(getMobileCodeFormRule);
    const validateConfirmPasswordRule = unref(getValidateConfirmPasswordRule);

    const mobileRule = {
      code: mobileCodeFormRule,
      phoneNumber: mobileFormRule,
    };
    switch (unref(currentState)) {
      // register form rules
      case LoginStateEnum.REGISTER:
        return {
          userName: userNameFormRule,
          password: [...passwordFormRule, ...regPasswordFormRule],
          emailAddress: emailFormRule,
          // policy: [{ validator: validatePolicy, trigger: 'change' }],
        };

      // register form rules
      case LoginStateEnum.MOBILE_REGISTER:
        const mobileCodeRule = unref(getMobileCodeFormRule);
        return {
          userName: userNameFormRule,
          password: [...passwordFormRule, ...regPasswordFormRule],
          phoneNumber: mobileFormRule,
          code: mobileCodeRule,
        };

      // reset password form rules
      case LoginStateEnum.RESET_PASSWORD:
        return {
          ...mobileRule,
          newPassword: [...passwordFormRule, ...regPasswordFormRule],
          newPasswordConfirm: [...validateConfirmPasswordRule(formData?.newPassword), ...regPasswordFormRule],
        };

      // mobile form rules
      case LoginStateEnum.MOBILE:
        return mobileRule;

      // login form rules
      default:
        return {
          userName: userNameFormRule,
          password: passwordFormRule,
        };
    }
  });
  return { getFormRules };
}
