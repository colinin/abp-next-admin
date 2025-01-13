<script setup lang="ts">
import type { IdentityUserDto } from '../../types';
import type {
  IdentityClaimCreateDto,
  IdentityClaimDeleteDto,
  IdentityClaimUpdateDto,
} from '../../types/claims';

import { defineAsyncComponent } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useUsersApi } from '../../api/useUsersApi';
import { IdentityUserPermissions } from '../../constants/permissions';

defineOptions({
  name: 'UserClaimModal',
});

const ClaimTable = defineAsyncComponent(
  () => import('../claims/ClaimTable.vue'),
);

const { cancel, createClaimApi, deleteClaimApi, getClaimsApi, updateClaimApi } =
  useUsersApi();
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('User claim modal has closed!');
  },
  showCancelButton: false,
  showConfirmButton: false,
  title: $t('AbpIdentity.ManageClaim'),
});

async function onGet() {
  const { id } = modalApi.getData<IdentityUserDto>();
  return await getClaimsApi(id);
}

async function onCreate(input: IdentityClaimCreateDto) {
  const { id } = modalApi.getData<IdentityUserDto>();
  await createClaimApi(id, input);
}

async function onDelete(input: IdentityClaimDeleteDto) {
  const { id } = modalApi.getData<IdentityUserDto>();
  await deleteClaimApi(id, input);
}

async function onUpdate(input: IdentityClaimUpdateDto) {
  const { id } = modalApi.getData<IdentityUserDto>();
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
      :create-policy="IdentityUserPermissions.ManageClaims"
      :delete-api="onDelete"
      :delete-policy="IdentityUserPermissions.ManageClaims"
      :get-api="onGet"
      :update-api="onUpdate"
      :update-policy="IdentityUserPermissions.ManageClaims"
    />
  </Modal>
</template>

<style scoped></style>
