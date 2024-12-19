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

import {
  createClaimApi,
  deleteClaimApi,
  getClaimsApi,
  updateClaimApi,
} from '../../api/roles';
import { IdentityRolePermissions } from '../../constants/permissions';

defineOptions({
  name: 'RoleClaimModal',
});

const ClaimTable = defineAsyncComponent(
  () => import('../claims/ClaimTable.vue'),
);

const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {},
  showCancelButton: false,
  showConfirmButton: false,
  title: $t('AbpIdentity.ManageClaim'),
});

async function onGet() {
  const { id } = modalApi.getData<IdentityRoleDto>();
  return await getClaimsApi(id);
}

async function onCreate(input: IdentityClaimCreateDto) {
  const { id } = modalApi.getData<IdentityRoleDto>();
  await createClaimApi(id, input);
}

async function onDelete(input: IdentityClaimDeleteDto) {
  const { id } = modalApi.getData<IdentityRoleDto>();
  await deleteClaimApi(id, input);
}

async function onUpdate(input: IdentityClaimUpdateDto) {
  const { id } = modalApi.getData<IdentityRoleDto>();
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
