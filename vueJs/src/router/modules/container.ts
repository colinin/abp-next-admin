import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const containerRouter: RouteConfig = {
  path: '/container',
  component: Layout,
  meta: {
    title: 'container',
    icon: 'container',
    roles: ['Platform.Layout', 'Platform.Menus'],
    alwaysShow: true
  },
  children: [
    {
      path: 'layouts',
      component: () => import(/* webpackChunkName: "layouts" */ '@/views/container/layouts/index.vue'),
      name: 'layouts',
      meta: {
        title: 'layouts',
        icon: 'layouts',
        roles: ['Platform.Layout']
      }
    },
    {
      path: 'menus',
      component: () => import(/* webpackChunkName: "menus" */ '@/views/container/menus/index.vue'),
      name: 'menus',
      meta: {
        title: 'menus',
        icon: 'menus',
        roles: ['ApiGateway.Menus']
      }
    }
  ]
}

export default containerRouter
