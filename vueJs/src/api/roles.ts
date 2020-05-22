import ApiService from './serviceBase'
import { ListResultDto } from './types'

const IdentityServiceUrl = process.env.VUE_APP_BASE_API

export default class RoleService {
  public static getRoles() {
    return ApiService.Get<ListResultDto<RoleDto>>('/api/identity/roles', IdentityServiceUrl)
  }

  public static getRoleById(id: string) {
    let _url = '/api/identity/roles/'
    _url += id
    return ApiService.Get<RoleDto>(_url, IdentityServiceUrl)
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
