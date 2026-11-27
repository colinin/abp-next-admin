interface NotificationOptions {
  /** 通知标题 */
  title: string;
  /** 通知内容 */
  content?: string;
  /** 通知类型，默认 info */
  type?: 'error' | 'info' | 'success' | 'warning';
  /** 自动关闭时长(ms)，0 表示不自动关闭 */
  duration?: number;
  /** 是否可手动关闭，默认 true */
  closable?: boolean;
  /** 附加数据，订阅方可自行解释 */
  data?: Record<string, any>;
}

export const NOTIFICATION_EVENT = 'app:notifications' as const;

export type { NotificationOptions };
