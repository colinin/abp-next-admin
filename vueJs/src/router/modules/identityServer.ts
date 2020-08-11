import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const identityServerRouter: RouteConfig = {
  path: '/identityServer',
  component: Layout,
  meta: {
    title: 'identityServer',
    icon: 'identity-server',
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
        icon: 'client',
        roles: ['IdentityServer.Clients']
      }
    },
    {
      path: 'apiresources',
      component: () => import('@/views/admin/identityServer/api-resources/index.vue'),
      name: 'apiresources',
      meta: {
        title: 'apiresources',
        icon: 'api',
        roles: ['IdentityServer.ApiResources']
      }
    },
    {
      path: 'identityresources',
      component: () => import('@/views/admin/identityServer/identity-resources/index.vue'),
      name: 'identityresources',
      meta: {
        title: 'identityresources',
        icon: 'identity',
        roles: ['IdentityServer.IdentityResources']
      }
    }
  ]
}

export default identityServerRouter
