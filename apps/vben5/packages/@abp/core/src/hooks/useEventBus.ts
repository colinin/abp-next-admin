import type { EventType, Handler, WildcardHandler } from '../utils/mitt';

import mitt from '../utils/mitt';

const emitter = mitt();

interface EventBus {
  /** 发布事件 */
  publish: (type: EventType, event?: any) => void;

  /** 订阅事件 */
  subscribe: <T = any>(type: EventType, handler: Handler<T>) => void;

  /** 退订事件 */
  unSubscribe: {
    (type: '*', handler: WildcardHandler): void;
    <T = any>(type: EventType, handler: Handler<T>): void;
  };
}

export function useEventBus(): EventBus {
  return {
    publish: emitter.emit.bind(null),
    subscribe: emitter.on.bind(null),
    unSubscribe: emitter.off.bind(null),
  };
}
