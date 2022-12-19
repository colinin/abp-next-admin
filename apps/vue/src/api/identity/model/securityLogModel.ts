export interface SecurityLog extends ExtensibleObject {
  id: string;
  applicationName?: string;
  identity?: string;
  action?: string;
  userId?: string;
  userName?: string;
  tenantName?: string;
  clientId?: string;
  correlationId?: string;
  clientIpAddress?: string;
  browserInfo?: string;
  creationTime?: Date;
}

export interface GetSecurityLogPagedRequest extends PagedAndSortedResultRequestDto {
  startTime?: Date;
  endTime?: Date;
  applicationName?: string;
  identity?: string;
  actionName?: string;
  userId?: string;
  userName?: string;
  clientId?: string;
  correlationId?: string;
}
