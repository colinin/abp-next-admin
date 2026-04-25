<script setup lang="ts">
import { defineEmits, defineOptions } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { toDate } from '@abp/core';
import { useMessage } from '@abp/ui';
import { Alert } from 'ant-design-vue';

import { useCacheManagementApi } from '../api/useCacheManagementApi';

defineOptions({
  name: 'CacheEditModal',
});
const emit = defineEmits<{
  (event: 'change', key: string): void;
}>();

const message = useMessage();
const { getKeyValueApi, setValueApi } = useCacheManagementApi();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Textarea',
      disabled: true,
      fieldName: 'key',
      label: $t('CachingManagement.DisplayName:Key'),
      rules: 'required',
      componentProps: {
        autoSize: {
          minRows: 3,
        },
      },
    },
    {
      component: 'DatePicker',
      disabled: true,
      fieldName: 'absoluteExpiration',
      label: $t('CachingManagement.DisplayName:AbsoluteExpiration'),
      componentProps: {
        format: 'YYYY-MM-DD HH:mm:ss',
      },
    },
    {
      component: 'Input',
      disabled: true,
      fieldName: 'type',
      label: $t('CachingManagement.DisplayName:Type'),
    },
    {
      component: 'InputNumber',
      disabled: true,
      fieldName: 'size',
      label: $t('CachingManagement.DisplayName:Size'),
    },
    {
      component: 'Textarea',
      fieldName: 'value',
      label: $t('CachingManagement.DisplayName:Values'),
      rules: 'required',
      componentProps: {
        autoSize: {
          minRows: 3,
          maxRows: 8,
        },
        showCount: true,
      },
    },
  ],
  showDefaultActions: false,
});

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/3',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      try {
        modalApi.setState({ loading: true });
        const { key } = modalApi.getData();
        await onGet(key);
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('CachingManagement.CacheInfo'),
});

async function onGet(key: string) {
  const cacheValue = await getKeyValueApi({ key });
  formApi.setValues({
    key,
    absoluteExpiration: toDate(cacheValue.expiration),
    type: cacheValue.type,
    size: cacheValue.size,
    value: cacheValue.values.data,
  });
}

async function onSubmit(input: Record<string, any>) {
  modalApi.lock();
  try {
    await setValueApi({
      key: input.key,
      value: input.value,
      absoluteExpiration: input.absoluteExpiration,
    });
    message.success($t('AbpUi.SavedSuccessfully'));
    emit('change', input.key);
    modalApi.close();
  } finally {
    modalApi.unlock();
  }
}
</script>

<template>
  <Modal>
    <div class="flex flex-col gap-4">
      <Alert
        closable
        show-icon
        type="warning"
        :message="$t('CachingManagement.EditCacheValueAlertMessage')"
      />
      <Form />
    </div>
  </Modal>
</template>

<style scoped></style>
