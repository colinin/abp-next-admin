export interface Resource {
  id: string;
  name: string;
  displayName: string;
  description: string;
}

export interface ResourceCreateOrUpdate {
  enable: boolean;
  displayName: string;
  description?: string;
  defaultCultureName?: string;
}

export interface ResourceCreate extends ResourceCreateOrUpdate {
  name: string;
}

export interface ResourceUpdate extends ResourceCreateOrUpdate {}


export interface ResourceListResult extends ListResultDto<Resource> {}

export interface ResourcePagedResult extends PagedResultDto<Resource> {}

export interface GetResourcePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface GetWithFilter {
  filter?: string;
}