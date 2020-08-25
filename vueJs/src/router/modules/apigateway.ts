import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'

const apigatewayRouter: RouteConfig = {
  path: '/apigateway',
  component: Layout,
  meta: {
    title: 'apigateway',
    icon: 'api-gateway',
    roles: ['ApiGateway.RouteGroup', 'ApiGateway.Global', 'ApiGateway.Route', 'ApiGateway.DynamicRoute', 'ApiGateway.AggregateRoute'],
    alwaysShow: true
  },
  children: [
    {
      path: 'group',
      component: () => import(/* webpackChunkName: "group" */ '@/views/admin/apigateway/group.vue'),
      name: 'group',
      meta: {
        title: 'group',
        icon: 'group',
        roles: ['ApiGateway.RouteGroup']
      }
    },
    {
      path: 'global',
      component: () => import(/* webpackChunkName: "global" */ '@/views/admin/apigateway/global.vue'),
      name: 'global',
      meta: {
        title: 'global',
        icon: 'global-setting',
        roles: ['ApiGateway.Global']
      }
    },
    {
      path: 'route',
      component: () => import(/* webpackChunkName: "route" */ '@/views/admin/apigateway/route.vue'),
      name: 'route',
      meta: {
        title: 'route',
        icon: 'route',
        roles: ['ApiGateway.Route']
      }
    },
    {
      path: 'aggregate-route',
      component: () => import(/* webpackChunkName: "aggregate-route" */ '@/views/admin/apigateway/aggregateRoute.vue'),
      name: 'aggregateRoute',
      meta: {
        title: 'aggregateRoute',
        icon: 'aggregate',
        roles: ['ApiGateway.AggregateRoute']
      }
    }
  ]
}

export default apigatewayRouter
