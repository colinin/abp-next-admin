<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { CarbonCopyDto } from '../../types/carbon-copies';

import { ref } from 'vue';

import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { Button, Tag } from 'ant-design-vue';

import { useApprovalCarbonCopiesApi } from '../../api/useApprovalCarbonCopiesApi';
import { formatDateTime } from './approval-table-utils';
import ApprovalInstanceDetailModal from './ApprovalInstanceDetailModal.vue';

const carbonCopiesApi = useApprovalCarbonCopiesApi();

const detailOpen = ref(false);
const detailInstanceId = ref<null | string>(null);

const formOptions: VbenFormProps = {
  collapsed: true,
  schema: [
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Search'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<CarbonCopyDto> = {
  columns: [
    {
      field: 'title',
      minWidth: 240,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Title'),
    },
    {
      field: 'summary',
      minWidth: 200,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Summary'),
    },
    {
      field: 'senderDisplayName',
      minWidth: 140,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Initiator'),
    },
    {
      field: 'notifiedAt',
      slots: { default: 'notifiedAt' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:CarbonCopy:CopiedAt'),
      width: 180,
    },
    {
      field: 'isRead',
      slots: { default: 'readState' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Status'),
      width: 90,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Actions'),
      width: 100,
    },
  ],
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await carbonCopiesApi.getListApi({
          sorting,
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    custom: true,
    refresh: true,
    refreshOptions: { code: 'query' },
    zoom: true,
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridOptions,
});

async function openCarbonCopy(item: CarbonCopyDto) {
  if (!item.instanceId) {
    return;
  }
  try {
    if (!item.isRead) {
      await carbonCopiesApi.markReadApi(item.id);
    }
  } catch {
    // ignore read-state errors and still open the detail
  }
  detailInstanceId.value = item.instanceId;
  detailOpen.value = true;
  await gridApi.query();
}
</script>

<template>
  <Grid :table-title="$t('ElsaNextApprovalFlow.ApprovalFlow:CarbonCopy:Title')">
    <template #notifiedAt="{ row }">
      {{ formatDateTime(row.notifiedAt) }}
    </template>
    <template #readState="{ row }">
      <Tag :color="row.isRead ? 'default' : 'warning'">
        {{
          row.isRead
            ? $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Read')
            : $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Unread')
        }}
      </Tag>
    </template>
    <template #action="{ row }">
      <Button
        size="small"
        type="link"
        @click="openCarbonCopy(row as CarbonCopyDto)"
      >
        {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:View') }}
      </Button>
    </template>
  </Grid>

  <ApprovalInstanceDetailModal
    v-model:open="detailOpen"
    :instance-id="detailInstanceId"
  />
</template>
