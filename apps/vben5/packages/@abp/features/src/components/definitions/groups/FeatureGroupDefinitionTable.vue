<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { FeatureGroupDefinitionDto } from '../../../types/groups';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { sortby, useLocalization, useLocalizationSerializer } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useFeatureGroupDefinitionsApi } from '../../../api/useFeatureGroupDefinitionsApi';
import {
  FeatureDefinitionsPermissions,
  GroupDefinitionsPermissions,
} from '../../../constants/permissions';

defineOptions({
  name: 'FeatureGroupDefinitionTable',
});

const MenuItem = Menu.Item;

const FeaturesOutlined = createIconifyIcon('pajamas:feature-flag');

const featureGroups = ref<FeatureGroupDefinitionDto[]>([]);

const { Lr } = useLocalization();
const { hasAccessByCodes } = useAccess();
const { deserialize } = useLocalizationSerializer();
const { deleteApi, getListApi } = useFeatureGroupDefinitionsApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  handleReset: onReset,
  async handleSubmit(params) {
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
      sortable: true,
      title: $t('AbpFeatureManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
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
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        let items = sortby(featureGroups.value, sort.field);
        if (sort.order === 'desc') {
          items = items.reverse();
        }
        const result = {
          totalCount: featureGroups.value.length,
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

const gridEvents: VxeGridListeners<FeatureGroupDefinitionDto> = {
  sortChange: () => {
    gridApi.query();
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
const [FeatureDefinitionModal, defineModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('../features/FeatureDefinitionModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const { items } = await getListApi(input);
    featureGroups.value = items.map((item) => {
      const localizableString = deserialize(item.displayName);
      return {
        ...item,
        displayName: Lr(localizableString.resourceName, localizableString.name),
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

function onCreate() {
  groupModalApi.setData({});
  groupModalApi.open();
}

function onUpdate(row: FeatureGroupDefinitionDto) {
  groupModalApi.setData(row);
  groupModalApi.open();
}

function onMenuClick(row: FeatureGroupDefinitionDto, info: MenuInfo) {
  switch (info.key) {
    case 'features': {
      defineModalApi.setData({
        groupName: row.name,
      });
      defineModalApi.open();
      break;
    }
  }
}

function onDelete(row: FeatureGroupDefinitionDto) {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name])}`,
    onOk: async () => {
      await deleteApi(row.name);
      message.success($t('AbpUi.DeletedSuccessfully'));
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
        <Dropdown v-if="!row.isStatic">
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="hasAccessByCodes([FeatureDefinitionsPermissions.Create])"
                key="features"
                :icon="h(FeaturesOutlined)"
              >
                {{ $t('AbpFeatureManagement.GroupDefinitions:AddNew') }}
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <FeatureGroupDefinitionModal @change="() => onGet()" />
  <FeatureDefinitionModal />
</template>

<style scoped></style>
