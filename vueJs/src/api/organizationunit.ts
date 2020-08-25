import ApiService from './serviceBase'
import { pagerFormat } from '@/utils/index'
import { AuditedEntityDto, PagedResultDto, PagedAndSortedResultRequestDto, ListResultDto } from './types'
import { RoleDto } from './roles'
import { UserDataDto } from './users'

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
   * 查询组织机构列表
   * @param payload 分页查询对象
   * @returns 返回类型为 OrganizationUnit 的对象列表
   */
  public static getOrganizationUnits(payload: OrganizationUnitGetByPaged) {
    let _url = '/api/identity/organization-units'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + pagerFormat(payload.skipCount)
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<OrganizationUnit>>(_url, serviceUrl)
  }

  /**
   * 查询组织机构
   * @param id 主键标识
   * @returns 返回类型为 OrganizationUnit 的对象
   */
  public static getOrganizationUnit(id: string) {
    let _url = '/api/identity/organization-units/'
    _url += '?id=' + id
    return ApiService.Get<OrganizationUnit>(_url, serviceUrl)
  }

  /**
   * 变更组织机构
   * @param payload 类型为 OrganizationUnitUpdate 的对象
   * @returns 返回类型为 OrganizationUnit 的对象
   */
  public static updateOrganizationUnit(payload: OrganizationUnitUpdate) {
    const _url = '/api/identity/organization-units'
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
   * 获取组织机构角色列表
   * @param payload 类型为 OrganizationUnitGetRoleByPaged 的对象
   * @returns 返回类型为 RoleDto 的对象列表
   */
  public static organizationUnitGetRoles(payload: OrganizationUnitGetRoleByPaged) {
    let _url = '/api/identity/organization-units/management-roles'
    _url += '?id=' + payload.id
    _url += '&filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    // abp设计原因,需要前端计算分页
    _url += '&skipCount=' + pagerFormat(payload.skipCount) * payload.maxResultCount
    // _url += '&skipCount=' + pagerFormat(payload.skipCount)
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<RoleDto>>(_url, serviceUrl)
  }

  /**
   * 增加角色
   * @param payload 类型为 OrganizationUnitAddRole 的对象
   */
  public static organizationUnitAddRole(payload: OrganizationUnitAddRole) {
    const _url = '/api/identity/organization-units/management-roles'
    return ApiService.Post<void>(_url, payload, serviceUrl)
  }

  /**
   * 删除角色
   * @param id 主键标识
   * @param roleId 角色标识
   */
  public static organizationUnitRemoveRole(id: string, roleId: string) {
    let _url = '/api/identity/organization-units/management-roles'
    _url += '?id=' + id
    _url += '&roleId=' + roleId
    return ApiService.Delete(_url, serviceUrl)
  }

  /**
   * 获取组织机构用户列表
   * @param payload 类型为 OrganizationUnitGetRoleByPaged 的对象
   * @returns 返回类型为 RoleDto 的对象列表
   */
  public static organizationUnitGetUsers(id: string) {
    let _url = '/api/identity/organization-units/management-users'
    _url += '?id=' + id
    return ApiService.Get<ListResultDto<UserDataDto>>(_url, serviceUrl)
  }

  /**
   * 增加用户
   * @param payload 类型为 OrganizationUnitAddUser 的对象
   */
  public static organizationUnitAddUser(payload: OrganizationUnitAddUser) {
    const _url = '/api/identity/organization-units/management-users'
    return ApiService.Post<void>(_url, payload, serviceUrl)
  }

  /**
   * 删除角色
   * @param id 主键标识
   * @param roleId 角色标识
   */
  public static organizationUnitRemoveUser(id: string, userId: string) {
    let _url = '/api/identity/organization-units/management-users'
    _url += '?id=' + id
    _url += '&userId=' + userId
    return ApiService.Delete(_url, serviceUrl)
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

/** 组织机构角色分页查询对象 */
export class OrganizationUnitGetRoleByPaged extends PagedAndSortedResultRequestDto {
  /** 主键标识 */
  id!: string
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
  /** 标识 */
  id!: string
  /** 显示名称 */
  displayName!: string
}

/** 组织机构增加部门对象 */
export class OrganizationUnitAddRole {
  /** 标识 */
  id!: string
  /** 部门标识 */
  roleId!: string
}

/** 组织机构增加用户对象 */
export class OrganizationUnitAddUser {
  /** 标识 */
  id!: string
  /** 用户标识 */
  userId!: string
}
