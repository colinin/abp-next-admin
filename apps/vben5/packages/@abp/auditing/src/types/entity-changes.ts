import type {
  ExtraPropertyDictionary,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

export enum ChangeType {
  Created = 0,
  Deleted = 2,
  Updated = 1,
}

interface PropertyChange {
  id: string;
  newValue?: string;
  originalValue?: string;
  propertyName?: string;
  propertyTypeFullName?: string;
}

interface EntityChangeDto {
  [key: string]: any;
  changeTime?: Date;
  changeType: ChangeType;
  entityId?: string;
  entityTenantId?: string;
  entityTypeFullName?: string;
  extraProperties?: ExtraPropertyDictionary;
  id: string;
  propertyChanges?: PropertyChange[];
}

interface EntityChangeWithUsernameDto {
  entityChange: EntityChangeDto;
  userName?: string;
}

interface EntityChangeGetListInput extends PagedAndSortedResultRequestDto {
  auditLogId?: string;
  changeType?: ChangeType;
  endTime?: Date;
  entityId?: string;
  entityTypeFullName?: string;
  startTime?: Date;
}

interface EntityChangeGetWithUsernameInput {
  entityId?: string;
  entityTypeFullName?: string;
}

interface RestoreEntityInput {
  entityChangeId?: string;
  entityId: string;
}

export type {
  EntityChangeDto,
  EntityChangeGetListInput,
  EntityChangeGetWithUsernameInput,
  EntityChangeWithUsernameDto,
  PropertyChange,
  RestoreEntityInput,
};
