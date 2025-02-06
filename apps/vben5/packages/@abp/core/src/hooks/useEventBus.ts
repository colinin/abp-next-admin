import type { EventType, Handler, WildcardHandler } from '../utils/mitt';

import mitt from '../utils/mitt';

const emitter = mitt();

interface EventBus {
  /** 发布事件 */
  publish(type: '*', event?: any): void;
  /** 发布事件 */
  publish<T = any>(type: EventType, event?: T): void;

  /** 订阅事件 */
  subscribe(type: '*', handler: WildcardHandler): void;
  /** 订阅事件 */
  subscribe<T = any>(type: EventType, handler: Handler<T>): void;

  /** 退订事件 */
  unSubscribe(type: '*', handler: WildcardHandler): void;
  /** 退订事件 */
  unSubscribe<T = any>(type: EventType, handler: Handler<T>): void;
}

export function useEventBus(): EventBus {
  return {
    publish: emitter.emit,
    subscribe: emitter.on,
    unSubscribe: emitter.off,
  };
}
