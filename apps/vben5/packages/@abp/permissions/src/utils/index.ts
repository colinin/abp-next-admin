import type {
  PermissionDto,
  PermissionGroupDto,
  PermissionTree,
} from '../types/permissions';

import { listToTree } from '@abp/core';

export function generatePermissionTree(
  permissionGroups: PermissionGroupDto[],
): PermissionTree[] {
  const trees: PermissionTree[] = [];
  permissionGroups.forEach((g) => {
    const tree: PermissionTree = {
      disabled: false,
      displayName: g.displayName,
      isRoot: true,
      name: g.name,
      parentName: g.name,
      children: [],
    };
    tree.children = listToTree(g.permissions, {
      id: 'name',
      pid: 'parentName',
    });
    trees.push(tree);
  });
  return trees;
}

export function findNode(
  children: PermissionTree[],
  key: string,
): PermissionTree | undefined {
  let findC: PermissionTree | undefined;
  for (const child of children) {
    if (child.name === key) {
      findC = child;
      return findC;
    }
    findC = findNode(child.children, key);
    if (findC) {
      return findC;
    }
  }
  return findC;
}

export function getPermissionsCount(children: PermissionTree[]): number {
  let count = 0;
  children.forEach((c) => {
    count += getPermissionCount(c);
  });
  return count;
}

export function getPermissionCount(tree: PermissionTree): number {
  let count = tree.children.length;
  tree.children.forEach((c) => {
    count += getPermissionCount(c);
  });
  return count;
}

export function getGrantPermissionsCount(children: PermissionTree[]): number {
  let count = 0;
  children.forEach((c) => {
    count += getGrantPermissionCount(c);
  });
  return count;
}

export function getGrantPermissionCount(tree: PermissionTree): number {
  return getGrantedPermissionKeys(tree.children).length;
}

export function getGrantedPermissionKeys(children: PermissionTree[]): string[] {
  const keys: string[] = [];
  children.forEach((c) => {
    if (c.isGranted === true) {
      keys.push(c.name);
    }
    keys.push(...getGrantedPermissionKeys(c.children));
  });
  return keys;
}

export function getParentList(
  children: PermissionTree[],
  name: string,
): PermissionTree[] | undefined {
  for (const child of children) {
    if (child.name === name) {
      return [child];
    }
    if (child.children) {
      const node = getParentList(child.children, name);
      if (node) {
        return [...node, child];
      }
    }
  }
}

export function toPermissionList(treeList: PermissionTree[]) {
  const permissions: PermissionDto[] = [];
  for (const element of treeList) {
    if (!element.isRoot && element.isGranted !== undefined) {
      permissions.push({
        allowedProviders: [],
        displayName: element.displayName,
        grantedProviders: [],
        isGranted: element.isGranted,
        name: element.name,
      });
    }
    permissions.push(...toPermissionList(element.children));
  }
  return permissions;
}

export function updateParentGrant(
  tree: PermissionTree,
  name: string,
  grant: boolean,
) {
  const parentList = getParentList(tree.children, name);
  if (parentList && Array.isArray(parentList)) {
    for (const element of parentList) {
      element.isGranted = grant;
    }
  }
}

export function updateChildrenGrant(
  children: PermissionTree[],
  grant: boolean,
) {
  for (const child of children) {
    child.isGranted = grant;
    updateChildrenGrant(child.children, grant);
  }
}
