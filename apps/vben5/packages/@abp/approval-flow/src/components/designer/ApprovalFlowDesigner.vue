<script setup lang="ts">
import type {
  ApprovalFlowBranch,
  ApprovalFlowModel,
  ApprovalFlowNode,
  ApprovalFlowNodeKind,
  FormFieldDto,
} from '../../types/approval-flow';

import { computed, onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import { Button, Empty, message, Modal, Space } from 'ant-design-vue';

import { useApprovalFormsApi } from '../../api/useApprovalFormsApi';
import { useApprovalTemplatesApi } from '../../api/useApprovalTemplatesApi';
import AddNode from './AddNode.vue';
import BranchConditionDrawer from './BranchConditionDrawer.vue';
import BranchFlow from './BranchFlow.vue';
import FormPreviewModal from './FormPreviewModal.vue';
import { createBranch, createNode, parseModel, summarizeNode } from './model';
import NodeConfigDrawer from './NodeConfigDrawer.vue';

const props = defineProps<{
  embedded?: boolean;
  flowJson?: null | string;
  formDefinitionId?: null | string;
  instanceStatus?: string;
  readonly?: boolean;
  statusMap?: Record<string, string>;
  templateId?: string;
}>();

const templatesApi = useApprovalTemplatesApi();
const formsApi = useApprovalFormsApi();

const loading = ref(false);
const saving = ref(false);
const model = ref<ApprovalFlowModel>(parseModel(null));
const templateName = ref('');
const formFields = ref<FormFieldDto[]>([]);
const selectedId = ref<null | string>(null);
const drawerOpen = ref(false);
const previewOpen = ref(false);
const zoom = ref(1);
const branchConditionOpen = ref(false);
const editingBranch = ref<ApprovalFlowBranch | null>(null);
const editingConditionNode = ref<ApprovalFlowNode | null>(null);

const selectedNode = computed(() =>
  findNode(model.value.nodes, selectedId.value),
);

const editableNodes = computed(() =>
  model.value.nodes.filter(
    (node) => node.kind !== 'start' && node.kind !== 'end',
  ),
);

const startNode = computed(
  () => model.value.nodes.find((node) => node.kind === 'start') ?? null,
);

const endNode = computed(
  () => model.value.nodes.find((node) => node.kind === 'end') ?? null,
);

const addActions: Array<{
  color?: string;
  key: ApprovalFlowNodeKind;
  label: string;
}> = [
  {
    key: 'approval',
    label: $t('antflow.designer.common.approval'),
    color: '#f2994a',
  },
  { key: 'cc', label: $t('antflow.designer.common.cc'), color: '#2f80ed' },
  {
    key: 'condition',
    label: $t('antflow.designer.common.condition'),
    color: '#27ae60',
  },
];

async function loadTemplate() {
  if (!props.templateId) {
    return;
  }

  loading.value = true;
  try {
    const template = await templatesApi.getApi(props.templateId);
    templateName.value = template.displayName;
    model.value = parseModel(template.flowDefinitionJson);
    const effectiveFormId =
      template.formDefinitionId ?? props.formDefinitionId ?? null;
    model.value.formDefinitionId = effectiveFormId;

    if (effectiveFormId) {
      try {
        const form = await formsApi.getApi(effectiveFormId);
        formFields.value = form.fields ?? [];
      } catch {
        formFields.value = [];
      }
    }
  } catch {
    message.error($t('antflow.designer.common.loadFailed'));
  } finally {
    loading.value = false;
  }
}

function addNode(kind: ApprovalFlowNodeKind) {
  const node = createNode(kind);
  const endIndex = model.value.nodes.findIndex((item) => item.kind === 'end');
  const insertAt = endIndex === -1 ? model.value.nodes.length : endIndex;
  model.value.nodes.splice(insertAt, 0, node);

  if (kind === 'condition') {
    const firstBranch = node.branches?.[0];
    if (firstBranch) {
      openBranchEditor(node, firstBranch);
    }
    return;
  }

  selectedId.value = node.id;
  drawerOpen.value = true;
}

function addConditionBranch(node: ApprovalFlowNode) {
  const branches = [...(node.branches ?? [])];
  const insertAt = Math.max(0, branches.length - 1);
  const branch = createBranch(insertAt);
  branch.nodes = [];
  branches.splice(insertAt, 0, branch);
  branches.forEach((item, index) => {
    item.name =
      index === branches.length - 1
        ? $t('antflow.designer.common.otherCondition')
        : $t('antflow.designer.common.conditionWithIndex').replace(
            '{0}',
            String(index + 1),
          );
    if (index === branches.length - 1) {
      item.expression = null;
    }
  });
  node.branches = branches;
  openBranchEditor(node, branch);
}

function openBranchEditor(node: ApprovalFlowNode, branch: ApprovalFlowBranch) {
  editingConditionNode.value = node;
  editingBranch.value = branch;
  branchConditionOpen.value = true;
}

function onBranchChanged(branch: ApprovalFlowBranch) {
  const node = editingConditionNode.value;
  if (!node) {
    return;
  }
  const branches = node.branches ?? [];
  const index = branches.findIndex((item) => item.id === branch.id);
  if (index !== -1) {
    branches[index] = { ...branch };
  }
  if (index === 0) {
    node.condition = {
      compareValue: branch.compareValue,
      expression: branch.expression,
      fieldKey: branch.fieldKey,
      operator: branch.operator,
    };
  }
  editingBranch.value = { ...branch };
}
function onAddMenuClick(kind: ApprovalFlowNodeKind) {
  addNode(kind);
}

function removeNode(node: ApprovalFlowNode) {
  removeNodeById(model.value.nodes, node.id);
  if (selectedId.value === node.id) {
    drawerOpen.value = false;
  }
}

function moveNode(node: ApprovalFlowNode, offset: number) {
  const index = model.value.nodes.findIndex((item) => item.id === node.id);
  const target = index + offset;
  if (index < 0 || target <= 0 || target >= model.value.nodes.length - 1) {
    return;
  }

  const nodes = [...model.value.nodes];
  const [moved] = nodes.splice(index, 1);
  if (moved) {
    nodes.splice(target, 0, moved);
    model.value.nodes = nodes;
  }
}

function selectNode(id: string) {
  if (props.readonly) {
    return;
  }
  selectedId.value = id;
  drawerOpen.value = true;
}

function onNodeChanged(node: ApprovalFlowNode) {
  replaceNode(model.value.nodes, node);
}

function findNode(
  nodes: ApprovalFlowNode[],
  id: null | string,
): ApprovalFlowNode | null {
  for (const node of nodes) {
    if (node.id === id) {
      return node;
    }
    for (const branch of node.branches ?? []) {
      const found = findNode(branch.nodes, id);
      if (found) {
        return found;
      }
    }
  }
  return null;
}

function replaceNode(
  nodes: ApprovalFlowNode[],
  node: ApprovalFlowNode,
): boolean {
  const index = nodes.findIndex((item) => item.id === node.id);
  if (index !== -1) {
    nodes[index] = node;
    return true;
  }
  for (const parent of nodes) {
    for (const branch of parent.branches ?? []) {
      if (replaceNode(branch.nodes, node)) {
        return true;
      }
    }
  }
  return false;
}

function removeNodeById(nodes: ApprovalFlowNode[], id: string): boolean {
  const index = nodes.findIndex((item) => item.id === id);
  if (index !== -1) {
    nodes.splice(index, 1);
    return true;
  }
  for (const parent of nodes) {
    for (const branch of parent.branches ?? []) {
      if (removeNodeById(branch.nodes, id)) {
        return true;
      }
    }
  }
  return false;
}

function addBranchNode(nodes: ApprovalFlowNode[], kind: ApprovalFlowNodeKind) {
  const node = createNode(kind);
  nodes.push(node);

  if (kind === 'condition') {
    const firstBranch = node.branches?.[0];
    if (firstBranch) {
      openBranchEditor(node, firstBranch);
    }
    return;
  }

  selectedId.value = node.id;
  drawerOpen.value = true;
}
function nodeIcon(kind: ApprovalFlowNodeKind): string {
  switch (kind) {
    case 'approval': {
      return $t('antflow.designer.common.approval');
    }
    case 'cc': {
      return $t('antflow.designer.common.cc');
    }
    case 'condition': {
      return $t('antflow.designer.common.condition');
    }
    default: {
      return '•';
    }
  }
}

function statusOf(id: string): string {
  return props.statusMap?.[id] ?? '';
}

function statusClass(id: string): string {
  const status = statusOf(id);
  return status ? `status-${status}` : '';
}

function statusText(id: string): string {
  switch (statusOf(id)) {
    case 'approved': {
      return $t('antflow.designer.common.statusApproved');
    }
    case 'rejected': {
      return $t('antflow.designer.common.statusRejected');
    }
    case 'running': {
      return $t('antflow.designer.common.statusRunning');
    }
    default: {
      return $t('antflow.designer.common.statusNotStarted');
    }
  }
}
function conditionTitle(index: number): string {
  return $t('antflow.designer.common.conditionWithIndex').replace(
    '{0}',
    String(index + 1),
  );
}

function conditionPriority(index: number): string {
  return $t('antflow.designer.common.priority').replace(
    '{0}',
    String(index + 1),
  );
}

function conditionExpression(
  expression: null | string | undefined,
  index: number,
  total = 2,
): string {
  if (expression?.trim()) {
    return expression
      .replaceAll(/\bform\./g, '')
      .replaceAll('>=', '≥')
      .replaceAll('<=', '≤')
      .replaceAll('===', '=')
      .replaceAll('!==', '≠')
      .replaceAll('==', '=');
  }
  return index === total - 1
    ? $t('antflow.designer.common.otherConditionFlow')
    : $t('antflow.designer.common.setCondition');
}
function zoomIn() {
  zoom.value = Math.min(1.5, Math.round((zoom.value + 0.1) * 10) / 10);
}

function zoomOut() {
  zoom.value = Math.max(0.5, Math.round((zoom.value - 0.1) * 10) / 10);
}

function resetZoom() {
  zoom.value = 1;
}

async function saveDesign(): Promise<null | string> {
  if (!props.templateId) {
    message.warning($t('antflow.designer.common.saveFirst'));
    return null;
  }

  saving.value = true;
  try {
    model.value.formDefinitionId =
      props.formDefinitionId ?? model.value.formDefinitionId ?? null;
    const json = JSON.stringify(model.value);
    await templatesApi.saveDesignApi(props.templateId, {
      flowDefinitionJson: json,
    });
    message.success($t('antflow.designer.common.saveSuccess'));
    return json;
  } catch {
    message.error($t('antflow.designer.common.saveFailed'));
    return null;
  } finally {
    saving.value = false;
  }
}

async function publish() {
  if (!props.templateId) {
    return;
  }

  saving.value = true;
  try {
    await templatesApi.publishApi(props.templateId, {
      formDefinitionId: model.value.formDefinitionId ?? null,
    });
    message.success($t('antflow.designer.common.publishSuccess'));
  } catch {
    message.error($t('antflow.designer.common.publishFailed'));
  } finally {
    saving.value = false;
  }
}

function confirmPublish() {
  if (!props.templateId) {
    message.warning($t('antflow.designer.common.saveFirst'));
    return;
  }

  Modal.confirm({
    content: $t('antflow.designer.common.publishConfirm'),
    onOk: publish,
    title: $t('antflow.designer.common.publishTitle'),
  });
}

function getModelJson() {
  return JSON.stringify(model.value);
}

defineExpose({ getModelJson, saveDesign });

onMounted(() => {
  if (props.flowJson) {
    model.value = parseModel(props.flowJson);
    return;
  }
  loadTemplate();
});
</script>

<template>
  <div
    class="approval-designer"
    :class="{ 'is-embedded': embedded, 'is-readonly': readonly }"
  >
    <div v-if="!embedded" class="designer-header">
      <span class="designer-title">{{
        templateName || $t('antflow.designer.common.flowDesign')
      }}</span>
      <Space>
        <Button :disabled="formFields.length === 0" @click="previewOpen = true">
          {{ $t('antflow.designer.common.previewForm') }}
        </Button>
        <Button :loading="saving" @click="saveDesign">
          {{ $t('antflow.designer.common.saveDesign') }}
        </Button>
        <Button type="primary" @click="confirmPublish">
          {{ $t('antflow.designer.common.publishTemplate') }}
        </Button>
      </Space>
    </div>

    <div class="flow-viewport">
      <div class="zoom-toolbar">
        <button
          class="zoom-btn"
          :disabled="zoom <= 0.5"
          :title="$t('antflow.designer.common.zoomOut')"
          @click="zoomOut"
        >
          −
        </button>
        <span class="zoom-percent">{{ Math.round(zoom * 100) }}%</span>
        <button
          class="zoom-btn"
          :disabled="zoom >= 1.5"
          :title="$t('antflow.designer.common.zoomIn')"
          @click="zoomIn"
        >
          ＋
        </button>
        <button
          class="zoom-btn"
          :title="$t('antflow.designer.common.reset')"
          @click="resetZoom"
        >
          ↺
        </button>
      </div>

      <div class="flow-container" :style="{ transform: `scale(${zoom})` }">
        <div
          v-if="startNode"
          class="start-card"
          :class="statusClass(startNode.id)"
          @click="selectNode(startNode.id)"
        >
          <div class="start-card-header">
            <span>{{ $t('antflow.designer.common.initiator') }}</span>
            <span v-if="readonly" class="status-chip">{{
              statusText(startNode.id)
            }}</span>
          </div>
          <div class="start-card-body">
            <span>{{ $t('antflow.designer.common.everyone') }}</span>
            <span class="node-arrow">›</span>
          </div>
        </div>

        <template v-for="node in editableNodes" :key="node.id">
          <div class="connecting-line">
            <div class="line-top"></div>
            <AddNode
              v-if="!readonly"
              :actions="addActions"
              @add="onAddMenuClick"
            />
            <div class="line-bottom"></div>
            <div class="arrow"></div>
          </div>

          <template v-if="node.kind === 'condition'">
            <div class="condition-flow">
              <div
                v-if="!readonly"
                class="condition-pill"
                @click="addConditionBranch(node)"
              >
                {{ $t('antflow.designer.common.addCondition') }}
              </div>
              <div class="condition-branches">
                <div
                  v-for="(branch, index) in node.branches ?? []"
                  :key="branch.id"
                  class="condition-column"
                >
                  <div
                    class="node-card condition-card"
                    :class="[
                      { 'is-active': node.id === selectedId },
                      statusClass(node.id),
                    ]"
                    @click="
                      index < (node.branches ?? []).length - 1 &&
                      openBranchEditor(node, branch)
                    "
                  >
                    <span class="node-priority">{{
                      conditionPriority(index)
                    }}</span>
                    <button
                      v-if="!readonly"
                      class="node-delete"
                      type="button"
                      @click.stop="removeNode(node)"
                    >
                      ×
                    </button>
                    <div class="node-header hd-condition">
                      <span class="title">{{ conditionTitle(index) }}</span>
                      <span v-if="readonly" class="status-chip">{{
                        statusText(node.id)
                      }}</span>
                    </div>
                    <div class="node-content">
                      <span class="content">{{
                        conditionExpression(
                          branch.expression,
                          index,
                          (node.branches ?? []).length,
                        )
                      }}</span>
                      <span class="arrow-right">›</span>
                    </div>
                  </div>
                  <BranchFlow
                    v-if="branch.nodes.length > 0 || !readonly"
                    :fields="formFields"
                    :nodes="branch.nodes"
                    :readonly="readonly"
                    :selected-id="selectedId"
                    :status-map="statusMap"
                    @add="addBranchNode"
                    @add-condition="addConditionBranch"
                    @edit-branch="openBranchEditor"
                    @remove="removeNode"
                    @select="selectNode"
                  />
                  <div class="branch-empty-space"></div>
                  <template v-if="index === 0">
                    <div class="top-left-cover-line"></div>
                    <div class="bottom-left-cover-line"></div>
                  </template>
                  <template v-if="index === (node.branches ?? []).length - 1">
                    <div class="top-right-cover-line"></div>
                    <div class="bottom-right-cover-line"></div>
                  </template>
                </div>
              </div>
            </div>
          </template>

          <div
            v-else
            class="node-card"
            :class="[
              { 'is-active': node.id === selectedId },
              statusClass(node.id),
            ]"
            @click="selectNode(node.id)"
          >
            <button
              v-if="!readonly"
              class="node-delete"
              type="button"
              @click.stop="removeNode(node)"
            >
              ×
            </button>
            <div class="node-header" :class="`hd-${node.kind}`">
              <span class="node-icon">{{ nodeIcon(node.kind) }}</span>
              <span class="node-title">{{ node.name }}</span>
              <span v-if="readonly" class="status-chip">{{
                statusText(node.id)
              }}</span>
              <Space v-if="!readonly" class="node-tools" size="small">
                <Button
                  size="small"
                  type="text"
                  @click.stop="moveNode(node, -1)"
                >
                  ↑
                </Button>
                <Button
                  size="small"
                  type="text"
                  @click.stop="moveNode(node, 1)"
                >
                  ↓
                </Button>
              </Space>
            </div>
            <div class="node-content">
              <span class="node-summary">
                {{
                  summarizeNode(node) ||
                  $t('antflow.designer.common.selectCondition')
                }}
              </span>
              <span class="node-arrow">›</span>
            </div>
          </div>
        </template>

        <div class="connector">
          <div class="connector-line"></div>
          <AddNode
            v-if="!readonly"
            :actions="addActions"
            @add="onAddMenuClick"
          />
          <div class="connector-line"></div>
          <div class="arrow"></div>
        </div>

        <div v-if="endNode" class="end-node" :class="statusClass(endNode.id)">
          <div class="end-circle"></div>
          <span class="end-text">{{
            $t('antflow.designer.common.flowEnd')
          }}</span>
        </div>

        <Empty
          v-if="editableNodes.length === 0"
          class="mt-4"
          :description="$t('antflow.designer.common.configHint')"
        />
      </div>
    </div>

    <FormPreviewModal v-model:open="previewOpen" :fields="formFields" />

    <BranchConditionDrawer
      v-if="!readonly"
      v-model:open="branchConditionOpen"
      :branch="editingBranch"
      :fields="formFields"
      @update:branch="onBranchChanged"
    />

    <NodeConfigDrawer
      v-if="!readonly"
      v-model:open="drawerOpen"
      :form-fields="formFields"
      :node="selectedNode"
      @update:node="onNodeChanged"
    />
  </div>
</template>

<style scoped>
/* stylelint-disable no-duplicate-selectors */
.approval-designer {
  display: flex;
  flex-direction: column;
  height: 100%;
  background: #f5f6f8;
}

.designer-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 20px;
  background: #fff;
  border-bottom: 1px solid #ebeef5;
}

.designer-title {
  font-size: 15px;
  font-weight: 600;
  color: #303133;
}

.flow-viewport {
  position: relative;
  flex: 1;
  min-height: 520px;
  padding: 28px 0 60px;
  overflow: auto;
  background: #f5f6f8;
}

.zoom-toolbar {
  position: sticky;
  top: 16px;
  right: 20px;
  z-index: 20;
  float: right;
  display: flex;
  gap: 6px;
  align-items: center;
  padding: 6px 8px;
  margin-right: 20px;
  background: rgb(255 255 255 / 95%);
  border: 1px solid #dcdfe6;
  border-radius: 4px;
  box-shadow: 0 2px 10px rgb(0 0 0 / 8%);
}

.zoom-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 26px;
  height: 26px;
  color: #606266;
  cursor: pointer;
  background: #fff;
  border: 1px solid #dcdfe6;
  border-radius: 2px;
}

.zoom-btn:hover:not(:disabled) {
  color: #3296fa;
  border-color: #3296fa;
}

.zoom-btn:disabled {
  color: #c0c4cc;
  cursor: not-allowed;
}

.zoom-percent {
  min-width: 40px;
  font-size: 12px;
  color: #606266;
  text-align: center;
}

.flow-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 520px;
  margin: 0 auto;
  transform-origin: top center;
}

.start-node,
.end-node {
  display: flex;
  gap: 8px;
  align-items: center;
  justify-content: center;
  font-size: 14px;
  color: #606266;
}

.start-dot,
.end-dot {
  display: inline-block;
  width: 16px;
  height: 16px;
  background: #fff;
  border: 2px solid #c0c4cc;
  border-radius: 50%;
}

.connector {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.connector-line {
  width: 1px;
  height: 18px;
  background: #cacaca;
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
  width: 100%;
  overflow: hidden;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgb(20 40 80 / 8%);
  transition:
    box-shadow 0.2s ease,
    transform 0.2s ease;
}

.node-card:hover,
.node-card.is-active {
  box-shadow: 0 6px 20px rgb(50 150 250 / 18%);
}

.node-header {
  display: flex;
  gap: 8px;
  align-items: center;
  padding: 10px 40px 10px 14px;
  color: #fff;
}

.hd-approval {
  background: #f2994a;
}

.hd-cc {
  background: #2f80ed;
}

.hd-condition {
  background: #27ae60;
}

.node-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 22px;
  height: 22px;
  font-size: 12px;
  font-weight: 700;
  background: rgb(255 255 255 / 22%);
  border-radius: 50%;
}

.node-title {
  flex: 1;
  overflow: hidden;
  text-overflow: ellipsis;
  font-size: 14px;
  font-weight: 600;
  white-space: nowrap;
}

.node-tools {
  opacity: 0.85;
}

.node-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 14px;
  font-size: 13px;
  color: #606266;
  cursor: pointer;
}

.node-summary {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.node-arrow {
  color: #c0c4cc;
}

.node-delete {
  position: absolute;
  top: 8px;
  right: 8px;
  z-index: 2;
  width: 20px;
  height: 20px;
  font-size: 14px;
  line-height: 18px;
  color: rgb(255 255 255 / 85%);
  cursor: pointer;
  background: transparent;
  border: none;
}

.node-delete:hover {
  color: #fff;
}

.condition-gateway {
  width: 100%;
  overflow: hidden;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgb(20 40 80 / 8%);
}

.gateway-header {
  position: relative;
  display: flex;
  gap: 8px;
  align-items: center;
  padding: 10px 40px 10px 14px;
  color: #fff;
  cursor: pointer;
  background: #27ae60;
}

.gateway-hint {
  margin-right: 10px;
  margin-left: auto;
  font-size: 12px;
  opacity: 0.85;
}

.branch-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 12px;
  padding: 14px;
}

.branch-column {
  background: #fafbfc;
  border: 1px solid #e4e7ed;
  border-radius: 8px;
}

.branch-title {
  display: flex;
  flex-direction: column;
  gap: 2px;
  padding: 8px 10px;
  font-size: 13px;
  font-weight: 600;
  color: #303133;
  border-bottom: 1px dashed #e4e7ed;
}

.branch-expression {
  font-size: 11px;
  font-weight: 400;
  color: #909399;
  word-break: break-all;
}

.branch-flow {
  display: flex;
  flex-direction: column;
  gap: 8px;
  min-height: 86px;
  padding: 10px;
}

.branch-node {
  position: relative;
  display: flex;
  gap: 6px;
  align-items: center;
  padding: 8px;
  cursor: pointer;
  background: #fff;
  border: 1px solid #ebeef5;
  border-radius: 6px;
}

.branch-node:hover {
  border-color: #3296fa;
}

.branch-node-name {
  flex: 1;
  overflow: hidden;
  text-overflow: ellipsis;
  font-size: 12px;
  white-space: nowrap;
}

.branch-node-delete {
  color: #c0c4cc;
  cursor: pointer;
  background: transparent;
  border: none;
}

.branch-add {
  padding: 6px;
  font-size: 12px;
  color: #3296fa;
  text-align: center;
  cursor: pointer;
  border: 1px dashed #b3d8ff;
  border-radius: 6px;
}

.branch-add:hover {
  background: #ecf5ff;
}

/* AntFlow-style canvas */
.flow-container {
  width: 720px;
}

.start-card {
  width: 260px;
  overflow: hidden;
  cursor: pointer;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgb(20 40 80 / 10%);
}

.start-card-header {
  padding: 7px 12px;
  font-size: 13px;
  font-weight: 600;
  color: #fff;
  background: #4b4f73;
}

.start-card-body {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px;
  font-size: 13px;
  color: #303133;
}

.condition-flow {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.condition-pill {
  padding: 4px 14px;
  margin-bottom: 18px;
  font-size: 12px;
  color: #606266;
  cursor: pointer;
  background: #fff;
  border: 1px solid #e4e7ed;
  border-radius: 14px;
  box-shadow: 0 2px 6px rgb(20 40 80 / 6%);
}

.condition-pill:hover {
  color: #1677ff;
  border-color: #b3d8ff;
}

.condition-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 28px;
  width: 100%;
}

.condition-column {
  display: flex;
  flex-direction: column;
  align-items: center;
  min-width: 0;
}

.condition-card {
  position: relative;
  width: 100%;
  padding: 10px 14px;
  cursor: pointer;
  background: #fff;
  border: 1px solid #ebeef5;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgb(20 40 80 / 7%);
  transition: border-color 0.2s ease;
}

.condition-card:hover,
.condition-card.is-active {
  border-color: #3296fa;
}

.condition-card-head {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-bottom: 8px;
  border-bottom: 1px solid #f2f3f5;
}

.condition-name {
  font-size: 13px;
  font-weight: 600;
  color: #303133;
}

.condition-priority {
  font-size: 12px;
  color: #909399;
}

.condition-body {
  min-height: 20px;
  padding-top: 9px;
  font-size: 13px;
  color: #303133;
  word-break: break-all;
}

.condition-delete {
  top: 6px;
  right: 6px;
  color: #c0c4cc;
  opacity: 0;
}

.condition-card:hover .condition-delete {
  opacity: 1;
}

.node-card {
  width: 360px;
}

.node-header {
  padding: 8px 14px;
}

.node-content {
  padding: 11px 14px;
}

.condition-grid {
  position: relative;
  padding-top: 16px;
}

.condition-grid::before {
  position: absolute;
  top: 0;
  left: 25%;
  width: 50%;
  height: 1px;
  content: '';
  background: #c9ced6;
}

.condition-column::before {
  position: absolute;
  top: -16px;
  left: 50%;
  width: 1px;
  height: 16px;
  content: '';
  background: #c9ced6;
}

.condition-stem {
  width: 1px;
  height: 14px;
  background: #c9ced6;
}

.condition-horizontal-line {
  position: absolute;
  top: 0;
  left: 25%;
  width: 50%;
  height: 1px;
  background: #c9ced6;
}

.condition-vertical-line {
  position: absolute;
  top: -16px;
  left: 50%;
  width: 1px;
  height: 16px;
  background: #c9ced6;
}

.condition-column {
  position: relative;
}

.end-card {
  width: 260px;
  overflow: hidden;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgb(20 40 80 / 8%);
}

.end-card-header {
  padding: 8px 12px;
  font-size: 13px;
  font-weight: 600;
  color: #fff;
  background: #8c8c8c;
}

.end-card-body {
  padding: 10px 12px;
  font-size: 12px;
  color: #606266;
}

.is-readonly .status-approved .start-card-header,
.is-readonly .status-approved .node-header,
.is-readonly .status-approved .end-card-header {
  background: #52c41a !important;
}

.is-readonly .status-rejected .start-card-header,
.is-readonly .status-rejected .node-header,
.is-readonly .status-rejected .end-card-header {
  background: #cf1322 !important;
}

.is-readonly .status-running .start-card-header,
.is-readonly .status-running .node-header,
.is-readonly .status-running .end-card-header {
  background: #1677ff !important;
}

.is-readonly .status-pending .start-card-header,
.is-readonly .status-pending .node-header,
.is-readonly .status-pending .end-card-header {
  background: #8c8c8c !important;
}

.is-readonly .condition-card.status-approved {
  border-color: #52c41a;
}

.is-readonly .condition-card.status-rejected {
  border-color: #cf1322;
}

.is-readonly .condition-card.status-running {
  border-color: #1677ff;
}

.status-chip {
  padding: 1px 7px;
  font-size: 11px;
  font-weight: 400;
  color: #fff;
  background: #8c8c8c;
  border-radius: 9px;
}

.status-approved .status-chip {
  background: #52c41a;
}

.status-rejected .status-chip {
  background: #cf1322;
}

.status-running .status-chip {
  background: #1677ff;
}

.node-title {
  margin-right: auto;
}

.connector-line {
  width: 2px;
  height: 24px;
  background: #c0c4cc;
}

.condition-stem {
  width: 2px;
  height: 20px;
}

.condition-horizontal-line {
  height: 2px;
}

.condition-vertical-line {
  width: 2px;
}

.condition-grid::after {
  position: absolute;
  bottom: 0;
  left: 25%;
  width: 50%;
  height: 2px;
  content: '';
  background: #c0c4cc;
}

.condition-column::after {
  position: absolute;
  bottom: 0;
  left: 50%;
  width: 2px;
  height: 18px;
  content: '';
  background: #c0c4cc;
}

.connector-line:last-child {
  position: relative;
}

.connector-line:last-child::after {
  position: absolute;
  bottom: -1px;
  left: 50%;
  width: 0;
  height: 0;
  content: '';
  border-top: 7px solid #c0c4cc;
  border-right: 5px solid transparent;
  border-left: 5px solid transparent;
  transform: translateX(-50%);
}

.condition-vertical-line {
  position: relative;
}

.condition-vertical-line::after {
  position: absolute;
  bottom: -1px;
  left: 50%;
  width: 0;
  height: 0;
  content: '';
  border-top: 7px solid #c0c4cc;
  border-right: 5px solid transparent;
  border-left: 5px solid transparent;
  transform: translateX(-50%);
}

.condition-column::after {
  position: absolute;
} /* ===== AntFlow canvas ===== */
.flow-container {
  width: fit-content;
  min-width: 720px;
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

.start-card {
  width: 260px;
}

.start-card-header {
  background: #5b698d;
}

.start-card-body {
  min-height: 48px;
  padding: 3px 6px 10px 12px;
}

.node-card {
  width: 260px;
  border-radius: 8px;
  box-shadow: 0 4px 16px rgb(180 180 180 / 20%);
}

.node-header {
  padding: 6px 12px;
  font-size: 14px;
  background: #5b698d;
}

.hd-approval {
  background: #ff943e;
}

.hd-cc {
  background: #3296fa;
}

.hd-condition {
  color: #27ae60;
  background: rgb(39 174 96 / 12%);
}

.node-content {
  min-height: 48px;
  padding: 3px 6px 10px;
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

.condition-flow {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  width: fit-content;
}

.condition-pill {
  position: absolute;
  top: -10px;
  z-index: 2;
  padding: 2px 10px;
  margin: 0;
  font-size: 12px;
  color: #3296fa;
  background: #fff;
  border: none;
  border-radius: 20px;
  box-shadow: 0 2px 6px rgb(0 0 0 / 10%);
}

.condition-pill:hover {
  color: #fff;
  background: #3296fa;
}

.condition-branches {
  position: relative;
  display: flex;
  align-items: stretch;
  justify-content: center;
  width: fit-content;
  max-width: 100%;
  padding: 20px 0;
  margin: 0 20px;
}

.condition-branches::before,
.condition-branches::after {
  position: absolute;
  left: 50%;
  width: calc(100% - 299px);
  height: 1px;
  content: '';
  background: #cacaca;
  transform: translateX(-50%);
}

.condition-branches::before {
  top: 0;
}

.condition-branches::after {
  bottom: 0;
}

.condition-column {
  position: relative;
  display: flex;
  flex: 0 0 260px;
  flex-direction: column;
  align-items: center;
  align-self: stretch;
  margin: 0 20px;
}

.condition-column::before,
.condition-column::after {
  position: absolute;
  left: 50%;
  width: 1px;
  height: 20px;
  content: '';
  background: #cacaca;
  transform: translateX(-50%);
}

.condition-column::before {
  top: -20px;
}

.condition-column::after {
  bottom: -20px;
}

.condition-column .node-card {
  width: 260px;
}

.condition-column .node-header {
  padding-right: 52px;
}

.condition-column .node-content {
  cursor: pointer;
}

.branch-empty-space {
  flex: 1 1 auto;
  width: 1px;
  min-height: 24px;
  background: #cacaca !important;
}

.top-left-cover-line,
.top-right-cover-line,
.bottom-left-cover-line,
.bottom-right-cover-line {
  position: absolute;
  width: 50%;
  height: 8px;
  background: #f5f6f8;
}

.top-left-cover-line,
.top-right-cover-line {
  top: -22px;
}

.bottom-left-cover-line,
.bottom-right-cover-line {
  bottom: -22px;
}

.top-left-cover-line,
.bottom-left-cover-line {
  left: -1px;
}

.top-right-cover-line,
.bottom-right-cover-line {
  right: -1px;
}

.end-node {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 5px;
}

.end-circle {
  width: 18px;
  height: 18px;
  margin-bottom: 10px;
  background: #dbdbdb;
  border-radius: 50%;
}

.end-text {
  font-size: 14px;
  color: #999;
}

.status-approved .end-circle {
  background: #52c41a;
}

.status-rejected .end-circle {
  background: #cf1322;
}

.status-running .end-circle {
  background: #1677ff;
}

.status-pending .end-circle {
  background: #8c8c8c;
}

.condition-column {
  flex: 0 0 auto;
  min-width: 260px;
}

.condition-branches {
  max-width: none;
}

.condition-branches::before,
.condition-branches::after {
  left: 0;
  width: 100%;
  transform: none;
}

.condition-column {
  padding: 20px 0;
}

.condition-column::before,
.condition-column::after {
  display: none;
}

.condition-branches {
  padding: 0;
}

.condition-top-line {
  flex: none;
  width: 1px;
  height: 20px;
  background: #cacaca;
}

.condition-bottom-line {
  flex: 1 1 auto;
  width: 1px;
  min-height: 20px;
  background: #cacaca;
}

.top-left-cover-line,
.top-right-cover-line {
  top: -4px;
}

.bottom-left-cover-line,
.bottom-right-cover-line {
  bottom: -4px;
} /* AntFlow branch connectors (fixed 260 columns) */
.condition-branches {
  padding: 20px 0;
}

.condition-branches::before,
.condition-branches::after {
  left: 50%;
  width: calc(100% - 299px);
  transform: translateX(-50%);
}

.condition-column {
  flex: 0 0 260px;
  min-width: 260px;
  padding: 0;
}

.condition-column::before,
.condition-column::after {
  display: block;
  width: 1px;
  height: 20px;
}

.condition-column::before {
  top: -20px;
}

.condition-column::after {
  bottom: -20px;
}

.top-left-cover-line,
.top-right-cover-line {
  top: -22px;
}

.bottom-left-cover-line,
.bottom-right-cover-line {
  bottom: -22px;
}

.node-card {
  cursor: grab;
}

.node-card.is-dragging {
  cursor: grabbing;
  opacity: 0.45;
}

.flow-container {
  position: relative;
}
</style>
