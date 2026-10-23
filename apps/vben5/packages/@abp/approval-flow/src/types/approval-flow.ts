import type { PagedResultDto } from '@abp/core';

export type ApprovalSignMode = 'All' | 'Any' | 'Sequential';
export type ApproverKind =
  | 'FormField'
  | 'Initiator'
  | 'InitiatorSelection'
  | 'OrganizationUnit'
  | 'Participant'
  | 'Role'
  | 'Supervisor';
export type SupervisorLevel = 'LevelN' | 'Nearest' | 'TopMost';
export type EmptyApproverPolicy = 'EscalateToManager' | 'Skip';
export type ApprovalActionEffect = 'Approve' | 'Neutral' | 'Reject';
export type ApprovalFlowNodeKind =
  | 'approval'
  | 'cc'
  | 'condition'
  | 'end'
  | 'start';

export interface ParticipantRef {
  provider: string;
  type: 'Group' | 'User';
  id: string;
  displayName?: null | string;
}

export interface ApproverRule {
  kind: ApproverKind;
  participants: ParticipantRef[];
  level?: number;
  supervisorLevel?: SupervisorLevel;
  formFieldKey?: null | string;
}

export interface ApprovalAction {
  key: string;
  label: string;
  effect: ApprovalActionEffect;
}

export interface ApprovalFlowCondition {
  fieldKey?: null | string;
  operator?: null | string;
  compareValue?: null | string;
  expression?: null | string;
}

export interface ApprovalFlowBranch {
  id: string;
  name: string;
  fieldKey?: null | string;
  operator?: null | string;
  compareValue?: null | string;
  expression?: null | string;
  conditions?: ApprovalFlowCondition[];
  logic?: 'and' | 'or';
  position?: { x: number; y: number };
  nodes: ApprovalFlowNode[];
}

export interface ApprovalFlowNode {
  id: string;
  kind: ApprovalFlowNodeKind;
  name: string;
  signMode: ApprovalSignMode;
  rules: ApproverRule[];
  onEmpty: EmptyApproverPolicy;
  actions: ApprovalAction[];
  conditionExpression?: null | string;
  condition?: ApprovalFlowCondition;
  branches?: ApprovalFlowBranch[];
  position?: { x: number; y: number };
  expression?: null | string;
}

export interface ApprovalFlowModel {
  version: number;
  formDefinitionId?: null | string;
  nodes: ApprovalFlowNode[];
}

export interface ApprovalTemplateDto {
  id: string;
  name: string;
  displayName: string;
  description?: null | string;
  category?: null | string;
  icon?: null | string;
  businessKeyHint?: null | string;
  isPublished: boolean;
  isEnabled: boolean;
  elsaDefinitionId?: null | string;
  elsaDefinitionKey?: null | string;
  elsaDefinitionVersion?: null | number;
  formDefinitionId?: null | string;
  flowDefinitionJson?: null | string;
}

export interface CreateApprovalTemplateDto {
  name: string;
  displayName: string;
  description?: null | string;
  category?: null | string;
  icon?: null | string;
  businessKeyHint?: null | string;
}

export interface UpdateApprovalTemplateDto {
  displayName: string;
  description?: null | string;
  category?: null | string;
  icon?: null | string;
  businessKeyHint?: null | string;
}

export interface PublishApprovalTemplateDto {
  formDefinitionId?: null | string;
}

export interface SaveApprovalDesignDto {
  flowDefinitionJson: string;
  formDefinitionId?: null | string;
}

export interface ApprovalInstanceDto {
  id: string;
  templateId: string;
  templateName: string;
  templateCategory?: null | string;
  businessKey?: null | string;
  title: string;
  status: string;
  currentNodeId?: null | string;
  currentNodeName?: null | string;
  initiatorId?: null | string;
  initiatorDisplayName?: null | string;
  elsaWorkflowInstanceId?: null | string;
  formDataJson?: null | string;
  outcome?: null | string;
  startedAt: string;
  finishedAt?: null | string;
}

export interface StartApprovalDto {
  templateId: string;
  title: string;
  businessKey?: null | string;
  formDataJson?: null | string;
}

export interface GetApprovalInstancesDto {
  templateId?: null | string;
  status?: null | string;
  filter?: null | string;
  onlyMine?: boolean;
  maxResultCount?: number;
  skipCount?: number;
}

export interface ApprovalPendingTaskDto {
  taskId: string;
  instanceId: string;
  instanceTitle: string;
  templateName?: null | string;
  nodeId?: null | string;
  nodeName?: null | string;
  title: string;
  summary?: null | string;
  priority: number;
  dueAt?: null | string;
  createdAt: string;
  revision: number;
  allowedActions: string[];
}

export interface ApprovalDecisionDto {
  taskId: string;
  actionKey: string;
  comment?: null | string;
  dataJson?: null | string;
  operationId?: null | string;
}

export interface ApprovalParticipantDto {
  id: string;
  displayName?: null | string;
  provider?: null | string;
  type?: 'Group' | 'User';
}

export interface ReturnApprovalDto {
  taskId?: null | string;
  comment?: null | string;
}

export interface TransferApprovalDto {
  taskId?: null | string;
  transferee: ApprovalParticipantDto;
  comment?: null | string;
}

export interface AddApprovalDto {
  taskId?: null | string;
  approver: ApprovalParticipantDto;
  title?: null | string;
  comment?: null | string;
}
export interface ApprovalDecisionResultDto {
  recordId?: null | string;
  ignored: boolean;
}

export interface ApprovalRecordDto {
  id: string;
  instanceId: string;
  nodeId?: null | string;
  nodeName?: null | string;
  taskId?: null | string;
  actionKey: string;
  actionLabel?: null | string;
  effect: string;
  comment?: null | string;
  actorDisplayName?: null | string;
  occurredAt: string;
  firstViewedAt?: null | string;
  completedAt?: null | string;
}

export interface ApprovalNodeTaskDto {
  nodeId?: null | string;
  nodeName?: null | string;
  taskId: string;
  status: string;
  approverDisplayName?: null | string;
  signMode: string;
}

export interface ApprovalInstanceDetailDto {
  instance: ApprovalInstanceDto;
  records: ApprovalRecordDto[];
  nodeTasks: ApprovalNodeTaskDto[];
  flowDefinitionJson?: null | string;
  formDefinitionId?: null | string;
}

export interface FormFieldDto {
  id: string;
  key: string;
  label: string;
  type: string;
  permission: string;
  order: number;
  isRequired: boolean;
  hint?: null | string;
  optionsJson?: null | string;
}

export interface FormDefinitionDto {
  id: string;
  name: string;
  code: string;
  version: number;
  description?: null | string;
  isPublished: boolean;
  publishedTime?: null | string;
  fields: FormFieldDto[];
}

export type FormDefinitionPagedResult = PagedResultDto<FormDefinitionDto>;
