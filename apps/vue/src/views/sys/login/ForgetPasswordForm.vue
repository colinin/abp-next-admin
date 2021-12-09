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
    >
      <FormItem>
        <MultiTenancyBox />
      </FormItem>
      <FormItem name="phoneNumber" class="enter-x" :label="L('DisplayName:PhoneNumber')">
        <Input
          size="large"
          v-model:value="formData.phoneNumber"
          :placeholder="L('DisplayName:PhoneNumber')"
        />
      </FormItem>
      <FormItem name="code" class="enter-x" :label="L('DisplayName:SmsVerifyCode')">
        <CountdownInput
          size="large"
          v-model:value="formData.code"
          :placeholder="L('DisplayName:SmsVerifyCode')"
          :send-code-api="handleSendCode"
        />
      </FormItem>
      <FormItem name="newPassword" class="enter-x" :label="L('DisplayName:NewPassword')">
        <StrengthMeter size="large" v-model:value="formData.newPassword" />
      </FormItem>
      <FormItem
        name="newPasswordConfirm"
        class="enter-x"
        :label="L('DisplayName:NewPasswordConfirm')"
      >
        <InputPassword size="large" v-model:value="formData.newPasswordConfirm" />
      </FormItem>

      <FormItem class="enter-x">
        <Button type="primary" size="large" block @click="handleReset" :loading="loading">
          {{ L('ResetPassword') }}
        </Button>
        <Button size="large" block class="mt-4" @click="handleBackLogin">
          {{ L('Back') }}
        </Button>
      </FormItem>
    </Form>
  </template>
</template>
<script lang="ts" setup>
  import { reactive, ref, computed, unref } from 'vue';
  import LoginFormTitle from './LoginFormTitle.vue';
  import { Form, Button, InputPassword } from 'ant-design-vue';
  import { Input } from '/@/components/Input';
  import { CountdownInput } from '/@/components/CountDown';
  import { StrengthMeter } from '/@/components/StrengthMeter';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { MultiTenancyBox } from '/@/components/MultiTenancyBox';
  import { useLoginState, useFormRules, useFormValid, LoginStateEnum } from './useLogin';
  import { resetPassword, sendPhoneResetPasswordCode } from '/@/api/account/accounts';

  const FormItem = Form.Item;
  const { L } = useLocalization('AbpAccount');
  const { handleBackLogin, getLoginState, setLoginState } = useLoginState();

  const formRef = ref();
  const loading = ref(false);
  const formData = reactive({
    phoneNumber: '',
    code: '',
    newPassword: '',
    newPasswordConfirm: '',
  });

  const { getFormRules } = useFormRules(formData);
  const { validForm } = useFormValid(formRef);

  const getShow = computed(() => unref(getLoginState) === LoginStateEnum.RESET_PASSWORD);

  function handleSendCode() {
    return sendPhoneResetPasswordCode(formData.phoneNumber)
      .then(() => {
        return Promise.resolve(true);
      })
      .catch(() => {
        return Promise.reject(false);
      });
  }

  async function handleReset() {
    const data = await validForm();
    if (!data) return;
    resetPassword(data).then(() => {
      formRef.value.resetFields();
      setLoginState(LoginStateEnum.LOGIN);
    });
  }
</script>
