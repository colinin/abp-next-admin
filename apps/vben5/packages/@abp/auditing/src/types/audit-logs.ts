import type {
  ExtraPropertyDictionary,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

import type { EntityChangeDto } from './entity-changes';

interface Action {
  [key: string]: any;
  executionDuration?: number;
  executionTime: Date;
  extraProperties?: ExtraPropertyDictionary;
  id: string;
  methodName?: string;
  parameters?: string;
  serviceName?: string;
}

interface AuditLogDto {
  [key: string]: any;
  actions?: Action[];
  applicationName?: string;
  browserInfo?: string;
  clientId?: string;
  clientIpAddress?: string;
  clientName?: string;
  comments?: string;
  correlationId?: string;
  entityChanges?: EntityChangeDto[];
  exceptions?: string;
  executionDuration?: number;
  executionTime?: Date;
  extraProperties?: ExtraPropertyDictionary;
  httpMethod?: string;
  httpStatusCode?: number;
  id: string;
  impersonatorTenantId?: string;
  impersonatorUserId?: string;
  tenantId?: string;
  tenantName?: string;
  url?: string;
  userId?: string;
  userName?: string;
}
interface AuditLogGetListInput extends PagedAndSortedResultRequestDto {
  applicationName?: string;
  clientId?: string;
  clientIpAddress?: string;
  correlationId?: string;
  endTime?: Date;
  hasException?: boolean;
  httpMethod?: string;
  httpStatusCode?: number;
  maxExecutionDuration?: number;
  minExecutionDuration?: number;
  startTime?: Date;
  url?: string;
  userId?: string;
  userName?: string;
}

export type { Action, AuditLogDto, AuditLogGetListInput };
