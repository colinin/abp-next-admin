<script setup lang="ts">
import type { TenantConnectionStringDto, TenantDto } from '../../types';

import { ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { message } from 'ant-design-vue';

import { useTenantsApi } from '../../api/useTenantsApi';
import ConnectionStringTable from './ConnectionStringTable.vue';

defineProps<{
  dataBaseOptions: { label: string; value: string }[];
}>();

const connectionStrings = ref<TenantConnectionStringDto[]>([]);

const {
  cancel,
  deleteConnectionStringApi,
  getConnectionStringsApi,
  setConnectionStringApi,
} = useTenantsApi();
const [Modal, modalApi] = useVbenModal({
  class: 'w-[800px]',
  onClosed: cancel,
  async onOpenChange(isOpen) {
    connectionStrings.value = [];
    if (isOpen) {
      const dto = modalApi.getData<TenantDto>();
      await onGet(dto.id);
    }
  },
});
async function onGet(id: string) {
  const { items } = await getConnectionStringsApi(id);
  connectionStrings.value = items;
}
async function onChange(data: TenantConnectionStringDto) {
  const dto = modalApi.getData<TenantDto>();
  try {
    modalApi.setState({ submitting: true });
    await setConnectionStringApi(dto.id, data);
    message.success($t('AbpUi.SavedSuccessfully'));
    await onGet(dto.id);
  } finally {
    modalApi.setState({ submitting: false });
  }
}
async function onDelete(data: TenantConnectionStringDto) {
  const dto = modalApi.getData<TenantDto>();
  try {
    modalApi.setState({ submitting: true });
    await deleteConnectionStringApi(dto.id, data.name);
    message.success($t('AbpUi.DeletedSuccessfully'));
    await onGet(dto.id);
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('AbpSaas.ConnectionStrings')">
    <ConnectionStringTable
      :data-base-options="dataBaseOptions"
      :connection-strings="connectionStrings"
      :delete="onDelete"
      :submit="onChange"
    />
  </Modal>
</template>

<style scoped></style>
