<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { DataDto, DataItemDto } from '../../types/dataDictionaries';

import { h, reactive, ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { isNullOrWhiteSpace } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button } from 'ant-design-vue';

import { useDataDictionariesApi } from '../../api';
import { ValueType } from '../../types/dataDictionaries';

const { getApi } = useDataDictionariesApi();

const dataItems = ref<DataItemDto[]>([]);
const pageState = reactive({
  current: 1,
  size: 10,
  total: 0,
});
const valueTypeMaps: { [key: number]: string } = {
  [ValueType.Array]: 'Array',
  [ValueType.Boolean]: 'Boolean',
  [ValueType.Date]: 'Date',
  [ValueType.DateTime]: 'DateTime',
  [ValueType.Numeic]: 'Number',
  [ValueType.Object]: 'Object',
  [ValueType.String]: 'String',
};

const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-1/2',
  async onOpenChange(isOpen) {
    if (isOpen) {
      const { name } = drawerApi.getData<DataDto>();
      drawerApi.setState({
        title: `${$t('AppPlatform.DisplayName:DataDictionary')} - ${name}`,
      });
      await onGet();
    }
  },
  showConfirmButton: false,
  title: $t('AppPlatform.Data:Items'),
});

const gridOptions: VxeGridProps<DataItemDto> = {
  columns: [
    {
      align: 'center',
      type: 'seq',
      width: 50,
    },
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:Name'),
      treeNode: true,
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:DisplayName'),
    },
    {
      align: 'left',
      field: 'description',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:Description'),
    },
    {
      align: 'left',
      field: 'valueType',
      formatter: ({ row }) => {
        return valueTypeMaps[row.valueType] ?? row.valueType;
      },
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:ValueType'),
    },
    {
      align: 'left',
      field: 'defaultValue',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:DefaultValue'),
    },
    {
      align: 'left',
      field: 'allowBeNull',
      minWidth: 150,
      slots: { default: 'allowBeNull' },
      title: $t('AppPlatform.DisplayName:AllowBeNull'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: false,
    zoom: true,
  },
  treeConfig: {
    accordion: true,
    parentField: 'parentId',
    rowField: 'id',
    transform: true,
  },
};

const gridEvents: VxeGridListeners<DataItemDto> = {
  pageChange(params) {
    pageState.current = params.currentPage;
    pageState.size = params.pageSize;
    onPageChange();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});

async function onGet() {
  const { id } = drawerApi.getData<DataDto>();
  if (isNullOrWhiteSpace(id)) {
    return;
  }
  try {
    drawerApi.setState({ loading: true });
    const dto = await getApi(id);
    dataItems.value = dto.items;
    pageState.total = dto.items.length;
    onPageChange();
  } finally {
    drawerApi.setState({
      loading: false,
    });
  }
}

function onPageChange() {
  const items = dataItems.value.slice(
    (pageState.current - 1) * pageState.size,
    pageState.current * pageState.size,
  );
  gridApi.setGridOptions({
    data: items,
    pagerConfig: {
      currentPage: pageState.current,
      pageSize: pageState.size,
      total: pageState.total,
    },
  });
}

function onCreate() {}

function onUpdate(_row: DataItemDto) {}

function onDelete(_row: DataItemDto) {}
</script>

<template>
  <Drawer>
    <Grid :table-title="$t('AppPlatform.Data:Items')">
      <template #toolbar-tools>
        <Button :icon="h(PlusOutlined)" type="primary" @click="onCreate">
          {{ $t('AppPlatform.Data:AppendItem') }}
        </Button>
      </template>
      <template #allowBeNull="{ row }">
        <div class="flex flex-row justify-center">
          <CheckOutlined v-if="row.allowBeNull" class="text-green-500" />
          <CloseOutlined v-else class="text-red-500" />
        </div>
      </template>
      <template #action="{ row }">
        <div class="flex flex-row">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            @click="onUpdate(row)"
          >
            {{ $t('AbpUi.Edit') }}
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
  </Drawer>
</template>

<style scoped></style>
