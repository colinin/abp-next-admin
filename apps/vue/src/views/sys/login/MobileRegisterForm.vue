<template>
  <template v-if="getShow">
    <LoginFormTitle class="enter-x" />
    <Form
      class="p-4 enter-x"
      ref="formRef"
      :model="formData"
      :rules="getFormRules"
      colon
      labelAlign="left"
    >
      <FormItem>
        <MultiTenancyBox />
      </FormItem>
      <FormItem name="userName" class="enter-x" :label="L('DisplayName:UserName')">
        <BInput class="fix-auto-fill" size="large" v-model:value="formData.userName" />
      </FormItem>
      <FormItem name="phoneNumber" class="enter-x" :label="L('DisplayName:PhoneNumber')">
        <BInput size="large" v-model:value="formData.phoneNumber" class="fix-auto-fill" />
      </FormItem>
      <FormItem name="code" class="enter-x" :label="L('DisplayName:SmsVerifyCode')">
        <CountdownInput
          size="large"
          class="fix-auto-fill"
          v-model:value="formData.code"
          :sendCodeApi="handleSendCode"
        />
      </FormItem>
      <FormItem name="password" class="enter-x" :label="L('DisplayName:Password')">
        <StrengthMeter size="large" v-model:value="formData.password" />
      </FormItem>

      <FormItem class="enter-x">
        <Button type="primary" size="large" block @click="handleRegister" :loading="loading">
          {{ L('Register') }}
        </Button>
      </FormItem>

      <ARow class="enter-x">
        <ACol :md="24" :xs="24">
          <span>{{ L('AlreadyRegistered') }}</span>
          <Button type="link" @click="handleBackLogin">
            {{ L('Login') }}
          </Button>
        </ACol>
      </ARow>
    </Form>
  </template>
</template>
<script lang="ts" setup>
  import { reactive, ref, unref, computed } from 'vue';
  import LoginFormTitle from './LoginFormTitle.vue';
  import { Form, Button, Row, Col } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { StrengthMeter } from '/@/components/StrengthMeter';
  import { CountdownInput } from '/@/components/CountDown';
  import { MultiTenancyBox } from '/@/components/MultiTenancyBox';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLoginState, useFormRules, useFormValid, LoginStateEnum } from './useLogin';
  import { registerByPhone, sendPhoneRegisterCode } from '/@/api/account/accounts';

  const ACol = Col;
  const ARow = Row;
  const FormItem = Form.Item;
  const { L } = useLocalization('AbpAccount');
  const { handleBackLogin, getLoginState, setLoginState } = useLoginState();

  const formRef = ref();
  const loading = ref(false);

  const formData = reactive({
    userName: '',
    phoneNumber: '',
    password: '',
    code: '',
  });

  const { getFormRules } = useFormRules();
  const { validForm } = useFormValid(formRef);

  const getShow = computed(() => unref(getLoginState) === LoginStateEnum.MOBILE_REGISTER);

  function handleSendCode() {
    return sendPhoneRegisterCode(formData.phoneNumber)
      .then(() => {
        return Promise.resolve(true);
      })
      .catch(() => {
        return Promise.reject(false);
      });
  }

  async function handleRegister() {
    const data = await validForm();
    if (!data) return;
    registerByPhone(data).then(() => {
      formRef.value.resetFields();
      setLoginState(LoginStateEnum.MOBILE);
    });
  }
</script>
