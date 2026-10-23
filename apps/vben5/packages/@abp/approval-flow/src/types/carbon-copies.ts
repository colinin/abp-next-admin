import type { PagedResultDto } from '@abp/core';

export interface CarbonCopyDto {
  id: string;
  instanceId?: null | string;
  instanceReference?: null | string;
  nodeId?: null | string;
  title: string;
  summary?: null | string;
  senderDisplayName?: null | string;
  dataJson?: null | string;
  notifiedAt: string;
  isRead: boolean;
  readAt?: null | string;
}

export interface CarbonCopyGetPagedListInput {
  filter?: null | string;
  isRead?: boolean;
  maxResultCount?: number;
  skipCount?: number;
  sorting?: string;
}

export type CarbonCopyPagedResult = PagedResultDto<CarbonCopyDto>;
