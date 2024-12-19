<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { IdentityUserClaimDto, IdentityUserDto } from '../../types';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
import { Button, Popconfirm } from 'ant-design-vue';

import { deleteClaimApi, getClaimsApi } from '../../api/users';
import { IdentityUserPermissions } from '../../constants/permissions';

defineOptions({
  name: 'UserClaimModal',
});

const ClaimEditModal = defineAsyncComponent(
  () => import('./UserClaimEditModal.vue'),
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
const gridOptions: VxeGridProps<IdentityUserClaimDto> = {
  columns: [
    {
      field: 'claimType',
      minWidth: 120,
      title: $t('AbpIdentity.DisplayName:ClaimType'),
    },
    {
      field: 'claimValue',
      title: $t('AbpIdentity.DisplayName:ClaimValue'),
      width: 'auto',
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 180,
    },
  ],
  exportConfig: {},
  keepSource: true,
  pagerConfig: {
    enabled: false,
  },
  proxyConfig: {
    ajax: {
      query: async () => {
        const { id } = modalApi.getData<IdentityUserDto>();
        return await getClaimsApi(id);
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    custom: true,
    export: true,
    // import: true,
    refresh: true,
    zoom: true,
  },
};
const [Grid, { query }] = useVbenVxeGrid({
  gridOptions,
});

const [UserClaimEditModal, editModalApi] = useVbenModal({
  connectedComponent: ClaimEditModal,
});
function onCreate() {
  const { id } = modalApi.getData<IdentityUserDto>();
  editModalApi.setData({
    userId: id,
  });
  editModalApi.open();
}

async function onDelete(row: IdentityUserClaimDto) {
  const { id } = modalApi.getData<IdentityUserDto>();
  await deleteClaimApi(id, {
    claimType: row.claimType,
    claimValue: row.claimValue,
  }).then(() => query());
}

function onUpdate(row: IdentityUserClaimDto) {
  const { id } = modalApi.getData<IdentityUserDto>();
  editModalApi.setData({
    userId: id,
    ...row,
  });
  editModalApi.open();
}
</script>

<template>
  <Modal>
    <Grid>
      <template #toolbar-tools>
        <Button
          type="primary"
          v-access:code="[IdentityUserPermissions.ManageClaims]"
          @click="onCreate"
        >
          {{ $t('AbpIdentity.AddClaim') }}
        </Button>
      </template>
      <template #action="{ row }">
        <div class="flex flex-row">
          <div class="basis-1/2">
            <Button
              :icon="h(EditOutlined)"
              block
              type="link"
              v-access:code="[IdentityUserPermissions.ManageClaims]"
              @click="onUpdate(row)"
            >
              {{ $t('AbpUi.Edit') }}
            </Button>
          </div>
          <div class="basis-1/2">
            <Popconfirm
              :title="$t('AbpIdentity.WillDeleteClaim', [row.claimType])"
              @confirm="onDelete(row)"
            >
              <Button
                :icon="h(DeleteOutlined)"
                block
                danger
                type="link"
                v-access:code="[IdentityUserPermissions.Delete]"
              >
                {{ $t('AbpUi.Delete') }}
              </Button>
            </Popconfirm>
          </div>
        </div>
      </template>
    </Grid>
    <UserClaimEditModal @change="() => query()" />
  </Modal>
</template>

<style scoped></style>
