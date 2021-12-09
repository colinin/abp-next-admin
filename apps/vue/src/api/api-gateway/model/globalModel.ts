import { PagedAndSortedResultRequestDto, PagedResultDto } from '../../model/baseModel';
import {
  HttpHandlerOptions,
  LoadBalancerOptions,
  QoSOptions,
  RateLimitOptions,
  ServiceDiscoveryProvider,
} from './basicModel';

export class GlobalConfigurationBase {
  baseUrl = '';
  requestIdKey?: string = '';
  downstreamScheme?: string = '';
  downstreamHttpVersion?: string = '';
  qoSOptions!: QoSOptions;
  rateLimitOptions!: RateLimitOptions;
  httpHandlerOptions!: HttpHandlerOptions;
  loadBalancerOptions!: LoadBalancerOptions;
  serviceDiscoveryProvider!: ServiceDiscoveryProvider;
}

export class GlobalConfiguration extends GlobalConfigurationBase {
  appId!: string;
  itemId!: string;
}

export class CreateGlobalConfiguration extends GlobalConfigurationBase {
  appId = '';
}

export class UpdateGlobalConfiguration extends GlobalConfigurationBase {
  itemId!: string;
}

export class GetGlobalPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}

export class GlobalConfigurationPagedResult extends PagedResultDto<GlobalConfiguration> {}
