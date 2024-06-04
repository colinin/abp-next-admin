export interface PersistedGrant extends ExtensibleEntityDto<string> {
    key: string;
    type: string;
    subjectId: string;
    sessionId: string;
    description: string;
    consumedTime?: Date;
    clientId: string;
    creationTime: Date;
    expiration?: Date;
    data: string;
  }
  
  export interface GetPersistedGrantPagedRequest extends PagedAndSortedResultRequestDto {
    filter?: string;
    subjectId?: string;
  }
  