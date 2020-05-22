import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const taskRouter: RouteConfig = {
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
}

export default taskRouter
