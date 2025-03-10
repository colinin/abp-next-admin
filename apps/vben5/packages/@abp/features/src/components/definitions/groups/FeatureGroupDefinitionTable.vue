<script setup lang="ts">
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { FeatureGroupDefinitionDto } from '../../../types/groups';

import { defineAsyncComponent, h, onMounted, reactive, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useLocalization, useLocalizationSerializer } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useFeatureGroupDefinitionsApi } from '../../../api/useFeatureGroupDefinitionsApi';
import { GroupDefinitionsPermissions } from '../../../constants/permissions';

defineOptions({
  name: 'FeatureGroupDefinitionTable',
});

const permissionGroups = ref<FeatureGroupDefinitionDto[]>([]);
const pageState = reactive({
  current: 1,
  size: 10,
  total: 0,
});

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();
const { deleteApi, getListApi } = useFeatureGroupDefinitionsApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  handleReset: onReset,
  async handleSubmit(params) {
    pageState.current = 1;
    await onGet(params);
  },
  schema: [
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<FeatureGroupDefinitionDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      title: $t('AbpFeatureManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('AbpFeatureManagement.DisplayName:DisplayName'),
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
};

const gridEvents: VxeGridListeners<FeatureGroupDefinitionDto> = {
  pageChange(params) {
    pageState.current = params.currentPage;
    pageState.size = params.pageSize;
    onPageChange();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [FeatureGroupDefinitionModal, groupModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./FeatureGroupDefinitionModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const { items } = await getListApi(input);
    pageState.total = items.length;
    permissionGroups.value = items.map((item) => {
      const localizableString = deserialize(item.displayName);
      return {
        ...item,
        displayName: Lr(localizableString.resourceName, localizableString.name),
      };
    });
    onPageChange();
  } finally {
    gridApi.setLoading(false);
  }
}

async function onReset() {
  await gridApi.formApi.resetForm();
  const input = await gridApi.formApi.getValues();
  await onGet(input);
}

function onPageChange() {
  const items = permissionGroups.value.slice(
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

function onCreate() {
  groupModalApi.setData({});
  groupModalApi.open();
}

function onUpdate(row: FeatureGroupDefinitionDto) {
  groupModalApi.setData(row);
  groupModalApi.open();
}

function onDelete(row: FeatureGroupDefinitionDto) {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name])}`,
    onOk: async () => {
      await deleteApi(row.name);
      message.success($t('AbpUi.SuccessfullyDeleted'));
      onGet();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

onMounted(onGet);
</script>

<template>
  <Grid :table-title="$t('AbpFeatureManagement.GroupDefinitions')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[GroupDefinitionsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpFeatureManagement.GroupDefinitions:AddNew') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[GroupDefinitionsPermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="!row.isStatic"
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[GroupDefinitionsPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <FeatureGroupDefinitionModal @change="() => onGet()" />
</template>

<style scoped></style>
