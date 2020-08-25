import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const fileManagementRouter: RouteConfig = {
  path: '/file-management',
  component: Layout,
  meta: {
    title: 'filemanagement',
    icon: 'file-manager',
    roles: ['AbpFileManagement.FileSystem'],
    alwaysShow: true
  },
  children: [
    {
      path: 'file-system',
      component: () => import(/* webpackChunkName: "file-system" */  '@/views/file-management/index.vue'),
      name: 'filesystem',
      meta: {
        title: 'filesystem',
        icon: 'file-storage',
        roles: ['AbpFileManagement.FileSystem']
      }
    }
  ]
}

export default fileManagementRouter
