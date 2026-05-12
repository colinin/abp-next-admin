<script setup lang="ts">
import { defineEmits, defineOptions } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { toDate } from '@abp/core';
import { useMessage } from '@abp/ui';
import { Alert } from 'ant-design-vue';

import { useCacheManagementApi } from '../api/useCacheManagementApi';

defineOptions({
  name: 'CacheRefreshModal',
});
const emit = defineEmits<{
  (event: 'change', key: string): void;
}>();

const message = useMessage();
const { getKeyValueApi, refreshApi } = useCacheManagementApi();

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
      component: 'Textarea',
      disabled: true,
      fieldName: 'key',
      label: $t('CachingManagement.DisplayName:Key'),
      rules: 'required',
      componentProps: {
        autoSize: {
          minRows: 3,
          maxRows: 8,
        },
        showCount: true,
      },
    },
    {
      component: 'DatePicker',
      fieldName: 'absoluteExpiration',
      label: $t('CachingManagement.DisplayName:AbsoluteExpiration'),
      rules: 'required',
      componentProps: {
        format: 'YYYY-MM-DD HH:mm:ss',
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
  });
}

async function onSubmit(input: Record<string, any>) {
  modalApi.lock();
  try {
    await refreshApi({
      key: input.key,
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
