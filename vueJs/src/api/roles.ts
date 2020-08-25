import { pagerFormat } from '@/utils/index'
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
    _url += '?skipCount=' + pagerFormat(payload.skipCount) * payload.maxResultCount
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
    const _url = '/api/identity/roles/organization-units/' + roleId
    return ApiService.Get<ListResultDto<OrganizationUnit>>(_url, IdentityServiceUrl);
  }

  public static changeRoleOrganizationUnits(roleId: string, payload: ChangeRoleOrganizationUnitDto) {
    const _url = '/api/identity/roles/organization-units/' + roleId
    return ApiService.Put<void>(_url, payload, IdentityServiceUrl);
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
  filter?: string
}

export class CreateRoleDto extends RoleBaseDto {
  constructor() {
    super()
    this.isDefault = false
    this.isPublic = true
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
