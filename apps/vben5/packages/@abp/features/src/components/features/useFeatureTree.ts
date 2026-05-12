import type { FeatureDto } from '../../types/features';
import type { TreeNode } from './tree';

import { $t } from '@vben/locales';

/**
 * 将扁平的特性列表转换为树形结构
 */
export function buildFeatureTree(features: FeatureDto[]): TreeNode[] {
  const nodeMap = new Map<string, TreeNode>();
  const roots: TreeNode[] = [];

  // 创建所有节点
  features.forEach((feature) => {
    const node: TreeNode = {
      feature,
      children: [],
      level: 0,
      visible: true,
    };
    nodeMap.set(feature.name, node);
  });

  // 建立父子关系
  features.forEach((feature) => {
    const node = nodeMap.get(feature.name);
    if (node && feature.parentName && nodeMap.has(feature.parentName)) {
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      const parent = nodeMap.get(feature.parentName)!;
      parent.children.push(node);
      node.parent = parent;
      node.level = parent.level + 1;
    } else if (node && !feature.parentName) {
      roots.push(node);
    }
  });

  // 对子节点进行排序
  const sortChildren = (nodes: TreeNode[]) => {
    nodes.forEach((node) => {
      if (node.children.length > 0) {
        node.children.sort((a, b) =>
          a.feature.displayName.localeCompare(b.feature.displayName),
        );
        sortChildren(node.children);
      }
    });
  };
  sortChildren(roots);

  return roots;
}

/**
 * 更新节点的可见性（基于父级状态）
 */
export function updateVisibility(
  node: TreeNode,
  parentVisible: boolean = true,
): void {
  node.visible = parentVisible;

  if (isCheckboxFeature(node.feature)) {
    const isChecked = Boolean(node.feature.value);
    node.children.forEach((child) => {
      updateVisibility(child, parentVisible && isChecked);
    });
  } else {
    node.children.forEach((child) => {
      updateVisibility(child, parentVisible);
    });
  }
}

/**
 * 检查是否为复选框类型的特性
 */
export function isCheckboxFeature(feature: FeatureDto): boolean {
  return (
    feature.valueType?.name === 'ToggleStringValueType' &&
    feature.valueType?.validator?.name === 'BOOLEAN'
  );
}

/**
 * 当特性值改变时，更新其子节点的可见性
 */
export function onFeatureValueChange(
  feature: FeatureDto,
  treeNodes: TreeNode[],
): void {
  const findNode = (nodes: TreeNode[]): null | TreeNode => {
    for (const node of nodes) {
      if (node.feature.name === feature.name) {
        return node;
      }
      const found = findNode(node.children);
      if (found) return found;
    }
    return null;
  };

  const node = findNode(treeNodes);
  if (node && isCheckboxFeature(node.feature)) {
    updateVisibility(node, true);
  }
}

/**
 * 处理特性值类型转换
 */
export function processFeatureValue(feature: FeatureDto): void {
  if (feature.valueType?.name === 'SelectionStringValueType') {
    const valueType = feature.valueType as any;
    if (valueType.itemSource?.items) {
      valueType.itemSource.items.forEach((valueItem: any) => {
        if (valueItem.displayText?.resourceName === 'Fixed') {
          valueItem.displayName = valueItem.displayText.name;
        } else if (
          valueItem.displayText?.resourceName &&
          valueItem.displayText?.name
        ) {
          valueItem.displayName = $t(
            `${valueItem.displayText.resourceName}.${valueItem.displayText.name}`,
          );
        }
      });
    }
  } else {
    switch (feature.valueType?.validator?.name) {
      case 'BOOLEAN': {
        feature.value = String(feature.value).toLocaleLowerCase() === 'true';
        break;
      }
      case 'NUMERIC': {
        if (
          feature.value !== null &&
          feature.value !== undefined &&
          feature.value !== ''
        ) {
          feature.value = Number(feature.value);
        }
        break;
      }
    }
  }
}

/**
 * 递归处理所有特性值
 */
export function processAllFeatures(treeNodes: TreeNode[]): void {
  const traverse = (nodes: TreeNode[]) => {
    nodes.forEach((node) => {
      processFeatureValue(node.feature);
      traverse(node.children);
    });
  };
  traverse(treeNodes);
}

/**
 * 获取缩进样式
 */
export function getIndentStyle(level: number, indentSize: number = 8): string {
  return `${level * indentSize}px`;
}
