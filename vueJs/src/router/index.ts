import Vue from 'vue'
import Router, { RouteConfig } from 'vue-router'

/* Layout */
import Layout from '@/layout/index.vue'

/* Router modules */
// import componentsRouter from './modules/components'
// import chartsRouter from './modules/charts'
// import tableRouter from './modules/table'
// import nestedRouter from './modules/nested'

Vue.use(Router)

/*
  Note: sub-menu only appear when children.length>=1
  Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
*/

/*
  name:'router-name'             the name field is required when using <keep-alive>, it should also match its component's name property
                                 detail see : https://vuejs.org/v2/guide/components-dynamic-async.html#keep-alive-with-Dynamic-Components
  redirect:                      if set to 'noredirect', no redirect action will be trigger when clicking the breadcrumb
  meta: {
    roles: ['admin', 'editor']   will control the page roles (allow setting multiple roles)
    title: 'title'               the name showed in subMenu and breadcrumb (recommend set)
    icon: 'svg-name'             the icon showed in the sidebar
    hidden: true                 if true, this route will not show in the sidebar (default is false)
    alwaysShow: true             if true, will always show the root menu (default is false)
                                 if false, hide the root menu when has less or equal than one children route
    breadcrumb: false            if false, the item will be hidden in breadcrumb (default is true)
    noCache: true                if true, the page will not be cached (default is false)
    affix: true                  if true, the tag will affix in the tags-view
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
*/

/**
  ConstantRoutes
  a base page that does not have permission requirements
  all roles can be accessed
*/
export const constantRoutes: RouteConfig[] = [
  {
    path: '/redirect',
    component: Layout,
    meta: { hidden: true },
    children: [
      {
        path: '/redirect/:path(.*)',
        component: () => import(/* webpackChunkName: "redirect" */ '@/views/redirect/index.vue')
      }
    ]
  },
  {
    path: '/login',
    component: () => import(/* webpackChunkName: "login" */ '@/views/login/index.vue'),
    meta: { hidden: true }
  },
  {
    path: '/auth-redirect',
    component: () => import(/* webpackChunkName: "auth-redirect" */ '@/views/login/auth-redirect.vue'),
    meta: { hidden: true }
  },
  {
    path: '/404',
    component: () => import(/* webpackChunkName: "404" */ '@/views/error-page/404.vue'),
    meta: { hidden: true }
  },
  {
    path: '/401',
    component: () => import(/* webpackChunkName: "401" */ '@/views/error-page/401.vue'),
    meta: { hidden: true }
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [
      {
        path: 'dashboard',
        component: () => import(/* webpackChunkName: "dashboard" */ '@/views/dashboard/index.vue'),
        name: 'Dashboard',
        meta: {
          title: 'dashboard',
          icon: 'dashboard',
          affix: true
        }
      }
    ]
  },
  // // {
  // //   path: '/documentation',
  // //   component: Layout,
  // //   children: [
  // //     {
  // //       path: 'index',
  // //       component: () => import(/* webpackChunkName: "documentation" */ '@/views/documentation/index.vue'),
  // //       name: 'Documentation',
  // //       meta: { title: 'documentation', icon: 'documentation', affix: true }
  // //     }
  // //   ]
  // // },
  // {
  //   path: '/guide',
  //   component: Layout,
  //   redirect: '/guide/index',
  //   children: [
  //     {
  //       path: 'index',
  //       component: () => import(/* webpackChunkName: "guide" */ '@/views/guide/index.vue'),
  //       name: 'Guide',
  //       meta: {
  //         title: 'guide',
  //         icon: 'guide',
  //         noCache: true
  //       }
  //     }
  //   ]
  // },
  {
    path: '/icon',
    component: Layout,
    children: [
      {
        path: 'index',
        component: () => import(/* webpackChunkName: "icons" */ '@/views/icons/index.vue'),
        name: 'Icons',
        meta: {
          title: 'icons',
          icon: 'icon',
          noCache: true
        }
      }
    ]
  },
  /** when your routing map is too long, you can split it into small modules **/
  // componentsRouter,
  // chartsRouter,
  // nestedRouter,
  // tableRouter,
  {
    path: '/error',
    component: Layout,
    redirect: 'noredirect',
    meta: {
      title: 'errorPages',
      icon: '404'
    },
    children: [
      {
        path: '401',
        component: () => import(/* webpackChunkName: "error-page-401" */ '@/views/error-page/401.vue'),
        name: 'Page401',
        meta: {
          title: 'page401',
          noCache: true
        }
      },
      {
        path: '404',
        component: () => import(/* webpackChunkName: "error-page-404" */ '@/views/error-page/404.vue'),
        name: 'Page404',
        meta: {
          title: 'page404',
          noCache: true
        }
      }
    ]
  }
]

export const asyncRoutes: RouteConfig[] = [
  {
    path: '/task',
    component: Layout,
    redirect: '/task',
    meta: {
      title: 'tasks',
      icon: 'lock',
      roles: ['AbpIdentity.Roles', 'TaskManagement.Task'], // you can set roles in root nav
      alwaysShow: true // will always show the root menu
    },
    children: [
      {
        path: 'list',
        component: () => import(/* webpackChunkName: "permission-page" */ '@/views/task/index.vue'),
        name: 'tasks',
        meta: {
          title: 'tasks',
          roles: ['AbpIdentity.Roles', 'TaskManagement.Task'] // or you can only set roles in sub nav
        }
      }
    ]
  },
  {
    path: '/permission',
    component: Layout,
    redirect: '/permission/directive',
    meta: {
      title: 'permission',
      icon: 'lock',
      roles: ['admin', 'editor', 'AbpIdentity.Roles'], // you can set roles in root nav
      alwaysShow: true // will always show the root menu
    },
    children: [
      {
        path: 'page',
        component: () => import(/* webpackChunkName: "permission-page" */ '@/views/permission/page.vue'),
        name: 'PagePermission',
        meta: {
          title: 'pagePermission',
          roles: ['admin'] // or you can only set roles in sub nav
        }
      },
      {
        path: 'directive',
        component: () => import(/* webpackChunkName: "permission-directive" */ '@/views/permission/directive.vue'),
        name: 'DirectivePermission',
        meta: {
          title: 'directivePermission'
          // if do not set roles, means: this page does not require permission
        }
      },
      {
        path: 'role',
        component: () => import(/* webpackChunkName: "permission-role" */ '@/views/permission/role.vue'),
        name: 'RolePermission',
        meta: {
          title: 'rolePermission',
          roles: ['admin']
        }
      }
    ]
  },
  {
    path: '/i18n',
    component: Layout,
    meta: {
      roles: ['AbpIdentity.Roles']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/i18n-demo/index.vue'),
        name: 'I18n',
        meta: {
          title: 'i18n',
          icon: 'international'
        }
      }
    ]
  },
  {
    path: '/apigateway',
    component: Layout,
    meta: {
      title: 'apigateway',
      icon: 'manager',
      roles: ['ApiGateway.RouteGroup', 'ApiGateway.Global', 'ApiGateway.Route', 'ApiGateway.DynamicRoute', 'ApiGateway.AggregateRoute'],
      alwaysShow: true
    },
    children: [
      {
        path: 'group',
        component: () => import('@/views/admin/apigateway/group.vue'),
        name: 'group',
        meta: {
          title: 'group',
          icon: 'group',
          roles: ['ApiGateway.RouteGroup']
        }
      },
      {
        path: 'global',
        component: () => import('@/views/admin/apigateway/global.vue'),
        name: 'global',
        meta: {
          title: 'global',
          icon: 'global',
          roles: ['ApiGateway.Global']
        }
      },
      {
        path: 'route',
        component: () => import('@/views/admin/apigateway/route.vue'),
        name: 'route',
        meta: {
          title: 'route',
          icon: 'route',
          roles: ['ApiGateway.Route']
        }
      },
      {
        path: 'aggregateRoute',
        component: () => import('@/views/admin/apigateway/aggregateRoute.vue'),
        name: 'aggregateRoute',
        meta: {
          title: 'aggregateRoute',
          icon: 'aggregateRoute',
          roles: ['ApiGateway.AggregateRoute']
        }
      }
    ]
  },
  {
    path: '/admin',
    component: Layout,
    meta: {
      title: 'admin',
      icon: 'manager',
      roles: ['AbpIdentity.Users', 'AbpIdentity.Roles'], // you can set roles in root nav
      alwaysShow: true // will always show the root menu
    },
    children: [
      {
        path: 'settings',
        component: () => import('@/views/admin/settings/index.vue'),
        name: 'settings',
        meta: {
          title: 'settings',
          icon: 'setting',
          roles: ['AbpSettingManagement.Settings']
        }
      },
      {
        path: 'users',
        component: () => import('@/views/admin/users/index.vue'),
        name: 'users',
        meta: {
          title: 'users',
          icon: 'user',
          roles: ['AbpIdentity.Users']
        }
      },
      {
        path: 'roles',
        component: () => import('@/views/admin/roles/index.vue'),
        name: 'roles',
        meta: {
          title: 'roles',
          icon: 'role',
          roles: ['AbpIdentity.Roles']
        }
      }
    ]
  },
  {
    path: '/profile',
    component: Layout,
    redirect: '/profile/index',
    meta: { hidden: true },
    children: [
      {
        path: 'index',
        component: () => import(/* webpackChunkName: "profile" */ '@/views/profile/index.vue'),
        name: 'Profile',
        meta: {
          title: 'profile',
          icon: 'user',
          noCache: true
        }
      }
    ]
  },
  {
    path: '/identityServer',
    component: Layout,
    meta: {
      title: 'identityServer',
      icon: 'manager',
      roles: ['IdentityServer.Clients', 'IdentityServer.ApiResources', 'IdentityServer.IdentityResources'],
      alwaysShow: true
    },
    children: [
      {
        path: 'clients',
        component: () => import('@/views/admin/identityServer/client/index.vue'),
        name: 'clients',
        meta: {
          title: 'clients',
          icon: 'clients',
          roles: ['IdentityServer.Clients']
        }
      },
      {
        path: 'apiresources',
        component: () => import('@/views/admin/identityServer/api-resources/index.vue'),
        name: 'apiresources',
        meta: {
          title: 'apiresources',
          icon: 'apiresources',
          roles: ['IdentityServer.ApiResources']
        }
      },
      {
        path: 'identityresources',
        component: () => import('@/views/admin/identityServer/identity-resources/index.vue'),
        name: 'identityresources',
        meta: {
          title: 'identityresources',
          icon: 'identityresources',
          roles: ['IdentityServer.IdentityResources']
        }
      }
    ]
  },
  {
    path: '*',
    component: () => import(/* webpackChunkName: "error-page-404" */ '@/views/error-page/404.vue'),
    meta: { hidden: true }
  }
]

const createRouter = () => new Router({
  // mode: 'history',  // Disabled due to Github Pages doesn't support this, enable this if you need.
  scrollBehavior: (to, from, savedPosition) => {
    if (savedPosition) {
      return savedPosition
    } else {
      return { x: 0, y: 0 }
    }
  },
  base: process.env.BASE_URL,
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter();
  (router as any).matcher = (newRouter as any).matcher // reset router
}

export default router
