<script setup lang="ts">
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { PermissionGroupDefinitionDto } from '../../../types/groups';

import { defineAsyncComponent, h, onMounted, reactive, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  listToTree,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal, Tag } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import {
  deleteApi,
  getListApi as getPermissionsApi,
} from '../../../api/definitions';
import { getListApi as getGroupsApi } from '../../../api/groups';
import { GroupDefinitionsPermissions } from '../../../constants/permissions';
import { type PermissionDefinitionDto } from '../../../types/definitions';
import { useTypesMap } from './types';

defineOptions({
  name: 'PermissionDefinitionTable',
});

interface PermissionVo {
  children: PermissionVo[];
  displayName: string;
  groupName: string;
  isEnabled: boolean;
  isStatic: boolean;
  name: string;
  parentName?: string;
  providers: string[];
  stateCheckers: string[];
}
interface PermissionGroupVo {
  displayName: string;
  name: string;
  permissions: PermissionVo[];
}

const permissionGroups = ref<PermissionGroupVo[]>([]);
const pageState = reactive({
  current: 1,
  size: 10,
  total: 0,
});

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();
const { multiTenancySidesMap, providersMap } = useTypesMap();

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

const gridOptions: VxeGridProps<PermissionGroupDefinitionDto> = {
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
      title: $t('AbpPermissionManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('AbpPermissionManagement.DisplayName:DisplayName'),
    },
  ],
  expandConfig: {
    padding: true,
    trigger: 'row',
  },
  exportConfig: {},
  keepSource: true,
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: false,
    zoom: true,
  },
};
const subGridColumns: VxeGridProps<PermissionDefinitionDto>['columns'] = [
  {
    align: 'center',
    type: 'seq',
    width: 50,
  },
  {
    align: 'left',
    field: 'name',
    minWidth: 150,
    title: $t('AbpPermissionManagement.DisplayName:Name'),
    treeNode: true,
  },
  {
    align: 'left',
    field: 'displayName',
    minWidth: 120,
    title: $t('AbpPermissionManagement.DisplayName:DisplayName'),
  },
  {
    align: 'center',
    field: 'multiTenancySide',
    minWidth: 100,
    slots: { default: 'tenant' },
    title: $t('AbpPermissionManagement.DisplayName:MultiTenancySide'),
  },
  {
    align: 'center',
    field: 'providers',
    minWidth: 100,
    slots: { default: 'providers' },
    title: $t('AbpPermissionManagement.DisplayName:Providers'),
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 180,
  },
];

const gridEvents: VxeGridListeners<PermissionGroupDefinitionDto> = {
  pageChange(params) {
    pageState.current = params.currentPage;
    pageState.size = params.pageSize;
    onPageChange();
  },
};

const [GroupGrid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [PermissionDefinitionModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./PermissionDefinitionModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const groupRes = await getGroupsApi(input);
    const permissionRes = await getPermissionsApi(input);
    pageState.total = groupRes.items.length;
    permissionGroups.value = groupRes.items.map((group) => {
      const localizableGroup = deserialize(group.displayName);
      const permissions = permissionRes.items
        .filter((permission) => permission.groupName === group.name)
        .map((permission) => {
          const localizablePermission = deserialize(permission.displayName);
          return {
            ...permission,
            displayName: Lr(
              localizablePermission.resourceName,
              localizablePermission.name,
            ),
          };
        });
      return {
        ...group,
        displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
        permissions: listToTree(permissions, {
          id: 'name',
          pid: 'parentName',
        }),
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
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: PermissionDefinitionDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: PermissionDefinitionDto) {
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
  <GroupGrid :table-title="$t('AbpPermissionManagement.PermissionDefinitions')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[GroupDefinitionsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpPermissionManagement.PermissionDefinitions:AddNew') }}
      </Button>
    </template>
    <template #group="{ row: group }">
      <VxeGrid
        :columns="subGridColumns"
        :data="group.permissions"
        :tree-config="{
          trigger: 'row',
          rowField: 'name',
          childrenField: 'children',
        }"
      >
        <template #tenant="{ row: permission }">
          <Tag color="blue">
            {{ multiTenancySidesMap[permission.multiTenancySide] }}
          </Tag>
        </template>
        <template #providers="{ row: permission }">
          <template v-for="provider in permission.providers" :key="provider">
            <Tag color="blue" style="margin: 5px">
              {{ providersMap[provider] }}
            </Tag>
          </template>
        </template>
        <template #action="{ row: permission }">
          <div class="flex flex-row">
            <Button
              :icon="h(EditOutlined)"
              block
              type="link"
              v-access:code="[GroupDefinitionsPermissions.Update]"
              @click="onUpdate(permission)"
            >
              {{ $t('AbpUi.Edit') }}
            </Button>
            <Button
              v-if="!permission.isStatic"
              :icon="h(DeleteOutlined)"
              block
              danger
              type="link"
              v-access:code="[GroupDefinitionsPermissions.Delete]"
              @click="onDelete(permission)"
            >
              {{ $t('AbpUi.Delete') }}
            </Button>
          </div>
        </template>
      </VxeGrid>
    </template>
  </GroupGrid>
  <PermissionDefinitionModal @change="() => onGet()" />
</template>

<style scoped></style>
