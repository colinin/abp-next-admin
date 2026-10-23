import type { PagedResultDto } from '@abp/core';

export interface DoneDto {
  id: string;
  instanceId: string;
  instanceTitle: string;
  templateName?: null | string;
  nodeId?: null | string;
  nodeName?: null | string;
  actionKey: string;
  actionLabel?: null | string;
  effect: string;
  comment?: null | string;
  actorDisplayName?: null | string;
  decidedAt: string;
  firstViewedAt?: null | string;
  completedAt?: null | string;
}

export interface DoneGetPagedListInput {
  filter?: null | string;
  maxResultCount?: number;
  skipCount?: number;
  sorting?: string;
}

export type DonePagedResult = PagedResultDto<DoneDto>;
