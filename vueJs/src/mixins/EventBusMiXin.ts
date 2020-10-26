import { Component, Vue } from 'vue-property-decorator'

/**
 * 封装领域事件组件
 * 提供简单的防重复订阅以及组件销毁自动注销事件功能
 */
@Component
export default class EventBusMiXin extends Vue {
  protected eventMap = new Array<string>()

  /**
   * 组件销毁事件
   * 注销所有事件
   */
  destroyed() {
    this.$events.removeAll()
    this.eventMap.length = 0
  }

  /**
   * 订阅事件
   * @param name 事件名称
   * @param callback 回调函数
   */
  protected subscribe(name: string, callback: (eventData: any) => void) {
    if (this.eventMap.includes(name)) {
      return
    }
    this.eventMap.push(name)
    this.$events.on(name, callback)
  }

  /**
   * 订阅事件一次
   * @param name 事件名称
   * @param callback 回调函数
   */
  protected subscribeOne(name: string, callback: (eventData: any) => void) {
    this.$events.once(name, callback)
  }

  /**
   * 注销订阅事件
   * @param name 事件名称
   * @param callback 注销回调
   */
  protected unSubscribe(name: string) {
    this.$events.off(name, undefined)
  }

  /**
   * 注销所有事件
   */
  protected unSubscribeAll() {
    this.$events.removeAll()
  }

  /**
   * 触发事件
   * @param name 事件名称
   * @param args 事件参数列表
   */
  protected trigger(name: string, ...args: any[]) {
    this.$events.emit(name, ...args)
  }
}
