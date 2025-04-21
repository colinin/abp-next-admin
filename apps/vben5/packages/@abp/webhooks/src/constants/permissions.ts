/** 分组权限 */
export const GroupDefinitionsPermissions = {
  /** 新增 */
  Create: 'AbpWebhooks.GroupDefinitions.Create',
  Default: 'AbpWebhooks.GroupDefinitions',
  /** 删除 */
  Delete: 'AbpWebhooks.GroupDefinitions.Delete',
  /** 更新 */
  Update: 'AbpWebhooks.GroupDefinitions.Update',
};

/** Webhook定义权限 */
export const WebhookDefinitionsPermissions = {
  /** 新增 */
  Create: 'AbpWebhooks.Definitions.Create',
  Default: 'AbpWebhooks.Definitions',
  /** 删除 */
  Delete: 'AbpWebhooks.Definitions.Delete',
  /** 更新 */
  Update: 'AbpWebhooks.Definitions.Update',
};

/** Webhook订阅权限 */
export const WebhookSubscriptionPermissions = {
  /** 新增 */
  Create: 'AbpWebhooks.Subscriptions.Create',
  Default: 'AbpWebhooks.Subscriptions',
  /** 删除 */
  Delete: 'AbpWebhooks.Subscriptions.Delete',
  /** 更新 */
  Update: 'AbpWebhooks.Subscriptions.Update',
};

/** Webhook发送记录权限 */
export const WebhooksSendAttemptsPermissions = {
  Default: 'AbpWebhooks.SendAttempts',
  /** 删除 */
  Delete: 'AbpWebhooks.SendAttempts.Delete',
  /** 更新 */
  Resend: 'AbpWebhooks.SendAttempts.Resend',
};
