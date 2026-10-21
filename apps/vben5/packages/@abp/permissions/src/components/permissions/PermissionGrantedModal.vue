<script setup lang="ts">
import type { NameValue } from '@abp/core';
import type { VxeGridProps } from '@abp/ui';
import type {
  DataNode,
  EventDataNode,
} from 'ant-design-vue/es/vc-tree/interface';
import type { CheckInfo } from 'ant-design-vue/es/vc-tree/props';

import type { PermissionProvider } from '../../types/permissions';

import { h, reactive, ref, watch } from 'vue';

import { ColPage, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  listToTree,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined } from '@ant-design/icons-vue';
import {
  Button,
  Card,
  InputSearch,
  Popconfirm,
  Select,
  Tree,
} from 'ant-design-vue';

import { usePermissionDefinitionsApi } from '../../api/usePermissionDefinitionsApi';
import { usePermissionGroupDefinitionsApi } from '../../api/usePermissionGroupDefinitionsApi';
import { usePermissionsApi } from '../../api/usePermissionsApi';

interface ModalState {
  providerName: string;
  readonly?: boolean;
}

interface PermissionVo {
  children: PermissionVo[];
  displayName: string;
  groupName: string;
  isEnabled: boolean;
  isStatic: boolean;
  key: string;
  name: string;
  parentName?: string;
  providers: string[];
  stateCheckers: string[];
}
interface PermissionGroupVo {
  checkable?: boolean;
  displayName: string;
  key: string;
  name: string;
  permissions: PermissionVo[];
}
interface PermissionInfo {
  displayName?: string;
  name: string;
}

const props = reactive({
  leftCollapsible: false,
  leftMaxWidth: 30,
  leftMinWidth: 20,
  leftWidth: 30,
  resizable: true,
  rightWidth: 70,
  splitHandle: true,
  splitLine: true,
});

const providerName = ref<string>('');
const searchPermission = ref<string>('');
const permissionInfo = ref<PermissionInfo>();
const expandNodeKeys = ref<string[]>([]);
const checkedNodeKeys = ref<string[]>([]);
const autoExpandParent = ref<boolean>(false);
const permissionGroups = ref<PermissionGroupVo[]>([]);
/** 根据搜索条件过滤后的权限树 */
const filteredPermissionGroups = ref<PermissionGroupVo[]>([]);
const permissionProviders = ref<NameValue<string>[]>([]);

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();

const { getListApi: getPermissionDefintionsApi, getAssignableProvidersApi } =
  usePermissionDefinitionsApi();
const { getListApi: getPermissionGroupDefinitionsApi } =
  usePermissionGroupDefinitionsApi();
const { getGrantedByProviderApi, updateApi } = usePermissionsApi();

/**
 * 递归过滤权限树
 * 数据结构的子节点字段是 permissions（不是 children）
 * - 当前节点自身匹配关键字 → 保留
 * - 当前节点不匹配，但子孙中有匹配 → 保留当前节点，并保留匹配的子孙
 * - 当前节点和所有子孙都不匹配 → 丢弃
 */
function filterPermissionTree(
  permissions: PermissionVo[],
  keyword: string,
): PermissionVo[] {
  if (!permissions?.length) return [];

  const lowerKeyword = keyword.toLowerCase();
  const result: PermissionVo[] = [];

  for (const permission of permissions) {
    const displayName = (permission.displayName ?? '').toLowerCase();
    const name = (permission.name ?? '').toLowerCase();

    const selfMatched =
      displayName.includes(lowerKeyword) || name.includes(lowerKeyword);

    // 递归过滤子节点（字段是 permissions）
    const children = (permission as any).permissions ?? [];
    const filteredChildren = filterPermissionTree(children, keyword);

    if (selfMatched || filteredChildren.length > 0) {
      result.push({
        ...permission,
        // 保留原字段名 permissions
        permissions: filteredChildren,
      } as any);
    }
  }

  return result;
}

/**
 * 收集过滤后树中所有非叶子节点的 key，用于全部展开
 * 字段名同样是 permissions
 */
function collectExpandKeys(
  permissions: PermissionVo[],
  keys: string[] = [],
): string[] {
  for (const permission of permissions) {
    const children = (permission as any).permissions ?? [];
    if (children.length > 0) {
      keys.push(permission.key);
      collectExpandKeys(children, keys);
    }
  }
  return keys;
}

watch(searchPermission, (value) => {
  const keyword = value?.trim() ?? '';

  // 清空搜索 → 恢复完整树，并收起所有展开节点
  if (!keyword) {
    filteredPermissionGroups.value = permissionGroups.value;
    expandNodeKeys.value = [];
    autoExpandParent.value = false;
    return;
  }

  const lowerKeyword = keyword.toLowerCase();
  const filtered: PermissionGroupVo[] = [];

  for (const group of permissionGroups.value) {
    // 组名本身也参与匹配
    const groupMatched = (group.displayName ?? '')
      .toLowerCase()
      .includes(lowerKeyword);

    const filteredPermissions = filterPermissionTree(
      group.permissions,
      keyword,
    );

    // 组名匹配 或 组内有匹配权限 → 保留该组
    if (groupMatched || filteredPermissions.length > 0) {
      filtered.push({
        ...group,
        permissions: filteredPermissions,
      });
    }
  }

  filteredPermissionGroups.value = filtered;

  // 收集所有非叶子节点 key，实现搜索后全部展开
  const expandKeys: string[] = [];
  for (const group of filtered) {
    // 如果组下面还有权限，组本身也要展开
    if (group.permissions?.length) {
      expandKeys.push(group.key);
      collectExpandKeys(group.permissions, expandKeys);
    }
  }
  expandNodeKeys.value = [...new Set(expandKeys)];
  autoExpandParent.value = false;
});

const [Modal, modalApi] = useVbenModal({
  fullscreen: true,
  fullscreenButton: false,
  async onOpenChange(isOpen) {
    if (isOpen) {
      await onInit();
    }
  },
  title: $t('AbpPermissionManagement.PermissionGranted'),
});

const gridOptions: VxeGridProps<PermissionProvider> = {
  columns: [
    {
      align: 'center',
      type: 'seq',
      width: 50,
    },
    {
      align: 'left',
      field: 'providerKey',
      minWidth: 150,
      sortable: true,
      title: $t('AbpPermissionManagement.DisplayName:PermissionGranted'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 150,
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
      query: async () => {
        if (!permissionInfo.value) {
          return { items: [], totalCount: 0 };
        }
        const { grantedProviders } = await getGrantedByProviderApi(
          permissionInfo.value.name,
          providerName.value,
        );
        return {
          items: grantedProviders,
          totalCount: grantedProviders.length,
        };
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

const [Grid, gridApi] = useVbenVxeGrid({
  gridOptions,
});

function onExpandNode(_keys: any, info: { node: EventDataNode }) {
  const nodeKey = String(info.node.key);
  const index = expandNodeKeys.value.indexOf(nodeKey);
  expandNodeKeys.value =
    index === -1
      ? [...expandNodeKeys.value, nodeKey]
      : expandNodeKeys.value.filter((key) => key !== nodeKey);
  autoExpandParent.value = false;
}

function onSelectNode(
  _: any,
  info: {
    node: EventDataNode;
    selectedNodes: DataNode[];
  },
) {
  const nodeKey = String(info.node.key);
  const index = expandNodeKeys.value.indexOf(nodeKey);
  expandNodeKeys.value =
    index === -1
      ? [...expandNodeKeys.value, nodeKey]
      : expandNodeKeys.value.filter((key) => key !== nodeKey);
}

async function onCheckNode(_keys: any, info: CheckInfo) {
  try {
    modalApi.lock();
    const nodeKey = String(info.node.key);
    checkedNodeKeys.value = [nodeKey];
    permissionInfo.value = {
      displayName: info.node.dataRef?.displayName,
      name: nodeKey,
    };
    await gridApi.query();
  } finally {
    modalApi.unlock();
  }
}

async function onProviderChange() {
  gridApi.setState((state) => {
    state.gridOptions!.data = [];
    return state;
  });
  await gridApi.query();
}

async function onInit() {
  permissionInfo.value = undefined;
  expandNodeKeys.value = [];
  checkedNodeKeys.value = [];
  permissionGroups.value = [];
  filteredPermissionGroups.value = [];
  searchPermission.value = '';
  const state = modalApi.getData<ModalState>();
  providerName.value = state.providerName;
  const [providerRes, permissionRes, permissionGroupRes] = await Promise.all([
    getAssignableProvidersApi(),
    getPermissionDefintionsApi(),
    getPermissionGroupDefinitionsApi(),
  ]);
  permissionProviders.value = providerRes.items;
  permissionGroups.value = permissionGroupRes.items.map((group) => {
    const localizableGroup = deserialize(group.displayName);
    const permissions = permissionRes.items
      .filter((permission) => permission.groupName === group.name)
      .map((permission) => {
        const localizablePermission = deserialize(permission.displayName);
        return {
          ...permission,
          key: permission.name,
          displayName: Lr(
            localizablePermission.resourceName,
            localizablePermission.name,
          ),
        };
      });
    return {
      ...group,
      checkable: false,
      key: group.name,
      displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
      permissions: listToTree(permissions, {
        id: 'name',
        pid: 'parentName',
        children: 'permissions',
      }),
    };
  });

  // 初始化过滤树（无搜索条件时等同于完整树）
  filteredPermissionGroups.value = permissionGroups.value;
}

async function onDeleteGrand(grantedInfo: PermissionProvider) {
  await updateApi(grantedInfo, {
    permissions: [
      {
        isGranted: false,
        name: permissionInfo.value!.name,
      },
    ],
  });
  await gridApi.query();
}
</script>

<template>
  <Modal>
    <ColPage auto-content-height v-bind="props">
      <template #left>
        <Card
          :body-style="{ height: '80vh' }"
          :title="$t('AbpPermissionManagement.AllPermissions')"
        >
          <template #extra>
            <InputSearch
              v-model:value="searchPermission"
              enter-button
              placeholder="搜索权限"
            />
          </template>
          <div class="h-full overflow-y-auto">
            <Tree
              :auto-expand-parent="autoExpandParent"
              :check-strictly="true"
              :checkable="true"
              :checked-keys="checkedNodeKeys"
              :expanded-keys="expandNodeKeys"
              :tree-data="filteredPermissionGroups"
              :field-names="{
                key: 'name',
                title: 'displayName',
                children: 'permissions',
              }"
              @check="onCheckNode"
              @expand="onExpandNode"
              @select="onSelectNode"
            >
              <template #title="{ displayName }">
                <span v-if="displayName.includes(searchPermission)">
                  {{
                    displayName.substring(
                      0,
                      displayName.indexOf(searchPermission),
                    )
                  }}
                  <span style="color: #f50">{{ searchPermission }}</span>
                  {{
                    displayName.substring(
                      displayName.indexOf(searchPermission) +
                        searchPermission.length,
                    )
                  }}
                </span>
                <span v-else>{{ displayName }}</span>
              </template>
            </Tree>
          </div>
        </Card>
      </template>
      <Card>
        <template #title>
          <div class="flex items-center gap-4">
            <span>{{
              $t('AbpPermissionManagement.DisplayName:PermissionProvider')
            }}</span>
            <Select
              class="min-w-[220px]"
              :options="permissionProviders"
              :field-names="{ label: 'name', value: 'value' }"
              v-model:value="providerName"
              @change="onProviderChange"
            />
          </div>
        </template>
        <Grid>
          <template #action="{ row }">
            <Popconfirm
              :title="$t('AbpUi.AreYouSure')"
              :description="
                $t('AbpPermissionManagement.RejectPermissionWarningMessage', [
                  row.providerKey,
                  permissionInfo!.displayName ?? permissionInfo!.name,
                ])
              "
              @confirm="onDeleteGrand(row)"
            >
              <Button :icon="h(DeleteOutlined)" block danger type="link">
                {{ $t('AbpPermissionManagement.RejectPermission') }}
              </Button>
            </Popconfirm>
          </template>
        </Grid>
      </Card>
    </ColPage>
  </Modal>
</template>

<style scoped></style>
