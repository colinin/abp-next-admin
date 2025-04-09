import type { AuditedEntityDto } from '@abp/core';

enum DataAccessOperation {
  /** 删除 */
  Delete = 2,
  /** 查询 */
  Read = 0,
  /** 更新 */
  Write = 1,
}

enum DataAccessFilterOperate {
  /** 包含 */
  Contains = 9,
  /** 右包含 */
  EndsWith = 8,
  /** 等于 */
  Equal = 1,
  /** 大于 */
  Greater = 5,
  /** 大于或等于 */
  GreaterOrEqual = 6,
  /** 小于 */
  Less = 3,
  /** 小于或等于 */
  LessOrEqual = 4,
  /** 不包含 */
  NotContains = 10,
  /** 不等于 */
  NotEqual = 2,
  /** 左包含 */
  StartsWith = 7,
}

enum DataAccessFilterLogic {
  /** 且 */
  And = 0,
  /** 或 */
  Or = 1,
}

interface DataAccessFilterRule {
  [key: string]: any;
  field: string;
  isLeft: boolean;
  javaScriptType: string;
  operate: DataAccessFilterOperate;
  typeFullName: string;
  value: string;
}

interface DataAccessFilterGroup {
  groups: DataAccessFilterGroup[];
  logic: DataAccessFilterLogic;
  rules: DataAccessFilterRule[];
}

interface EntityRuleDtoBase extends AuditedEntityDto<string> {
  accessedProperties: string[];
  entityTypeFullName: string;
  entityTypeId: string;
  filterGroup: DataAccessFilterGroup;
  isEnabled: boolean;
  operation: DataAccessOperation;
  tenantId?: string;
}

interface EntityEnumInfoModel {
  key: string;
  value: any;
}

interface EntityPropertyInfoModel {
  displayName: string;
  enums: EntityEnumInfoModel[];
  javaScriptName: string;
  javaScriptType: string;
  name: string;
  operates: DataAccessFilterOperate[];
  typeFullName: string;
}

interface EntityTypeInfoModel {
  displayName: string;
  name: string;
  properties: EntityPropertyInfoModel[];
}

interface EntityTypeInfoGetModel {
  operation: DataAccessOperation;
}

export { DataAccessFilterLogic, DataAccessFilterOperate, DataAccessOperation };

export type {
  DataAccessFilterGroup,
  DataAccessFilterRule,
  EntityRuleDtoBase,
  EntityTypeInfoGetModel,
  EntityTypeInfoModel,
};
