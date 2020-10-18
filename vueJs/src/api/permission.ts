import { IPermission } from './types'
import ApiService from './serviceBase'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class PermissionService {
  /** 获取指定权限提供者的权限数据
  * @param providerName 权限提供者 如User(用户)、Role(角色)
  * @param providerKey 权限提供者标识 User:userId Role:roleId
  * @description 老版本的abp vNext用的User、Role、Client等提供者名称
  * 新版本的简写为U、R、C等
  */
  public static getPermissionsByKey(providerName: string, providerKey: string) {
    let _url = '/api/permission-management/permissions?'
    _url += 'providerName=' + providerName
    _url += '&providerKey=' + providerKey
    return ApiService.Get<PermissionDto>(_url, serviceUrl)
  }

  /** 授权指定权限提供者的权限数据
  * @param providerName 权限提供者 如User(用户)、Role(角色)
  * @param providerKey 权限提供者标识 User:userId Role:roleId
  * @param payload 授权数据
  */
  public static setPermissionsByKey(providerName: string, providerKey: string, payload: UpdatePermissionsDto) {
    let _url = '/api/permission-management/permissions?'
    _url += 'providerName=' + providerName
    _url += '&providerKey=' + providerKey
    return ApiService.Put<any>(_url, payload, serviceUrl)
  }
}

export class UpdatePermissionDto implements IPermission {
  name!: string
  isGranted!: boolean

  constructor(
    name: string,
    isGranted: boolean
  ) {
    this.name = name
    this.isGranted = isGranted
  }
}

export class UpdatePermissionsDto {
  permissions!: UpdatePermissionDto[]

  constructor() {
    this.permissions = new Array<UpdatePermissionDto>()
  }

  public addPermission(name: string, isGranted: boolean) {
    this.permissions.push(new UpdatePermissionDto(name, isGranted))
  }
}

export class PermissionProvider {
  providerName!: string
  providerKey!: string
}

export class Permission implements IPermissionGrant {
  allowedProviders!: string[]
  grantedProviders!: PermissionProvider[]
  displayName!: string
  isGranted!: boolean
  name!: string
  parentName?: string

  constructor() {
    this.allowedProviders = new Array<string>()
    this.grantedProviders = new Array<PermissionProvider>()
  }
}

export class PermissionGroup {
  displayName!: string
  name!: string
  permissions!: Permission[]

  constructor() {
    this.permissions = new Array<Permission>()
  }
}

export class PermissionDto {
  entityDisplayName!: string
  groups!: PermissionGroup[]

  constructor() {
    this.groups = new Array<PermissionGroup>()
  }
}

export interface IPermissionGrant {
  allowedProviders: string[]
  grantedProviders: PermissionProvider[]
  displayName: string
  isGranted: boolean
  name: string
  parentName?: string
}
