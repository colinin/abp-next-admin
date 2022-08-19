<template>
  <BasicForm
    @register="registerForm"
    :submitButtonOptions="{
      text: L('SendVerifyCode'),
      loading: sendingCode,
    }"
    :submit-func="handleSendCode" />
</template>

<script lang="ts" setup>
  import { ref, watch, nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getTwoFactorProviders, sendEmailSignCode, sendPhoneSignCode } from '/@/api/account/accounts';

  const emits = defineEmits(['send-code']);
  const props = defineProps({
    userInfo: {
      type: Object as PropType<Recordable>,
      required: true,
    },
  });
  const sendingCode = ref(false);
  const { ruleCreator } = useValidation();
  const { L }  = useLocalization('AbpAccount');
  const [registerForm, { updateSchema, setFieldsValue, clearValidate, validate }] = useForm({
    labelAlign: 'left',
    labelWidth: 120,
    layout: 'vertical',
    schemas: [
      {
        field: 'type',
        label: L('SelectedProvider'),
        component: 'Select',
        required: true,
        colProps: { span: 24 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'Email',
        label: L('DisplayName:EmailAddress'),
        component: 'Input',
        ifShow: false,
        colProps: { span: 24 },
        rules: [
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
        ],
      },
      {
        field: 'Phone',
        label: L('DisplayName:PhoneNumber'),
        component: 'Input',
        ifShow: false,
        colProps: { span: 24 },
        rules: [
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
        ],
      },
    ],
    actionColOptions: {
      span: 24,
    },
    showResetButton: false,
  });

  watch(
    () => props.userInfo,
    (userInfo) => {
      if (userInfo.userId) {
        fetchUserTwoFactorProviders(userInfo.userId);
      }
    },
    {
      immediate: true,
    }
  )

  function fetchUserTwoFactorProviders(userId: string) {
    getTwoFactorProviders(userId).then((res) => {
      nextTick(() => {
        updateSchema({
          field: 'type',
          componentProps: {
            options: res.items,
            style: { width: '100%' },
            fieldNames: { label: 'name', value: 'value' },
            onChange: (val) => {
              setFieldsValue({
                Email: '',
                Phone: '',
              });
              clearValidate();
              updateSchema([
                { field: 'Email', ifShow: false },
                { field: 'Phone', ifShow: false },
              ]);
              if (val) {
                updateSchema({
                  field: val,
                  ifShow: true,
                });
              }
            }
          },
        });
      });
    });
  }

  function handleSendCode() {
    return validate().then((input) => {
      switch (input.type) {
        // 短信验证码
        case 'Phone':
          sendingCode.value = true;
          return sendPhoneSignCode(input.Phone).then(() => {
            emits('send-code', input.type);
            sendingCode.value = false;
          });
        // 邮件验证码
        case 'Email':
          sendingCode.value = true;
          return sendEmailSignCode(input.Email).then(() => {
            emits('send-code', input.type);
            sendingCode.value = false;
          });
        // 不发送验证码, Rfc6238
        case 'Authenticator':
          emits('send-code', input.type);
          return Promise.resolve(input.type);
        default: return Promise.reject('invalid provider');
      }
    });
  }
</script>
