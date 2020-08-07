import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const fileManagementRouter: RouteConfig = {
  path: '/file-management',
  component: Layout,
  meta: {
    title: 'filemanagement',
    icon: 'manager',
    roles: ['Abp.FileManagement.FileSystem'],
    alwaysShow: true
  },
  children: [
    {
      path: 'file-system',
      component: () => import('@/views/file-management/index.vue'),
      name: 'filesystem',
      meta: {
        title: 'filesystem',
        icon: 'file-system',
        roles: ['Abp.FileManagement.FileSystem']
      }
    }
  ]
}

export default fileManagementRouter
