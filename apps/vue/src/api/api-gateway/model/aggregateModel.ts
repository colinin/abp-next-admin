import { PagedAndSortedResultRequestDto, PagedResultDto } from '../../model/baseModel';

export class AggregateRouteConfig {
  reRouteKey = '';
  parameter = '';
  jsonPath = '';
}

export class CreateAggregateRouteConfig extends AggregateRouteConfig {
  routeId = '';
}

export class AggregateRouteBase {
  reRouteKeys: string[] = [];
  upstreamPathTemplate = '';
  upstreamHost = '';
  reRouteIsCaseSensitive = true;
  aggregator = '';
  priority?: number;
  upstreamHttpMethod: string[] = [];
}

export class AggregateRoute extends AggregateRouteBase {
  appId = '';
  name = '';
  reRouteId = '';
  concurrencyStamp = '';
  reRouteKeysConfig: AggregateRouteConfig[] = [];
}

export class CreateAggregateRoute extends AggregateRouteBase {
  appId = '';
  name = '';
}

export class UpdateAggregateRoute extends AggregateRouteBase {
  routeId = '';
  concurrencyStamp = '';
}

export class GetAggregateRoutePagedRequest extends PagedAndSortedResultRequestDto {
  appId = '';
  filter = '';
}

export class AggregateRoutePagedResult extends PagedResultDto<AggregateRoute> {}
