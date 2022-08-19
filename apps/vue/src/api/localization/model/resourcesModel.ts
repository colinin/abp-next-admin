import {
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '/@/api/model/baseModel';

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

export class ResourceListResult extends ListResultDto<Resource> {}

export class ResourcePagedResult extends PagedResultDto<Resource> {}

export class GetResourcePagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}
