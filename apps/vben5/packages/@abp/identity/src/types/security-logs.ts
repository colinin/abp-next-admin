import type {
  ExtensibleObject,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface SecurityLogDto extends ExtensibleObject {
  action?: string;
  applicationName?: string;
  browserInfo?: string;
  clientId?: string;
  clientIpAddress?: string;
  correlationId?: string;
  creationTime?: Date;
  id: string;
  identity?: string;
  tenantName?: string;
  userId?: string;
  userName?: string;
}

interface GetSecurityLogPagedRequest extends PagedAndSortedResultRequestDto {
  actionName?: string;
  applicationName?: string;
  clientId?: string;
  correlationId?: string;
  endTime?: Date;
  identity?: string;
  startTime?: Date;
  userId?: string;
  userName?: string;
}

interface SecurityLogDeleteManyInput {
  ids: string[];
}

export type {
  GetSecurityLogPagedRequest,
  SecurityLogDeleteManyInput,
  SecurityLogDto,
};
