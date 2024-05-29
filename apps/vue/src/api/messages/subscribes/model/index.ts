export interface UserSubscreNotification {
    name: string;
  }
  
  export interface UserSubscriptionsResult {
    isSubscribed: boolean;
  }
  
  export interface GetSubscriptionsPagedRequest extends PagedAndSortedResultRequestDto {}
