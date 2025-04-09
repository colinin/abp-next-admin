<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { GdprRequestDto } from '../types/requests';

import { h } from 'vue';

import { $t } from '@vben/locales';

import { formatToDateTime, useAbpStore } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, DownloadOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal, Tag } from 'ant-design-vue';
import dayJs from 'dayjs';

import { useGdprRequestsApi } from '../api/useGdprRequestsApi';

defineOptions({
  name: 'GdprTable',
});

const {
  cancel,
  deleteApi,
  deletePersonalDataApi,
  downloadPersonalDataApi,
  getPagedListApi,
  preparePersonalDataApi,
} = useGdprRequestsApi();

const abpStore = useAbpStore();
const gridOptions: VxeGridProps<GdprRequestDto> = {
  columns: [
    {
      align: 'left',
      field: 'readyTime',
      slots: { default: 'readly' },
      title: $t('AbpGdpr.DisplayName:ReadyTime'),
    },
    {
      align: 'left',
      field: 'creationTime',
      title: $t('AbpGdpr.DisplayName:CreationTime'),
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
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        const dto = await getPagedListApi({
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
        const now = dayJs();
        return {
          totalCount: dto.totalCount,
          items: dto.items.map((item) => {
            return {
              creationTime: formatToDateTime(item.creationTime),
              id: item.id,
              isReadly: now.diff(dayJs(item.readyTime)) >= 0,
              readyTime: formatToDateTime(item.readyTime),
            };
          }),
        };
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    refresh: true,
  },
};

const gridEvents: VxeGridListeners<GdprRequestDto> = {
  cellClick: () => {},
};
const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});

const onRequestData = async () => {
  gridApi.setLoading(true);
  try {
    await preparePersonalDataApi();
    Modal.success({
      centered: true,
      content: $t('AbpGdpr.PersonalDataPrepareRequestReceived'),
      title: $t('AbpGdpr.RequestedSuccessfully'),
    });
    await gridApi.query();
  } finally {
    gridApi.setLoading(false);
  }
};

const onDeleteData = () => {
  Modal.confirm({
    centered: true,
    content: $t('AbpGdpr.DeletePersonalDataWarning'),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      gridApi.setLoading(true);
      try {
        await deletePersonalDataApi();
        Modal.success({
          centered: true,
          content: $t('AbpGdpr.PersonalDataDeleteRequestReceived'),
          onOk: () => {
            // 刷新页面重载用户信息
            window.location.reload();
          },
          title: $t('AbpGdpr.RequestedSuccessfully'),
        });
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const onDownload = async (row: GdprRequestDto) => {
  const blob = await downloadPersonalDataApi(row.id);
  const url = window.URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  const { userName } = abpStore.application!.currentUser;
  const fileName = `${$t('AbpGdpr.PersonalData')}_${userName}_${formatToDateTime(row.readyTime)}.xlsx`;
  link.setAttribute('download', decodeURIComponent(fileName));
  document.body.append(link);
  link.click();
  window.URL.revokeObjectURL(url);
  link.remove();
};

const onDelete = (row: GdprRequestDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessage'),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      gridApi.setLoading(true);
      try {
        await deleteApi(row.id);
        message.success($t('AbpUi.DeletedSuccessfully'));
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
</script>

<template>
  <Grid :table-title="$t('AbpGdpr.PersonalData')">
    <template #toolbar-tools>
      <div class="flex flex-row gap-2">
        <Button type="primary" @click="onRequestData">
          {{ $t('AbpGdpr.RequestPersonalData') }}
        </Button>
        <Button type="primary" danger @click="onDeleteData">
          {{ $t('AbpGdpr.DeletePersonalData') }}
        </Button>
      </div>
    </template>
    <template #readly="{ row }">
      {{ row.readyTime }}
      <Tag v-if="!row.isReadly" color="warning">
        {{ $t('AbpGdpr.Preparing') }}
      </Tag>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="row.isReadly"
          :icon="h(DownloadOutlined)"
          block
          type="link"
          @click="onDownload(row)"
        >
          {{ $t('AbpGdpr.Download') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
</template>

<style lang="scss" scoped></style>
