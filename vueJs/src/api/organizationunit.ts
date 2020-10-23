import ApiService from './serviceBase'
import { AuditedEntityDto, PagedResultDto, PagedAndSortedResultRequestDto, ListResultDto } from './types'
import { RoleDto, RoleGetPagedDto } from './roles'
import { User, UsersGetPagedDto } from './users'

/** 远程服务地址 */
const serviceUrl = process.env.VUE_APP_BASE_API

export default class OrganizationUnitService {
  /**
   * 创建组织机构
   * @param payload 类型为 OrganizationUnitCreate 的对象
   * @returns 返回类型为 OrganizationUnit 的对象
   */
  public static createOrganizationUnit(payload: OrganizationUnitCreate) {
    const _url = '/api/identity/organization-units'
    return ApiService.Post<OrganizationUnit>(_url, payload, serviceUrl)
  }

  /**
   * 查询组织机构根节点
   * @returns 返回类型为 OrganizationUnit 的对象列表
   */
  public static getRootOrganizationUnits() {
    const _url = '/api/identity/organization-units/root-node'
    return ApiService.Get<ListResultDto<OrganizationUnit>>(_url, serviceUrl)
  }

  /**
   * 查询所有组织结构
   * @returns 返回类型为 OrganizationUnit 的对象列表
   */
  public static getAllOrganizationUnits() {
    const _url = '/api/identity/organization-units/all'
    return ApiService.Get<ListResultDto<OrganizationUnit>>(_url, serviceUrl)
  }

  /**
   * 查询组织机构列表
   * @param payload 分页查询对象
   * @returns 返回类型为 OrganizationUnit 的对象列表
   */
  public static getOrganizationUnits(payload: OrganizationUnitGetByPaged) {
    let _url = '/api/identity/organization-units'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<OrganizationUnit>>(_url, serviceUrl)
  }

  /**
   * 查询组织机构
   * @param id 主键标识
   * @returns 返回类型为 OrganizationUnit 的对象
   */
  public static getOrganizationUnit(id: string) {
    const _url = '/api/identity/organization-units/' + id
    return ApiService.Get<OrganizationUnit>(_url, serviceUrl)
  }

  /**
   * 变更组织机构
   * @param payload 类型为 OrganizationUnitUpdate 的对象
   * @returns 返回类型为 OrganizationUnit 的对象
   */
  public static updateOrganizationUnit(id: string, payload: OrganizationUnitUpdate) {
    const _url = '/api/identity/organization-units/' + id
    return ApiService.Put<OrganizationUnit>(_url, payload, serviceUrl)
  }

  /**
   * 查询组织机构下属列表
   * @param id 主键标识
   * @param recursive 是否查询所有子级
   * @returns 返回类型为 OrganizationUnit 的对象列表
   */
  public static findOrganizationUnitChildren(id: string, recursive: boolean | undefined) {
    let _url = 'api/identity/organization-units/find-children'
    _url += '?id=' + id
    if (recursive !== undefined) {
      _url += '&recursive=' + recursive
    }
    return ApiService.Get<ListResultDto<OrganizationUnit>>(_url, serviceUrl)
  }

  /**
   * 查询指定节点的最后一个组织机构
   * @param parentId 父级节点标识
   * @returns 返回类型为 OrganizationUnit 的对象
   */
  public static findOrganizationUnitLastChildren(parentId: string | undefined) {
    let _url = 'api/identity/organization-units/last-children'
    if (parentId !== undefined) {
      _url += '?parentId=' + parentId
    }
    return ApiService.Get<OrganizationUnit>(_url, serviceUrl)
  }

  /**
   * 移动组织机构
   * @param id 主键标识
   * @param parentId 移动到的父节点标识
   */
  public static moveOrganizationUnit(id: string, parentId: string | undefined) {
    let _url = 'api/identity/organization-units/' + id + '/move'
    _url += '?id=' + id
    return ApiService.Put<OrganizationUnit>(_url, { parentId: parentId }, serviceUrl)
  }

  /**
   * 删除组织机构
   * @param id 主键标识
   */
  public static deleteOrganizationUnit(id: string) {
    const _url = 'api/identity/organization-units/' + id
    return ApiService.Delete(_url, serviceUrl)
  }

  /**
   * 获取未加入组织机构的用户列表
   * @param id 主键
   * @param payload 类型为 UsersGetPagedDto 的对象
   * @returns 返回类型为 User 的对象列表
   */
  public static getUnaddedUsers(id: string, payload: UsersGetPagedDto) {
    let _url = '/api/identity/organization-units/' + id
    _url += '/unadded-users'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<User>>(_url, serviceUrl)
  }

  /**
   * 获取组织机构的用户列表
   * @param id 主键
   * @param payload 类型为 UsersGetPagedDto 的对象
   * @returns 返回类型为 User 的对象列表
   */
  public static getUsers(id: string, payload: UsersGetPagedDto) {
    let _url = '/api/identity/organization-units/' + id
    _url += '/users'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<User>>(_url, serviceUrl)
  }

  /**
   * 组织机构添加用户
   * @param id 主键
   * @param payload 用户主键列表
   */
  public static addUsers(id: string, payload: OrganizationUnitAddUser) {
    let _url = '/api/identity/organization-units/' + id
    _url += '/users'
    return ApiService.Post<void>(_url, payload, serviceUrl)
  }

  /**
   * 获取未加入组织机构的角色列表
   * @param id 主键
   * @param payload 类型为 UsersGetPagedDto 的对象
   * @returns 返回类型为 User 的对象列表
   */
  public static getUnaddedRoles(id: string, payload: RoleGetPagedDto) {
    let _url = '/api/identity/organization-units/' + id
    _url += '/unadded-roles'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<RoleDto>>(_url, serviceUrl)
  }

  /**
   * 获取组织机构的角色列表
   * @param id 主键
   * @param payload 类型为 UsersGetPagedDto 的对象
   * @returns 返回类型为 User 的对象列表
   */
  public static getRoles(id: string, payload: RoleGetPagedDto) {
    let _url = '/api/identity/organization-units/' + id
    _url += '/roles'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<RoleDto>>(_url, serviceUrl)
  }

  /**
   * 增加角色
   * @param id 主键
   * @param payload 角色主键列表
   */
  public static addRoles(id: string, payload: OrganizationUnitAddRole) {
    let _url = '/api/identity/organization-units/' + id
    _url += '/roles'
    return ApiService.Post<void>(_url, payload, serviceUrl)
  }
}

/** 组织机构 */
export class OrganizationUnit extends AuditedEntityDto {
  /** 标识 */
  id!: string
  /** 父节点标识 */
  parentId?: string
  /** 编号 */
  code!: string
  /** 显示名称 */
  displayName!: string
}

/** 组织机构分页查询对象 */
export class OrganizationUnitGetByPaged extends PagedAndSortedResultRequestDto {
  /** 过滤字符 */
  filter!: string
}

/** 组织机构创建对象 */
export class OrganizationUnitCreate {
  /** 显示名称 */
  displayName!: string
  /** 父节点标识 */
  parentId?: string
}

/** 组织机构变更对象 */
export class OrganizationUnitUpdate {
  /** 显示名称 */
  displayName!: string
}

/** 组织机构增加部门对象 */
export class OrganizationUnitAddRole {
  /** 部门标识列表 */
  roleIds = new Array<string>()

  public isInOrganizationUnit(roleId: string) {
    return this.roleIds.some(id => id === roleId)
  }

  public addRole(roleId: string) {
    this.roleIds.push(roleId)
  }

  public removeRole(roleId: string) {
    const index = this.roleIds.findIndex(id => id === roleId)
    this.roleIds.splice(index, 1)
  }
}

/** 组织机构增加用户对象 */
export class OrganizationUnitAddUser {
  /** 用户标识列表 */
  userIds = new Array<string>()

  public isInOrganizationUnit(userId: string) {
    return this.userIds.some(id => id === userId)
  }

  public addUser(userId: string) {
    this.userIds.push(userId)
  }

  public removeUser(userId: string) {
    const index = this.userIds.findIndex(id => id === userId)
    this.userIds.splice(index, 1)
  }
}
