import { EntityChangeDto } from "/@/api/auditing/entity-changes/model";
  
export interface Action {
    id: string;
    serviceName?: string;
    methodName?: string;
    parameters?: string;
    executionTime: Date;
    executionDuration?: number;
    extraProperties?: ExtraPropertyDictionary;
}

export interface AuditLogDto {
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
    entityChanges?: EntityChangeDto[];
    actions?: Action[];
    extraProperties?: ExtraPropertyDictionary;
}

export interface AuditLogGetByPagedDto extends PagedAndSortedResultRequestDto {
    startTime?: Date;
    endTime?: Date;
    httpMethod?: string;
    url?: string;
    userId?: string;
    userName?: string;
    applicationName?: string;
    correlationId?: string;
    clientId?: string;
    clientIpAddress?: string;
    maxExecutionDuration?: number;
    minExecutionDuration?: number;
    hasException?: boolean;
    httpStatusCode?: number;
}
  