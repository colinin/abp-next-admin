<!-- eslint-disable prettier/prettier -->
<script setup lang="ts">
import type {
  ApprovalFlowModel,
  ApprovalFlowNode,
  ApprovalNodeTaskDto,
  ApprovalRecordDto,
} from '../../types/approval-flow';

import { computed } from 'vue';

import { Empty } from 'ant-design-vue';

import FlowDesign from '../antflow/FlowDesign.vue';
import { antFlowToOld, oldToAntFlow } from '../antflow/model';
import { parseModel } from '../designer/model';

const props = defineProps<{
  flowJson?: null | string;
  instanceStatus: string;
  nodeTasks: ApprovalNodeTaskDto[];
  records: ApprovalRecordDto[];
}>();

const antFlowRoot = computed(() => {
  if (!props.flowJson) {
    return null;
  }
  try {
    const parsed = JSON.parse(props.flowJson);
    if (parsed?.nodeType) {
      return parsed;
    }
  } catch {
    return null;
  }
  return oldToAntFlow(props.flowJson);
});

const model = computed<ApprovalFlowModel | null>(() => {
  if (!props.flowJson) {
    return null;
  }
  try {
    const parsed = JSON.parse(props.flowJson);
    if (parsed?.nodeType || parsed?.nodes?.[0]?.nodeType) {
      return antFlowToOld(parsed);
    }
  } catch {
    return null;
  }
  return parseModel(props.flowJson);
});

const startNode = computed(
  () => model.value?.nodes.find((node) => node.kind === 'start') ?? null,
);

const endNode = computed(
  () => model.value?.nodes.find((node) => node.kind === 'end') ?? null,
);

const statusMap = computed<Record<string, string>>(() => {
  const map: Record<string, string> = {};
  if (!model.value) {
    return map;
  }

  const visit = (node: ApprovalFlowNode) => {
    if (node.kind === 'condition') {
      for (const branch of node.branches ?? []) {
        for (const child of branch.nodes) {
          visit(child);
        }
      }
      const statuses = new Set(
        (node.branches ?? []).flatMap((branch) =>
          branch.nodes.map((child) => map[child.id] ?? 'pending'),
        ),
      );
      if (statuses.has('rejected')) {
        map[node.id] = 'rejected';
      } else if (statuses.has('running')) {
        map[node.id] = 'running';
      } else if (statuses.has('approved')) {
        map[node.id] = 'approved';
      } else {
        map[node.id] = 'pending';
      }
      return;
    }

    if (node.kind === 'approval') {
      const tasks = props.nodeTasks.filter((task) => task.nodeId === node.id);
      const records = props.records.filter(
        (record) => record.nodeId === node.id,
      );
      if (
        tasks.some(
          (task) => task.status === 'Pending' || task.status === 'Transferred',
        )
      ) {
        map[node.id] = 'running';
      } else if (records.some((record) => record.effect === 'Reject')) {
        map[node.id] = 'rejected';
      } else if (
        tasks.some((task) => task.status === 'Decided') ||
        records.some((record) => record.effect === 'Approve')
      ) {
        map[node.id] = 'approved';
      } else {
        map[node.id] = 'pending';
      }
      return;
    }

    map[node.id] = props.instanceStatus === 'Approved' ? 'approved' : 'pending';
  };

  model.value.nodes.forEach((node) => visit(node));

  if (startNode.value) {
    map[startNode.value.id] = 'approved';
  }
  if (endNode.value) {
    map[endNode.value.id] =
      props.instanceStatus === 'Approved'
        ? 'approved'
        : (props.instanceStatus === 'Rejected'
          ? 'rejected'
          : 'pending');
  }

  return map;
});
</script>

<template>
  <div class="flow-viewer">
    <Empty v-if="!model" description="没有可展示的流程设计" />
    <FlowDesign v-else :status-map="statusMap" :value="antFlowRoot" readonly />
  </div>
</template>

<style scoped>
.flow-viewer {
  height: 560px;
  overflow: hidden;
  background: #f5f6f8;
}
</style>
