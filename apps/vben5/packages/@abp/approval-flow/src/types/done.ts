import type { PagedResultDto } from '@abp/core';

export interface DoneDto {
  actionKey: string;
  actionLabel?: null | string;
  actorDisplayName?: null | string;
  comment?: null | string;
  completedAt?: null | string;
  decidedAt: string;
  effect: string;
  firstViewedAt?: null | string;
  id: string;
  instanceId: string;
  instanceTitle: string;
  nodeId?: null | string;
  nodeName?: null | string;
  templateName?: null | string;
}

export interface DoneGetPagedListInput {
  filter?: null | string;
  maxResultCount?: number;
  skipCount?: number;
  sorting?: string;
}

export type DonePagedResult = PagedResultDto<DoneDto>;
