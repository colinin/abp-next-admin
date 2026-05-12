import type { FeatureDto } from '../../types/features';

export interface TreeNode {
  children: TreeNode[];
  feature: FeatureDto;
  level: number;
  parent?: TreeNode;
  visible: boolean;
}
