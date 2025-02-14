<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { IdentityClaimDto } from '../../types/claims';
import type { ClaimModalEvents, ClaimModalProps } from './types';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
import { Button, message, Popconfirm } from 'ant-design-vue';

defineOptions({
  name: 'ClaimTable',
});

const {
  createApi,
  createPolicy,
  deleteApi,
  deletePolicy,
  getApi,
  updateApi,
  updatePolicy,
} = defineProps<ClaimModalProps>();
const emits = defineEmits<ClaimModalEvents>();

const ClaimModal = defineAsyncComponent(() => import('./ClaimModal.vue'));

const gridOptions: VxeGridProps<IdentityClaimDto> = {
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
        return await getApi();
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

const [ClaimEditModal, editModalApi] = useVbenModal({
  connectedComponent: ClaimModal,
});
function onCreate() {
  editModalApi.setData({});
  editModalApi.open();
}

async function onDelete(row: IdentityClaimDto) {
  await deleteApi({
    claimType: row.claimType,
    claimValue: row.claimValue,
  });
  message.success($t('AbpUi.SuccessfullyDeleted'));
  query();
  emits('onDelete');
}

function onUpdate(row: IdentityClaimDto) {
  editModalApi.setData(row);
  editModalApi.open();
}
</script>

<template>
  <div>
    <Grid>
      <template #toolbar-tools>
        <Button type="primary" v-access:code="[createPolicy]" @click="onCreate">
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
              v-access:code="[updatePolicy]"
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
                v-access:code="[deletePolicy]"
              >
                {{ $t('AbpUi.Delete') }}
              </Button>
            </Popconfirm>
          </div>
        </div>
      </template>
    </Grid>
    <ClaimEditModal
      :create-api="createApi"
      :create-policy="createPolicy"
      :delete-api="deleteApi"
      :delete-policy="deletePolicy"
      :update-api="updateApi"
      :update-policy="updatePolicy"
      @change="() => query()"
    />
  </div>
</template>

<style scoped></style>
