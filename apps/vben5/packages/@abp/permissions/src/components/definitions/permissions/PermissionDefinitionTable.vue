<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { PermissionDefinitionDto } from '../../../types/definitions';
import type { PermissionGroupDefinitionDto } from '../../../types/groups';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  listToTree,
  sortby,
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

import { usePermissionDefinitionsApi } from '../../../api/usePermissionDefinitionsApi';
import { usePermissionGroupDefinitionsApi } from '../../../api/usePermissionGroupDefinitionsApi';
import { GroupDefinitionsPermissions } from '../../../constants/permissions';
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

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();
const { multiTenancySidesMap, providersMap } = useTypesMap();
const { getListApi: getGroupsApi } = usePermissionGroupDefinitionsApi();
const { deleteApi, getListApi: getPermissionsApi } =
  usePermissionDefinitionsApi();

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
      sortable: true,
      title: $t('AbpPermissionManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpPermissionManagement.DisplayName:DisplayName'),
    },
  ],
  expandConfig: {
    padding: true,
    trigger: 'row',
  },
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        let items = sortby(permissionGroups.value, sort.field);
        if (sort.order === 'desc') {
          items = items.reverse();
        }
        const result = {
          totalCount: permissionGroups.value.length,
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
    resizable: true,
    sortable: true,
    title: $t('AbpPermissionManagement.DisplayName:Name'),
    treeNode: true,
  },
  {
    align: 'left',
    field: 'displayName',
    minWidth: 120,
    resizable: true,
    sortable: true,
    title: $t('AbpPermissionManagement.DisplayName:DisplayName'),
  },
  {
    align: 'center',
    field: 'multiTenancySide',
    minWidth: 100,
    resizable: true,
    slots: { default: 'tenant' },
    sortable: true,
    title: $t('AbpPermissionManagement.DisplayName:MultiTenancySide'),
  },
  {
    align: 'center',
    field: 'providers',
    minWidth: 100,
    resizable: true,
    slots: { default: 'providers' },
    sortable: true,
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
  sortChange: () => {
    gridApi.query();
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
      message.success($t('AbpUi.DeletedSuccessfully'));
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
