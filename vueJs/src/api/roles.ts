import ApiService from './serviceBase'
import { OrganizationUnit } from './organizationunit'
import { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const IdentityServiceUrl = process.env.VUE_APP_BASE_API

export default class RoleService {
  public static getAllRoles() {
    return ApiService.Get<ListResultDto<RoleDto>>('/api/identity/roles', IdentityServiceUrl)
  }

  public static getRoles(payload: RoleGetPagedDto) {
    let _url = '/api/identity/roles'
    // 因为abp设计的原因, 需要前端组合页面
    _url += '?skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    _url += '&sorting=' + payload.sorting
    _url += '&filter=' + payload.filter
    return ApiService.Get<PagedResultDto<RoleDto>>(_url, IdentityServiceUrl)
  }

  public static getRoleById(id: string) {
    let _url = '/api/identity/roles/'
    _url += id
    return ApiService.Get<RoleDto>(_url, IdentityServiceUrl)
  }

  public static getRoleOrganizationUnits(roleId: string) {
    const _url = '/api/identity/roles/' + roleId + '/organization-units'
    return ApiService.Get<ListResultDto<OrganizationUnit>>(_url, IdentityServiceUrl)
  }

  public static removeOrganizationUnits(roleId: string, ouId: string) {
    const _url = '/api/identity/roles/' + roleId + '/organization-units/' + ouId
    return ApiService.Delete(_url, IdentityServiceUrl)
  }

  public static changeRoleOrganizationUnits(roleId: string, payload: ChangeRoleOrganizationUnitDto) {
    const _url = '/api/identity/roles/organization-units/' + roleId
    return ApiService.Put<void>(_url, payload, IdentityServiceUrl)
  }

  public static createRole(input: CreateRoleDto) {
    return ApiService.Post<RoleDto>('/api/identity/roles', input, IdentityServiceUrl)
  }

  public static updateRole(id: string, input: UpdateRoleDto) {
    let _url = '/api/identity/roles/'
    _url += id
    return ApiService.Put<RoleDto>(_url, input, IdentityServiceUrl)
  }

  public static deleteRole(id: string) {
    let _url = '/api/identity/roles/'
    _url += id
    return ApiService.Delete(_url, IdentityServiceUrl)
  }

  public static getRoleClaims(roleId: string) {
    const _url = '/api/identity/roles/' + roleId + '/claims'
    return ApiService.Get<ListResultDto<RoleClaim>>(_url, IdentityServiceUrl)
  }

  public static addRoleClaim(roleId: string, payload: RoleClaimCreateOrUpdate) {
    const _url = '/api/identity/roles/' + roleId + '/claims'
    return ApiService.Post<void>(_url, payload, IdentityServiceUrl)
  }

  public static updateRoleClaim(roleId: string, payload: RoleClaimCreateOrUpdate) {
    const _url = '/api/identity/roles/' + roleId + '/claims'
    return ApiService.Put<void>(_url, payload, IdentityServiceUrl)
  }

  public static deleteRoleClaim(roleId: string, payload: RoleClaimDelete) {
    let _url = '/api/identity/roles/' + roleId + '/claims'
    _url += '?claimType=' + payload.claimType
    _url += '&claimValue=' + payload.claimValue
    return ApiService.Delete(_url, IdentityServiceUrl)
  }
}

export class RoleBaseDto {
  name!: string
  isDefault!: boolean
  isPublic!: boolean
}

export class RoleDto extends RoleBaseDto {
  id!: string
  isStatic!: boolean
  concurrencyStamp?: string
}

export class RoleGetPagedDto extends PagedAndSortedResultRequestDto {
  filter = ''
}

export class CreateRoleDto extends RoleBaseDto {
  constructor() {
    super()
    this.isDefault = false
    this.isPublic = true
    this.name = ''
  }
}

export class UpdateRoleDto extends RoleBaseDto {
  concurrencyStamp?: string

  constructor() {
    super()
    this.isDefault = false
    this.isPublic = true
  }
}

export class ChangeRoleOrganizationUnitDto {
  organizationUnitIds = new Array<string>()
}

export class RoleClaimCreateOrUpdate {
  claimType = ''
  claimValue = ''
}

export class RoleClaimDelete {
  claimType = ''
  claimValue = ''
}

export class RoleClaim extends RoleClaimCreateOrUpdate {
  id!: string
}
