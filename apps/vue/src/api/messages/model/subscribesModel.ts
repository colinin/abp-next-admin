export interface UserSubscreNotification {
  name: string;
}

export interface UserSubscriptionsResult {
  isSubscribed: boolean;
}

export interface GetSubscriptionsPagedRequest extends PagedAndSortedResultRequestDto {}

export interface UserSubscreNotificationPagedResult
  extends PagedResultDto<UserSubscreNotification> {}

export interface UserSubscreNotificationListResult extends ListResultDto<UserSubscreNotification> {}
