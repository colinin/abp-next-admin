<!-- components/FeatureGroup.vue -->
<script setup lang="ts">
import type { FeatureDto, FeatureGroupDto } from '../../types/features';
import type { TreeNode } from './tree';

import { Card } from 'ant-design-vue';

import FeatureTreeNode from './FeatureTreeNode.vue';

defineProps<{
  baseIndentSize?: number;
  group: FeatureGroupDto & { _treeRoots?: TreeNode[] };
  groupIndex: number;
}>();

const emit = defineEmits<{
  (event: 'change', feature: FeatureDto, groupIndex: number): void;
}>();
</script>

<template>
  <Card :bordered="false" :title="group.displayName">
    <div v-for="root in group._treeRoots" :key="root.feature.name">
      <FeatureTreeNode
        :node="root"
        :group-index="groupIndex"
        :base-indent-size="baseIndentSize"
        @change="(feature, idx) => emit('change', feature, idx)"
      />
    </div>
  </Card>
</template>
