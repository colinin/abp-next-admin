<template>
  <BasicForm
    @register="registerForm"
    :submitButtonOptions="{
      text: L('VerifyAuthenticatorCode'),
      loading: loging,
    }"
    :submit-func="handleLogin"
  >
    <template #submitBefore>
      <Button type="link" @click="handleGoBack" style="margin-right: 5px;">{{ L('ReSendVerifyCode') }}</Button>
    </template>
  </BasicForm>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { Button } from '/@/components/Button';
  import { BasicForm, useForm } from '/@/components/Form';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useUserStore } from '/@/store/modules/user';

  const emits = defineEmits(['go-back']);
  const props = defineProps({
    userInfo: {
      type: Object as PropType<Recordable>,
      required: true,
    },
    provider: {
      type: String,
      required: false,
      default: '',
    }
  });
  const loging = ref(false);
  const { t } = useI18n();
  const { notification } = useMessage();
  const { L }  = useLocalization('AbpAccount');
  const [registerForm, { validate }] = useForm({
    labelAlign: 'left',
    labelWidth: 120,
    layout: 'vertical',
    actionColOptions: {
      span: 24,
    },
    showResetButton: false,
    schemas: [
      {
        field: 'code',
        label: L('VerifyCode'),
        component: 'Input',
        required: true,
        colProps: { span: 24 },
      },
    ],
  });

  function handleLogin() {
    return validate().then((input) => {
      const userStore = useUserStore();
      loging.value = true;
      return userStore.login({
        username: props.userInfo.userName,
        password: props.userInfo.password,
        twoFactorProvider: props.provider,
        twoFactorCode: input.code,
        mode: 'none'
      }).then(() => {
        loging.value = false;
        notification.success({
          message: t('sys.login.loginSuccessTitle'),
          description: `${t('sys.login.loginSuccessDesc')}: ${
            props.userInfo.userName
          }`,
          duration: 3,
        });
      });
    });
  }

  function handleGoBack() {
    emits('go-back');
  }
</script>
