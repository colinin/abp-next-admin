<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { BlobDto } from '../../types/blobs';

import { defineAsyncComponent, h, nextTick, ref, watch } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { downloadFileFromUrl } from '@vben/utils';

import { formatToDateTime, isNullOrWhiteSpace } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  DownloadOutlined,
  EyeOutlined,
  UploadOutlined,
} from '@ant-design/icons-vue';
import { Button, Card, message, Modal } from 'ant-design-vue';

import { useBlobsApi } from '../../api/useBlobsApi';
import { BlobType } from '../../types/blobs';

defineOptions({
  name: 'FileList',
});

const props = defineProps<{
  containerId: string;
  folderId?: string;
}>();

const kbUnit = 1 * 1024;
const mbUnit = kbUnit * 1024;
const gbUnit = mbUnit * 1024;

const { deleteApi, getPagedListApi } = useBlobsApi();

const selectedKeys = ref<string[]>([]);

const gridOptions: VxeGridProps<BlobDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      sortable: true,
      title: $t('BlobManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'type',
      formatter: ({ row }) => {
        if (row.type === BlobType.Folder) {
          return $t('BlobManagement.BlobType:Folder');
        }
        return $t('BlobManagement.BlobType:File');
      },
      minWidth: 150,
      sortable: true,
      title: $t('BlobManagement.DisplayName:BlobType'),
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
      title: $t('BlobManagement.DisplayName:Size'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      sortable: true,
      title: $t('BlobManagement.DisplayName:CreationTime'),
    },
    {
      align: 'left',
      field: 'lastModificationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      sortable: true,
      title: $t('BlobManagement.DisplayName:LastModificationTime'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 260,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await getPagedListApi({
          containerId: props.containerId,
          maxResultCount: page.pageSize,
          parentId: props.folderId,
          sorting,
          skipCount: (page.currentPage - 1) * page.pageSize,
        });
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
    refresh: false,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<BlobDto> = {
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

const [BlobFileUploadModal, uploadModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./BlobFileUploadModal.vue'),
  ),
});
const [BlobFilePreviewModal, previewModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./BlobFilePreviewModal.vue'),
  ),
});

const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});

function onUpload() {
  uploadModalApi.setData({
    containerId: props.containerId,
    folderId: props.folderId,
  });
  uploadModalApi.open();
}

function onDownload(row: BlobDto) {
  Modal.confirm({
    centered: true,
    content: $t('BlobManagement.BlobWellDownloadMessage', [row.name]),
    onCancel: () => {},
    onOk: () => {
      downloadFileFromUrl({
        fileName: row.name,
        source: `/api/blob-management/blobs/uid/${row.id}/content`,
      });
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onPreview(row: BlobDto) {
  previewModalApi.setData(row);
  previewModalApi.open();
}

function onDelete(row: BlobDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name]),
    onCancel: () => {},
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

watch(
  () => props.containerId,
  (containerId) => {
    nextTick(() => {
      gridApi.setGridOptions({
        toolbarConfig: {
          refresh: !isNullOrWhiteSpace(containerId),
        },
      });
      if (!isNullOrWhiteSpace(containerId)) {
        gridApi.query();
      }
    });
  },
);

watch(
  () => props.folderId,
  (_) => {
    nextTick(() => {
      if (!isNullOrWhiteSpace(props.containerId)) {
        gridApi.query();
      }
    });
  },
);
</script>

<template>
  <Card :title="$t('BlobManagement.Blobs:Files')">
    <Grid>
      <template #toolbar-tools>
        <Button
          v-if="props.containerId"
          :icon="h(UploadOutlined)"
          type="primary"
          @click="onUpload"
        >
          {{ $t('BlobManagement.Blobs:UploadFile') }}
        </Button>
      </template>
      <template #action="{ row }">
        <div class="flex flex-row">
          <Button
            v-if="row.type === BlobType.File"
            :icon="h(EyeOutlined)"
            block
            type="link"
            @click="onPreview(row)"
          >
            {{ $t('BlobManagement.Blobs:Preview') }}
          </Button>
          <Button
            v-if="row.type === BlobType.File"
            :icon="h(DownloadOutlined)"
            block
            type="link"
            @click="onDownload(row)"
          >
            {{ $t('BlobManagement.Blobs:Download') }}
          </Button>
          <Button
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
  </Card>
  <BlobFilePreviewModal />
  <BlobFileUploadModal @file-uploaded="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
