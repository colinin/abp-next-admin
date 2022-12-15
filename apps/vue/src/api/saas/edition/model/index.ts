export interface EditionCreateDto extends EditionCreateOrUpdateBase {
}

export interface EditionGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface EditionUpdateDto extends EditionCreateOrUpdateBase {
  concurrencyStamp?: string;
}

export interface EditionCreateOrUpdateBase extends ExtensibleObject {
  displayName: string;
}

export interface EditionDto extends ExtensibleAuditedEntityDto<string> {
  displayName?: string;
  concurrencyStamp?: string;
}

