import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const adminRouter: RouteConfig = {
  path: '/admin',
  component: Layout,
  meta: {
    title: 'admin',
    icon: 'admin',
    roles: ['AbpIdentity.Users', 'AbpIdentity.Roles', 'AbpSettingManagement.Settings', 'AbpTenantManagement.Tenants'], // you can set roles in root nav
    alwaysShow: true // will always show the root menu
  },
  children: [
    {
      path: 'settings',
      component: () => import(/* webpackChunkName: "settings" */ '@/views/admin/settings/index.vue'),
      name: 'settings',
      meta: {
        title: 'settings',
        icon: 'setting',
        roles: ['AbpSettingManagement.Settings']
      }
    },
    {
      path: 'users',
      component: () => import(/* webpackChunkName: "users" */ '@/views/admin/users/index.vue'),
      name: 'users',
      meta: {
        title: 'users',
        icon: 'user',
        roles: ['AbpIdentity.Users']
      }
    },
    {
      path: 'roles',
      component: () => import(/* webpackChunkName: "roles" */ '@/views/admin/roles/index.vue'),
      name: 'roles',
      meta: {
        title: 'roles',
        icon: 'role',
        roles: ['AbpIdentity.Roles']
      }
    },
    {
      path: 'tenants',
      component: () => import(/* webpackChunkName: "tenants" */ '@/views/admin/tenants/index.vue'),
      name: 'tenants',
      meta: {
        title: 'tenants',
        icon: 'tenant',
        roles: ['AbpTenantManagement.Tenants']
      }
    },
    {
      path: 'organization-unit',
      component: () => import(/* webpackChunkName: "organization-unit" */ '@/views/admin/organization-unit/index.vue'),
      name: 'organization-unit',
      meta: {
        title: 'organizationUnit',
        icon: 'organization-unit',
        roles: ['AbpIdentity.OrganizationUnits']
      }
    },
    {
      path: 'claim-type',
      component: () => import(/* webpackChunkName: "claim-type" */ '@/views/admin/claim-type/index.vue'),
      name: 'claim-type',
      meta: {
        title: 'claimType',
        icon: 'claim-type',
        roles: ['AbpIdentity.IdentityClaimTypes']
      }
    }
  ]
}

export default adminRouter
