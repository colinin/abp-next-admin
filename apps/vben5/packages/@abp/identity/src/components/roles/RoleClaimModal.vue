<script setup lang="ts">
import type { IdentityRoleDto } from '../../types';
import type {
  IdentityClaimCreateDto,
  IdentityClaimDeleteDto,
  IdentityClaimUpdateDto,
} from '../../types/claims';

import { defineAsyncComponent } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getModalData } from '@abp/core';

import { useRolesApi } from '../../api/useRolesApi';
import { IdentityRolePermissions } from '../../constants/permissions';

defineOptions({
  name: 'RoleClaimModal',
});

const ClaimTable = defineAsyncComponent(
  () => import('../claims/ClaimTable.vue'),
);

const { cancel, createClaimApi, deleteClaimApi, getClaimsApi, updateClaimApi } =
  useRolesApi();
const [Modal, modalApi] = useVbenModal<IdentityRoleDto>({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('Role Claim modal has closed!');
  },
  onOpenChange(isOpen) {
    let title = $t('AbpIdentity.ManageClaim');
    if (isOpen) {
      const { name } = getModalData(modalApi);
      title += ` - ${name}`;
    }
    modalApi.setState({ title });
  },
  showCancelButton: false,
  showConfirmButton: false,
  title: $t('AbpIdentity.ManageClaim'),
});

async function onGet() {
  const { id } = getModalData(modalApi);
  return await getClaimsApi(id);
}

async function onCreate(input: IdentityClaimCreateDto) {
  const { id } = getModalData(modalApi);
  await createClaimApi(id, input);
}

async function onDelete(input: IdentityClaimDeleteDto) {
  const { id } = getModalData(modalApi);
  await deleteClaimApi(id, input);
}

async function onUpdate(input: IdentityClaimUpdateDto) {
  const { id } = getModalData(modalApi);
  await updateClaimApi(id, input);
}

defineExpose({
  modalApi,
});
</script>

<template>
  <Modal>
    <ClaimTable
      :create-api="onCreate"
      :create-policy="IdentityRolePermissions.ManageClaims"
      :delete-api="onDelete"
      :delete-policy="IdentityRolePermissions.ManageClaims"
      :get-api="onGet"
      :update-api="onUpdate"
      :update-policy="IdentityRolePermissions.ManageClaims"
    />
  </Modal>
</template>

<style scoped></style>
