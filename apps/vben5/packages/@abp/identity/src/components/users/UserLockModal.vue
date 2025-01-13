<!-- eslint-disable no-unused-vars -->
<script setup lang="ts">
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useVbenForm } from '@abp/ui';

import { useUsersApi } from '../../api/useUsersApi';

defineOptions({
  name: 'UserLockModal',
});
const emits = defineEmits<{
  (event: 'change'): void;
}>();

enum LockType {
  Days = 86_400,
  Hours = 3600,
  Minutes = 60,
  Months = 2_678_400, // 按31天计算
  Seconds = 1,
  Years = 32_140_800, // 按31*12天计算
}

const { cancel, lockApi } = useUsersApi();
const [Form, formApi] = useVbenForm({
  commonConfig: {
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      componentProps: {
        style: {
          display: 'none',
        },
      },
      fieldName: 'userId',
    },
    {
      component: 'InputNumber',
      fieldName: 'seconds',
      label: $t('AbpIdentity.LockTime'),
      rules: 'required',
    },
    {
      component: 'Select',
      componentProps: {
        options: [
          // TODO: 本地化
          { label: $t('LockType:Seconds'), value: LockType.Seconds },
          { label: $t('LockType:Minutes'), value: LockType.Minutes },
          { label: $t('LockType:Hours'), value: LockType.Hours },
          { label: $t('LockType:Days'), value: LockType.Days },
          { label: $t('LockType:Months'), value: LockType.Months },
          { label: $t('LockType:Years'), value: LockType.Years },
        ],
      },
      defaultValue: LockType.Seconds,
      fieldName: 'type',
      label: $t('AbpIdentity.LockType'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});

const [Modal, modalApi] = useVbenModal({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('User lock modal has closed!');
  },
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen: boolean) {
    if (isOpen) {
      const { id } = modalApi.getData<Record<string, any>>();
      formApi.setValues({
        userId: id,
      });
    }
  },
  title: $t('AbpIdentity.Lock'),
});

async function onSubmit(input: Record<string, any>) {
  try {
    modalApi.setState({
      confirmLoading: true,
    });
    const lockSeconds = input.type * input.seconds;
    await lockApi(input.userId, lockSeconds);
    emits('change');
    modalApi.close();
  } finally {
    modalApi.setState({
      confirmLoading: false,
    });
  }
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
