import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import { RouteConfig } from 'vue-router'
// eslint-disable-next-line
import { asyncRoutes, constantRoutes } from '@/router'
import store from '@/store'
import { AbpModule } from '@/store/modules/abp'
import MenuService, { Menu } from '@/api/menu'
import { PlatformType } from '@/api/layout'
import { generateTree } from '@/utils'

const mapMetaBoolean = (key: string, meta: any) => {
  return typeof meta[key] === 'boolean' ? meta[key] : meta[key] === 'true'
}

const mapMetaArray = (key: string, meta: any) => {
  return Array.isArray(meta[key]) ? meta[key] : String(meta[key]).split(',')
}

const hasPermission = (roles: string[], route: RouteConfig) => {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

export const filterAsyncRoutes = (routes: RouteConfig[], roles: string[]) => {
  const res: RouteConfig[] = []
  routes.forEach(route => {
    const r = { ...route }
    if (hasPermission(roles, r)) {
      if (r.children) {
        r.children = filterAsyncRoutes(r.children, roles)
      }
      res.push(r)
    }
  })
  return res
}

const filterDynamicRoutes = (menus: Menu[]) => {
  const res: RouteConfig[] = []

  menus.forEach(menu => {
    const r: RouteConfig = {
      path: menu.path,
      name: menu.name,
      redirect: menu.redirect,
      // meta自行转换
      meta: {
        activeMenu: menu.meta.activeMenu,
        affix: mapMetaBoolean('affix', menu.meta), // 需要转换为正确的bool类型
        noCache: mapMetaBoolean('noCache', menu.meta),
        breadcrumb: mapMetaBoolean('breadcrumb', menu.meta),
        alwaysShow: mapMetaBoolean('alwaysShow', menu.meta),
        hidden: mapMetaBoolean('hidden', menu.meta),
        icon: menu.meta.icon,
        title: menu.meta.title,
        displayName: menu.displayName,
        roles: mapMetaArray('roles', menu.meta) // 需要转换为正确的array类型
      },
      component: resolve => require([`@/${menu.component}`], resolve) // 需要这种格式才可以正确加载动态路由
    }
    if (menu.children && menu.children.length > 0) {
      r.children = filterDynamicRoutes(menu.children)
    }
    res.push(r)
  })
  return res
}

export interface IPermissionState {
  routes: RouteConfig[]
  dynamicRoutes: RouteConfig[]
}

@Module({ dynamic: true, store, name: 'permission' })
class Permission extends VuexModule implements IPermissionState {
  public routes: RouteConfig[] = []
  public dynamicRoutes: RouteConfig[] = []
  public authorizedPermissions: string[] = []

  @Mutation
  private SET_ROUTES(routes: RouteConfig[]) {
    this.routes = constantRoutes.concat(routes)
    this.dynamicRoutes = routes
  }

  @Mutation
  private SET_AUTHPERMISSIONS(permissions: Array<string>) {
    this.authorizedPermissions = permissions
  }

  @Action
  public async RefreshPermissions() {
    const authPermissions = new Array<string>()
    const grantedPolicies = AbpModule.configuration.auth.grantedPolicies
    if (grantedPolicies) {
      Object.keys(grantedPolicies).forEach(key => {
        if (grantedPolicies[key]) {
          authPermissions.push(key)
        }
      })
    }
    if (authPermissions.length === 0) {
      // 防止没有任何权限无限刷新页面
      this.SET_AUTHPERMISSIONS(['guest'])
    } else {
      this.SET_AUTHPERMISSIONS(authPermissions)
    }
  }

  @Action
  public async GenerateRoutes() {
    await this.RefreshPermissions() // 保留授权
    // 没必要再针对admin角色授权,改成全部后台授权
    // if (this.authorizedPermissions.includes('admin')) {
    //   accessedRoutes = asyncRoutes
    // } else {
    //   accessedRoutes = filterAsyncRoutes(asyncRoutes, this.authorizedPermissions)
    // }

    // 取消注释用来启用后端动态路由配置
    const { items } = await MenuService.getMyMenuList(PlatformType.WebMvvm)
    const dynamicRoutes = filterDynamicRoutes(generateTree(items))
    this.SET_ROUTES(dynamicRoutes)

    // 取消注释用来启用前端动态路由配置
    // const accessedRoutes = filterAsyncRoutes(asyncRoutes, this.authorizedPermissions)
    // this.SET_ROUTES(accessedRoutes)
  }

  @Action ResetPermissions() {
    this.SET_AUTHPERMISSIONS([])
  }

  @Action ResetRoutes() {
    this.SET_ROUTES([])
  }
}

export const PermissionModule = getModule(Permission)
