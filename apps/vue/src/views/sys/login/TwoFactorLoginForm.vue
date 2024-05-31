<template>
  <template v-if="getShow">
    <LoginFormTitle class="enter-x" />
    <Form
      class="p-4 enter-x"
      :model="formData"
      :rules="getFormRules"
      ref="formRef"
      colon
      labelAlign="left"
      layout="vertical"
      style="width: 100%"
      @keypress.enter="handleLogin"
    >
      <FormItem name="type" class="enter-x" :label="L('SelectedProvider')" required>
        <Select
          size="large"
          v-model:value="formData.type"
          class="fix-auto-fill"
          :options="twoFactorOptions"
          @change="handleResetValidate"
        />
      </FormItem>
      <template v-if="formData.type === 'Email'">
        <FormItem
          name="emailAddress"
          class="enter-x"
          :label="L('DisplayName:EmailAddress')"
          :rules="[
            ...ruleCreator.fieldRequired({
              name: 'EmailAddress',
              resourceName: 'AbpAccount',
              prefix: 'DisplayName',
            }),
            ...ruleCreator.fieldDoNotValidEmailAddress({
              name: 'EmailAddress',
              resourceName: 'AbpAccount',
              prefix: 'DisplayName',
            }),
          ]"
        >
          <Input
            size="large"
            class="fix-auto-fill"
            autocomplete="off"
            v-model:value="formData.emailAddress"
          />
        </FormItem>
        <FormItem name="code" class="enter-x" :label="L('DisplayName:EmailVerifyCode')" required>
          <CountdownInput
            size="large"
            class="fix-auto-fill"
            autocomplete="off"
            v-model:value="formData.code"
            :sendCodeApi="() => handleSendCode('emailAddress', sendEmailSigninCode)"
          />
        </FormItem>
      </template>
      <template v-else-if="formData.type === 'Phone'">
        <FormItem
          name="phoneNumber"
          class="enter-x"
          :label="L('DisplayName:PhoneNumber')"
          :rules="[
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
          ]"
        >
          <Input
            size="large"
            class="fix-auto-fill"
            autocomplete="off"
            v-model:value="formData.emailAddress"
          />
        </FormItem>
        <FormItem name="code" class="enter-x" :label="L('DisplayName:SmsVerifyCode')" required>
          <CountdownInput
            size="large"
            class="fix-auto-fill"
            autocomplete="off"
            v-model:value="formData.code"
            :placeholder="L('DisplayName:SmsVerifyCode')"
            :sendCodeApi="() => handleSendCode('phoneNumber', sendPhoneSigninCode)"
          />
        </FormItem>
      </template>
      <template v-else-if="formData.type === 'Authenticator'">
        <FormItem name="code" class="enter-x" :label="L('VerifyCode')" required>
          <Input
            size="large"
            class="fix-auto-fill"
            autocomplete="off"
            v-model:value="formData.code"
          />
        </FormItem>
      </template>
      <FormItem class="enter-x">
        <Button type="primary" size="large" block @click="handleLogin" :loading="loading">
          {{ L('Login') }}
        </Button>
        <Button size="large" block class="mt-4" @click="handleBackLogin">
          {{ L('GoBack') }}
        </Button>
      </FormItem>
    </Form>
  </template>
</template>

<script setup lang="ts">
  import type { SelectProps, FormInstance } from 'ant-design-vue';
  import { reactive, ref, computed, nextTick, unref, watch } from 'vue';
  import { Form, Input, Button, Select } from 'ant-design-vue';
  import { CountdownInput } from '/@/components/CountDown';
  import LoginFormTitle from './LoginFormTitle.vue';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import {
    useLoginState,
    useFormRules,
    useFormValid,
    useFormFieldsValid,
    LoginStateEnum,
  } from './useLogin';
  import {
    sendPhoneSigninCode,
    sendEmailSigninCode,
    getTwoFactorProviders,
  } from '/@/api/account/accounts';
  import { useUserStore } from '/@/store/modules/user';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useMessage } from '/@/hooks/web/useMessage';

  const FormItem = Form.Item;

  const { notification } = useMessage();
  const { t } = useI18n();
  const { ruleCreator } = useValidation();
  const { L } = useLocalization('AbpAccount');
  const { handleBackLogin, getLoginState, getLoginInfoState } = useLoginState();

  const loading = ref(false);
  const formRef = ref<FormInstance>();
  const twoFactorOptions = ref<SelectProps['options']>([]);

  const formData = reactive({
    type: '',
    emailAddress: '',
    phoneNumber: '',
    code: '',
  });

  const { validForm } = useFormValid(formRef);
  const { validFormFields } = useFormFieldsValid(formRef);
  const { getFormRules } = useFormRules();

  const getShow = computed(() => unref(getLoginState) === LoginStateEnum.TwoFactor);

  function handleResetValidate() {
    formRef.value?.clearValidate();
    formData.code = '';
    formData.emailAddress = '';
    formData.phoneNumber = '';
  }

  function handleSendCode(field: string, sendCodeApi: (...args) => Promise<void>) {
    return validFormFields([field])
      .then(() => {
        return sendCodeApi({
          [field]: formData[field],
        })
          .then(() => {
            return Promise.resolve(true);
          })
          .catch(() => {
            return Promise.reject(false);
          });
      })
      .catch(() => {
        return Promise.reject(false);
      });
  }

  function fetchUserTwoFactorProviders(userId: string) {
    getTwoFactorProviders({ userId: userId }).then((res) => {
      nextTick(() => {
        twoFactorOptions.value = res.items.map((item) => {
          return {
            label: item.name,
            value: item.value.toString(),
          };
        });
        if (res.items.length > 0) {
          formData.type = res.items[0].value.toString();
        }
      });
    });
  }

  function handleLogin() {
    return validForm().then((input) => {
      const userStore = useUserStore();
      loading.value = true;
      const loginInfoState = getLoginInfoState.value;
      return userStore
        .login({
          username: loginInfoState.userName,
          password: loginInfoState.password,
          twoFactorProvider: input.type,
          twoFactorCode: input.code,
          mode: 'none',
        })
        .then(() => {
          notification.success({
            message: t('sys.login.loginSuccessTitle'),
            description: `${t('sys.login.loginSuccessDesc')}: ${loginInfoState.userName}`,
            duration: 3,
          });
        })
        .finally(() => {
          loading.value = false;
        });
    });
  }

  watch(
    () => getLoginInfoState.value.userId,
    (userId) => {
      userId && fetchUserTwoFactorProviders(userId);
    },
    {
      immediate: true,
    },
  );
</script>

<style scoped></style>
