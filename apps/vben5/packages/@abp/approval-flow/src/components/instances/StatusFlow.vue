<script setup lang="ts">
import type {
  ApprovalFlowNode,
  ApprovalNodeTaskDto,
  ApprovalRecordDto,
} from '../../types/approval-flow';

import { $t } from '@vben/locales';

import { Tag } from 'ant-design-vue';

const props = defineProps<{
  nodes: ApprovalFlowNode[];
  nodeTasks: ApprovalNodeTaskDto[];
  records: ApprovalRecordDto[];
  statusMap: Record<string, string>;
}>();

function statusClass(node: ApprovalFlowNode): string {
  return `status-${props.statusMap[node.id] ?? 'pending'}`;
}

function tasksOf(nodeId: string): ApprovalNodeTaskDto[] {
  return props.nodeTasks.filter((task) => task.nodeId === nodeId);
}

function recordsOf(nodeId: string): ApprovalRecordDto[] {
  return props.records.filter((record) => record.nodeId === nodeId);
}

function approverNames(nodeId: string): string {
  return tasksOf(nodeId)
    .map((task) => task.approverDisplayName)
    .filter(Boolean)
    .join('、');
}

function latestRecord(nodeId: string): ApprovalRecordDto | null {
  const records = recordsOf(nodeId);
  return records.length > 0 ? (records[records.length - 1] ?? null) : null;
}

function actionText(actionKey?: null | string): string {
  if (!actionKey) {
    return '-';
  }
  return $t(`ElsaNextApprovalFlow.ApprovalRecordAction:${actionKey}`);
}

function statusText(node: ApprovalFlowNode): string {
  switch (props.statusMap[node.id]) {
    case 'approved': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Approved');
    }
    case 'rejected': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Rejected');
    }
    case 'running': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:Running');
    }
    default: {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Status:NotExecuted');
    }
  }
}

function onChildAdd() {
  // Read-only view.
}

function onChildRemove() {
  // Read-only view.
}

function onChildSelect() {
  // Read-only view.
}
</script>

<template>
  <div class="status-flow">
    <template v-for="node in nodes" :key="node.id">
      <div
        v-if="node.kind === 'condition'"
        class="status-gateway"
        :class="statusClass(node)"
      >
        <div class="gateway-header">
          <span class="gateway-title">{{ node.name }}</span>
          <Tag class="gateway-tag">{{ statusText(node) }}</Tag>
        </div>
        <div class="gateway-branches">
          <div
            v-for="branch in node.branches ?? []"
            :key="branch.id"
            class="gateway-branch"
          >
            <div class="gateway-branch-title">
              {{ branch.name }}
              <span v-if="branch.expression" class="gateway-expression">
                {{ branch.expression }}
              </span>
            </div>
            <StatusFlow
              :node-tasks="nodeTasks"
              :nodes="branch.nodes"
              :records="records"
              :status-map="statusMap"
              @add="onChildAdd"
              @remove="onChildRemove"
              @select="onChildSelect"
            />
          </div>
        </div>
      </div>

      <div v-else class="status-card" :class="statusClass(node)">
        <div class="status-card-header">
          <span class="status-icon">{{
            node.kind === 'approval'
              ? $t('ElsaNextApprovalFlow.ApprovalFlow:StatusFlow:ApprovalBadge')
              : $t('ElsaNextApprovalFlow.ApprovalFlow:StatusFlow:CopyBadge')
          }}</span>
          <span class="status-name">{{ node.name }}</span>
          <Tag class="status-tag">{{ statusText(node) }}</Tag>
        </div>
        <div class="status-card-body">
          <div v-if="approverNames(node.id)" class="status-line">
            {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Approver') }}：{{
              approverNames(node.id)
            }}
          </div>
          <div v-if="latestRecord(node.id)" class="status-line">
            {{ actionText(latestRecord(node.id)?.actionKey) }}
            <span v-if="latestRecord(node.id)?.comment">
              · {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Comment') }}：{{
                latestRecord(node.id)?.comment
              }}
            </span>
            <span v-if="latestRecord(node.id)?.occurredAt" class="status-time">
              {{ new Date(latestRecord(node.id)!.occurredAt).toLocaleString() }}
            </span>
          </div>
          <div v-if="node.kind === 'cc'" class="status-line status-muted">
            {{ $t('ElsaNextApprovalFlow.ApprovalFlow:StatusFlow:CopyNode') }}
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<style scoped>
.status-flow {
  display: flex;
  flex-direction: column;
  gap: 8px;
  align-items: center;
  width: 100%;
}

.status-card {
  width: 100%;
  max-width: 560px;
  overflow: hidden;
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
}

.status-card-header {
  display: flex;
  gap: 8px;
  align-items: center;
  padding: 8px 12px;
  color: #fff;
}

.status-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 20px;
  height: 20px;
  font-size: 11px;
  background: rgb(255 255 255 / 22%);
  border-radius: 50%;
}

.status-name {
  flex: 1;
  font-size: 13px;
  font-weight: 600;
}

.status-tag,
.gateway-tag {
  margin-inline-end: 0;
  color: #fff;
  background: rgb(255 255 255 / 20%);
  border: none;
}

.status-card-body {
  padding: 8px 12px;
  font-size: 12px;
  color: #606266;
}

.status-line + .status-line {
  margin-top: 4px;
}

.status-time {
  margin-left: 6px;
  color: #909399;
}

.status-muted {
  color: #909399;
}

.status-approved .status-card-header,
.status-approved .gateway-header {
  background: #52c41a;
}

.status-rejected .status-card-header,
.status-rejected .gateway-header {
  background: #cf1322;
}

.status-running .status-card-header,
.status-running .gateway-header {
  background: #1677ff;
}

.status-pending .status-card-header,
.status-pending .gateway-header {
  background: #8c8c8c;
}

.status-gateway {
  width: 100%;
  max-width: 720px;
  overflow: hidden;
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
}

.gateway-header {
  display: flex;
  gap: 8px;
  align-items: center;
  padding: 8px 12px;
  color: #fff;
}

.gateway-title {
  flex: 1;
  font-size: 13px;
  font-weight: 600;
}

.gateway-branches {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 10px;
  padding: 10px;
}

.gateway-branch {
  padding: 8px;
  background: #fafbfc;
  border: 1px solid #e4e7ed;
  border-radius: 8px;
}

.gateway-branch-title {
  display: flex;
  flex-direction: column;
  gap: 2px;
  padding-bottom: 6px;
  margin-bottom: 6px;
  font-size: 12px;
  font-weight: 600;
  color: #303133;
  border-bottom: 1px dashed #e4e7ed;
}

.gateway-expression {
  font-size: 11px;
  font-weight: 400;
  color: #909399;
  word-break: break-all;
}
</style>
