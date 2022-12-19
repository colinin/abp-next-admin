export interface Resource {
  name: string;
  displayName: string;
  description: string;
}

export interface ResourceCreateOrUpdate {
  enable: boolean;
  name: string;
  displayName: string;
  description: string;
}

export interface ResourceListResult extends ListResultDto<Resource> {}

export interface ResourcePagedResult extends PagedResultDto<Resource> {}

export interface GetResourcePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}
