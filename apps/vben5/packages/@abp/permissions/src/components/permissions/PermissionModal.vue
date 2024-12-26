<script setup lang="ts">
import type { CheckboxChangeEvent } from 'ant-design-vue/es/checkbox/interface';
import type {
  DataNode,
  EventDataNode,
} from 'ant-design-vue/es/vc-tree/interface';
import type { CheckInfo } from 'ant-design-vue/es/vc-tree/props';

import type { PermissionTree } from '../../types/permissions';

import { computed, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Card, Checkbox, Divider, message, Tabs, Tree } from 'ant-design-vue';

import { getApi, updateApi } from '../../api/permissions';
import {
  generatePermissionTree,
  getGrantedPermissionKeys,
  getGrantPermissionCount,
  getGrantPermissionsCount,
  getParentList,
  getPermissionCount,
  getPermissionsCount,
  toPermissionList,
} from '../../utils';

defineOptions({
  name: 'PermissionModal',
});

const TabPane = Tabs.TabPane;

interface ModalState {
  displayName?: string;
  providerKey?: string;
  providerName: string;
  readonly?: boolean;
}

const modelState = ref<ModalState>();
const expandNodeKeys = ref<string[]>([]);
const checkedNodeKeys = ref<string[]>([]);
const permissionTree = ref<PermissionTree[]>([]);

const getPermissionTab = computed(() => {
  return (tree: PermissionTree) => {
    const grantCount = getGrantPermissionCount(tree);
    const permissionCount = getPermissionCount(tree);
    return `${tree.displayName} (${grantCount}/${permissionCount})`;
  };
});

const getPermissionState = computed(() => {
  const treeList = permissionTree.value;
  const grantCount = getGrantPermissionsCount(treeList);
  const permissionCount = getPermissionsCount(treeList);
  return {
    checked: grantCount === permissionCount,
    indeterminate: grantCount > 0 && grantCount < permissionCount,
  };
});

const getPermissionNodeState = computed(() => {
  return (tree: PermissionTree) => {
    const grantCount = getGrantPermissionCount(tree);
    const permissionCount = getPermissionCount(tree);
    return {
      checked: grantCount === permissionCount,
      indeterminate: grantCount > 0 && grantCount < permissionCount,
    };
  };
});

const [Modal, modalApi] = useVbenModal({
  centered: true,
  class: 'w-1/2',
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    const permissions = toPermissionList(permissionTree.value);
    try {
      modalApi.setState({
        closable: false,
        confirmLoading: true,
      });
      await updateApi(
        {
          providerKey: modelState.value!.providerKey,
          providerName: modelState.value!.providerName,
        },
        {
          permissions,
        },
      );
      message.success($t('AbpUi.SavedSuccessfully'));
      modalApi.close();
    } finally {
      modalApi.setState({
        closable: true,
        confirmLoading: false,
      });
    }
  },
  async onOpenChange(isOpen: boolean) {
    permissionTree.value = [];
    modelState.value = undefined;
    if (isOpen) {
      const state = modalApi.getData<ModalState>();
      modelState.value = state;
      modalApi.setState({
        confirmLoading: true,
        loading: true,
      });
      try {
        const dto = await getApi({
          providerKey: state.providerKey,
          providerName: state.providerName,
        });
        modalApi.setState({
          title: `${$t('AbpPermissionManagement.Permissions')} - ${state.displayName ?? dto.entityDisplayName}`,
        });
        permissionTree.value = generatePermissionTree(dto.groups);
        checkedNodeKeys.value = getGrantedPermissionKeys(permissionTree.value);
      } finally {
        modalApi.setState({
          confirmLoading: false,
          loading: false,
        });
      }
    }
  },
  title: $t('AbpPermissionManagement.Permissions'),
});

/** 全选所有节点权限 */
function onCheckAll(e: CheckboxChangeEvent) {
  checkedNodeKeys.value = [];
  permissionTree.value.forEach((current) => {
    const children = getChildren(current.children);
    children.forEach((permission) => {
      permission.isGranted = e.target.checked;
      if (e.target.checked) {
        checkedNodeKeys.value.push(permission.name);
      }
    });
    current.isGranted = e.target.checked;
  });
}

/** 全选当前节点权限 */
function onCheckNodeAll(e: CheckboxChangeEvent, permission: PermissionTree) {
  const children = getChildren(permission.children ?? []);
  children.forEach((permission) => {
    permission.isGranted = e.target.checked;
  });
  const childKeys = children.map((node) => node.name);
  checkedNodeKeys.value = e.target.checked
    ? [...checkedNodeKeys.value, ...childKeys]
    : checkedNodeKeys.value.filter((key) => !childKeys.includes(key));
  permission.isGranted = e.target.checked;
}

function onExpandNode(_keys: any, info: { node: EventDataNode }) {
  const nodeKey = String(info.node.key);
  const index = expandNodeKeys.value.indexOf(nodeKey);
  expandNodeKeys.value =
    index === -1
      ? [...expandNodeKeys.value, nodeKey]
      : expandNodeKeys.value.filter((key) => key !== nodeKey);
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

function onCheckNode(permission: PermissionTree, _keys: any, info: CheckInfo) {
  const nodeKey = String(info.node.key);
  const index = checkedNodeKeys.value.indexOf(nodeKey);
  checkedNodeKeys.value =
    index === -1
      ? [...checkedNodeKeys.value, nodeKey]
      : checkedNodeKeys.value.filter((key) => key !== nodeKey);
  const currentPermission = info.node.dataRef as unknown as PermissionTree;
  // 上级权限联动
  checkParentGrant(permission, currentPermission, info.checked);
  // 下级权限联动
  checkChildrenGrant(currentPermission, info.checked);
  // 自身授权
  currentPermission.isGranted = info.checked;
}

function checkChildrenGrant(current: PermissionTree, isGranted: boolean) {
  const children = getChildren(current.children);
  // 应取消子权限的所有授权
  if (!isGranted) {
    const childKeys: string[] = [];
    children.forEach((node) => {
      !isGranted && (node.isGranted = false);
      childKeys.push(node.name);
    });
    checkedNodeKeys.value = checkedNodeKeys.value.filter(
      (key) => !childKeys.includes(key),
    );
  }
}

function checkParentGrant(
  root: PermissionTree,
  current: PermissionTree,
  isGranted: boolean,
) {
  if (!isGranted || !current.parentName) {
    return;
  }
  const parentNodes = getParentList(root.children, current.parentName);
  if (parentNodes) {
    const parentKeys: string[] = [];
    parentNodes.forEach((node) => {
      node.isGranted = true;
      parentKeys.push(node.name);
      if (!checkedNodeKeys.value.includes(node.name)) {
        parentKeys.push(node.name);
      }
    });
    checkedNodeKeys.value = [...checkedNodeKeys.value, ...parentKeys];
  }
}

function getChildren(permissions: PermissionTree[]): PermissionTree[] {
  const children: PermissionTree[] = [];
  permissions.forEach((permission) => {
    children.push(permission);
    permission.children && children.push(...getChildren(permission.children));
  });
  return children;
}
</script>

<template>
  <Modal>
    <div class="flex flex-col content-center justify-center">
      <div>
        <Checkbox
          :disabled="modelState?.readonly"
          @change="onCheckAll"
          v-bind="getPermissionState"
        >
          {{ $t('AbpPermissionManagement.SelectAllInAllTabs') }}
        </Checkbox>
      </div>
      <Divider />
      <Tabs tab-position="left" type="card">
        <template v-for="permission in permissionTree" :key="permission.name">
          <TabPane
            :tab="getPermissionTab(permission)"
            :tab-key="permission.name"
          >
            <Card :bordered="false" :title="permission.displayName">
              <div class="flex flex-col">
                <Checkbox
                  :disabled="modelState?.readonly"
                  v-bind="getPermissionNodeState(permission)"
                  @change="(e: any) => onCheckNodeAll(e, permission)"
                >
                  {{ $t('AbpPermissionManagement.SelectAllInThisTab') }}
                </Checkbox>
                <Divider />
                <Tree
                  :check-strictly="true"
                  :checkable="true"
                  :checked-keys="checkedNodeKeys"
                  :disabled="modelState?.readonly"
                  :expanded-keys="expandNodeKeys"
                  :field-names="{
                    key: 'name',
                    title: 'displayName',
                    children: 'children',
                  }"
                  :tree-data="permission.children"
                  @check="
                    (keys: any, info: CheckInfo) =>
                      onCheckNode(permission, keys, info)
                  "
                  @expand="onExpandNode"
                  @select="onSelectNode"
                />
              </div>
            </Card>
          </TabPane>
        </template>
      </Tabs>
    </div>
  </Modal>
</template>

<style lang="scss" scoped>
:deep(.ant-tabs) {
  height: 34rem;

  .ant-tabs-nav {
    width: 14rem;
  }

  .ant-tabs-content-holder {
    overflow: hidden auto !important;
  }
}
</style>
