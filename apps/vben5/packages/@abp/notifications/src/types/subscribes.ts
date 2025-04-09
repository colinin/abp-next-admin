import type { PagedAndSortedResultRequestDto } from '@abp/core';

interface UserSubscreNotification {
  name: string;
}

interface UserSubscriptionsResult {
  isSubscribed: boolean;
}

type GetSubscriptionsPagedListInput = PagedAndSortedResultRequestDto;

export type {
  GetSubscriptionsPagedListInput,
  UserSubscreNotification,
  UserSubscriptionsResult,
};
