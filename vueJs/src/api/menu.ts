import ApiService from './serviceBase'
import { Route, PlatformType } from './layout'
import { urlStringify } from '@/utils/index'
import { ISortedResultRequest, PagedResultDto, PagedAndSortedResultRequestDto, ListResultDto } from './types'

const sourceUrl = '/api/platform/menus'
/** 远程服务地址 */
const serviceUrl = process.env.VUE_APP_BASE_API

/** 路由相关pi接口 */
export default class MenuService {
  public static get(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<Menu>(_url, serviceUrl)
  }

  public static getAll(payload: GetAllMenu) {
    const _url = sourceUrl + '/all?' + urlStringify(payload)
    return ApiService.Get<ListResultDto<Menu>>(_url, serviceUrl)
  }

  public static getList(payload: GetMenuByPaged) {
    const _url = sourceUrl + '?' + urlStringify(payload)
    return ApiService.Get<PagedResultDto<Menu>>(_url, serviceUrl)
  }

  public static getMyMenuList(platformType: PlatformType) {
    const _url = sourceUrl + '/by-current-user?platformType=' + platformType
    return ApiService.Get<ListResultDto<Menu>>(_url, serviceUrl)
  }

  public static getRoleMenuList(role: string, platformType: PlatformType) {
    const _url = sourceUrl + `/by-role/${role}/${platformType}`
    return ApiService.Get<ListResultDto<Menu>>(_url, serviceUrl)
  }

  public static getUserMenuList(userId: string, platformType: PlatformType) {
    const _url = sourceUrl + `/by-user/${userId}/${platformType}`
    return ApiService.Get<ListResultDto<Menu>>(_url, serviceUrl)
  }

  public static create(payload: MenuCreate) {
    return ApiService.Post<Menu>(sourceUrl, payload, serviceUrl)
  }

  public static update(id: string, payload: MenuUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<Menu>(_url, payload, serviceUrl)
  }

  public static delete(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }

  public static setRoleMenu(payload: RoleMenu) {
    const _url = sourceUrl + '/by-role'
    return ApiService.Put<void>(_url, payload, serviceUrl)
  }

  public static setUserMenu(payload: UserMenu) {
    const _url = sourceUrl + '/by-user'
    return ApiService.Put<void>(_url, payload, serviceUrl)
  }
}

export class MenuCreateOrUpdate {
  name!: string
  path!: string
  component!: string
  displayName!: string
  description?: string
  redirect?: string
  isPublic!: boolean
  platformType!: PlatformType
  meta: {[key: string]: any} = {}
}

export class MenuCreate extends MenuCreateOrUpdate {
  layoutId!: string
  parentId?: string
}

export class MenuUpdate extends MenuCreateOrUpdate {

}

export class GetAllMenu implements ISortedResultRequest {
  filter = ''
  sorting = ''
  reverse = false
  parentId?: string
  layoutId?: string
  platformType?: PlatformType
}

export class GetMenuByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
  reverse = false
  layoutId?: string
  parentId?: string
  platformType?: PlatformType
}

export class Menu extends Route {
  code!: string
  layoutId!: string
  component!: string
  platformType!: PlatformType
  parentId?: string
  isPublic!: boolean
  children = new Array<Menu>()
}

export class RoleMenu {
  roleName!: string
  menuIds = new Array<string>()
}

export class UserMenu {
  userId!: string
  menuIds = new Array<string>()
}
