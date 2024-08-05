export interface IdentitySessionDto extends EntityDto<string> {
  sessionId: string;
  device: string;
  deviceInfo: string;
  userId: string;
  clientId?: string;
  ipAddresses?: string;
  signedIn: Date;
  lastAccessed?: Date;
}

export interface GetUserSessionsInput extends PagedAndSortedResultRequestDto {
  userId?: string;
  device?: string;
  clientId?: string;
}