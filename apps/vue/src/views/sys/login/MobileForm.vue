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
          class="fix-auto-fill"
        />
      </FormItem>
      <FormItem name="code" class="enter-x" :label="L('DisplayName:SmsVerifyCode')">
        <CountdownInput
          size="large"
          class="fix-auto-fill"
          v-model:value="formData.code"
          :placeholder="L('DisplayName:SmsVerifyCode')"
          :send-code-api="handleSendCode"
        />
      </FormItem>

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

<script lang="ts" setup>
  import { reactive, ref, computed, unref } from 'vue';
  import { Form, Input, Button } from 'ant-design-vue';
  import { CountdownInput } from '/@/components/CountDown';
  import LoginFormTitle from './LoginFormTitle.vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLoginState, useFormRules, useFormValid, LoginStateEnum } from './useLogin';
  import { MultiTenancyBox } from '/@/components/MultiTenancyBox';
  import { sendPhoneSignCode } from '/@/api/account/accounts';
  import { useUserStore } from '/@/store/modules/user';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useMessage } from '/@/hooks/web/useMessage';

  const FormItem = Form.Item;
  const { notification } = useMessage();
  const { t } = useI18n();
  const { L } = useLocalization('AbpAccount');
  const { handleBackLogin, getLoginState } = useLoginState();
  const userStore = useUserStore();

  const formRef = ref();
  const loading = ref(false);

  const formData = reactive({
    phoneNumber: '',
    code: '',
  });

  const { validForm } = useFormValid(formRef);
  const { getFormRules } = useFormRules();

  const getShow = computed(() => unref(getLoginState) === LoginStateEnum.MOBILE);

  function handleSendCode() {
    return sendPhoneSignCode(formData.phoneNumber)
      .then(() => {
        return Promise.resolve(true);
      })
      .catch(() => {
        return Promise.reject(false);
      });
  }

  async function handleLogin() {
    const data = await validForm();
    if (!data) return;
    try {
      loading.value = true;
      const userInfo = await userStore.loginByPhone({
        phoneNumber: data.phoneNumber,
        code: data.code,
        mode: 'none', //不要默认的错误提示
      });
      if (userInfo) {
        notification.success({
          message: t('sys.login.loginSuccessTitle'),
          description: `${t('sys.login.loginSuccessDesc')}: ${
            userInfo.realName ?? userInfo.username
          }`,
          duration: 3,
        });
      }
    } finally {
      loading.value = false;
    }
  }
</script>
