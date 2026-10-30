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
  'approval' | 'cc' | 'condition' | 'end' | 'start';

export interface ParticipantRef {
  displayName?: null | string;
  id: string;
  provider: string;
  type: 'Group' | 'User';
}

export interface ApproverRule {
  formFieldKey?: null | string;
  kind: ApproverKind;
  level?: number;
  participants: ParticipantRef[];
  supervisorLevel?: SupervisorLevel;
}

export interface ApprovalAction {
  effect: ApprovalActionEffect;
  key: string;
  label: string;
}

export interface ApprovalFlowCondition {
  compareValue?: null | string;
  expression?: null | string;
  fieldKey?: null | string;
  operator?: null | string;
}

export interface ApprovalFlowBranch {
  compareValue?: null | string;
  conditions?: ApprovalFlowCondition[];
  expression?: null | string;
  fieldKey?: null | string;
  id: string;
  logic?: 'and' | 'or';
  name: string;
  nodes: ApprovalFlowNode[];
  operator?: null | string;
  position?: { x: number; y: number };
}

export interface ApprovalFlowNode {
  actions: ApprovalAction[];
  branches?: ApprovalFlowBranch[];
  condition?: ApprovalFlowCondition;
  conditionExpression?: null | string;
  expression?: null | string;
  id: string;
  kind: ApprovalFlowNodeKind;
  name: string;
  onEmpty: EmptyApproverPolicy;
  position?: { x: number; y: number };
  rules: ApproverRule[];
  signMode: ApprovalSignMode;
}

export interface ApprovalFlowModel {
  formDefinitionId?: null | string;
  nodes: ApprovalFlowNode[];
  version: number;
}

export interface ApprovalTemplateDto {
  businessKeyHint?: null | string;
  category?: null | string;
  description?: null | string;
  displayName: string;
  elsaDefinitionId?: null | string;
  elsaDefinitionKey?: null | string;
  elsaDefinitionVersion?: null | number;
  flowDefinitionJson?: null | string;
  formDefinitionId?: null | string;
  icon?: null | string;
  id: string;
  isEnabled: boolean;
  isPublished: boolean;
  name: string;
}

export interface CreateApprovalTemplateDto {
  businessKeyHint?: null | string;
  category?: null | string;
  description?: null | string;
  displayName: string;
  icon?: null | string;
  name: string;
}

export interface UpdateApprovalTemplateDto {
  businessKeyHint?: null | string;
  category?: null | string;
  description?: null | string;
  displayName: string;
  icon?: null | string;
}

export interface PublishApprovalTemplateDto {
  formDefinitionId?: null | string;
}

export interface SaveApprovalDesignDto {
  flowDefinitionJson: string;
  formDefinitionId?: null | string;
}

export interface ApprovalInstanceDto {
  businessKey?: null | string;
  currentNodeId?: null | string;
  currentNodeName?: null | string;
  elsaWorkflowInstanceId?: null | string;
  finishedAt?: null | string;
  formDataJson?: null | string;
  id: string;
  initiatorDisplayName?: null | string;
  initiatorId?: null | string;
  outcome?: null | string;
  startedAt: string;
  status: string;
  templateCategory?: null | string;
  templateId: string;
  templateName: string;
  title: string;
}

export interface StartApprovalDto {
  businessKey?: null | string;
  formDataJson?: null | string;
  templateId: string;
  title: string;
}

export interface GetApprovalInstancesDto {
  filter?: null | string;
  maxResultCount?: number;
  onlyMine?: boolean;
  skipCount?: number;
  status?: null | string;
  templateId?: null | string;
}

export interface ApprovalPendingTaskDto {
  allowedActions: string[];
  createdAt: string;
  dueAt?: null | string;
  instanceId: string;
  instanceTitle: string;
  nodeId?: null | string;
  nodeName?: null | string;
  priority: number;
  revision: number;
  summary?: null | string;
  taskId: string;
  templateName?: null | string;
  title: string;
}

export interface ApprovalDecisionDto {
  actionKey: string;
  comment?: null | string;
  dataJson?: null | string;
  operationId?: null | string;
  taskId: string;
}

export interface ApprovalParticipantDto {
  displayName?: null | string;
  id: string;
  provider?: null | string;
  type?: 'Group' | 'User';
}

export interface ReturnApprovalDto {
  comment?: null | string;
  taskId?: null | string;
}

export interface TransferApprovalDto {
  comment?: null | string;
  taskId?: null | string;
  transferee: ApprovalParticipantDto;
}

export interface AddApprovalDto {
  approver: ApprovalParticipantDto;
  comment?: null | string;
  taskId?: null | string;
  title?: null | string;
}
export interface ApprovalDecisionResultDto {
  ignored: boolean;
  recordId?: null | string;
}

export interface ApprovalRecordDto {
  actionKey: string;
  actionLabel?: null | string;
  actorDisplayName?: null | string;
  comment?: null | string;
  completedAt?: null | string;
  effect: string;
  firstViewedAt?: null | string;
  id: string;
  instanceId: string;
  nodeId?: null | string;
  nodeName?: null | string;
  occurredAt: string;
  taskId?: null | string;
}

export interface ApprovalNodeTaskDto {
  approverDisplayName?: null | string;
  nodeId?: null | string;
  nodeName?: null | string;
  signMode: string;
  status: string;
  taskId: string;
}

export interface ApprovalInstanceDetailDto {
  flowDefinitionJson?: null | string;
  formDefinitionId?: null | string;
  instance: ApprovalInstanceDto;
  nodeTasks: ApprovalNodeTaskDto[];
  records: ApprovalRecordDto[];
}

export interface FormFieldDto {
  hint?: null | string;
  id: string;
  isRequired: boolean;
  key: string;
  label: string;
  optionsJson?: null | string;
  order: number;
  permission: string;
  type: string;
}

export interface FormDefinitionDto {
  code: string;
  description?: null | string;
  fields: FormFieldDto[];
  id: string;
  isPublished: boolean;
  name: string;
  publishedTime?: null | string;
  version: number;
}

export type FormDefinitionPagedResult = PagedResultDto<FormDefinitionDto>;
