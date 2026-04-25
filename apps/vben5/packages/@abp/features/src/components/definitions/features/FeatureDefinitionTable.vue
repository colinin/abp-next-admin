<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { FeatureDefinitionDto } from '../../../types/definitions';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import {
  listToTree,
  sortby,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import { useFeatureDefinitionsApi } from '../../../api/useFeatureDefinitionsApi';
import { useFeatureGroupDefinitionsApi } from '../../../api/useFeatureGroupDefinitionsApi';
import { FeatureDefinitionsPermissions } from '../../../constants/permissions';

defineOptions({
  name: 'FeatureDefinitionTable',
});
const FeaturesOutlined = createIconifyIcon('pajamas:feature-flag');
const MenuItem = Menu.Item;

interface FeatureVo {
  children: FeatureVo[];
  displayName: string;
  groupName: string;
  isEnabled: boolean;
  isStatic: boolean;
  name: string;
  parentName?: string;
  providers: string[];
  stateCheckers: string[];
}
interface FeatureGroupVo {
  displayName: string;
  features: FeatureVo[];
  name: string;
}

const featureGroups = ref<FeatureGroupVo[]>([]);

const { Lr } = useLocalization();
const { hasAccessByCodes } = useAccess();
const { deserialize } = useLocalizationSerializer();
const { getListApi: getGroupsApi } = useFeatureGroupDefinitionsApi();
const { deleteApi, getListApi: getFeaturesApi } = useFeatureDefinitionsApi();

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

const gridOptions: VxeGridProps<FeatureGroupVo> = {
  columns: [
    {
      align: 'center',
      type: 'seq',
      width: 50,
    },
    {
      align: 'left',
      field: 'group',
      slots: { content: 'group' },
      type: 'expand',
      width: 50,
    },
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
        let items = sortby(featureGroups.value, sort.field);
        if (sort.order === 'desc') {
          items = items.toReversed();
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
const subGridColumns: VxeGridProps<FeatureGroupVo>['columns'] = [
  {
    align: 'center',
    type: 'seq',
    width: 50,
  },
  {
    align: 'left',
    field: 'name',
    minWidth: 150,
    sortable: true,
    title: $t('AbpFeatureManagement.DisplayName:Name'),
    treeNode: true,
  },
  {
    align: 'left',
    field: 'displayName',
    minWidth: 120,
    sortable: true,
    title: $t('AbpFeatureManagement.DisplayName:DisplayName'),
  },
  {
    align: 'left',
    field: 'description',
    minWidth: 120,
    sortable: true,
    title: $t('AbpFeatureManagement.DisplayName:Description'),
  },
  {
    align: 'left',
    field: 'isVisibleToClients',
    minWidth: 120,
    slots: { default: 'isVisibleToClients' },
    sortable: true,
    title: $t('AbpFeatureManagement.DisplayName:IsVisibleToClients'),
  },
  {
    align: 'left',
    field: 'isAvailableToHost',
    minWidth: 120,
    slots: { default: 'isAvailableToHost' },
    sortable: true,
    title: $t('AbpFeatureManagement.DisplayName:IsAvailableToHost'),
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 220,
  },
];

const gridEvents: VxeGridListeners<FeatureGroupVo> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [GroupGrid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [FeatureDefinitionModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./FeatureDefinitionModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const groupRes = await getGroupsApi(input);
    const featureRes = await getFeaturesApi(input);
    featureGroups.value = groupRes.items.map((group) => {
      const localizableGroup = deserialize(group.displayName);
      const features = featureRes.items
        .filter((feature) => feature.groupName === group.name)
        .map((feature) => {
          const displayName = deserialize(feature.displayName);
          const description = deserialize(feature.description);
          return {
            ...feature,
            description: Lr(description.resourceName, description.name),
            displayName: Lr(displayName.resourceName, displayName.name),
          };
        });
      return {
        ...group,
        displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
        features: listToTree(features, {
          id: 'name',
          pid: 'parentName',
        }),
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
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: FeatureDefinitionDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: FeatureDefinitionDto) {
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

function onMenuClick(row: FeatureDefinitionDto, info: MenuInfo) {
  switch (info.key) {
    case 'features': {
      modalApi.setData({
        groupName: row.groupName,
        parentName: row.name,
      });
      modalApi.open();
      break;
    }
  }
}

onMounted(onGet);
</script>

<template>
  <GroupGrid :table-title="$t('AbpFeatureManagement.FeatureDefinitions')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[FeatureDefinitionsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpFeatureManagement.FeatureDefinitions:AddNew') }}
      </Button>
    </template>
    <template #group="{ row: group }">
      <VxeGrid
        :columns="subGridColumns"
        :data="group.features"
        :tree-config="{
          trigger: 'row',
          rowField: 'name',
          childrenField: 'children',
        }"
      >
        <template #isVisibleToClients="{ row }">
          <div class="flex flex-row justify-center">
            <CheckOutlined
              v-if="row.isVisibleToClients"
              class="text-green-500"
            />
            <CloseOutlined v-else class="text-red-500" />
          </div>
        </template>
        <template #isAvailableToHost="{ row }">
          <div class="flex flex-row justify-center">
            <CheckOutlined
              v-if="row.isAvailableToHost"
              class="text-green-500"
            />
            <CloseOutlined v-else class="text-red-500" />
          </div>
        </template>
        <template #action="{ row: feature }">
          <div class="flex flex-row">
            <Button
              :icon="h(EditOutlined)"
              block
              type="link"
              v-access:code="[FeatureDefinitionsPermissions.Update]"
              @click="onUpdate(feature)"
            >
              {{ $t('AbpUi.Edit') }}
            </Button>
            <Button
              v-if="!feature.isStatic"
              :icon="h(DeleteOutlined)"
              block
              danger
              type="link"
              v-access:code="[FeatureDefinitionsPermissions.Delete]"
              @click="onDelete(feature)"
            >
              {{ $t('AbpUi.Delete') }}
            </Button>
            <Dropdown v-if="!feature.isStatic">
              <template #overlay>
                <Menu @click="(info) => onMenuClick(feature, info)">
                  <MenuItem
                    v-if="
                      hasAccessByCodes([FeatureDefinitionsPermissions.Create])
                    "
                    key="features"
                    :icon="h(FeaturesOutlined)"
                  >
                    {{ $t('AbpFeatureManagement.FeatureDefinitions:AddNew') }}
                  </MenuItem>
                </Menu>
              </template>
              <Button :icon="h(EllipsisOutlined)" type="link" />
            </Dropdown>
          </div>
        </template>
      </VxeGrid>
    </template>
  </GroupGrid>
  <FeatureDefinitionModal @change="() => onGet()" />
</template>

<style scoped></style>
