import VueEvents from './index'

declare module 'vue/types/vue' {
  interface Vue {
    $events: VueEvents
  }
}
