import {
  FullAuditedEntityDto,
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';

export class RouteGroup extends FullAuditedEntityDto {
  id!: string;
  name!: string;
  appId!: string;
  appName!: string;
  appIpAddress?: string;
  description?: string;
  isActive!: boolean;
}

export class CreateOrUpdateRouteGroup {
  name = '';
  appId = '';
  appName = '';
  isActive = true;
  appIpAddress?: string = '';
  description?: string = '';
}

export class CreateRouteGroup extends CreateOrUpdateRouteGroup {}

export class UpdateRouteGroup extends CreateOrUpdateRouteGroup {}

export class GetRouteGroupPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
  sorting = 'AppId';
}

export class RouteGroupPagedResult extends PagedResultDto<RouteGroup> {}

export class RouteGroupListResult extends ListResultDto<RouteGroup> {}
