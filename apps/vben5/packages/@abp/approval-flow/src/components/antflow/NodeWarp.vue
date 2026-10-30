<!-- eslint-disable unicorn/prefer-structured-clone -->
<script setup lang="ts">
import type {
  AntFlowAddKind,
  AntFlowNode,
  AntFlowNodeStatus,
  AntFlowStatusMap,
} from './types';

import { computed, ref } from 'vue';

import { $t } from '@vben/locales';

import AddNode from './AddNode.vue';
import {
  createApproveNode,
  createConditionNode,
  createCopyNode,
  createGatewayNode,
  resetNodeWeights,
} from './model';
import { NodeType, placeholderList } from './types';

defineOptions({
  name: 'NodeWarp',
});

const props = withDefaults(
  defineProps<{
    /** Render the terminal flow-end marker for the main chain. */
    endNode?: boolean;
    node?: AntFlowNode | null;
    readonly?: boolean;
    statusMap?: AntFlowStatusMap;
  }>(),
  {
    endNode: false,
    node: null,
    readonly: false,
    statusMap: () => ({}),
  },
);

const emit = defineEmits<{
  select: [node: AntFlowNode];
  'update:node': [node: AntFlowNode | null];
}>();

const editing = ref(false);
const draftName = ref('');
const editingBranchIndex = ref<null | number>(null);
const branchDraftName = ref('');

const vFocus = {
  mounted(element: HTMLInputElement) {
    element.focus();
    element.select();
  },
};

const isGateway = computed(() => props.node?.nodeType === NodeType.GatewayNode);
const isCondition = computed(
  () => props.node?.nodeType === NodeType.ConditionNode,
);
const canDelete = computed(
  () =>
    Boolean(props.node) &&
    !props.readonly &&
    props.node?.nodeType !== NodeType.StartNode,
);
const defaultText = computed(() => {
  const nodeType = props.node?.nodeType;
  return nodeType === undefined
    ? $t('antflow.nodeWarp.node')
    : $t(placeholderList[nodeType] ?? 'antflow.nodeWarp.node');
});
const conditionNodes = computed<AntFlowNode[]>(() => {
  const nodes = props.node?.nodeProperty?.conditionNodes;
  return Array.isArray(nodes) ? (nodes as AntFlowNode[]) : [];
});

function cloneNode<T>(value: T): T {
  return JSON.parse(JSON.stringify(value)) as T;
}

function iconOf(node: AntFlowNode): string {
  switch (node.nodeType) {
    case NodeType.ApproveNode:
    case NodeType.ParallelApproveNode: {
      return '✓';
    }
    case NodeType.ConditionNode: {
      return '◇';
    }
    case NodeType.CopyNode: {
      return 'CC';
    }
    case NodeType.StartNode: {
      return '▶';
    }
    default: {
      return '•';
    }
  }
}

function normalizeStatus(value: unknown): '' | AntFlowNodeStatus {
  const text = String(value ?? '').toLowerCase();
  switch (text) {
    case 'active':
    case 'current':
    case 'in_progress':
    case 'inprogress':
    case 'processing':
    case 'running':
    case '进行中': {
      return 'current';
    }
    case 'agree':
    case 'approved':
    case 'complete':
    case 'completed':
    case 'passed':
    case 'success':
    case '已通过': {
      return 'approved';
    }
    case 'denied':
    case 'failed':
    case 'refused':
    case 'reject':
    case 'rejected':
    case '已拒绝': {
      return 'rejected';
    }
    case 'not_started':
    case 'notstarted':
    case 'pending':
    case 'skipped':
    case 'waiting':
    case '未执行': {
      return 'pending';
    }
    default: {
      return '';
    }
  }
}

function statusOf(node: AntFlowNode): '' | AntFlowNodeStatus {
  return normalizeStatus(props.statusMap[node.nodeId]);
}

function statusLabelOf(node: AntFlowNode): string {
  switch (statusOf(node)) {
    case 'approved': {
      return $t('antflow.nodeWarp.statusApproved');
    }
    case 'current': {
      return $t('antflow.nodeWarp.statusRunning');
    }
    case 'pending': {
      return $t('antflow.nodeWarp.statusNotStarted');
    }
    case 'rejected': {
      return $t('antflow.nodeWarp.statusRejected');
    }
    default: {
      return '';
    }
  }
}

function cardClass(node: AntFlowNode): Record<string, boolean> {
  const status = statusOf(node);
  return {
    'node-card': true,
    'has-error': Boolean(node.error),
    [`is-${status}`]: Boolean(status),
  };
}

function handleSelect(node: AntFlowNode) {
  emit('select', node);
}

function forwardSelect(node: AntFlowNode) {
  emit('select', node);
}

function handleDelete() {
  const current = props.node;
  if (!current || props.readonly) {
    return;
  }
  emit('update:node', current.childNode ?? null);
}

function handleDeleteBranch(branch: AntFlowNode) {
  const current = props.node;
  if (!current || props.readonly) {
    return;
  }

  const updated = cloneNode(current);
  const nodes = updated.nodeProperty?.conditionNodes;
  if (!Array.isArray(nodes)) {
    return;
  }

  if (nodes.length <= 2) {
    emit('update:node', updated.childNode ?? null);
    return;
  }

  const index = nodes.findIndex(
    (item: AntFlowNode) => item.nodeId === branch.nodeId,
  );
  if (index === -1) {
    return;
  }

  nodes.splice(index, 1);
  updated.nodeProperty.conditionNodes = resetNodeWeights(nodes);
  emit('update:node', updated);
}

function handleAddCondition() {
  const current = props.node;
  if (!current || props.readonly) {
    return;
  }

  const updated = cloneNode(current);
  const nodes = Array.isArray(updated.nodeProperty?.conditionNodes)
    ? ([...updated.nodeProperty.conditionNodes] as AntFlowNode[])
    : [];
  const created = createConditionNode(
    $t('antflow.nodeWarp.conditionName').replace(
      '{0}',
      String(nodes.length + 1),
    ),
    null,
    nodes.length + 1,
    0,
  );

  updated.nodeProperty = {
    ...updated.nodeProperty,
    conditionNodes: resetNodeWeights([...nodes, created]),
  };
  emit('update:node', updated);
}

function createByKind(
  kind: AntFlowAddKind,
  child: AntFlowNode | null,
): AntFlowNode {
  switch (kind) {
    case 'approval': {
      return createApproveNode(child);
    }
    case 'cc': {
      return createCopyNode(child);
    }
    case 'condition': {
      return createGatewayNode(child);
    }
    default: {
      return createApproveNode(child);
    }
  }
}

function handleBranchAdd(index: number, kind: AntFlowAddKind) {
  const current = props.node;
  if (!current || props.readonly) {
    return;
  }

  const updated = cloneNode(current);
  const nodes = updated.nodeProperty?.conditionNodes;
  if (!Array.isArray(nodes) || !nodes[index]) {
    return;
  }

  const branch = nodes[index] as AntFlowNode;
  const created = createByKind(kind, branch.childNode ?? null);
  nodes[index] = {
    ...branch,
    childNode: created,
  };
  emit('update:node', updated);
}
function handleAdd(kind: AntFlowAddKind) {
  const current = props.node;
  if (!current || props.readonly) {
    return;
  }

  const updated = cloneNode(current);
  updated.childNode = createByKind(kind, updated.childNode ?? null);
  emit('update:node', updated);
}

function handleChildUpdate(child: AntFlowNode | null) {
  const current = props.node;
  if (!current) {
    return;
  }

  const updated = cloneNode(current);
  updated.childNode = child;
  emit('update:node', updated);
}

function handleBranchUpdate(index: number, child: AntFlowNode | null) {
  const current = props.node;
  if (!current) {
    return;
  }

  const updated = cloneNode(current);
  const nodes = updated.nodeProperty?.conditionNodes;
  if (!Array.isArray(nodes) || !nodes[index]) {
    return;
  }

  nodes[index] = {
    ...(nodes[index] as AntFlowNode),
    childNode: child,
  };
  emit('update:node', updated);
}

function startEdit() {
  const current = props.node;
  if (!current || props.readonly || isGateway.value) {
    return;
  }
  draftName.value = current.nodeName;
  editing.value = true;
}

function commitEdit() {
  const current = props.node;
  if (!editing.value || !current) {
    return;
  }

  const updated = cloneNode(current);
  updated.nodeName = draftName.value.trim() || defaultText.value;
  if (updated.nodeType === NodeType.ConditionNode) {
    updated.nodeDisplayName = updated.nodeName;
  }
  emit('update:node', updated);
  editing.value = false;
}

function cancelEdit() {
  editing.value = false;
}

function startBranchEdit(index: number, branch: AntFlowNode) {
  if (props.readonly) {
    return;
  }
  editingBranchIndex.value = index;
  branchDraftName.value = branch.nodeName;
}

function commitBranchEdit(index: number) {
  const current = props.node;
  if (!current || editingBranchIndex.value !== index) {
    return;
  }

  const updated = cloneNode(current);
  const nodes = updated.nodeProperty?.conditionNodes;
  if (!Array.isArray(nodes) || !nodes[index]) {
    editingBranchIndex.value = null;
    return;
  }

  const name =
    branchDraftName.value.trim() ||
    $t('antflow.nodeWarp.conditionName', [String(index + 1)]);
  nodes[index] = {
    ...(nodes[index] as AntFlowNode),
    nodeDisplayName: name,
    nodeName: name,
  };
  emit('update:node', updated);
  editingBranchIndex.value = null;
}

function cancelBranchEdit() {
  editingBranchIndex.value = null;
}
</script>

<template>
  <div class="node-warp">
    <!-- 1. Regular node -->
    <template v-if="node && !isGateway">
      <div class="node-card-wrap">
        <span
          v-if="node.error"
          class="node-error-badge"
          :aria-label="$t('antflow.nodeWarp.configError')"
        >
          !
        </span>
        <div :class="cardClass(node)">
          <span v-if="isCondition" class="node-priority">
            {{
              $t('antflow.nodeWarp.priority').replace(
                '{0}',
                String(node.nodeWeight),
              )
            }}
          </span>
          <button
            v-if="canDelete"
            class="node-delete"
            type="button"
            :aria-label="$t('antflow.nodeWarp.deleteNode')"
            @click.stop="handleDelete"
          >
            ×
          </button>
          <div class="node-header" :class="[`hd-${node.nodeType}`]">
            <div class="header-left">
              <span class="node-icon">{{ iconOf(node) }}</span>
              <span v-if="!editing" class="title" @click.stop="startEdit">
                {{ node.nodeName }}
              </span>
              <input
                v-else
                v-focus
                v-model="draftName"
                class="node-title-input"
                type="text"
                @blur="commitEdit"
                @click.stop
                @keydown.enter.stop.prevent="commitEdit"
                @keydown.esc.stop.prevent="cancelEdit"
              />
            </div>
          </div>
          <div class="node-content" @click.stop="handleSelect(node)">
            <span v-if="node.error" class="placeholder">
              {{
                $t('antflow.nodeWarp.selectPlaceholder').replace(
                  '{0}',
                  defaultText,
                )
              }}
            </span>
            <span v-else class="content">{{ node.nodeDisplayName }}</span>
            <span
              v-if="statusLabelOf(node)"
              class="node-status"
              :class="`status-${statusOf(node)}`"
            >
              {{ statusLabelOf(node) }}
            </span>
            <span class="arrow-right">›</span>
          </div>
        </div>
      </div>

      <div class="connecting-line">
        <template v-if="!readonly">
          <div class="line-top"></div>
          <AddNode @add="handleAdd" />
          <div class="line-bottom"></div>
        </template>
        <div v-else class="line-full"></div>
        <div class="arrow"></div>
      </div>

      <NodeWarp
        v-if="node.childNode"
        :end-node="endNode"
        :node="node.childNode"
        :readonly="readonly"
        :status-map="statusMap"
        @select="forwardSelect"
        @update:node="handleChildUpdate"
      />
      <div v-else-if="endNode" class="end-node">
        <div class="circle"></div>
        <span class="text">{{ $t('antflow.nodeWarp.flowEnd') }}</span>
      </div>
    </template>

    <!-- 2. Condition gateway -->
    <template v-if="node && isGateway">
      <div class="parallel-branches-container">
        <div
          v-if="!readonly"
          class="branch-label condition-branch-label"
          @click.stop="handleAddCondition"
        >
          {{ $t('antflow.nodeWarp.addCondition') }}
        </div>
        <div v-else class="branch-label">
          {{ $t('antflow.nodeWarp.conditionBranch') }}
        </div>

        <div class="branches-wrapper">
          <div
            v-for="(branch, index) in conditionNodes"
            :key="branch.nodeId || index"
            class="branch-column"
          >
            <div class="node-card-wrap">
              <span
                v-if="branch.error"
                class="node-error-badge"
                :aria-label="$t('antflow.nodeWarp.configError')"
              >
                !
              </span>
              <div :class="cardClass(branch)">
                <span class="node-priority">
                  {{
                    $t('antflow.nodeWarp.priority').replace(
                      '{0}',
                      String(branch.nodeWeight),
                    )
                  }}
                </span>
                <button
                  v-if="!readonly"
                  class="node-delete"
                  type="button"
                  :aria-label="$t('antflow.nodeWarp.deleteCondition')"
                  @click.stop="handleDeleteBranch(branch)"
                >
                  ×
                </button>
                <div class="node-header" :class="[`hd-${branch.nodeType}`]">
                  <div class="header-left">
                    <span class="node-icon">{{ iconOf(branch) }}</span>
                    <span
                      v-if="editingBranchIndex !== index"
                      class="title"
                      @click.stop="startBranchEdit(index, branch)"
                    >
                      {{ branch.nodeName }}
                    </span>
                    <input
                      v-else
                      v-focus
                      v-model="branchDraftName"
                      class="node-title-input"
                      type="text"
                      @blur="commitBranchEdit(index)"
                      @click.stop
                      @keydown.enter.stop.prevent="commitBranchEdit(index)"
                      @keydown.esc.stop.prevent="cancelBranchEdit"
                    />
                  </div>
                </div>
                <div class="node-content" @click.stop="handleSelect(branch)">
                  <span v-if="branch.error" class="placeholder">
                    {{
                      $t('antflow.nodeWarp.selectPlaceholder').replace(
                        '{0}',
                        defaultText,
                      )
                    }}
                  </span>
                  <span v-else class="content">
                    {{ branch.nodeDisplayName }}
                  </span>
                  <span
                    v-if="statusLabelOf(branch)"
                    class="node-status"
                    :class="`status-${statusOf(branch)}`"
                  >
                    {{ statusLabelOf(branch) }}
                  </span>
                  <span class="arrow-right">›</span>
                </div>
              </div>
            </div>

            <div class="connecting-line">
              <template v-if="!readonly">
                <div class="line-top"></div>
                <AddNode @add="handleBranchAdd(index, $event)" />
                <div class="line-bottom"></div>
              </template>
              <div v-else class="line-full"></div>
              <div class="arrow"></div>
            </div>

            <div class="branch-empty-space"></div>

            <NodeWarp
              v-if="branch.childNode"
              :node="branch.childNode"
              :readonly="readonly"
              :status-map="statusMap"
              @select="forwardSelect"
              @update:node="handleBranchUpdate(index, $event)"
            />
            <div v-if="index === 0" class="top-left-cover-line"></div>
            <div v-if="index === 0" class="bottom-left-cover-line"></div>
            <div
              v-if="index === conditionNodes.length - 1"
              class="top-right-cover-line"
            ></div>
            <div
              v-if="index === conditionNodes.length - 1"
              class="bottom-right-cover-line"
            ></div>
          </div>
        </div>
      </div>

      <div class="merge-line">
        <template v-if="!readonly">
          <div class="line-top"></div>
          <AddNode @add="handleAdd" />
          <div class="line-bottom"></div>
        </template>
        <div v-else class="line-full"></div>
        <div class="arrow"></div>
      </div>

      <NodeWarp
        v-if="node.childNode"
        :end-node="endNode"
        :node="node.childNode"
        :readonly="readonly"
        :status-map="statusMap"
        @select="forwardSelect"
        @update:node="handleChildUpdate"
      />
      <div v-else-if="endNode" class="end-node">
        <div class="circle"></div>
        <span class="text">{{ $t('antflow.nodeWarp.flowEnd') }}</span>
      </div>
    </template>

    <div v-if="!node && endNode" class="end-node">
      <div class="circle"></div>
      <span class="text">{{ $t('antflow.nodeWarp.flowEnd') }}</span>
    </div>
  </div>
</template>

<style scoped>
.node-warp {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.node-card-wrap {
  position: relative;
  width: 260px;
}

.node-card {
  position: relative;
  width: 100%;
  overflow: hidden;
  cursor: pointer;
  background-color: #fff;
  border: 1px solid transparent;
  border-radius: 8px;
  box-shadow: 0 4px 16px rgb(180 180 180 / 20%);
  transition: all 0.3s ease;
}

.node-card.has-error {
  border-color: #ff4d4f;
}

.node-card.is-approved {
  box-shadow: 0 0 0 2px #52c41a;
}

.node-card.is-current {
  box-shadow: 0 0 0 2px #3296fa;
}

.node-card.is-pending {
  box-shadow: 0 0 0 2px #bfbfbf;
}

.node-card.is-rejected {
  box-shadow: 0 0 0 2px #ff4d4f;
}

.node-error-badge {
  position: absolute;
  top: 0;
  right: -25px;
  z-index: 1;
  width: 18px;
  height: 18px;
  font-size: 12px;
  font-weight: 700;
  line-height: 18px;
  color: #fff;
  text-align: center;
  pointer-events: none;
  background: #ff4d4f;
  border-radius: 50%;
  box-shadow: 0 2px 8px rgb(255 77 79 / 45%);
}

.node-delete {
  position: absolute;
  top: 6px;
  right: 6px;
  z-index: 3;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  padding: 0;
  font-size: 12px;
  line-height: 1;
  color: #fff;
  cursor: pointer;
  background: rgb(0 0 0 / 35%);
  border: none;
  border-radius: 50%;
  opacity: 0;
  transition:
    opacity 0.2s ease,
    background 0.2s ease;
}

.node-card:hover .node-delete {
  opacity: 1;
}

.node-delete:hover {
  background: rgb(0 0 0 / 55%);
}

.node-priority {
  position: absolute;
  top: 6px;
  right: 30px;
  z-index: 2;
  padding: 0 6px;
  font-size: 11px;
  line-height: 18px;
  color: #555353;
  background: #eee;
  border-radius: 10px;
}

.node-card:hover {
  box-shadow: 0 8px 22px rgb(50 150 250 / 80%);
  transform: translateY(-2px);
}

.node-header {
  display: flex;
  align-items: center;
  padding: 6px 12px;
  font-size: 14px;
  color: #fff;
  background-color: #5b698d;
}

.node-header.hd-1 {
  background-color: #5b698d;
}

.node-header.hd-3 {
  color: #27ae60;
  background-color: rgb(39 174 96 / 12%);
}

.node-header.hd-4,
.node-header.hd-7 {
  background-color: #ff943e;
}

.node-header.hd-6 {
  background-color: #3296fa;
}

.node-header.hd-3 .node-title-input {
  border-color: rgb(39 174 96 / 90%);
}

.node-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  margin-right: 6px;
  font-size: 11px;
  line-height: 1;
  color: inherit;
  border: 1px solid currentcolor;
  border-radius: 50%;
  opacity: 0.9;
}

.header-left {
  display: flex;
  align-items: center;
}

.title {
  display: inline-block;
  max-width: 7em;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.node-title-input {
  width: 12em;
  padding: 2px 4px;
  font-size: 12px;
  line-height: 1.2;
  color: inherit;
  outline: none;
  background: rgb(255 255 255 / 20%);
  border: 1px solid rgb(255 255 255 / 70%);
  border-radius: 4px;
}

.node-content {
  display: flex;
  gap: 6px;
  align-items: center;
  justify-content: space-between;
  min-height: 48px;
  padding: 3px 6px 10px;
  font-size: 14px;
  color: #333;
}

.content {
  display: -webkit-box;
  flex: 1;
  min-width: 0;
  max-height: 54px;
  overflow: hidden;
  text-overflow: ellipsis;
  -webkit-line-clamp: 3;
  line-clamp: 3;
  line-height: 18px;
  -webkit-box-orient: vertical;
}

.placeholder {
  color: #999;
}

.node-status {
  flex: 0 0 auto;
  padding: 0 6px;
  font-size: 11px;
  line-height: 18px;
  white-space: nowrap;
  border-radius: 10px;
}

.node-status.status-approved {
  color: #389e0d;
  background: #f6ffed;
}

.node-status.status-current {
  color: #0958d9;
  background: #e6f4ff;
}

.node-status.status-pending {
  color: #8c8c8c;
  background: #f5f5f5;
}

.node-status.status-rejected {
  color: #cf1322;
  background: #fff1f0;
}

.arrow-right {
  display: flex;
  flex: 0 0 15px;
  align-items: center;
  justify-content: center;
  width: 15px;
  height: 22px;
  margin-left: 0;
  font-size: 20px;
  color: #999;
  pointer-events: none;
  border-left: 1px solid #ececec;
}

.branches-wrapper .branch-column:not(:first-child) .node-content::before {
  display: flex;
  flex: 0 0 15px;
  align-items: center;
  justify-content: center;
  width: 15px;
  height: 22px;
  margin-right: 0;
  font-size: 20px;
  line-height: 1;
  color: #999;
  pointer-events: none;
  content: '\2039';
  border-right: 1px solid #ececec;
}

.branches-wrapper .branch-column:last-child .node-content .arrow-right {
  visibility: hidden;
}

.connecting-line {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.line-top {
  width: 1px;
  height: 20px;
  background-color: #cacaca;
}

.line-bottom {
  width: 1px;
  height: 20px;
  background-color: #cacaca;
}

.line-full {
  width: 1px;
  height: 70px;
  background-color: #cacaca;
}

.connecting-line .arrow {
  width: 0;
  height: 0;
  border-top: 6px solid #cacaca;
  border-right: 5px solid transparent;
  border-left: 5px solid transparent;
}

.parallel-branches-container {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 12px 0 0;
}

.branch-label {
  position: absolute;
  top: -10px;
  z-index: 2;
  padding: 2px 10px;
  font-size: 12px;
  color: #27ae60;
  background-color: #fff;
  border-radius: 20px;
  box-shadow: 0 2px 6px rgb(0 0 0 / 10%);
  transition: all 0.2s ease;
}

.condition-branch-label {
  color: #3296fa;
  cursor: pointer;
}

.condition-branch-label:hover {
  color: #fff;
  background-color: #3296fa;
  box-shadow: 0 4px 12px rgb(50 150 250 / 35%);
}

.branches-wrapper {
  position: relative;
  display: flex;
  justify-content: center;
  width: fit-content;
  max-width: 100%;
  padding-top: 20px;
  padding-bottom: 20px;
  margin: 0 20px;
}

.branches-wrapper::before {
  position: absolute;
  top: 0;
  left: 50%;
  width: calc(100% - 299px);
  height: 1px;
  content: '';
  background-color: #cacaca;
  transform: translateX(-50%);
}

.branches-wrapper::after {
  position: absolute;
  bottom: 0;
  left: 50%;
  width: calc(100% - 299px);
  height: 1px;
  content: '';
  background-color: #cacaca;
  transform: translateX(-50%);
}

.branch-column {
  position: relative;
  display: flex;
  flex: 0 0 260px;
  flex-direction: column;
  align-items: center;
  margin: 0 20px;
}

.branch-column::before {
  position: absolute;
  top: -20px;
  left: 50%;
  width: 1px;
  height: 20px;
  content: '';
  background-color: #cacaca;
  transform: translateX(-50%);
}

.branch-column::after {
  position: absolute;
  bottom: -20px;
  left: 50%;
  width: 1px;
  height: 20px;
  content: '';
  background-color: #cacaca;
  transform: translateX(-50%);
}

.branch-empty-space {
  flex: 1;
  width: 1px;
  background-color: #cacaca;
}

.merge-line {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.merge-line .line-top,
.merge-line .line-bottom {
  width: 1px;
  height: 20px;
  background-color: #cacaca;
}

.merge-line .line-full {
  width: 1px;
  height: 40px;
  background-color: #cacaca;
}

.merge-line .arrow {
  width: 0;
  height: 0;
  border-top: 6px solid #cacaca;
  border-right: 5px solid transparent;
  border-left: 5px solid transparent;
}

.top-left-cover-line,
.top-right-cover-line {
  position: absolute;
  top: -22px;
  width: 50%;
  height: 8px;
  background-color: #f5f6f8;
}

.top-left-cover-line {
  left: -1px;
}

.top-right-cover-line {
  right: -1px;
}

.bottom-left-cover-line,
.bottom-right-cover-line {
  position: absolute;
  bottom: -22px;
  z-index: 1;
  width: 50%;
  height: 8px;
  background-color: #f5f6f8;
}

.bottom-left-cover-line {
  left: -1px;
}

.bottom-right-cover-line {
  right: -1px;
}

.end-node {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 5px;
}

.end-node .circle {
  width: 18px;
  height: 18px;
  margin-bottom: 10px;
  background-color: #dbdbdb;
  border-radius: 50%;
}

.end-node .text {
  font-size: 14px;
  color: #999;
}
</style>
