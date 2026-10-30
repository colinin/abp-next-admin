import type {
  ApprovalAction,
  ApprovalFlowBranch,
  ApprovalFlowCondition,
  ApprovalFlowModel,
  ApprovalFlowNode,
  ApprovalSignMode,
  ApproverKind,
  ApproverRule,
  ParticipantRef,
} from '../../types/approval-flow';
/* eslint-disable prettier/prettier */
import type {
  AntFlowApproveNodeProperty,
  AntFlowAssignee,
  AntFlowConditionGroup,
  AntFlowConditionItem,
  AntFlowConditionNodeProperty,
  AntFlowNode,
} from './types';

import { $t } from '@vben/locales';

import {
  createBranch,
  createId,
  createNode,
  getDefaultApproveRejectActions,
  parseModel,
} from '../designer/model';
import { NodeType } from './types';

let idSeed = 0;

/**
 * Generates a short, URL-safe id similar to AntFlow's `NodeUtils.idGenerator`.
 * A counter is mixed in so ids created in the same millisecond stay unique.
 */
export function idGenerator(): string {
  idSeed += 1;
  let quotient =
    Date.now() -
    Date.UTC(2024, 4, 1) +
    idSeed * 97 +
    Math.ceil(Math.random() * 1000);
  const chars =
    '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
  const charArray = [...chars];
  const radix = charArray.length;
  const result: string[] = [];

  do {
    const mod = quotient % radix;
    quotient = (quotient - mod) / radix;
    const char = charArray[mod];
    result.push(char ?? '0');
  } while (quotient > 0);

  return result.join('').toUpperCase();
}

interface CreateBaseNodeOptions {
  childNode?: AntFlowNode | null;
  error?: boolean;
  nodeDisplayName?: string;
  nodeFrom?: string;
  nodeName: string;
  nodeTo?: string[];
  nodeType: number;
  nodeWeight?: number;
}

function createBaseNode(options: CreateBaseNodeOptions): AntFlowNode {
  return {
    nodeId: idGenerator(),
    nodeName: options.nodeName,
    nodeDisplayName: options.nodeDisplayName ?? options.nodeName,
    nodeType: options.nodeType,
    nodeFrom: options.nodeFrom ?? '',
    nodeTo: options.nodeTo ?? [],
    nodeWeight: options.nodeWeight ?? 0,
    childNode: options.childNode ?? null,
    error: options.error ?? false,
  };
}

function createButtons(
  startPage: number[] = [],
  approvalPage: number[] = [],
  viewPage: number[] = [],
) {
  return { startPage, approvalPage, viewPage };
}

export function createApproveProperty(): AntFlowApproveNodeProperty {
  return {
    assigneeType: 1,
    signType: 1,
    noHeaderAction: 1,
    directorLevel: 0,
    IsDistinct: 0,
    sort: 0,
    assigneeList: [],
    fieldPrems: [],
    buttons: createButtons([1], [3, 4], [0]),
  };
}

export function createCopyProperty(): AntFlowApproveNodeProperty {
  return {
    assigneeType: 1,
    signType: 1,
    noHeaderAction: 0,
    directorLevel: 0,
    ccFlag: 0,
    IsDistinct: 0,
    sort: 0,
    assigneeList: [],
    fieldPrems: [],
    buttons: createButtons(),
  };
}

export function createConditionProperty(
  isDefault: number,
): AntFlowConditionNodeProperty {
  return {
    groupRelation: false,
    isDefault,
    sort: 0,
    groupConditions: [
      {
        groupId: idGenerator(),
        condRelation: true,
        sort: 0,
        conditionList: [],
      },
    ],
  };
}

/** Start node.  `child` is linked to keep the AntFlow single-linked structure. */
export function createStartNode(child: AntFlowNode | null = null): AntFlowNode {
  return createBaseNode({
    nodeName: $t('antflow.model.start'),
    nodeType: NodeType.StartNode,
    nodeWeight: 0,
    childNode: child,
    error: false,
  });
}

/** Approval node. */
export function createApproveNode(
  child: AntFlowNode | null = null,
): AntFlowNode {
  const node = createBaseNode({
    nodeName: $t('antflow.model.reviewer'),
    nodeType: NodeType.ApproveNode,
    childNode: child,
    error: true,
  });
  node.nodeProperty = createApproveProperty();
  return node;
}

/** Copy (CC) node. */
export function createCopyNode(child: AntFlowNode | null = null): AntFlowNode {
  const node = createBaseNode({
    nodeName: $t('antflow.model.cc'),
    nodeType: NodeType.CopyNode,
    childNode: child,
    error: true,
  });
  node.nodeProperty = createCopyProperty();
  return node;
}

/** Condition branch node used inside a gateway. */
export function createConditionNode(
  name: string,
  child: AntFlowNode | null = null,
  weight = 0,
  isDefault = 0,
): AntFlowNode {
  const node = createBaseNode({
    nodeName: name || $t('antflow.model.conditionName', ['1']),
    nodeDisplayName: name || $t('antflow.model.conditionName', ['1']),
    nodeType: NodeType.ConditionNode,
    nodeWeight: weight,
    childNode: child,
    error: isDefault !== 1,
  });
  node.nodeProperty = createConditionProperty(isDefault);
  return node;
}

/**
 * Conditional gateway.
 *
 * Mirrors AntFlow: the supplied `child` becomes the first branch's child, while
 * the gateway's own `childNode` starts empty and is filled by the merge-line
 * add button (or by `oldToAntFlow`, which knows the real continuation).
 */
export function createGatewayNode(
  child: AntFlowNode | null = null,
): AntFlowNode {
  const node = createBaseNode({
    nodeName: $t('antflow.model.gateway'),
    nodeType: NodeType.GatewayNode,
    nodeWeight: 1,
    childNode: null,
    error: false,
  });
  node.nodeProperty = {
    groupRelation: false,
    isDefault: 0,
    sort: 0,
    conditionNodes: [
      createConditionNode($t('antflow.model.conditionName', ['1']), child, 1, 0),
      createConditionNode($t('antflow.model.defaultBranch'), null, 2, 1),
    ],
  };
  return node;
}

/**
 * AntFlow keeps non-default branches first and default branches last.  The
 * weight is used as the displayed priority.
 */
export function resetNodeWeights(nodes: AntFlowNode[]): AntFlowNode[] {
  const validNodes = nodes.filter(Boolean);
  const normalNodes = validNodes.filter(
    (item) => item.nodeProperty?.isDefault !== 1,
  );
  const defaultNodes = validNodes.filter(
    (item) => item.nodeProperty?.isDefault === 1,
  );
  return [...normalNodes, ...defaultNodes].map((item, index) => ({
    ...item,
    nodeWeight: index + 1,
  }));
}

/* -------------------------------------------------------------------------- */
/* Old model -> AntFlow model                                                  */
/* -------------------------------------------------------------------------- */

function assigneeTypeFromRules(rules: ApproverRule[]): number {
  const rule = rules[0];
  if (!rule) {
    return 1;
  }
  switch (rule.kind) {
    case 'FormField':
    case 'Participant': {
      return 1;
    }
    case 'Initiator': {
      return 5;
    }
    case 'InitiatorSelection': {
      return 8;
    }
    case 'OrganizationUnit': {
      return 4;
    }
    case 'Role': {
      return 2;
    }
    case 'Supervisor': {
      return rule.supervisorLevel === 'TopMost' ? 6 : 3;
    }
    default: {
      return 1;
    }
  }
}

function signTypeFromSignMode(mode: ApprovalSignMode | undefined): number {
  switch (mode) {
    case 'All': {
      return 2;
    }
    case 'Sequential': {
      return 3;
    }
    default: {
      return 1;
    }
  }
}

function collectAssignees(rules: ApproverRule[]): AntFlowAssignee[] {
  const result: AntFlowAssignee[] = [];
  const seen = new Set<string>();

  for (const rule of rules) {
    for (const participant of rule.participants ?? []) {
      const type = participant.type === 'Group' ? 2 : 1;
      const key = `${participant.provider}:${type}:${participant.id}`;
      if (seen.has(key)) {
        continue;
      }
      seen.add(key);
      result.push({
        id: participant.id,
        name: participant.displayName ?? participant.id,
        displayName: participant.displayName ?? participant.id,
        type,
        provider: participant.provider,
      });
    }
  }

  return result;
}

function isApprovePropertyConfigured(
  property: AntFlowApproveNodeProperty,
): boolean {
  if (
    property.ruleKind === 'FormField' ||
    property.ruleKind === 'Initiator' ||
    property.ruleKind === 'InitiatorSelection' ||
    property.ruleKind === 'OrganizationUnit' ||
    property.ruleKind === 'Supervisor'
  ) {
    return true;
  }

  const assigneeType = Number(property.assigneeType ?? 1);
  if (assigneeType === 1 || assigneeType === 2) {
    return (
      Array.isArray(property.assigneeList) && property.assigneeList.length > 0
    );
  }
  return true;
}

function buildApproveDisplayName(
  property: AntFlowApproveNodeProperty,
  kind: 'approval' | 'cc',
): string {
  const assigneeType = Number(property.assigneeType ?? 1);
  const signType = Number(property.signType ?? 1);
  const names = (property.assigneeList ?? [])
    .map((item) => item.displayName || item.name)
    .filter(Boolean);

  if (names.length > 0 && (assigneeType === 1 || assigneeType === 2)) {
    if (kind === 'approval' && signType === 2) {
      return $t('antflow.model.signSummary', [String(names.length), names.join(',')]);
    }
    if (kind === 'approval' && signType === 3) {
      return $t('antflow.model.sequentialSignSummary', [String(names.length), names.join(',')]);
    }
    return names.join('、');
  }

  if (property.ruleKind === 'FormField') {
    return $t('antflow.model.formField', [property.formFieldKey || '-']);
  }
  if (property.ruleKind === 'OrganizationUnit') {
    return $t('antflow.model.departmentMember');
  }
  if (assigneeType === 3) {
    return $t('antflow.model.directLeader');
  }
  if (assigneeType === 5) {
    return $t('antflow.model.initiator');
  }
  if (assigneeType === 6) {
    return $t('antflow.model.levelSupervisor', [String(property.directorLevel || 0)]);
  }
  if (assigneeType === 7) {
    return $t('antflow.model.chainApproval');
  }
  if (assigneeType === 8) {
    return $t('antflow.model.selfSelect');
  }

  return kind === 'cc' ? $t('antflow.model.pendingCc') : $t('antflow.model.pendingApprover');
}

function toConditionItem(source: ApprovalFlowCondition): AntFlowConditionItem {
  const fieldKey = source.fieldKey ?? '';
  return {
    key: fieldKey,
    label: fieldKey,
    type: '',
    optType: source.operator ?? '=',
    zdy1: source.compareValue ?? '',
    opt1: '<',
    zdy2: '',
    opt2: '<',
    columnDbName: fieldKey,
    columnType: '',
    expression: source.expression ?? null,
  };
}

function branchToGroups(branch: ApprovalFlowBranch): AntFlowConditionGroup[] {
  const conditions = branch.conditions ?? [];
  if (conditions.length > 0) {
    return [
      {
        groupId: idGenerator(),
        condRelation: branch.logic !== 'or',
        sort: 0,
        conditionList: conditions.map((condition) =>
          toConditionItem(condition),
        ),
      },
    ];
  }

  if (branch.fieldKey || branch.compareValue || branch.expression) {
    return [
      {
        groupId: idGenerator(),
        condRelation: branch.logic !== 'or',
        sort: 0,
        conditionList: [toConditionItem(branch)],
      },
    ];
  }

  return [
    {
      groupId: idGenerator(),
      condRelation: true,
      sort: 0,
      conditionList: [],
    },
  ];
}

function hasBranchCondition(branch: ApprovalFlowBranch): boolean {
  return Boolean(
    (branch.conditions && branch.conditions.length > 0) ||
    branch.fieldKey ||
    branch.compareValue ||
    branch.expression,
  );
}

function conditionBranchesOf(node: ApprovalFlowNode): ApprovalFlowBranch[] {
  const branches = node.branches ?? [];
  if (branches.length > 0) {
    return branches;
  }

  const fallback: ApprovalFlowBranch[] = [];
  const condition = node.condition;
  if (condition && (condition.fieldKey || condition.expression)) {
    fallback.push({
      id: createId('branch'),
      name: $t('antflow.model.conditionName', ['1']),
      fieldKey: condition.fieldKey ?? null,
      operator: condition.operator ?? '=',
      compareValue: condition.compareValue ?? null,
      expression: condition.expression ?? null,
      conditions: [{ ...condition }],
      logic: 'and',
      nodes: [],
    });
  } else {
    fallback.push({
      id: createId('branch'),
      name: $t('antflow.model.conditionName', ['1']),
      fieldKey: null,
      operator: '=',
      compareValue: null,
      expression: null,
      conditions: [],
      logic: 'and',
      nodes: [],
    });
  }
  fallback.push({
    id: createId('branch'),
    name: $t('antflow.model.otherCondition'),
    fieldKey: null,
    operator: '=',
    compareValue: null,
    expression: null,
    conditions: [],
    logic: 'and',
    nodes: [],
  });

  return fallback;
}

function branchToConditionNode(
  branch: ApprovalFlowBranch,
  index: number,
  total: number,
): AntFlowNode {
  const isDefault = index === total - 1 ? 1 : 0;
  const child = convertChain(branch.nodes ?? [], 0);
  const node = createConditionNode(
    branch.name || (isDefault === 1 ? $t('antflow.model.otherCondition') : $t('antflow.model.conditionName', [String(index + 1)])),
    child,
    index + 1,
    isDefault,
  );

  node.nodeId = branch.id || node.nodeId;
  node.nodeDisplayName = node.nodeName;
  node.error = isDefault === 1 ? false : !hasBranchCondition(branch);
  node.nodeProperty = {
    isDefault,
    groupRelation: false,
    sort: 0,
    groupConditions: branchToGroups(branch),
    expression: branch.expression ?? null,
    logic: branch.logic ?? 'and',
  } satisfies AntFlowConditionNodeProperty;

  return node;
}

function conditionToGateway(
  node: ApprovalFlowNode,
  next: AntFlowNode | null,
): AntFlowNode {
  const branches = conditionBranchesOf(node);
  const gateway = createGatewayNode(null);

  gateway.nodeId = node.id || gateway.nodeId;
  gateway.nodeName = node.name || $t('antflow.model.conditionBranch');
  gateway.nodeDisplayName = gateway.nodeName;
  gateway.childNode = next;
  gateway.error = false;
  gateway.nodeProperty = {
    groupRelation: false,
    isDefault: 0,
    sort: 0,
    conditionNodes: branches.map((branch, index) =>
      branchToConditionNode(branch, index, branches.length),
    ),
  };

  return gateway;
}

function approveFromOld(
  node: ApprovalFlowNode,
  kind: 'approval' | 'cc',
  child: AntFlowNode | null,
): AntFlowNode {
  const result =
    kind === 'approval' ? createApproveNode(child) : createCopyNode(child);
  const rules = node.rules ?? [];
  const property =
    kind === 'approval' ? createApproveProperty() : createCopyProperty();
  const firstRule = rules[0];

  property.assigneeList = collectAssignees(rules);
  property.assigneeType = assigneeTypeFromRules(rules);
  property.signType = signTypeFromSignMode(node.signMode);
  property.ruleKind = firstRule?.kind;
  property.formFieldKey = firstRule?.formFieldKey ?? null;
  property.supervisorLevel = firstRule?.supervisorLevel;
  property.directorLevel =
    firstRule?.kind === 'Supervisor' ? (firstRule.level ?? 0) : 0;
  property.actions = node.actions ?? [];

  result.nodeId = node.id || result.nodeId;
  result.nodeName = node.name || result.nodeName;
  result.nodeDisplayName = buildApproveDisplayName(property, kind);
  result.error = !isApprovePropertyConfigured(property);
  result.nodeProperty = property;

  return result;
}

function convertOldNode(
  node: ApprovalFlowNode,
  next: () => AntFlowNode | null,
): AntFlowNode | null {
  switch (node.kind) {
    case 'approval': {
      return approveFromOld(node, 'approval', next());
    }
    case 'cc': {
      return approveFromOld(node, 'cc', next());
    }
    case 'condition': {
      return conditionToGateway(node, next());
    }
    case 'end': {
      return next();
    }
    case 'start': {
      const start = createStartNode(next());
      start.nodeId = node.id || start.nodeId;
      start.nodeName = node.name || start.nodeName;
      start.nodeDisplayName = start.nodeName;
      return start;
    }
    default: {
      return next();
    }
  }
}

function convertChain(
  nodes: ApprovalFlowNode[],
  index: number,
): AntFlowNode | null {
  const node = nodes[index];
  if (!node) {
    return null;
  }
  return convertOldNode(node, () => convertChain(nodes, index + 1));
}

/**
 * Converts the current approval-flow model (object or JSON string) to the
 * AntFlow linked-node structure.  Existing node ids are preserved.
 */
export function oldToAntFlow(
  flowJson?: ApprovalFlowModel | null | string,
): AntFlowNode | null {
  const model =
    typeof flowJson === 'string' ? parseModel(flowJson) : (flowJson ?? null);
  if (!model || !Array.isArray(model.nodes) || model.nodes.length === 0) {
    return null;
  }

  const nodes = model.nodes.filter(
    (node): node is ApprovalFlowNode => Boolean(node) && node.kind !== 'end',
  );
  if (nodes.length === 0) {
    return null;
  }

  const startIndex = nodes.findIndex((node) => node.kind === 'start');
  if (startIndex === -1) {
    return createStartNode(convertChain(nodes, 0));
  }

  const start = nodes[startIndex];
  if (!start) {
    return null;
  }

  const root = convertOldNode(start, () => null);
  if (!root) {
    return null;
  }
  root.childNode = convertChain(nodes, startIndex + 1);
  return root;
}

/* -------------------------------------------------------------------------- */
/* AntFlow model -> old model                                                  */
/* -------------------------------------------------------------------------- */

const APPROVER_KINDS = new Set([
  'FormField',
  'Initiator',
  'InitiatorSelection',
  'OrganizationUnit',
  'Participant',
  'Role',
  'Supervisor',
]);

function isApproverKind(value: unknown): value is ApproverKind {
  return (
    typeof value === 'string' && APPROVER_KINDS.has(value as ApproverKind)
  );
}

function kindFromAssigneeType(type: unknown): ApproverKind {
  switch (Number(type)) {
    case 2: {
      return 'Role';
    }
    case 3:
    case 6: {
      return 'Supervisor';
    }
    case 4: {
      return 'OrganizationUnit';
    }
    case 5: {
      return 'Initiator';
    }
    case 7: {
      return 'FormField';
    }
    case 8: {
      return 'InitiatorSelection';
    }
    default: {
      return 'Participant';
    }
  }
}

function signModeFromSignType(signType: unknown): ApprovalSignMode {
  switch (Number(signType)) {
    case 2: {
      return 'All';
    }
    case 3: {
      return 'Sequential';
    }
    default: {
      return 'Any';
    }
  }
}

function rulesFromAntFlow(property: any): ApproverRule[] {
  const assignees: AntFlowAssignee[] = Array.isArray(property?.assigneeList)
    ? property.assigneeList
    : [];
  const kind = isApproverKind(property?.ruleKind)
    ? property.ruleKind
    : kindFromAssigneeType(property?.assigneeType);

  const participants: ParticipantRef[] = assignees.map((assignee) => ({
    provider: typeof assignee.provider === 'string' ? assignee.provider : '',
    type: assignee.type === 2 || assignee.type === 'Group' ? 'Group' : 'User',
    id: assignee.id,
    displayName: assignee.displayName ?? assignee.name,
  }));

  const rule: ApproverRule = { kind, participants };
  if (kind === 'Supervisor') {
    rule.supervisorLevel =
      property?.supervisorLevel === 'TopMost' ||
      Number(property?.assigneeType) === 6
        ? 'TopMost'
        : (property?.supervisorLevel === 'LevelN'
          ? 'LevelN'
          : 'Nearest');
    rule.level = Number(property?.directorLevel) || 1;
  }
  if (kind === 'FormField') {
    rule.formFieldKey = property?.formFieldKey ?? null;
  }

  return [rule];
}

function expressionFromItem(item: AntFlowConditionItem): null | string {
  if (item.expression) {
    return item.expression;
  }
  const left = item.label || item.key || item.columnDbName;
  if (!left) {
    return null;
  }
  return `${left} ${item.optType || '='} ${item.zdy1 ?? ''}`.trim();
}

function itemToOldCondition(item: AntFlowConditionItem): ApprovalFlowCondition {
  return {
    fieldKey: item.key || item.columnDbName || null,
    operator: item.optType || '=',
    compareValue: item.zdy1 || null,
    expression: expressionFromItem(item),
  };
}

function buildExpression(
  groups: AntFlowConditionGroup[],
  groupRelation: unknown,
): null | string {
  const groupExpressions = groups
    .map((group) => {
      const items = Array.isArray(group.conditionList)
        ? group.conditionList
        : [];
      return items
        .map((item) => expressionFromItem(item))
        .filter(Boolean)
        .join(group.condRelation === false ? ' || ' : ' && ');
    })
    .filter(Boolean);

  if (groupExpressions.length === 0) {
    return null;
  }
  return groupExpressions.join(groupRelation ? ' && ' : ' || ');
}

function conditionNodeToOldBranch(
  node: AntFlowNode,
  index: number,
  lastIndex: number,
  hasExplicitDefault: boolean,
): ApprovalFlowBranch {
  const property = node.nodeProperty ?? {};
  const isDefault =
    Number(property.isDefault) === 1 ||
    (!hasExplicitDefault && index === lastIndex);
  const groups: AntFlowConditionGroup[] = Array.isArray(
    property.groupConditions,
  )
    ? property.groupConditions
    : [];

  const conditions: ApprovalFlowCondition[] = isDefault
    ? []
    : groups.flatMap((group) =>
        (Array.isArray(group.conditionList) ? group.conditionList : []).map(
          (item) => itemToOldCondition(item),
        ),
      );

  const first = conditions[0];
  const expression = isDefault
    ? null
    : (typeof property.expression === 'string'
      ? property.expression
      : buildExpression(groups, property.groupRelation));

  return {
    id: node.nodeId || createId('branch'),
    name: node.nodeName || (isDefault ? $t('antflow.model.otherCondition') : $t('antflow.model.conditionName', [String(index + 1)])),
    fieldKey: first?.fieldKey ?? null,
    operator: first?.operator ?? '=',
    compareValue: first?.compareValue ?? null,
    expression,
    conditions,
    logic:
      property.logic === 'or' || groups[0]?.condRelation === false
        ? 'or'
        : 'and',
    nodes: chainToOldArray(node.childNode ?? null),
  };
}

function startToOld(node: AntFlowNode): ApprovalFlowNode {
  const oldNode = createNode('start', node.nodeName || $t('antflow.model.start'));
  oldNode.id = node.nodeId || oldNode.id;
  return oldNode;
}

function approveToOld(node: AntFlowNode): ApprovalFlowNode {
  const property = node.nodeProperty ?? {};
  const oldNode = createNode('approval', node.nodeName || $t('antflow.model.reviewer'));
  oldNode.id = node.nodeId || oldNode.id;
  oldNode.signMode = signModeFromSignType(property.signType);
  oldNode.rules = rulesFromAntFlow(property);
  oldNode.onEmpty =
    property.onEmpty === 'EscalateToManager' ? 'EscalateToManager' : 'Skip';

  const actions = Array.isArray(property.actions)
    ? (property.actions as ApprovalAction[])
    : [];
  oldNode.actions =
    actions.length > 0 ? actions : getDefaultApproveRejectActions();

  return oldNode;
}

function copyToOld(node: AntFlowNode): ApprovalFlowNode {
  const oldNode = createNode('cc', node.nodeName || $t('antflow.model.cc'));
  oldNode.id = node.nodeId || oldNode.id;
  oldNode.signMode = 'Any';
  oldNode.rules = rulesFromAntFlow(node.nodeProperty ?? {});
  oldNode.actions = [];
  return oldNode;
}

function gatewayToOld(gateway: AntFlowNode): ApprovalFlowNode {
  const oldNode = createNode('condition', gateway.nodeName || $t('antflow.model.conditionBranch'));
  oldNode.id = gateway.nodeId || oldNode.id;

  const conditionNodes: AntFlowNode[] = Array.isArray(
    gateway.nodeProperty?.conditionNodes,
  )
    ? gateway.nodeProperty.conditionNodes
    : [];
  const lastIndex = conditionNodes.length - 1;
  const hasExplicitDefault = conditionNodes.some(
    (node) => Number(node?.nodeProperty?.isDefault) === 1,
  );

  const branches: ApprovalFlowBranch[] =
    conditionNodes.length > 0
      ? conditionNodes.map((node, index) =>
          conditionNodeToOldBranch(node, index, lastIndex, hasExplicitDefault),
        )
      : [createBranch(0, $t('antflow.model.conditionName', ['1'])), createBranch(1, $t('antflow.model.otherCondition'))];

  oldNode.branches = branches;

  const first =
    branches.find(
      (branch) =>
        Boolean(branch.expression) || (branch.conditions?.length ?? 0) > 0,
    ) ?? branches[0];
  if (first) {
    oldNode.condition = {
      fieldKey: first.fieldKey ?? null,
      operator: first.operator ?? '=',
      compareValue: first.compareValue ?? null,
      expression: first.expression ?? null,
    };
    oldNode.conditionExpression = first.expression ?? null;
  }

  return oldNode;
}

/**
 * A standalone condition node is not a valid AntFlow element (conditions live
 * inside a gateway), but accept it defensively and render it as a one-branch
 * condition node.
 */
function standaloneConditionToOld(node: AntFlowNode): ApprovalFlowNode {
  const oldNode = createNode('condition', node.nodeName || $t('antflow.model.conditionBranch'));
  oldNode.id = node.nodeId || oldNode.id;

  const branch = conditionNodeToOldBranch(node, 0, 1, false);
  branch.nodes = [];
  branch.name = node.nodeName || $t('antflow.model.conditionName', ['1']);
  oldNode.branches = [branch, createBranch(1, $t('antflow.model.otherCondition'))];
  oldNode.condition = {
    fieldKey: branch.fieldKey ?? null,
    operator: branch.operator ?? '=',
    compareValue: branch.compareValue ?? null,
    expression: branch.expression ?? null,
  };
  oldNode.conditionExpression = branch.expression ?? null;
  return oldNode;
}

function antFlowNodeToOld(node: AntFlowNode): ApprovalFlowNode {
  switch (node.nodeType) {
    case NodeType.ApproveNode:
    case NodeType.ParallelApproveNode: {
      return approveToOld(node);
    }
    case NodeType.ConditionNode: {
      return standaloneConditionToOld(node);
    }
    case NodeType.CopyNode: {
      return copyToOld(node);
    }
    case NodeType.GatewayNode: {
      return gatewayToOld(node);
    }
    case NodeType.StartNode: {
      return startToOld(node);
    }
    default: {
      return approveToOld(node);
    }
  }
}

function chainToOldArray(start: AntFlowNode | null): ApprovalFlowNode[] {
  const result: ApprovalFlowNode[] = [];
  const visited = new Set<string>();
  let current = start;

  while (current) {
    if (current.nodeId) {
      if (visited.has(current.nodeId)) {
        break;
      }
      visited.add(current.nodeId);
    }
    result.push(antFlowNodeToOld(current));
    current = current.childNode ?? null;
  }

  return result;
}

export interface AntFlowToOldOptions {
  formDefinitionId?: null | string;
  version?: number;
}

/**
 * Converts AntFlow's linked nodes back to the current approval-flow model.
 */
export function antFlowToOld(
  root: AntFlowNode | null,
  options: AntFlowToOldOptions = {},
): ApprovalFlowModel {
  const nodes: ApprovalFlowNode[] = [];
  const visited = new Set<string>();
  let current = root;
  let hasStart = false;

  while (current) {
    if (current.nodeId) {
      if (visited.has(current.nodeId)) {
        break;
      }
      visited.add(current.nodeId);
    }

    nodes.push(antFlowNodeToOld(current));
    if (current.nodeType === NodeType.StartNode) {
      hasStart = true;
    }
    current = current.childNode ?? null;
  }

  if (!hasStart) {
    nodes.unshift(createNode('start', $t('antflow.model.start')));
  }
  nodes.push(createNode('end', $t('antflow.model.end')));

  return {
    version: options.version ?? 1,
    formDefinitionId: options.formDefinitionId ?? null,
    nodes,
  };
}

/** Convenience helper used by detail views. */
export function summarizeAntFlowNode(node: AntFlowNode | null): string {
  if (!node) {
    return '';
  }
  if (node.nodeType === NodeType.ConditionNode) {
    const groups: AntFlowConditionGroup[] =
      node.nodeProperty?.groupConditions ?? [];
    return buildExpression(groups, node.nodeProperty?.groupRelation) ?? '';
  }
  return node.nodeDisplayName || node.nodeName;
}
