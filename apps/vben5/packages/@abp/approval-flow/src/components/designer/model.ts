import type {
  ApprovalAction,
  ApprovalFlowBranch,
  ApprovalFlowCondition,
  ApprovalFlowModel,
  ApprovalFlowNode,
  ApprovalFlowNodeKind,
  ApproverRule,
} from '../../types/approval-flow';

export const defaultApproveRejectActions: ApprovalAction[] = [
  { key: 'Approve', label: '同意', effect: 'Approve' },
  { key: 'Reject', label: '拒绝', effect: 'Reject' },
];

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
    actions: kind === 'approval' ? [...defaultApproveRejectActions] : [],
    conditionExpression: null,
    expression: null,
    condition:
      kind === 'condition'
        ? { fieldKey: null, operator: '=', compareValue: null, expression: null }
        : undefined,
    branches:
      kind === 'condition'
        ? [
            { ...createBranch(0, '条件1'), nodes: [] },
            { ...createBranch(1, '其他条件'), nodes: [] },
          ]
        : undefined,
  };
}

export function createBranch(index: number, name?: string): ApprovalFlowBranch {
  return {
    id: createId('branch'),
    name: name ?? `条件${index + 1}`,
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
      return '审批人';
    }
    case 'cc': {
      return '抄送人';
    }
    case 'condition': {
      return '条件分支';
    }
    case 'end': {
      return '结束';
    }
    case 'start': {
      return '发起人';
    }
    default: {
      return '节点';
    }
  }
}

export function createDefaultModel(): ApprovalFlowModel {
  return {
    version: 1,
    formDefinitionId: null,
    nodes: [
      { ...createNode('start', '发起人'), rules: [] },
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
      branches = [createBranch(0, '条件1')];
    }
    while (branches.length < 2) {
      branches = [
        ...branches,
        createBranch(branches.length, '其他条件'),
      ];
    }

    normalized.branches = branches.map((branch, index) => {
      const isDefault = index === branches.length - 1;
      return {
        ...branch,
        name: isDefault ? '其他条件' : `条件${index + 1}`,
        expression: isDefault
          ? null
          : branch.expression ?? (index === 0 ? condition.expression : null),
        nodes: (branch.nodes ?? []).map((child) => normalizeNode(child)),
      };
    });

    const defaultBranch = normalized.branches[normalized.branches.length - 1];
    if (defaultBranch && defaultBranch.nodes.length === 1) {
      const onlyNode = defaultBranch.nodes[0];
      const isAutoDefault =
        onlyNode?.kind === 'approval' &&
        onlyNode.name === '审批人' &&
        onlyNode.rules.every(
          (rule) => rule.kind === 'Participant' && rule.participants.length === 0,
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
      if (node.kind === 'approval' && node.signMode === 'All' && names.length > 1) {
        return `${names.length}人(${names.join(',')})会签`;
      }
      if (node.kind === 'approval' && node.signMode === 'Sequential' && names.length > 1) {
        return `${names.length}人(${names.join('→')})依次审批`;
      }
      return names.join('、');
    }

    const summaries = node.rules
      .map((rule) => summarizeRule(rule))
      .filter(Boolean);
    return summaries.length > 0
      ? summaries.join(' / ')
      : node.kind === 'cc'
        ? '待选择抄送人'
        : '待配置审批人';
  }

  if (node.kind === 'condition') {
    return node.condition?.expression ? `条件：${node.condition.expression}` : '未设置条件';
  }

  return '';
}

export function summarizeRule(rule: ApproverRule): string {
  switch (rule.kind) {
    case 'FormField': {
      return `表单字段：${rule.formFieldKey ?? '-'}`;
    }
    case 'Initiator': {
      return '发起人';
    }
    case 'InitiatorSelection': {
      return '发起人自选';
    }
    case 'OrganizationUnit': {
      return '部门成员';
    }
    case 'Supervisor': {
      return rule.supervisorLevel === 'TopMost' ? '最高层级主管' : '部门主管';
    }
    case 'Role': {
      const roleNames = rule.participants
        .map((participant) => participant.displayName ?? participant.id)
        .filter(Boolean);
      return roleNames.length > 0 ? `角色：${roleNames.join(',')}` : '未选择角色';
    }
    case 'Participant': {
      const names = rule.participants
        .map((participant) => participant.displayName ?? participant.id)
        .filter(Boolean);
      return names.length > 0 ? names.join(',') : '未选择人员';
    }
    default: {
      return '';
    }
  }
}
