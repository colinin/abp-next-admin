<script setup lang="ts">
import type {
  ApprovalFlowBranch,
  ApprovalFlowNode,
  ApprovalFlowNodeKind,
  FormFieldDto,
} from '../../types/approval-flow';


import AddNode from './AddNode.vue';
import { summarizeNode } from './model';

const props = defineProps<{
  fields?: FormFieldDto[];
  nodes: ApprovalFlowNode[];
  readonly?: boolean;
  selectedId?: null | string;
  statusMap?: Record<string, string>;
}>();

const emit = defineEmits<{
  add: [nodes: ApprovalFlowNode[], kind: ApprovalFlowNodeKind];
  'add-condition': [node: ApprovalFlowNode];
  'edit-branch': [node: ApprovalFlowNode, branch: ApprovalFlowBranch];
  remove: [node: ApprovalFlowNode];
  select: [id: string];
}>();

const addActions: Array<{ color?: string; key: ApprovalFlowNodeKind; label: string }> = [
  { key: 'approval', label: '审批人', color: '#f2994a' },
  { key: 'cc', label: '抄送人', color: '#2f80ed' },
  { key: 'condition', label: '条件分支', color: '#27ae60' },
];

function nodeIcon(kind: ApprovalFlowNodeKind): string {
  switch (kind) {
    case 'approval': {
      return '审';
    }
    case 'cc': {
      return '抄';
    }
    case 'condition': {
      return '条';
    }
    default: {
      return '•';
    }
  }
}

function summary(node: ApprovalFlowNode): string {
  return summarizeNode(node) || (node.kind === 'cc' ? '待选择抄送人' : '待配置审批人');
}

function statusClass(id: string): string {
  const status = props.statusMap?.[id];
  return status ? `status-${status}` : '';
}

function conditionTitle(index: number): string {
  return `条件${index + 1}`;
}

function conditionExpression(expression: null | string | undefined, index: number, total: number): string {
  if (expression?.trim()) {
    return expression
      .replace(/\bform\./g, '')
      .replace(/>=/g, '≥')
      .replace(/<=/g, '≤')
      .replace(/===/g, '=')
      .replace(/!==/g, '≠')
      .replace(/==/g, '=');
  }
  return index === total - 1 ? '其他条件进入此流程' : '请设置条件';
}

function onAddCondition(node: ApprovalFlowNode) {
  emit('add-condition', node);
}

function onEditBranch(node: ApprovalFlowNode, branch: ApprovalFlowBranch) {
  emit('edit-branch', node, branch);
}
function onAdd(info: any) {
  emit('add', props.nodes, info.key as ApprovalFlowNodeKind);
}

function onSelect(id: string) {
  emit('select', id);
}

function onRemove(node: ApprovalFlowNode) {
  emit('remove', node);
}
</script>

<template>
  <div class="branch-flow">
    <template v-for="node in nodes" :key="node.id">
      <div class="connecting-line">
        <div class="line-top"></div>
        <AddNode :actions="addActions" :readonly="readonly" @add="onAdd" />
        <div class="line-bottom"></div>
        <div class="arrow"></div>
      </div>

      <div v-if="node.kind === 'condition'" class="nested-condition" :class="statusClass(node.id)">
        <div v-if="!readonly" class="nested-add-condition" @click="onAddCondition(node)">
          添加条件
        </div>
        <div class="nested-branches">
          <div
            v-for="(branch, index) in node.branches ?? []"
            :key="branch.id"
            class="nested-column"
          >
            <div
              class="node-card nested-condition-card"
              :class="[statusClass(node.id)]"
              @click="index < (node.branches ?? []).length - 1 && onEditBranch(node, branch)"
            >
              <span class="node-priority">优先级{{ index + 1 }}</span>
              <button v-if="!readonly" class="node-delete" type="button" @click.stop="onRemove(node)">
                ×
              </button>
              <div class="node-header hd-condition">
                <span class="title">{{ conditionTitle(index) }}</span>
              </div>
              <div class="node-content">
                <span class="content">{{ conditionExpression(branch.expression, index, (node.branches ?? []).length) }}</span>
                <span class="arrow-right">›</span>
              </div>
            </div>

            <BranchFlow
              :fields="fields"
              :nodes="branch.nodes"
              :readonly="readonly"
              :selected-id="selectedId"
              :status-map="statusMap"
              @add="(nodes, kind) => emit('add', nodes, kind)"
              @add-condition="onAddCondition"
              @edit-branch="onEditBranch"
              @remove="onRemove"
              @select="onSelect"
            />
            <div class="branch-empty-space"></div>
          </div>
        </div>
      </div>

      <div
        v-else
        class="node-card branch-card"
        :class="[statusClass(node.id), { 'is-active': node.id === selectedId }]"
      >
        <button v-if="!readonly" class="node-delete" type="button" @click.stop="onRemove(node)">
          ×
        </button>
        <div class="node-header" :class="`hd-${node.kind}`">
          <span class="node-icon">{{ nodeIcon(node.kind) }}</span>
          <span class="title">{{ node.name }}</span>
        </div>
        <div class="node-content" @click="onSelect(node.id)">
          <span class="content">{{ summary(node) }}</span>
          <span class="arrow-right">›</span>
        </div>
      </div>
    </template>

    <div v-if="nodes.length === 0 && !readonly" class="branch-add-wrap">
      <AddNode :actions="addActions" :readonly="readonly" @add="onAdd" />
    </div>
  </div>
</template>

<style scoped>
.branch-flow {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.connecting-line {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.line-top,
.line-bottom {
  width: 1px;
  height: 20px;
  background-color: #cacaca;
}

.arrow {
  width: 0;
  height: 0;
  border-top: 6px solid #cacaca;
  border-right: 5px solid transparent;
  border-left: 5px solid transparent;
}

.add-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 30px;
  height: 30px;
  font-size: 18px;
  color: #fff;
  cursor: pointer;
  background: #3296fa;
  border-radius: 50%;
  box-shadow: 0 4px 10px rgb(50 150 250 / 30%);
  transition: transform 0.2s ease;
}

.add-btn:hover {
  transform: scale(1.1);
}

.node-card {
  position: relative;
  width: 260px;
  overflow: hidden;
  background: #fff;
  border: 1px solid transparent;
  border-radius: 8px;
  box-shadow: 0 4px 16px rgb(180 180 180 / 20%);
  transition: all 0.25s ease;
}

.node-card:hover,
.node-card.is-active {
  box-shadow: 0 8px 22px rgb(50 150 250 / 35%);
}

.node-header {
  display: flex;
  gap: 4px;
  align-items: center;
  padding: 6px 12px;
  font-size: 14px;
  color: #fff;
  background: #5b698d;
}

.node-header.hd-approval {
  background: #ff943e;
}

.node-header.hd-cc {
  background: #3296fa;
}

.node-header.hd-condition {
  color: #27ae60;
  background: rgb(39 174 96 / 12%);
}

.node-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 16px;
  height: 16px;
  font-size: 10px;
  background: rgb(255 255 255 / 22%);
  border-radius: 50%;
}

.title {
  max-width: 8em;
  overflow: hidden;
  font-size: 14px;
  text-overflow: ellipsis;
  white-space: nowrap;
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
  cursor: pointer;
}

.content {
  display: -webkit-box;
  flex: 1;
  min-width: 0;
  max-height: 54px;
  overflow: hidden;
  line-height: 18px;
  text-overflow: ellipsis;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
}

.arrow-right {
  display: flex;
  flex: 0 0 15px;
  align-items: center;
  justify-content: center;
  width: 15px;
  height: 22px;
  font-size: 20px;
  color: #999;
  border-left: 1px solid #ececec;
}

.node-delete {
  position: absolute;
  top: 6px;
  right: 6px;
  z-index: 2;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  padding: 0;
  font-size: 12px;
  color: #fff;
  cursor: pointer;
  background: rgb(0 0 0 / 35%);
  border: none;
  border-radius: 50%;
  opacity: 0;
  transition: opacity 0.2s ease;
}

.node-card:hover .node-delete {
  opacity: 1;
}

.node-card.status-approved .node-header {
  background: #52c41a;
}

.node-card.status-rejected .node-header {
  background: #cf1322;
}

.node-card.status-running .node-header {
  background: #1677ff;
}

.node-card.status-pending .node-header {
  background: #8c8c8c;
}
.nested-condition {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
  padding-top: 18px;
}

.nested-add-condition {
  position: absolute;
  top: -8px;
  z-index: 2;
  padding: 2px 10px;
  font-size: 12px;
  color: #3296fa;
  cursor: pointer;
  background: #fff;
  border-radius: 20px;
  box-shadow: 0 2px 6px rgb(0 0 0 / 10%);
}

.nested-add-condition:hover {
  color: #fff;
  background: #3296fa;
}

.nested-branches {
  position: relative;
  display: flex;
  align-items: stretch;
  justify-content: center;
  width: fit-content;
  padding: 16px 0;
}

.nested-branches::before,
.nested-branches::after {
  position: absolute;
  left: 50%;
  width: calc(100% - 299px);
  height: 1px;
  content: "";
  background: #cacaca;
  transform: translateX(-50%);
}

.nested-branches::before {
  top: 0;
}

.nested-branches::after {
  bottom: 0;
}

.nested-column {
  position: relative;
  display: flex;
  flex: 0 0 260px;
  flex-direction: column;
  align-items: center;
  align-self: stretch;
  margin: 0 20px;
}

.nested-column::before,
.nested-column::after {
  position: absolute;
  left: 50%;
  width: 1px;
  height: 16px;
  content: "";
  background: #cacaca;
  transform: translateX(-50%);
}

.nested-column::before {
  top: -16px;
}

.nested-column::after {
  bottom: -16px;
}

.nested-condition-card {
  width: 260px;
}

.nested-condition-card .node-header {
  padding-right: 40px;
}

.nested-condition .branch-empty-space {
  flex: 1;
  width: 1px;
  min-height: 16px;
  background: #cacaca;
}.nested-column {
  flex: 0 0 auto;
  min-width: 260px;
}

.nested-branches {
  max-width: none;
}

.nested-branches::before,
.nested-branches::after {
  left: 0;
  width: 100%;
  transform: none;
}.nested-column {
  padding: 16px 0;
}

.nested-column::before,
.nested-column::after {
  display: none;
}

.nested-branches {
  padding: 0;
}

.nested-top-line {
  flex: none;
  width: 1px;
  height: 16px;
  background: #cacaca;
}

.nested-bottom-line {
  flex: 1 1 auto;
  width: 1px;
  min-height: 16px;
  background: #cacaca;
}.nested-branches {
  padding: 16px 0;
}

.nested-branches::before,
.nested-branches::after {
  left: 50%;
  width: calc(100% - 299px);
  transform: translateX(-50%);
}

.nested-column {
  flex: 0 0 260px;
  min-width: 260px;
  padding: 0;
}

.nested-column::before,
.nested-column::after {
  display: block;
  width: 1px;
  height: 16px;
}

.nested-column::before {
  top: -16px;
}

.nested-column::after {
  bottom: -16px;
}.node-card {
  cursor: grab;
}

.node-card.is-dragging {
  cursor: grabbing;
  opacity: 0.45;
}</style>
