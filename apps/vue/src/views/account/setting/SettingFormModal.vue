<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    @ok="handleSubmit"
    :width="600"
    :min-height="300"
    :title="commandRef.title"
    :mask-closable="false"
    :can-fullscreen="false"
  >
    <BasicForm
      ref="formElRef"
      layout="vertical"
      :schemas="formSchemas"
      :showActionButtonGroup="false"
    />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, unref, watch, nextTick } from 'vue';
  import { BasicForm, FormActionType, FormSchema } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePasswordValidator } from '/@/hooks/security/usePasswordValidator';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import {
    changePassword,
    sendChangePhoneNumberCode,
    changePhoneNumber,
  } from '/@/api/account/profiles';

  const commandRef = ref<Recordable>({});
  const { createMessage } = useMessage();
  const { ruleCreator } = useValidation();
  const { L } = useLocalization('AbpAccount');
  const { validate: validatePassword } = usePasswordValidator();
  const formElRef = ref<Nullable<FormActionType>>(null);
  const [registerModal, { changeLoading, closeModal }] = useModalInner((command) => {
    commandRef.value = command;
    const formEl = unref(formElRef);
    nextTick(() => {
      formEl?.resetFields();
    });
  });

  const formSchemas = computed((): FormSchema[] => {
    const abpStore = useAbpStoreWithOut();
    const command = unref(commandRef);
    switch (command.key) {
      case 'password':
        return [
          {
            field: 'currentPassword',
            component: 'InputPassword',
            label: L('DisplayName:CurrentPassword'),
            required: true,
            colProps: { span: 24 },
          },
          {
            field: 'newPassword',
            component: 'StrengthMeter',
            label: L('DisplayName:NewPassword'),
            colProps: { span: 24 },
            dynamicRules: () => {
              return [
                ...ruleCreator.fieldRequired({
                  name: 'NewPassword',
                  resourceName: 'AbpAccount',
                  prefix: 'DisplayName',
                }),
                ...ruleCreator.defineValidator({
                  trigger: 'change',
                  validator: (_, value: any) => {
                    if (!value) {
                      return Promise.resolve();
                    }
                    return validatePassword(value)
                      .then(() => Promise.resolve())
                      .catch((error) => Promise.reject(error));
                  },
                }),
              ];
            },
          },
          {
            field: 'confirmNewPassword',
            component: 'StrengthMeter',
            label: L('DisplayName:NewPasswordConfirm'),
            colProps: { span: 24 },
            dynamicRules: ({ values }) => {
              return [
                ...ruleCreator.fieldRequired({
                  name: 'NewPasswordConfirm',
                  resourceName: 'AbpAccount',
                  prefix: 'DisplayName',
                }),
                ...ruleCreator.doNotMatch({
                  name: 'NewPasswordConfirm',
                  resourceName: 'AbpAccount',
                  prefix: 'DisplayName',
                  matchField: 'NewPassword',
                  matchValue: values.newPassword,
                }),
                ...ruleCreator.defineValidator({
                  trigger: 'change',
                  validator: (_, value: any) => {
                    if (!value) {
                      return Promise.resolve();
                    }
                    return validatePassword(value)
                      .then(() => Promise.resolve())
                      .catch((error) => Promise.reject(error));
                  },
                }),
              ];
            },
          },
        ];
      case 'phoneNumber':
        const currentPhoneNumber = abpStore.getApplication.currentUser.phoneNumber;
        return [
          {
            field: 'phoneNumber',
            component: 'Input',
            label: L('DisplayName:PhoneNumber'),
            colProps: { span: 24 },
            defaultValue: currentPhoneNumber,
            componentProps: {
              readonly: true,
            },
          },
          {
            field: 'newPhoneNumber',
            component: 'Input',
            label: L('DisplayName:NewPhoneNumber'),
            colProps: { span: 24 },
            dynamicDisabled: ({ model, field }) => {
              return currentPhoneNumber !== '' && model[field] === currentPhoneNumber;
            },
            dynamicRules: () => {
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
            },
          },
          {
            field: 'code',
            component: 'InputCountDown',
            label: L('DisplayName:SmsVerifyCode'),
            required: true,
            colProps: { span: 24 },
            dynamicDisabled: ({ values }) => {
              return currentPhoneNumber !== '' && values.newPhoneNumber === currentPhoneNumber;
            },
            componentProps: {
              sendCodeApi: _sendResetPasswordCode,
            },
          },
        ];
    }
    return [];
  });
  const commandApi = computed(() => {
    const command = unref(commandRef);
    switch (command.key) {
      case 'password':
        return changePassword;
      case 'phoneNumber':
        return changePhoneNumber;
    }
    return () => Promise.resolve();
  });

  watch(
    () => unref(formSchemas),
    () => {
      const formEl = unref(formElRef);
      nextTick(() => {
        formEl?.clearValidate();
      });
    }
  )

  function _sendResetPasswordCode() {
    const formEl = unref(formElRef);
    if (formEl) {
      return formEl.validateFields(['newPhoneNumber']).then((data) => {
        return sendChangePhoneNumberCode(data.newPhoneNumber)
          .then(() => {
            return Promise.resolve(true);
          })
          .catch(() => {
            return Promise.reject(false);
          });
      })
    }
    return Promise.reject(false);
  }

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl?.validate().then((input) => {
      changeLoading(true);
      commandApi.value(input)
        .then(() => {
          createMessage.success(L('Successful'));
          closeModal();
        })
        .finally(() => {
          changeLoading(false);
        });
    });
  }
</script>
