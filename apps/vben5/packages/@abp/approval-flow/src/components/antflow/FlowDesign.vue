<script setup lang="ts">
import type { FormFieldDto, ParticipantRef } from '../../types/approval-flow';
import type { AntFlowNode } from './types';

import { computed, onMounted, onUnmounted, ref, watch } from 'vue';

import { useApprovalParticipantsApi } from '../../api/useApprovalParticipantsApi';
import ApprovalNodeDrawer from './ApprovalNodeDrawer.vue';
import ConditionNodeDrawer from './ConditionNodeDrawer.vue';
import { createStartNode } from './model';
import NodeWarp from './NodeWarp.vue';
import { NodeType } from './types';

const props = withDefaults(
  defineProps<{
    data?: AntFlowNode | null;
    formFields?: FormFieldDto[];
    modelValue?: AntFlowNode | null;
    readonly?: boolean;
    statusMap?: Record<string, string>;
    value?: AntFlowNode | null;
  }>(),
  {
    data: null,
    formFields: undefined,
    modelValue: null,
    readonly: false,
    statusMap: () => ({}),
    value: null,
  },
);
const emit = defineEmits<{
  change: [node: AntFlowNode | null];
  select: [node: AntFlowNode];
  'update:data': [node: AntFlowNode | null];
  'update:modelValue': [node: AntFlowNode | null];
  'update:value': [node: AntFlowNode | null];
}>();
const MIN_SCALE = 0.4;
const MAX_SCALE = 2.5;
const ZOOM_IN_FACTOR = 1.1;
const ZOOM_OUT_FACTOR = 0.9;

const participantsApi = useApprovalParticipantsApi();
const participants = ref<ParticipantRef[]>([]);
const selectedNode = ref<AntFlowNode | null>(null);
const approvalDrawerOpen = ref(false);
const conditionDrawerOpen = ref(false);
const viewportRef = ref<HTMLDivElement | null>(null);
const flowData = ref<AntFlowNode | null>(null);
const isDragging = ref(false);
const scale = ref(1);
const offsetX = ref(0);
const offsetY = ref(0);

const dragState = {
  originX: 0,
  originY: 0,
  startX: 0,
  startY: 0,
};

const flowTransformStyle = computed(() => ({
  transform: `translate3d(${Math.round(offsetX.value)}px, ${Math.round(
    offsetY.value,
  )}px, 0) scale(${scale.value})`,
  transformOrigin: '0 0',
}));

const zoomPercent = computed(() => `${Math.round(scale.value * 100)}%`);
const canZoomIn = computed(() => scale.value < MAX_SCALE);
const canZoomOut = computed(() => scale.value > MIN_SCALE);

function cloneNode<T>(value: T): T {
  // eslint-disable-next-line unicorn/prefer-structured-clone
  return JSON.parse(JSON.stringify(value)) as T;
}

function clamp(value: number, min: number, max: number): number {
  return Math.max(min, Math.min(max, value));
}

function applyScale(nextScale: number, anchorX?: number, anchorY?: number) {
  const targetScale = clamp(nextScale, MIN_SCALE, MAX_SCALE);
  if (targetScale === scale.value) {
    return;
  }

  const rect = viewportRef.value?.getBoundingClientRect();
  const centerX = anchorX ?? (rect ? rect.width / 2 : 0);
  const centerY = anchorY ?? (rect ? rect.height / 2 : 0);
  const worldX = (centerX - offsetX.value) / scale.value;
  const worldY = (centerY - offsetY.value) / scale.value;

  scale.value = targetScale;
  offsetX.value = centerX - worldX * targetScale;
  offsetY.value = centerY - worldY * targetScale;
}

function zoomIn() {
  applyScale(scale.value * ZOOM_IN_FACTOR);
}

function zoomOut() {
  applyScale(scale.value * ZOOM_OUT_FACTOR);
}

function resetView() {
  offsetX.value = 0;
  offsetY.value = 0;
  scale.value = 1;
}

function onMouseDown(event: MouseEvent) {
  if (event.button !== 0) {
    return;
  }

  const target = event.target;
  if (
    target instanceof HTMLElement &&
    target.closest(
      '[data-flow-interactive="true"], .add-node, .node-card, button, input',
    )
  ) {
    return;
  }

  isDragging.value = true;
  dragState.startX = event.clientX;
  dragState.startY = event.clientY;
  dragState.originX = offsetX.value;
  dragState.originY = offsetY.value;
  event.preventDefault();
}

function onMouseMove(event: MouseEvent) {
  if (!isDragging.value) {
    return;
  }
  offsetX.value = dragState.originX + (event.clientX - dragState.startX);
  offsetY.value = dragState.originY + (event.clientY - dragState.startY);
}

function onMouseUp() {
  isDragging.value = false;
}

function onWheel(event: WheelEvent) {
  const rect = viewportRef.value?.getBoundingClientRect();
  if (!rect) {
    return;
  }

  if (event.cancelable) {
    event.preventDefault();
  }

  const pointerX = event.clientX - rect.left;
  const pointerY = event.clientY - rect.top;
  const factor = event.deltaY < 0 ? ZOOM_IN_FACTOR : ZOOM_OUT_FACTOR;
  applyScale(scale.value * factor, pointerX, pointerY);
}

watch(
  () => props.modelValue ?? props.value ?? props.data,
  (next) => {
    flowData.value = next ? cloneNode(next) : createStartNode();
  },
  { deep: true, immediate: true },
);

function handleUpdate(node: AntFlowNode | null) {
  flowData.value = node;
  emit('update:modelValue', node);
  emit('update:value', node);
  emit('update:data', node);
  emit('change', node);
}

function handleSelect(node: AntFlowNode) {
  if (props.readonly) {
    emit('select', node);
    return;
  }
  selectedNode.value = cloneNode(node);
  if (node.nodeType === NodeType.ConditionNode) {
    conditionDrawerOpen.value = true;
  } else if (
    node.nodeType === NodeType.ApproveNode ||
    node.nodeType === NodeType.ParallelApproveNode ||
    node.nodeType === NodeType.CopyNode
  ) {
    approvalDrawerOpen.value = true;
  }
  emit('select', node);
}

function replaceNode(
  root: AntFlowNode | null,
  targetId: string,
  updated: AntFlowNode,
): AntFlowNode | null {
  if (!root) {
    return root;
  }
  if (root.nodeId === targetId) {
    return updated;
  }
  const next: AntFlowNode = { ...root };
  if (next.childNode) {
    next.childNode = replaceNode(next.childNode, targetId, updated);
  }
  const property = next.nodeProperty;
  if (property) {
    if (Array.isArray(property.conditionNodes)) {
      property.conditionNodes = property.conditionNodes.map(
        (item: AntFlowNode) => replaceNode(item, targetId, updated),
      );
    }
    if (Array.isArray(property.parallelNodes)) {
      property.parallelNodes = property.parallelNodes.map((item: AntFlowNode) =>
        replaceNode(item, targetId, updated),
      );
    }
  }
  return next;
}

function handleNodeDrawerUpdate(node: AntFlowNode) {
  const updatedRoot = replaceNode(flowData.value, node.nodeId, node);
  if (updatedRoot) {
    handleUpdate(updatedRoot);
  }
  selectedNode.value = cloneNode(node);
}

async function loadParticipants() {
  try {
    const [users, units, roles] = await Promise.all([
      participantsApi.searchUsersAsync(),
      participantsApi.getOrganizationUnitsAsync(),
      participantsApi.getRolesAsync(),
    ]);
    participants.value = [...users, ...units, ...roles];
  } catch {
    participants.value = [];
  }
}

interface FlowNodeError {
  key: string;
  label: string;
  message: string;
}

function collectErrors(
  node: AntFlowNode | null,
  errors: FlowNodeError[],
  visited: Set<string>,
): FlowNodeError[] {
  if (!node) {
    return errors;
  }
  if (node.nodeId) {
    if (visited.has(node.nodeId)) {
      return errors;
    }
    visited.add(node.nodeId);
  }

  if (node.error && node.nodeType !== NodeType.StartNode) {
    errors.push({
      key: node.nodeId,
      label: node.nodeName || node.nodeDisplayName || '流程设计',
      message: `请完善：${node.nodeDisplayName || node.nodeName || '节点'}`,
    });
  }

  if (node.nodeType === NodeType.GatewayNode) {
    const conditionNodes = node.nodeProperty?.conditionNodes;
    if (Array.isArray(conditionNodes)) {
      for (const branch of conditionNodes as AntFlowNode[]) {
        collectErrors(branch, errors, visited);
      }
    }
  }

  collectErrors(node.childNode ?? null, errors, visited);
  return errors;
}

function validatePublish(): FlowNodeError[] {
  return collectErrors(flowData.value, [], new Set<string>());
}

function getData(): AntFlowNode | null {
  return flowData.value;
}

onMounted(() => {
  if (!props.readonly) {
    loadParticipants();
  }
  window.addEventListener('mousemove', onMouseMove);
  window.addEventListener('mouseup', onMouseUp);
  viewportRef.value?.addEventListener('wheel', onWheel, {
    passive: false,
  });
});

onUnmounted(() => {
  window.removeEventListener('mousemove', onMouseMove);
  window.removeEventListener('mouseup', onMouseUp);
  viewportRef.value?.removeEventListener('wheel', onWheel);
});

defineExpose({
  getData,
  resetView,
  validatePublish,
  zoomIn,
  zoomOut,
});
</script>

<template>
  <div class="review-warp">
    <div
      ref="viewportRef"
      class="flow-viewport"
      :class="{ dragging: isDragging }"
      @mousedown="onMouseDown"
    >
      <div class="zoom-toolbar" data-flow-interactive="true" @mousedown.stop>
        <button
          class="zoom-btn"
          type="button"
          title="缩小"
          :disabled="!canZoomOut"
          @click="zoomOut"
        >
          −
        </button>
        <span class="zoom-percent">{{ zoomPercent }}</span>
        <button
          class="zoom-btn"
          type="button"
          title="放大"
          :disabled="!canZoomIn"
          @click="zoomIn"
        >
          ＋
        </button>
        <button class="zoom-btn" type="button" title="重置" @click="resetView">
          ⟳
        </button>
      </div>

      <div class="flow-container" :style="flowTransformStyle">
        <NodeWarp
          v-if="flowData"
          :end-node="true"
          :node="flowData"
          :readonly="readonly"
          :status-map="statusMap"
          @select="handleSelect"
          @update:node="handleUpdate"
        />
        <div v-else class="empty-state">暂无流程数据</div>
      </div>
    </div>
  </div>

  <ApprovalNodeDrawer
    v-model:open="approvalDrawerOpen"
    :node="selectedNode"
    :participants="participants"
    :readonly="readonly"
    @update:node="handleNodeDrawerUpdate"
  />
  <ConditionNodeDrawer
    v-model:open="conditionDrawerOpen"
    :form-fields="formFields"
    :node="selectedNode"
    :readonly="readonly"
    @update:node="handleNodeDrawerUpdate"
  />
</template>

<style scoped>
.review-warp {
  display: flex;
  align-items: stretch;
  justify-content: center;
  min-height: 600px;
  padding: 30px 0;
  background-color: #f5f6f8;
}

.flow-viewport {
  position: relative;
  width: 100%;
  min-height: 600px;
  overflow: hidden;
  cursor: grab;
  user-select: none;
}

.flow-viewport.dragging {
  cursor: grabbing;
}

.zoom-toolbar {
  position: absolute;
  top: 20px;
  right: 20px;
  z-index: 20;
  display: flex;
  gap: 8px;
  align-items: center;
  padding: 8px 10px;
  cursor: default;
  background: rgb(255 255 255 / 95%);
  border: 1px solid #dcdfe6;
  border-radius: 4px;
  box-shadow: 0 2px 10px rgb(0 0 0 / 8%);
}

.zoom-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  font-size: 16px;
  color: #606266;
  cursor: pointer;
  background: #fff;
  border: 1px solid #dcdfe6;
  border-radius: 2px;
  transition: all 0.2s ease;
}

.zoom-btn:hover:not(:disabled) {
  color: #409eff;
  border-color: #409eff;
}

.zoom-btn:disabled {
  color: #c0c4cc;
  cursor: not-allowed;
  background: #f5f7fa;
}

.zoom-percent {
  min-width: 46px;
  font-size: 16px;
  line-height: 1;
  color: #303133;
  text-align: center;
}

.flow-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  width: 100%;
  min-height: 600px;
  padding: 20px 0 60px;
  transform-style: preserve-3d;
  backface-visibility: hidden;
  will-change: transform;
}

.empty-state {
  padding: 120px 0;
  font-size: 14px;
  color: #999;
}
</style>
