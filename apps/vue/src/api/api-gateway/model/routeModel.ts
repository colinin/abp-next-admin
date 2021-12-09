import { PagedAndSortedResultRequestDto, PagedResultDto } from '../../model/baseModel';
import {
  AuthenticationOptions,
  FileCacheOptions,
  HostAndPort,
  HttpHandlerOptions,
  LoadBalancerOptions,
  QoSOptions,
  RateLimitRuleOptions,
  SecurityOptions,
} from './basicModel';

export class RouteBase {
  reRouteName = ''; // TODO: 需要修改名称
  downstreamPathTemplate = '';
  changeDownstreamPathTemplate?: { [key: string]: string };
  upstreamPathTemplate = '';
  upstreamHttpMethod!: string[];
  addHeadersToRequest?: { [key: string]: string };
  upstreamHeaderTransform?: { [key: string]: string };
  downstreamHeaderTransform?: { [key: string]: string };
  addClaimsToRequest?: { [key: string]: string };
  routeClaimsRequirement?: { [key: string]: string };
  addQueriesToRequest?: { [key: string]: string };
  requestIdKey? = '';
  reRouteIsCaseSensitive? = true;
  serviceName? = '';
  serviceNamespace? = '';
  downstreamScheme? = 'HTTP';
  downstreamHostAndPorts!: HostAndPort[];
  upstreamHost = '';
  key? = '';
  delegatingHandlers?: string[];
  priority? = 0;
  timeout? = 30000;
  dangerousAcceptAnyServerCertificateValidator?: boolean = true;
  downstreamHttpVersion? = '';
  downstreamHttpMethod? = '';
  securityOptions?: SecurityOptions;
  qoSOptions?: QoSOptions;
  rateLimitOptions?: RateLimitRuleOptions;
  loadBalancerOptions?: LoadBalancerOptions;
  fileCacheOptions?: FileCacheOptions;
  authenticationOptions?: AuthenticationOptions;
  httpHandlerOptions?: HttpHandlerOptions;
}

export class Route extends RouteBase {
  id!: number;
  appId!: string;
  reRouteId!: string;
  concurrencyStamp!: string;
}

export class CreateRoute extends RouteBase {
  appId = '';
}

export class UpdateRoute extends RouteBase {
  reRouteId = '';
  concurrencyStamp!: string;
}

export class GetRoutePagedRequest extends PagedAndSortedResultRequestDto {
  appId!: string;
  filter = '';
  sorting = 'ReRouteName';
}

export class RoutePagedResult extends PagedResultDto<Route> {}
