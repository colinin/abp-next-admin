import type {
  ApprovalAction,
  ApprovalFlowBranch,
  ApprovalFlowCondition,
  ApprovalFlowModel,
  ApprovalFlowNode,
  ApprovalFlowNodeKind,
  ApproverRule,
} from '../../types/approval-flow';

import { $t } from '@vben/locales';

export function getDefaultApproveRejectActions(): ApprovalAction[] {
  return [
    { key: 'Approve', label: $t('antflow.model.approve'), effect: 'Approve' },
    { key: 'Reject', label: $t('antflow.model.reject'), effect: 'Reject' },
  ];
}

let sequence = 0;

export function createId(prefix: string): string {
  sequence += 1;
  return `${prefix}-${Date.now().toString(36)}-${sequence}`;
}

export function createNode(
  kind: ApprovalFlowNodeKind,
  name?: string,
): ApprovalFlowNode {
  return {
    id: createId(kind),
    kind,
    name: name ?? nodeDefaultName(kind),
    signMode: 'Any',
    rules: kind === 'approval' || kind === 'cc' ? [createRule()] : [],
    onEmpty: 'Skip',
    actions: kind === 'approval' ? [...getDefaultApproveRejectActions()] : [],
    conditionExpression: null,
    expression: null,
    condition:
      kind === 'condition'
        ? {
            fieldKey: null,
            operator: '=',
            compareValue: null,
            expression: null,
          }
        : undefined,
    branches:
      kind === 'condition'
        ? [
            {
              ...createBranch(
                0,
                $t('antflow.model.conditionName').replace('{0}', '1'),
              ),
              nodes: [],
            },
            {
              ...createBranch(1, $t('antflow.model.otherCondition')),
              nodes: [],
            },
          ]
        : undefined,
  };
}

export function createBranch(index: number, name?: string): ApprovalFlowBranch {
  return {
    id: createId('branch'),
    name:
      name ??
      $t('antflow.model.conditionName').replace('{0}', String(index + 1)),
    fieldKey: null,
    operator: '=',
    compareValue: null,
    expression: null,
    nodes: [],
  };
}

export function createRule(): ApproverRule {
  return {
    kind: 'Participant',
    participants: [],
    level: 1,
    supervisorLevel: 'Nearest',
    formFieldKey: null,
  };
}

export function nodeDefaultName(kind: ApprovalFlowNodeKind): string {
  switch (kind) {
    case 'approval': {
      return $t('antflow.model.reviewer');
    }
    case 'cc': {
      return $t('antflow.model.cc');
    }
    case 'condition': {
      return $t('antflow.model.conditionBranch');
    }
    case 'end': {
      return $t('antflow.model.end');
    }
    case 'start': {
      return $t('antflow.model.start');
    }
    default: {
      return $t('antflow.model.notSelected');
    }
  }
}

export function createDefaultModel(): ApprovalFlowModel {
  return {
    version: 1,
    formDefinitionId: null,
    nodes: [
      { ...createNode('start', $t('antflow.model.start')), rules: [] },
      createNode('approval'),
      { ...createNode('end'), rules: [] },
    ],
  };
}

function normalizeNode(node: ApprovalFlowNode): ApprovalFlowNode {
  const base = createNode(node.kind, node.name);
  const normalized: ApprovalFlowNode = {
    ...base,
    ...node,
    actions: node.actions ?? [],
    rules: node.rules ?? [],
  };

  if (node.kind === 'condition') {
    const condition: ApprovalFlowCondition = {
      fieldKey: node.condition?.fieldKey ?? null,
      operator: node.condition?.operator ?? '=',
      compareValue: node.condition?.compareValue ?? null,
      expression:
        node.condition?.expression ??
        node.branches?.[0]?.expression ??
        node.conditionExpression ??
        node.expression ??
        null,
    };

    let branches = node.branches ?? [];
    if (branches.length === 0) {
      branches = [
        createBranch(0, $t('antflow.model.conditionName').replace('{0}', '1')),
      ];
    }
    while (branches.length < 2) {
      branches = [
        ...branches,
        createBranch(branches.length, $t('antflow.model.otherCondition')),
      ];
    }

    normalized.branches = branches.map((branch, index) => {
      const isDefault = index === branches.length - 1;
      return {
        ...branch,
        name: isDefault
          ? $t('antflow.model.otherCondition')
          : $t('antflow.model.conditionName').replace('{0}', String(index + 1)),
        expression: isDefault
          ? null
          : (branch.expression ?? (index === 0 ? condition.expression : null)),
        nodes: (branch.nodes ?? []).map((child) => normalizeNode(child)),
      };
    });

    const defaultBranch = normalized.branches[normalized.branches.length - 1];
    if (defaultBranch && defaultBranch.nodes.length === 1) {
      const onlyNode = defaultBranch.nodes[0];
      const isAutoDefault =
        onlyNode?.kind === 'approval' &&
        onlyNode.name === $t('antflow.model.reviewer') &&
        onlyNode.rules.every(
          (rule) =>
            rule.kind === 'Participant' && rule.participants.length === 0,
        );
      if (isAutoDefault) {
        defaultBranch.nodes = [];
      }
    }
    normalized.condition = condition;
  } else {
    normalized.branches = (node.branches ?? []).map((branch) => ({
      ...branch,
      nodes: (branch.nodes ?? []).map((child) => normalizeNode(child)),
    }));
  }

  return normalized;
}
export function parseModel(json?: null | string): ApprovalFlowModel {
  if (!json) {
    return createDefaultModel();
  }

  try {
    const parsed = JSON.parse(json) as ApprovalFlowModel;
    if (!parsed || !Array.isArray(parsed.nodes) || parsed.nodes.length === 0) {
      return createDefaultModel();
    }

    return {
      version: parsed.version ?? 1,
      formDefinitionId: parsed.formDefinitionId ?? null,
      nodes: parsed.nodes.map((node) => normalizeNode(node)),
    };
  } catch {
    return createDefaultModel();
  }
}

export function summarizeNode(node: ApprovalFlowNode): string {
  if (node.kind === 'approval' || node.kind === 'cc') {
    const names = node.rules
      .flatMap((rule) => rule.participants)
      .map((participant) => participant.displayName ?? participant.id)
      .filter(Boolean);

    if (names.length > 0) {
      if (
        node.kind === 'approval' &&
        node.signMode === 'All' &&
        names.length > 1
      ) {
        return $t('antflow.model.signSummary')
          .replace('{0}', String(names.length))
          .replace('{1}', names.join(','));
      }
      if (
        node.kind === 'approval' &&
        node.signMode === 'Sequential' &&
        names.length > 1
      ) {
        return $t('antflow.model.sequentialSignSummary')
          .replace('{0}', String(names.length))
          .replace('{1}', names.join('→'));
      }
      return names.join('、');
    }

    const summaries = node.rules
      .map((rule) => summarizeRule(rule))
      .filter(Boolean);
    if (summaries.length > 0) {
      return summaries.join(' / ');
    }
    return node.kind === 'cc'
      ? $t('antflow.model.pendingCc')
      : $t('antflow.model.pendingApprover');
  }

  if (node.kind === 'condition') {
    return node.condition?.expression
      ? $t('antflow.model.conditionPrefix').replace(
          '{0}',
          node.condition.expression,
        )
      : $t('antflow.model.noCondition');
  }

  return '';
}

export function summarizeRule(rule: ApproverRule): string {
  switch (rule.kind) {
    case 'FormField': {
      return $t('antflow.model.formField').replace(
        '{0}',
        rule.formFieldKey ?? '-',
      );
    }
    case 'Initiator': {
      return $t('antflow.model.initiator');
    }
    case 'InitiatorSelection': {
      return $t('antflow.model.selfSelect');
    }
    case 'OrganizationUnit': {
      return $t('antflow.model.departmentMember');
    }
    case 'Participant': {
      const names = rule.participants
        .map((participant) => participant.displayName ?? participant.id)
        .filter(Boolean);
      return names.length > 0 ? names.join(',') : $t('antflow.model.noPerson');
    }
    case 'Role': {
      const roleNames = rule.participants
        .map((participant) => participant.displayName ?? participant.id)
        .filter(Boolean);
      return roleNames.length > 0
        ? $t('antflow.model.rolePrefix').replace('{0}', roleNames.join(','))
        : $t('antflow.model.noRole');
    }
    case 'Supervisor': {
      return rule.supervisorLevel === 'TopMost'
        ? $t('antflow.model.topMostSupervisor')
        : $t('antflow.model.supervisor');
    }
    default: {
      return '';
    }
  }
}
