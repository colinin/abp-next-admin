import { PermissionTree } from '../types/permission';
import { IPermission } from '/@/api/model/baseModel';
import { PermissionGroup } from '/@/api/permission-management/model/permissionModel';
import { listToTree } from '/@/utils/helper/treeHelper';

export function generatePermissionTree(permissionGroups: PermissionGroup[]) {
  const trees: PermissionTree[] = [];
  permissionGroups.forEach((g) => {
    const tree: PermissionTree = {
      name: g.name,
      displayName: g.displayName,
      disabled: false,
      children: [],
      parentName: g.name,
    };
    tree.children = listToTree(g.permissions, {
      id: 'name',
      pid: 'parentName',
    });
    trees.push(tree);
  });
  return trees;
}

export function find(children: PermissionTree[], key: string) {
  let findC: PermissionTree | null = null;
  for (let index = 0; index < children.length; index++) {
    if (children[index].name === key) {
      findC = children[index];
      break;
    }
    findC = find(children[index].children, key);
    if (findC) {
      return findC;
    }
  }
  return findC;
}

export function getPermissionsCount(children: PermissionTree[]) {
  let count = 0;
  children.forEach((c) => {
    count += getPermissionCount(c);
  });
  return count;
}

export function getPermissionCount(tree: PermissionTree) {
  let count = tree.children.length;
  tree.children.forEach((c) => {
    count += getPermissionCount(c);
  });
  return count;
}

export function getGrantPermissionsCount(children: PermissionTree[]) {
  let count = 0;
  children.forEach((c) => {
    count += getGrantPermissionCount(c);
  });
  return count;
}

export function getGrantPermissionCount(tree: PermissionTree) {
  return getGrantedPermissionKeys(tree.children).length;
}

export function getGrantedPermissionKeys(children: PermissionTree[]) {
  const keys: string[] = [];
  children.forEach((c) => {
    if (c.isGranted === true) {
      keys.push(c.name);
    }
    keys.push(...getGrantedPermissionKeys(c.children));
  });
  return keys;
}

export function getParentList(children: PermissionTree[], name: string) {
  for (let index = 0; index < children.length; index++) {
    if (children[index].name === name) {
      return [children[index]];
    }
    if (children[index].children) {
      const node = getParentList(children[index].children, name);
      if (node) {
        return node.concat(children[index]);
      }
    }
  }
}

export function toPermissionList(treeList: PermissionTree[]) {
  const permissions: IPermission[] = [];
  for (let index = 0; index < treeList.length; index++) {
    if (treeList[index].isGranted !== undefined) {
      permissions.push({
        name: treeList[index].name,
        isGranted: treeList[index].isGranted === true,
      });
    }
    permissions.push(...toPermissionList(treeList[index].children));
  }
  return permissions;
}

export function updateParentGrant(tree: PermissionTree, name: string, grant: boolean) {
  const parentList = getParentList(tree.children, name);
  if (parentList && Array.isArray(parentList)) {
    for (let index = 0; index < parentList.length; index++) {
      parentList[index].isGranted = grant;
    }
  }
}

export function updateChildrenGrant(children: PermissionTree[], grant: boolean) {
  for (let index = 0; index < children.length; index++) {
    children[index].isGranted = grant;
    updateChildrenGrant(children[index].children, grant);
  }
}
