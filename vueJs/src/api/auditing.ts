import { urlStringify } from '@/utils/index'
import ApiService from './serviceBase'
import { PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class AuditingService {
  public static getAuditLogById(id: string) {
    const _url = '/api/auditing/audit-log/' + id
    return ApiService.Get<AuditLog>(_url, serviceUrl)
  }

  public static getAuditLogs(payload: AuditLogGetPaged) {
    let _url = '/api/auditing/audit-log?'
    _url += urlStringify(payload)
    return ApiService.Get<PagedResultDto<AuditLog>>(_url, serviceUrl)
  }

  public static deleteAuditLog(id: string) {
    const _url = '/api/auditing/audit-log/' + id
    return ApiService.Delete(_url, serviceUrl)
  }

  public static getSecurityLogById(id: string) {
    const _url = '/api/auditing/security-log/' + id
    return ApiService.Get<SecurityLog>(_url, serviceUrl)
  }

  public static getSecurityLogs(payload: SecurityLogGetPaged) {
    let _url = '/api/auditing/security-log?'
    _url += urlStringify(payload)
    return ApiService.Get<PagedResultDto<SecurityLog>>(_url, serviceUrl)
  }

  public static deleteSecurityLog(id: string) {
    const _url = '/api/auditing/security-log/' + id
    return ApiService.Delete(_url, serviceUrl)
  }
}

export class AuditLogGetPaged extends PagedAndSortedResultRequestDto {
  startTime?: Date
  endTime?: Date
  httpMethod?: string
  url?: string
  userName?: string
  correlationId?: string
  applicationName?: string
  maxExecutionDuration?: number
  minExecutionDuration?: number
  hasException?: boolean
  httpStatusCode?: number
}

export class SecurityLogGetPaged extends PagedAndSortedResultRequestDto {
  startTime?: Date
  endTime?: Date
  applicationName?: string
  identity?: string
  actionName?: string
  userId?: string
  userName?: string
  clientId?: string
  correlationId?: string
}

export class SecurityLog {
  id!: string
  applicationName?: string
  identity?: string
  action?: string
  userId?: string
  userName?: string
  tenantName?: string
  clientId?: string
  correlationId?: string
  clientIpAddress?: string
  browserInfo?: string
  creationTime?: Date
  extraProperties?: {[key: string]: any}
}

export enum ChangeType {
  Created = 0,
  Updated = 1,
  Deleted = 2
}

export class PropertyChange {
  id!: string
  newValue?: string
  originalValue?: string
  propertyName?: string
  propertyTypeFullName?: string
}

export class EntityChange {
  id!: string
  changeTime?: Date
  changeType?: ChangeType
  entityTenantId?: string
  entityId?: string
  entityTypeFullName?: string
  propertyChanges?: PropertyChange[]
  extraProperties?: {[key: string]: any}
}

export class Action {
  id!: string
  serviceName?: string
  methodName?: string
  parameters?: string
  executionTime!: Date
  executionDuration?: number
  extraProperties?: {[key: string]: any}
}

export class AuditLog {
  id!: string
  applicationName?: string
  userId?: string
  userName?: string
  tenantId?: string
  tenantName?: string
  impersonatorUserId?: string
  impersonatorTenantId?: string
  executionTime?: Date
  executionDuration?: number
  clientIpAddress?: string
  clientName?: string
  clientId?: string
  correlationId?: string
  browserInfo?: string
  httpMethod?: string
  url?: string
  exceptions?: string
  comments?: string
  httpStatusCode?: number
  entityChanges?: EntityChange[]
  actions?: Action[]
  extraProperties?: {[key: string]: any}
}
