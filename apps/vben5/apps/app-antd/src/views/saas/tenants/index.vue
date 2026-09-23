<script lang="ts" setup>
import type { TenantDto } from '@abp/saas';

import { useRouter } from 'vue-router';

import { Page, useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { useTabbarStore } from '@vben/stores';

import { TenantTable } from '@abp/saas';

import { useAuthStore } from '#/store/auth';

defineOptions({
  name: 'Vben5SaasTenants',
});

interface ImpersonationModalState {
  tenantId: string;
}

const router = useRouter();
const authStore = useAuthStore();
const tabbarStore = useTabbarStore();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  showDefaultActions: false,
  schema: [
    {
      fieldName: 'tenantId',
      component: 'Input',
      hide: true,
      label: $t('AbpSaas.ImpersonationTenant'),
      rules: 'required',
    },
    {
      fieldName: 'userName',
      component: 'Input',
      componentProps: {
        allowClear: true,
      },
      defaultValue: 'admin',
      label: $t('AbpIdentity.DisplayName:UserName'),
      rules: 'required',
    },
  ],
  handleSubmit: onImpersonationSubmit,
});

const [Modal, modalApi] = useVbenModal({
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      await formApi.resetForm();
    }
  },
});

async function handleImpersonation(tenant: TenantDto) {
  modalApi.setData<ImpersonationModalState>({
    tenantId: tenant.id,
  });
  modalApi.open();
}

async function onImpersonationSubmit(values: Record<string, string>) {
  const state = modalApi.getData<ImpersonationModalState>();
  try {
    modalApi.lock();
    await authStore.impersonationUserLogin(
      {
        tenantUserName: values.userName,
        tenantId: state.tenantId,
      },
      () => {
        modalApi.unlock();
        tabbarStore.closeAllTabs(router);
        window.location.replace('/');
      },
    );
  } finally {
    modalApi.unlock();
  }
}
</script>

<template>
  <Page>
    <TenantTable @impersonation="handleImpersonation" />
    <Modal :title="$t('AbpSaas.ImpersonationTenant')">
      <Form />
    </Modal>
  </Page>
</template>
