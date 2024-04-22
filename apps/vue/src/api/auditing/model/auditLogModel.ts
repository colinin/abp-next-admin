export enum ChangeType {
  Created = 0,
  Updated = 1,
  Deleted = 2,
}

export interface PropertyChange {
  id: string;
  newValue?: string;
  originalValue?: string;
  propertyName?: string;
  propertyTypeFullName?: string;
}

export interface EntityChange {
  id: string;
  changeTime?: Date;
  changeType: ChangeType;
  entityTenantId?: string;
  entityId?: string;
  entityTypeFullName?: string;
  propertyChanges?: PropertyChange[];
  extraProperties?: ExtraPropertyDictionary;
}

export interface EntityChangeWithUsername {
  entityChange: EntityChange;
  userName?: string;
}

export interface Action {
  id: string;
  serviceName?: string;
  methodName?: string;
  parameters?: string;
  executionTime: Date;
  executionDuration?: number;
  extraProperties?: ExtraPropertyDictionary;
}

export interface AuditLog {
  id: string;
  applicationName?: string;
  userId?: string;
  userName?: string;
  tenantId?: string;
  tenantName?: string;
  impersonatorUserId?: string;
  impersonatorTenantId?: string;
  executionTime?: Date;
  executionDuration?: number;
  clientIpAddress?: string;
  clientName?: string;
  clientId?: string;
  correlationId?: string;
  browserInfo?: string;
  httpMethod?: string;
  url?: string;
  exceptions?: string;
  comments?: string;
  httpStatusCode?: number;
  entityChanges?: EntityChange[];
  actions?: Action[];
  extraProperties?: ExtraPropertyDictionary;
}

export interface GetAuditLogPagedRequest extends PagedAndSortedResultRequestDto {
  startTime?: Date;
  endTime?: Date;
  httpMethod?: string;
  url?: string;
  userName?: string;
  correlationId?: string;
  clientId?: string;
  clientIpAddress?: string;
  applicationName?: string;
  maxExecutionDuration?: number;
  minExecutionDuration?: number;
  hasException?: boolean;
  httpStatusCode?: number;
}

export interface EntityChangeGetByPagedRequest extends PagedAndSortedResultRequestDto {
  auditLogId?: string;
  startTime?: Date;
  endTime?: Date;
  changeType?: ChangeType;
  entityId?: string;
  entityTypeFullName?: string;
}

export interface EntityChangeGetWithUsernameInput {
  entityId?: string;
  entityTypeFullName?: string;
}

export interface RestoreEntityInput {
  entityId: string;
  entityChangeId?: string;
}
