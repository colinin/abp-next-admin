import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const identityServerRouter: RouteConfig = {
  path: '/identityServer',
  component: Layout,
  meta: {
    title: 'identityServer',
    icon: 'identity-server',
    roles: ['AbpIdentityServer.Clients', 'AbpIdentityServer.ApiResources', 'AbpIdentityServer.IdentityResources'],
    alwaysShow: true
  },
  children: [
    {
      path: 'clients',
      component: () => import(/* webpackChunkName: "clients" */ '@/views/admin/identityServer/client/index.vue'),
      name: 'clients',
      meta: {
        title: 'clients',
        icon: 'client',
        roles: ['AbpIdentityServer.Clients']
      }
    },
    {
      path: 'api-resources',
      component: () => import(/* webpackChunkName: "api-resources" */ '@/views/admin/identityServer/api-resources/index.vue'),
      name: 'apiresources',
      meta: {
        title: 'apiresources',
        icon: 'api',
        roles: ['AbpIdentityServer.ApiResources']
      }
    },
    {
      path: 'identity-resources',
      component: () => import(/* webpackChunkName: "identity-resources" */ '@/views/admin/identityServer/identity-resources/index.vue'),
      name: 'identityresources',
      meta: {
        title: 'identityresources',
        icon: 'identity',
        roles: ['AbpIdentityServer.IdentityResources']
      }
    }
  ]
}

export default identityServerRouter
