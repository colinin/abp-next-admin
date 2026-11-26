interface ToastOptions {
  /** 提示内容 */
  message: string;
  /** 类型，默认 info */
  type?: 'error' | 'info' | 'loading' | 'success' | 'warning';
  /** 展示时长(ms)，默认 3000；0 表示不自动关闭 */
  duration?: number;
  /** 位置 */
  position?:
    | 'bottom'
    | 'bottom-left'
    | 'bottom-right'
    | 'center'
    | 'top'
    | 'top-left'
    | 'top-right';
  /** 附加数据 */
  data?: Record<string, any>;
}

export const TOAST_EVENT = 'app:toast' as const;

export type { ToastOptions };
