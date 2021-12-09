import type { Ref } from 'vue';
import type { PermissionProps, PermissionTree } from '../types/permission';

import { computed, watch, unref, ref } from 'vue';
import { useI18n } from '/@/hooks/web/useI18n';
import { get, update } from '/@/api/permission-management/permission';
import { PermissionProvider } from '/@/api/permission-management/model/permissionModel';
import {
  generatePermissionTree,
  getGrantedPermissionKeys,
  getPermissionCount,
  getGrantPermissionCount,
  getPermissionsCount,
  getGrantPermissionsCount,
  find,
  updateParentGrant,
  updateChildrenGrant,
  toPermissionList,
} from '../utils/helper';
import { useAbpStoreWithOut } from '/@/store/modules/abp';

interface UsePermission {
  getPropsRef: Ref<PermissionProps>;
}

export function usePermissions({ getPropsRef }: UsePermission) {
  const { t } = useI18n();
  /** 弹出层标题 */
  const title = ref('');
  /** 权限树 */
  const permissionTree = ref<PermissionTree[]>([]);
  /**
   * 获取授权数据
   * @param name 授权提供者名称
   * @param key  授权提供者标识
   */
  function handleGetPermission(name: string, key?: string) {
    get({ providerName: name, providerKey: key }).then((res) => {
      title.value = `${t('AbpPermissionManagement.Permissions')} - ${res.entityDisplayName}`;
      permissionTree.value = generatePermissionTree(res.groups);
    });
  }

  /** 当前tab名称
   * @description 格式：权限显示名称+(已授权数量)
   */
  const permissionTab = computed(() => {
    return (tree: PermissionTree) => {
      const grantCount = getGrantPermissionCount(tree);
      return `${tree.displayName} (${grantCount})`;
    };
  });

  /** 是否禁用所有权限复选框 */
  const permissionTreeDisabled = computed(() => {
    const abpStore = useAbpStoreWithOut();
    const props = unref(getPropsRef);
    const { currentUser } = abpStore.getApplication;
    // if (props.providerName === 'R') {
    //   // 当前登录用户组禁止操作本身组权限
    //   // 应该允许
    //   return currentUser.roles.includes(props.providerKey!);
    // }
    // 当前登录用户禁止操作本身权限
    return currentUser.id === props.providerKey;
  });

  /** 当前tab下已授权权限节点标识（勾选复选框） */
  const permissionGrantKeys = computed(() => {
    return (tree: PermissionTree) => {
      return getGrantedPermissionKeys(tree.children);
    };
  });

  /** 当前tab下权限复选框状态
   * @returns checked 是否选中
   * @returns indeterminate 是否不确定状态（半选）
   */
  const permissionTabCheckState = computed(() => {
    return (tree: PermissionTree) => {
      const grantCount = getGrantPermissionCount(tree);
      const permissionCount = getPermissionCount(tree);
      return {
        indeterminate: grantCount > 0 && grantCount < permissionCount,
        checked: grantCount == permissionCount,
      };
    };
  });

  /** 权限树复选框状态 */
  const permissionTreeCheckState = computed(() => {
    const treeList = unref(permissionTree);
    const grantCount = getGrantPermissionsCount(treeList);
    const permissionCount = getPermissionsCount(treeList);
    return {
      indeterminate: grantCount > 0 && grantCount < permissionCount,
      checked: grantCount == permissionCount,
    };
  });

  /** 变更所有授权数据 */
  function handleGrantAllPermission(e) {
    if (!unref(permissionTreeDisabled)) {
      updateChildrenGrant(unref(permissionTree), e.target.checked);
    }
  }

  /** 变更当前tab下的授权数据 */
  function handleGrantPermissions(tree, e) {
    if (!unref(permissionTreeDisabled)) {
      updateChildrenGrant(tree.children, e.target.checked);
    }
  }

  /** 当选中某个授权节点时事件 */
  function handlePermissionGranted(tree, _selectedKeys, info) {
    const permission = find(tree.children, info.node.eventKey) as PermissionTree;
    if (!permission) return;
    permission.isGranted = info.checked;
    if (permission.parentName && info.checked) {
      // 子权限授权后父权限也必须授权
      updateParentGrant(tree, permission.parentName, true);
    }
    if (permission.children && !info.checked) {
      // 父权限取消授权，子权限全部取消授权
      updateChildrenGrant(permission.children, false);
    }
  }

  /** 保存授权数据 */
  function handleSavePermission() {
    const props = unref(getPropsRef);
    const provider: PermissionProvider = {
      providerName: props.providerName,
      providerKey: props.providerKey,
    };
    const permissions = toPermissionList(unref(permissionTree));
    return update(provider, {
      permissions: permissions,
    });
  }

  /** 监听授权提供者标识，重新检索权限 */
  watch(
    () => unref(getPropsRef).providerKey,
    (key) => {
      permissionTree.value = [];
      const props = unref(getPropsRef);
      handleGetPermission(props.providerName, key);
    },
  );

  return {
    title,
    permissionTree,
    permissionTab,
    permissionGrantKeys,
    permissionTabCheckState,
    permissionTreeCheckState,
    permissionTreeDisabled,
    handlePermissionGranted,
    handleSavePermission,
    handleGrantAllPermission,
    handleGrantPermissions,
  };
}
