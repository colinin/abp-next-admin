export enum ChangeType {
    Created = 0,
    Updated = 1,
    Deleted = 2,
  }
  
  export interface PropertyChange {
    id: string;
    newValue?: string;
    originalValue?: string;
    propertyName?: string;
    propertyTypeFullName?: string;
  }
  
  export interface EntityChangeDto {
    id: string;
    changeTime?: Date;
    changeType: ChangeType;
    entityTenantId?: string;
    entityId?: string;
    entityTypeFullName?: string;
    propertyChanges?: PropertyChange[];
    extraProperties?: ExtraPropertyDictionary;
  }
  
  export interface EntityChangeWithUsernameDto {
    entityChange: EntityChangeDto;
    userName?: string;
  }
  
  export interface EntityChangeGetByPagedDto extends PagedAndSortedResultRequestDto {
    auditLogId?: string;
    startTime?: Date;
    endTime?: Date;
    changeType?: ChangeType;
    entityId?: string;
    entityTypeFullName?: string;
  }
  
  export interface EntityChangeGetWithUsernameInput {
    entityId?: string;
    entityTypeFullName?: string;
  }
  
  export interface RestoreEntityInput {
    entityId: string;
    entityChangeId?: string;
  }
  