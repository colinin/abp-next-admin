<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import { computed, defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useAccess } from '@vben/access';
import { confirm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { sortby } from '@abp/core';
import { useMessage, useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  ReloadOutlined,
} from '@ant-design/icons-vue';
import { Button } from 'ant-design-vue';

import { useCacheManagementApi } from '../api/useCacheManagementApi';
import { CachingManagementPermissions } from '../constants/permissions';

defineOptions({
  name: 'CacheKeyTable',
});

interface CacheVto {
  key: string;
}

const cacheKeys = ref<CacheVto[]>([]);
const selectKeys = ref<string[]>([]);

const message = useMessage();
const { hasAccessByCodes } = useAccess();
const { bulkRemoveApi, getKeysApi, removeApi } = useCacheManagementApi();

const isBulkDeleteEnabled = computed(() => {
  if (selectKeys.value.length === 0) {
    return false;
  }
  return hasAccessByCodes([CachingManagementPermissions.Delete]);
});

const formOptions: VbenFormProps = {
  collapsed: false,
  handleReset: onReset,
  async handleSubmit(params) {
    await onGet(params);
  },
  schema: [
    {
      component: 'Input',
      fieldName: 'prefix',
      label: $t('CachingManagement.DisplayName:Prefix'),
    },
    {
      component: 'Input',
      fieldName: 'filter',
      label: $t('AbpUi.Search'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<CacheVto> = {
  columns: [
    {
      align: 'center',
      type: 'checkbox',
      width: 50,
    },
    {
      align: 'center',
      type: 'seq',
      width: 50,
    },
    {
      align: 'left',
      field: 'key',
      minWidth: 150,
      sortable: true,
      title: $t('CachingManagement.DisplayName:Key'),
    },
    {
      field: 'action',
      fixed: 'right',
      visible: hasAccessByCodes([
        CachingManagementPermissions.Default,
        CachingManagementPermissions.Refresh,
        CachingManagementPermissions.ManageValue,
      ]),
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 260,
    },
  ],
  expandConfig: {
    accordion: true,
    padding: true,
    trigger: 'row',
    height: 300,
  },
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        let items = sortby(cacheKeys.value, sort.field);
        if (sort.order === 'desc') {
          items = items.toReversed();
        }
        const result = {
          totalCount: cacheKeys.value.length,
          items: items.slice(
            (page.currentPage - 1) * page.pageSize,
            page.currentPage * page.pageSize,
          ),
        };
        return new Promise((resolve) => {
          resolve(result);
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
    export: true,
    refresh: false,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<CacheVto> = {
  checkboxAll: (params) => {
    selectKeys.value = params.records.map((x) => x.key);
  },
  checkboxChange: (params) => {
    selectKeys.value = params.records.map((x) => x.key);
  },
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [CacheRefreshModal, refreshModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./CacheRefreshModal.vue'),
  ),
});
const [CacheEditModal, editModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./CacheEditModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const result = await getKeysApi(input);
    cacheKeys.value = result.keys.map((key) => {
      return {
        key,
      };
    });
    setTimeout(() => gridApi.reload(), 100);
  } finally {
    gridApi.setLoading(false);
  }
}

async function onReset() {
  await gridApi.formApi.resetForm();
  const input = await gridApi.formApi.getValues();
  await onGet(input);
}

function onUpdate(row: CacheVto) {
  editModalApi.setData(row);
  editModalApi.open();
}

function onRefresh(row: CacheVto) {
  refreshModalApi.setData(row);
  refreshModalApi.open();
}

function onBulkDelete() {
  confirm({
    centered: true,
    content: $t('CachingManagement.MultipleCacheWillBeDeletedMessage'),
    beforeClose: async ({ isConfirm }) => {
      if (isConfirm) {
        try {
          await bulkRemoveApi({ keys: selectKeys.value });
          message.success($t('AbpUi.DeletedSuccessfully'));
          onGet();
          return true;
        } catch {
          return false;
        }
      }
    },
    icon: 'warning',
    title: $t('AbpUi.AreYouSure'),
  });
}

function onDelete(row: CacheVto) {
  confirm({
    centered: true,
    content: $t('CachingManagement.MultipleCacheWillBeDeletedMessage'),
    beforeClose: async ({ isConfirm }) => {
      if (isConfirm) {
        try {
          await removeApi({ key: row.key });
          message.success($t('AbpUi.DeletedSuccessfully'));
          onGet();
          return true;
        } catch {
          return false;
        }
      }
    },
    icon: 'warning',
    title: $t('AbpUi.AreYouSure'),
  });
}

onMounted(onGet);
</script>

<template>
  <Grid :table-title="$t('CachingManagement.Caches')">
    <template #toolbar-tools>
      <Button
        v-if="isBulkDeleteEnabled"
        :icon="h(DeleteOutlined)"
        type="primary"
        danger
        ghost
        @click="onBulkDelete"
      >
        {{ $t('AbpUi.Delete') }}
      </Button>
    </template>
    <template #action="{ row }">
      <Button
        :icon="h(EditOutlined)"
        type="link"
        v-access:code="[CachingManagementPermissions.ManageValue]"
        @click="onUpdate(row)"
      >
        {{ $t('AbpUi.Edit') }}
      </Button>
      <Button
        :icon="h(DeleteOutlined)"
        danger
        type="link"
        v-access:code="[CachingManagementPermissions.Delete]"
        @click="onDelete(row)"
      >
        {{ $t('AbpUi.Delete') }}
      </Button>
      <Button
        :icon="h(ReloadOutlined)"
        type="link"
        v-access:code="[CachingManagementPermissions.Refresh]"
        @click="onRefresh(row)"
      >
        {{ $t('AbpUi.Refresh') }}
      </Button>
    </template>
  </Grid>
  <CacheEditModal @change="() => onGet()" />
  <CacheRefreshModal @change="() => onGet()" />
</template>

<style scoped></style>
