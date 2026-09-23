import type {
  ExtensibleEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface IdentitySessionDto extends ExtensibleEntityDto<string> {
  clientId?: string;
  device: string;
  deviceInfo: string;
  ipAddresses?: string;
  lastAccessed?: Date;
  sessionId: string;
  signedIn: Date;
  userId: string;
}

interface GetUserSessionsInput extends PagedAndSortedResultRequestDto {
  clientId?: string;
  device?: string;
  userId?: string;
}

interface GetMySessionsInput extends PagedAndSortedResultRequestDto {
  clientId?: string;
  device?: string;
}

export type { GetMySessionsInput, GetUserSessionsInput, IdentitySessionDto };
