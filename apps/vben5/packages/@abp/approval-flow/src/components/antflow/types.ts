/**
 * AntFlow node model.
 *
 * The numeric node types mirror AntFlow's `NodeType` constants.  They are kept
 * as a frozen object (instead of a TypeScript enum) so the values can also be
 * consumed from plain JavaScript / JSON definitions.
 */
export const NodeType = {
  /** Start node */
  StartNode: 1,
  /** Condition gateway */
  GatewayNode: 2,
  /** Condition node (branch inside a gateway) */
  ConditionNode: 3,
  /** Approver */
  ApproveNode: 4,
  /** Parallel approval gateway */
  ParallelApproveWayNode: 5,
  /** CC recipient */
  CopyNode: 6,
  /** Parallel approver */
  ParallelApproveNode: 7,
} as const;

export type NodeType = (typeof NodeType)[keyof typeof NodeType];

/** Placeholder used by the node card when a node has no display name yet. */
export const placeholderList: Record<number, string> = {
  [NodeType.StartNode]: 'antflow.model.start',
  [NodeType.GatewayNode]: 'antflow.model.conditionBranch',
  [NodeType.ConditionNode]: 'antflow.model.conditionBranch',
  [NodeType.ApproveNode]: 'antflow.model.reviewer',
  [NodeType.ParallelApproveWayNode]: 'antflow.model.reviewer',
  [NodeType.CopyNode]: 'antflow.model.cc',
  [NodeType.ParallelApproveNode]: 'antflow.model.reviewer',
};

/** Actions exposed by the hover "add node" menu. */
export type AntFlowAddKind = 'approval' | 'cc' | 'condition';

/** Detail-view status keys supported by `NodeWarp` (green/red/blue/gray). */
export type AntFlowNodeStatus = 'approved' | 'current' | 'pending' | 'rejected';

export type AntFlowStatusMap = Record<string, AntFlowNodeStatus | string>;

export interface AntFlowAssignee {
  displayName?: string;
  id: string;
  name: string;
  provider?: string;
  /** 1 = user, 2 = group.  Kept loose for AntFlow's role/department payloads. */
  type: number | string;
}

export interface AntFlowButtons {
  approvalPage: number[];
  startPage: number[];
  viewPage: number[];
}

/**
 * Dynamic node property.  Different node types use different subsets, so the
 * model intentionally keeps this loose (`nodeProperty?: any`) like AntFlow.
 */
export interface AntFlowApproveNodeProperty {
  actions?: any[];
  assigneeList: AntFlowAssignee[];
  assigneeType: number;
  buttons: AntFlowButtons;
  ccFlag?: number;
  directorLevel: number;
  fieldPrems: any[];
  formFieldKey?: null | string;
  IsDistinct?: number;
  noHeaderAction: number;
  onEmpty?: string;
  /** Round-trip hint used by `antFlowToOld`. */
  ruleKind?: string;
  signType: number;
  sort?: number;
  supervisorLevel?: string;
}

export interface AntFlowConditionItem {
  columnDbName: string;
  columnType: string;
  /** Optional original expression, preserved for lossless round-trips. */
  expression?: null | string;
  key: string;
  label: string;
  opt1: string;
  opt2: string;
  optType: string;
  type: string;
  zdy1: string;
  zdy2: string;
}

export interface AntFlowConditionGroup {
  conditionList: AntFlowConditionItem[];
  /** true = AND, false = OR */
  condRelation: boolean;
  groupId: string;
  sort?: number;
}

export interface AntFlowConditionNodeProperty {
  expression?: null | string;
  groupConditions: AntFlowConditionGroup[];
  /** true = AND, false = OR (relation between groups). */
  groupRelation: boolean;
  isDefault: number;
  logic?: 'and' | 'or';
  sort?: number;
}

export interface AntFlowGatewayNodeProperty {
  conditionNodes: AntFlowNode[];
  groupRelation?: boolean;
  isDefault?: number;
  sort?: number;
}

export interface AntFlowNode {
  /** Allows host applications to carry additional AntFlow fields. */
  [key: string]: any;
  childNode?: AntFlowNode | null;
  error?: boolean;
  nodeDisplayName: string;
  nodeFrom?: string;
  nodeId: string;
  nodeName: string;
  nodeProperty?: any;
  nodeTo?: string[];
  /** One of the numeric `NodeType` values. */
  nodeType: number;
  nodeWeight?: number;
}
