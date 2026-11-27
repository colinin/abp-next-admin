<script setup lang="ts">
import { ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getModalData, useNotifications } from '@abp/core';
import Cookies from 'universal-cookie';

import { useMultiTenancyApi } from '../../api/useMultiTenancyApi';
const emits = defineEmits<{
  (event: 'change', data?: Tenant): void;
}>();

const { toastWarning } = useNotifications();

interface Tenant {
  id?: string;
  name?: string;
}

const tenant = ref<Tenant>();
const { findTenantByNameApi } = useMultiTenancyApi();
const cookies = new Cookies(null, { path: '/' });

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
        placeholder: $t('AbpUiMultiTenancy.SwitchTenantHint'),
      },
      fieldName: 'name',
      label: $t('AbpUiMultiTenancy.Name'),
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal<Tenant>({
  onCancel() {
    emits('change', tenant.value);
  },
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  onOpenChange(isOpen) {
    if (isOpen) {
      const { name } = getModalData(modalApi);
      formApi.setFieldValue('name', name);
    }
  },
});
async function onSubmit(values: Record<string, any>) {
  modalApi.setState({ submitting: true });
  try {
    tenant.value = undefined;
    cookies.remove('__tenant');
    if (values.name) {
      const result = await findTenantByNameApi(values.name);
      if (!result.success) {
        toastWarning(
          $t('AbpUiMultiTenancy.GivenTenantIsNotExist', [values.name]),
        );
        return;
      }
      if (!result.isActive) {
        toastWarning(
          $t('AbpUiMultiTenancy.GivenTenantIsNotAvailable', [values.name]),
        );
        return;
      }
      tenant.value = { id: result.tenantId, name: result.normalizedName };
    }
    emits('change', tenant.value);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('AbpUiMultiTenancy.SwitchTenant')">
    <Form />
  </Modal>
</template>

<style scoped></style>
