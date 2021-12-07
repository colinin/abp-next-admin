import { PagedAndSortedResultRequestDto } from '../../model/baseModel';

export interface SecurityLog {
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
  extraProperties?: { [key: string]: any };
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
