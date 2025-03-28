<script setup lang="ts">
import { ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useCookies } from '@vueuse/integrations/useCookies';
import { message } from 'ant-design-vue';

import { useMultiTenancyApi } from '../../api/useMultiTenancyApi';

interface Tenant {
  id?: string;
  name?: string;
}

const emits = defineEmits<{
  (event: 'change', data?: Tenant): void;
}>();

const tenant = ref<Tenant>();
const cookies = useCookies();
const { findTenantByNameApi } = useMultiTenancyApi();

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
const [Modal, modalApi] = useVbenModal({
  onCancel() {
    emits('change', tenant.value);
  },
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  onOpenChange(isOpen) {
    if (isOpen) {
      const { name } = modalApi.getData<Tenant>();
      formApi.setFieldValue('name', name);
    }
  },
});
async function onSubmit(values: Record<string, any>) {
  modalApi.setState({ submitting: true });
  try {
    tenant.value = undefined;
    cookies.remove('__tenant', {
      path: '/',
    });
    // localStorage.removeItem('__tenant');
    if (values.name) {
      const result = await findTenantByNameApi(values.name);
      if (!result.success) {
        message.warning(
          $t('AbpUiMultiTenancy.GivenTenantIsNotExist', [values.name]),
        );
        return;
      }
      if (!result.isActive) {
        message.warning(
          $t('AbpUiMultiTenancy.GivenTenantIsNotAvailable', [values.name]),
        );
        return;
      }
      tenant.value = { id: result.tenantId, name: result.normalizedName };
      if (result.tenantId) {
        // localStorage.setItem('__tenant', result.tenantId);
        cookies.set('__tenant', result.tenantId, {
          path: '/',
        });
      }
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
