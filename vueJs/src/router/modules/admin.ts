import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const adminRouter: RouteConfig = {
  path: '/admin',
  component: Layout,
  meta: {
    title: 'admin',
    icon: 'manager',
    roles: ['AbpIdentity.Users', 'AbpIdentity.Roles', 'AbpSettingManagement.Settings', 'AbpTenantManagement.Tenants'], // you can set roles in root nav
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
    },
    {
      path: 'tenants',
      component: () => import('@/views/admin/tenants/index.vue'),
      name: 'tenants',
      meta: {
        title: 'tenants',
        icon: 'tenants',
        roles: ['AbpTenantManagement.Tenants']
      }
    }
  ]
}

export default adminRouter
