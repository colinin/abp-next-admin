import type { PagedResultDto } from '@abp/core';

export interface CarbonCopyDto {
  dataJson?: null | string;
  id: string;
  instanceId?: null | string;
  instanceReference?: null | string;
  isRead: boolean;
  nodeId?: null | string;
  notifiedAt: string;
  readAt?: null | string;
  senderDisplayName?: null | string;
  summary?: null | string;
  title: string;
}

export interface CarbonCopyGetPagedListInput {
  filter?: null | string;
  isRead?: boolean;
  maxResultCount?: number;
  skipCount?: number;
  sorting?: string;
}

export type CarbonCopyPagedResult = PagedResultDto<CarbonCopyDto>;
