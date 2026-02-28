<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { OssContainerDto } from '../../types/containes';
import type { OssObjectDto } from '../../types/objects';

import { defineAsyncComponent, h, nextTick, ref, watch } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime, isNullOrWhiteSpace } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  DownloadOutlined,
  UploadOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useObjectsApi } from '../../api';
import { useContainesApi } from '../../api/useContainesApi';
import { OssObjectPermissions } from '../../constants/permissions';

defineOptions({
  name: 'FileList',
});

const props = defineProps<{
  bucket: string;
  path: string;
}>();

const kbUnit = 1 * 1024;
const mbUnit = kbUnit * 1024;
const gbUnit = mbUnit * 1024;

const { hasAccessByCodes } = useAccess();
const { cancel, getObjectsApi } = useContainesApi();
const { deleteApi, generateUrlApi } = useObjectsApi();

const selectedKeys = ref<string[]>([]);

const gridOptions: VxeGridProps<OssContainerDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      sortable: true,
      title: $t('AbpOssManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'isFolder',
      formatter: ({ cellValue }) => {
        return cellValue
          ? $t('AbpOssManagement.DisplayName:Folder')
          : $t('AbpOssManagement.DisplayName:Standard');
      },
      minWidth: 150,
      sortable: true,
      title: $t('AbpOssManagement.DisplayName:FileType'),
    },
    {
      align: 'left',
      field: 'size',
      formatter: ({ cellValue }) => {
        const size = Number(cellValue);
        if (size > gbUnit) {
          let gb = Math.round(size / gbUnit);
          if (gb < 1) {
            gb = 1;
          }
          return `${gb} GB`;
        }
        if (size > mbUnit) {
          let mb = Math.round(size / mbUnit);
          if (mb < 1) {
            mb = 1;
          }
          return `${mb} MB`;
        }
        let kb = Math.round(size / kbUnit);
        if (kb < 1) {
          kb = 1;
        }
        return `${kb} KB`;
      },
      minWidth: 150,
      sortable: true,
      title: $t('AbpOssManagement.DisplayName:Size'),
    },
    {
      align: 'left',
      field: 'creationDate',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      sortable: true,
      title: $t('AbpOssManagement.DisplayName:CreationDate'),
    },
    {
      align: 'left',
      field: 'lastModifiedDate',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      sortable: true,
      title: $t('AbpOssManagement.DisplayName:LastModifiedDate'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 200,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        const res = await getObjectsApi({
          bucket: props.bucket,
          maxResultCount: page.pageSize,
          prefix: props.path,
          sorting,
          skipCount: (page.currentPage - 1) * page.pageSize,
        });
        return {
          totalCount: res.maxKeys,
          items: res.objects,
        };
      },
    },
    autoLoad: false,
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  sortConfig: {
    remote: true,
  },
  toolbarConfig: {
    custom: true,
    export: false,
    refresh: {
      code: 'query',
    },
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<OssContainerDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.name);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.name);
  },
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});

const [FileUploadModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./FileUploadModal.vue'),
  ),
});

function onUpload() {
  modalApi.setData({
    bucket: props.bucket,
    path: props.path,
  });
  modalApi.open();
}

function onDelete(row: OssObjectDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name]),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await deleteApi({
        bucket: props.bucket,
        mD5: false,
        object: row.name,
        path: row.path,
      });
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

async function onDownload(row: OssObjectDto) {
  const downloadUrl = await generateUrlApi({
    bucket: props.bucket,
    mD5: false,
    object: row.name,
    path: row.path,
  });
  const link = document.createElement('a');
  link.style.display = 'none';
  link.href = downloadUrl;
  link.setAttribute('download', row.name);
  document.body.append(link);
  link.click();
}

watch(
  () => props.bucket,
  (bucket) => {
    nextTick(() => {
      gridApi.setGridOptions({
        toolbarConfig: {
          refresh: !isNullOrWhiteSpace(bucket),
        },
      });
      if (!isNullOrWhiteSpace(bucket)) {
        gridApi.query();
      }
    });
  },
);

watch(
  () => props.path,
  (newVal, oldVal) => {
    if (newVal !== oldVal) {
      nextTick(() => {
        gridApi.query();
      });
    }
  },
);
</script>

<template>
  <Grid :table-title="$t('AbpOssManagement.FileList')">
    <template #toolbar-tools>
      <Button
        v-if="props.path"
        :icon="h(UploadOutlined)"
        type="primary"
        @click="onUpload"
      >
        {{ $t('AbpOssManagement.Objects:UploadFile') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="
            !row.isFolder && hasAccessByCodes([OssObjectPermissions.Download])
          "
          :icon="h(DownloadOutlined)"
          block
          type="link"
          @click="onDownload(row)"
        >
          {{ $t('AbpOssManagement.Objects:Download') }}
        </Button>
        <Button
          v-if="hasAccessByCodes([OssObjectPermissions.Delete])"
          :icon="h(DeleteOutlined)"
          danger
          block
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <FileUploadModal @file-uploaded="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
