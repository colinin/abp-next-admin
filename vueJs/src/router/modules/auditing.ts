import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const auditingRouter: RouteConfig = {
  path: '/auditing',
  component: Layout,
  meta: {
    title: 'auditing',
    icon: 'auditing',
    roles: ['AbpAuditing.AuditLog', 'AbpAuditing.SecurityLog'], // you can set roles in root nav
    alwaysShow: true // will always show the root menu
  },
  children: [
    {
      path: 'audit-log',
      component: () => import(/* webpackChunkName: "settings" */ '@/views/admin/auditing/audit-log/index.vue'),
      name: 'auditLog',
      meta: {
        title: 'auditLog',
        icon: 'audit-log',
        roles: ['AbpAuditing.AuditLog']
      }
    },
    {
      path: 'security-log',
      component: () => import(/* webpackChunkName: "users" */ '@/views/admin/auditing/security-log/index.vue'),
      name: 'securityLog',
      meta: {
        title: 'securityLog',
        icon: 'security-log',
        roles: ['AbpAuditing.SecurityLog']
      }
    }
  ]
}

export default auditingRouter
