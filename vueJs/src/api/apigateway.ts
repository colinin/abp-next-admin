import ApiService from './serviceBase'
import { FullAuditedEntityDto, PagedResultDto, PagedAndSortedResultRequestDto, ListResultDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class ApiGateWay {
  public static getGlobalConfigurationByAppId(appId: string) {
    let _url = '/api/ApiGateway/Globals/By-AppId/'
    _url += appId
    return ApiService.Get<GlobalConfigurationDto>(_url, serviceUrl)
  }

  public static getGlobalConfigurations(payload: GlobalGetByPagedDto) {
    let _url = '/api/ApiGateway/Globals'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<GlobalConfigurationDto>>(_url, serviceUrl)
  }

  public static createGlobalConfiguration(payload: GlobalConfigurationCreateDto) {
    const _url = '/api/ApiGateway/Globals'
    return ApiService.Post<GlobalConfigurationDto>(_url, payload, serviceUrl)
  }

  public static updateGlobalConfiguration(payload: GlobalConfigurationUpdateDto) {
    const _url = '/api/ApiGateway/Globals'
    return ApiService.Put<GlobalConfigurationDto>(_url, payload, serviceUrl)
  }

  public static deleteGlobalConfiguration(appId: number) {
    let _url = '/api/ApiGateway/Globals'
    _url += '?appId=' + appId
    return ApiService.Delete(_url, serviceUrl)
  }

  public static getRouteGroupAppIds() {
    const _url = '/api/ApiGateway/RouteGroups/Actived'
    return ApiService.Get<ListResultDto<RouteGroupAppIdDto>>(_url, serviceUrl)
  }

  public static getRouteGroupByAppId(appId: string) {
    let _url = '/api/ApiGateway/RouteGroups/By-AppId/'
    _url += appId
    return ApiService.Get<RouteGroupDto>(_url, serviceUrl)
  }

  public static getRouteGroups(payload: RouteGroupGetByPagedDto) {
    let _url = '/api/ApiGateway/RouteGroups'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<RouteGroupDto>>(_url, serviceUrl)
  }

  public static createRouteGroup(payload: RouteGroupCreateDto) {
    const _url = '/api/ApiGateway/RouteGroups'
    return ApiService.Post<RouteGroupDto>(_url, payload, serviceUrl)
  }

  public static updateRouteGroup(payload: RouteGroupUpdateDto) {
    const _url = '/api/ApiGateway/RouteGroups'
    return ApiService.Put<RouteGroupDto>(_url, payload, serviceUrl)
  }

  public static deleteRouteGroup(appId: string) {
    let _url = '/api/ApiGateway/RouteGroups'
    _url += '?appId=' + appId
    return ApiService.Delete(_url, serviceUrl)
  }

  public static getReRouteByAppId(appId: string) {
    let _url = '/api/ApiGateway/Routes/By-AppId/'
    _url += appId
    return ApiService.Get<ReRouteDto>(_url, serviceUrl)
  }

  public static getReRouteByRouteId(routeId: number) {
    let _url = '/api/ApiGateway/Routes/By-RouteId/'
    _url += routeId
    return ApiService.Get<ReRouteDto>(_url, serviceUrl)
  }

  public static getReRouteByRouteName(routeName: string) {
    let _url = '/api/ApiGateway/Routes/By-RouteName/'
    _url += routeName
    return ApiService.Get<ReRouteDto>(_url, serviceUrl)
  }

  public static getReRoutes(payload: ReRouteGetByPagedDto) {
    let _url = '/api/ApiGateway/Routes'
    _url += '?appId=' + payload.appId
    _url += '&filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<ReRouteDto>>(_url, serviceUrl)
  }

  public static createReRoute(payload: ReRouteCreateDto) {
    const _url = '/api/ApiGateway/Routes'
    return ApiService.Post<ReRouteDto>(_url, payload, serviceUrl)
  }

  public static updateReRoute(payload: ReRouteUpdateDto) {
    const _url = '/api/ApiGateway/Routes'
    return ApiService.Put<ReRouteDto>(_url, payload, serviceUrl)
  }

  public static deleteReRoute(routeId: number) {
    let _url = '/api/ApiGateway/Routes'
    _url += '?routeId=' + routeId
    return ApiService.Delete(_url, serviceUrl)
  }

  public static clearReRoute(appId: string) {
    let _url = '/api/ApiGateway/Routes/Clear'
    _url += '?appId=' + appId
    return ApiService.Delete(_url, serviceUrl)
  }

  public static getAggregateReRoutes(payload: AggregateReRouteGetByPaged) {
    let _url = '/api/ApiGateway/Aggregates'
    _url += '?appId=' + payload.appId
    _url += '&filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<AggregateReRoute>>(_url, serviceUrl)
  }

  public static getAggregateReRouteByRouteId(routeId: string) {
    let _url = '/api/ApiGateway/Aggregates/'
    _url += routeId
    return ApiService.Get<AggregateReRoute>(_url, serviceUrl)
  }

  public static createAggregateReRoute(payload: AggregateReRouteCreate) {
    const _url = '/api/ApiGateway/Aggregates'
    return ApiService.Post<AggregateReRoute>(_url, payload, serviceUrl)
  }

  public static updateAggregateReRoute(payload: AggregateReRouteUpdate) {
    const _url = '/api/ApiGateway/Aggregates'
    return ApiService.Put<AggregateReRoute>(_url, payload, serviceUrl)
  }

  public static deleteAggregateReRoute(routeId: string) {
    let _url = '/api/ApiGateway/Aggregates'
    _url += '?routeId=' + routeId
    return ApiService.Delete(_url, serviceUrl)
  }

  public static createAggregateRouteConfig(payload: AggregateReRouteConfigCreate) {
    const _url = '/api/ApiGateway/Aggregates/RouteConfig'
    return ApiService.Post<AggregateReRouteConfig>(_url, payload, serviceUrl)
  }

  public static deleteAggregateRouteConfig(routeId: string, routeKey: string) {
    let _url = '/api/ApiGateway/Aggregates/RouteConfig'
    _url += '?routeId=' + routeId
    _url += '&ReRouteKey=' + routeKey
    return ApiService.Delete(_url, serviceUrl)
  }

  public static getDefinedAggregatorProviders() {
    const _url = '/api/ApiGateway/Basic/Aggregators'
    return ApiService.Get<ListResultDto<string>>(_url, serviceUrl)
  }

  public static getLoadBalancerProviders() {
    const _url = '/api/ApiGateway/Basic/LoadBalancers'
    return ApiService.Get<ListResultDto<LoadBalancerDescriptor>>(_url, serviceUrl)
  }
}

export class ServiceDiscoveryProvider {
  host!: string
  port?: number
  type!: string
  token?: string
  configurationKey?: string
  pollingInterval?: number
  namespace?: string
  scheme?: string
}

export class RateLimitOptions {
  clientIdHeader?: string
  httpStatusCode?: number
  quotaExceededMessage?: string
  rateLimitCounterPrefix?: string
  disableRateLimitHeaders!: boolean
}

export class RateLimitRuleOptions {
  clientWhitelist?: string[]
  enableRateLimiting!: boolean
  period?: string
  periodTimespan?: boolean
  limit?: number
  constructor() {
    this.clientWhitelist = new Array<string>()
  }
}

export class QoSOptions {
  timeoutValue?: number = 30000
  durationOfBreak?: number = 60000
  exceptionsAllowedBeforeBreaking?: number = 50
}

export class LoadBalancerOptions {
  type?: string = ''
  key?: string = ''
  expiry?: number = 0
}

export class HostAndPort {
  host = ''
  port?: number = 80
}

export class HttpHandlerOptions {
  useProxy = false
  useTracing = false
  allowAutoRedirect = false
  useCookieContainer = false
  maxConnectionsPerServer?: number = 0
}

export class FileCacheOptions {
  ttlSeconds?: number = 0
  region?: string = ''
}

export class AuthenticationOptions {
  authenticationProviderKey?: string = ''
  allowedScopes?: string[]
  constructor() {
    this.allowedScopes = new Array<string>()
  }
}

export class SecurityOptions {
  ipAllowedList?: string[]
  ipBlockedList?: string[]
  constructor() {
    this.ipAllowedList = new Array<string>()
    this.ipBlockedList = new Array<string>()
  }
}

export class GlobalConfigurationBase {
  baseUrl = ''
  requestIdKey?: string = ''
  downstreamScheme?: string = ''
  downstreamHttpVersion?: string = ''
  qoSOptions!: QoSOptions
  rateLimitOptions!: RateLimitOptions
  httpHandlerOptions!: HttpHandlerOptions
  loadBalancerOptions!: LoadBalancerOptions
  serviceDiscoveryProvider!: ServiceDiscoveryProvider

  constructor() {
    this.qoSOptions = new QoSOptions()
    this.rateLimitOptions = new RateLimitOptions()
    this.httpHandlerOptions = new HttpHandlerOptions()
    this.loadBalancerOptions = new LoadBalancerOptions()
    this.serviceDiscoveryProvider = new ServiceDiscoveryProvider()
  }

  public setBasicGlobalConfiguration(configuration: GlobalConfigurationBase) {
    this.baseUrl = configuration.baseUrl
    this.requestIdKey = configuration.requestIdKey
    this.downstreamScheme = configuration.downstreamScheme
    this.downstreamHttpVersion = configuration.downstreamHttpVersion
    this.qoSOptions = configuration.qoSOptions
    this.rateLimitOptions = configuration.rateLimitOptions
    this.httpHandlerOptions = configuration.httpHandlerOptions
    this.loadBalancerOptions = configuration.loadBalancerOptions
    this.serviceDiscoveryProvider = configuration.serviceDiscoveryProvider
  }
}

export class GlobalConfigurationDto extends GlobalConfigurationBase {
  appId!: string
  itemId!: string
}

export class GlobalConfigurationCreateDto extends GlobalConfigurationBase {
  appId = ''
}

export class GlobalConfigurationUpdateDto extends GlobalConfigurationBase {
  itemId!: string
}

export class GlobalGetByPagedDto extends PagedAndSortedResultRequestDto {
  filter?: string
  constructor() {
    super()
    this.filter = ''
  }
}

export class RouteGroupAppIdDto {
  appId!: string
  appName!: string
}

export class RouteGroupDto extends FullAuditedEntityDto {
  id!: string
  name!: string
  appId!: string
  appName!: string
  appIpAddress?: string
  description?: string
  isActive!: boolean
}

export class RouteGroupCreateDto {
  name = ''
  appId = ''
  appName = ''
  isActive = true
  appIpAddress?: string = ''
  description?: string = ''
  constructor() {
    this.isActive = true
  }
}

export class RouteGroupUpdateDto {
  name = ''
  appId = ''
  appName = ''
  isActive = true
  appIpAddress?: string = ''
  description?: string = ''
}

export class RouteGroupGetByPagedDto extends PagedAndSortedResultRequestDto {
  filter!: string
  constructor() {
    super()
    this.filter = ''
    this.sorting = 'AppId'
  }
}

export class ReRouteBase {
  reRouteName = ''
  downstreamPathTemplate = ''
  changeDownstreamPathTemplate?: {[key: string]: string}
  upstreamPathTemplate = ''
  upstreamHttpMethod!: string[]
  addHeadersToRequest?: {[key: string]: string}
  upstreamHeaderTransform?: {[key: string]: string}
  downstreamHeaderTransform?: {[key: string]: string}
  addClaimsToRequest?: {[key: string]: string}
  routeClaimsRequirement?: {[key: string]: string}
  addQueriesToRequest?: {[key: string]: string}
  requestIdKey? = ''
  reRouteIsCaseSensitive? = true
  serviceName? = ''
  serviceNamespace? = ''
  downstreamScheme? = 'HTTP'
  downstreamHostAndPorts!: HostAndPort[]
  upstreamHost = ''
  key? = ''
  delegatingHandlers?: string[]
  priority? = 0
  timeout? = 30000
  dangerousAcceptAnyServerCertificateValidator?: boolean = true
  downstreamHttpVersion? = ''
  downstreamHttpMethod? = ''
  securityOptions?: SecurityOptions
  qoSOptions?: QoSOptions
  rateLimitOptions?: RateLimitRuleOptions
  loadBalancerOptions?: LoadBalancerOptions
  fileCacheOptions?: FileCacheOptions
  authenticationOptions?: AuthenticationOptions
  httpHandlerOptions?: HttpHandlerOptions

  constructor() {
    this.upstreamHttpMethod = new Array<string>()
    this.downstreamHostAndPorts = new Array<HostAndPort>()

    this.changeDownstreamPathTemplate = {}
    this.addHeadersToRequest = {}
    this.upstreamHeaderTransform = {}
    this.downstreamHeaderTransform = {}
    this.addClaimsToRequest = {}
    this.routeClaimsRequirement = {}
    this.addQueriesToRequest = {}
    this.delegatingHandlers = new Array<string>()

    this.qoSOptions = new QoSOptions()
    this.fileCacheOptions = new FileCacheOptions()
    this.securityOptions = new SecurityOptions()
    this.rateLimitOptions = new RateLimitRuleOptions()
    this.loadBalancerOptions = new LoadBalancerOptions()
    this.httpHandlerOptions = new HttpHandlerOptions()
    this.authenticationOptions = new AuthenticationOptions()
  }

  public setBasicRoute(route: ReRouteBase) {
    this.reRouteName = route.reRouteName
    this.downstreamPathTemplate = route.downstreamPathTemplate
    this.changeDownstreamPathTemplate = route.changeDownstreamPathTemplate
    this.upstreamPathTemplate = route.upstreamPathTemplate
    this.upstreamHttpMethod = route.upstreamHttpMethod
    this.addHeadersToRequest = route.addHeadersToRequest
    this.upstreamHeaderTransform = route.upstreamHeaderTransform
    this.downstreamHeaderTransform = route.downstreamHeaderTransform
    this.addClaimsToRequest = route.addClaimsToRequest
    this.routeClaimsRequirement = route.routeClaimsRequirement
    this.addQueriesToRequest = route.addQueriesToRequest
    this.requestIdKey = route.requestIdKey
    this.reRouteIsCaseSensitive = route.reRouteIsCaseSensitive
    this.serviceName = route.serviceName
    this.serviceNamespace = route.serviceNamespace
    this.downstreamScheme = route.downstreamScheme
    this.downstreamHostAndPorts = route.downstreamHostAndPorts
    this.upstreamHost = route.upstreamHost
    this.key = route.key
    this.delegatingHandlers = route.delegatingHandlers
    this.priority = route.priority
    this.timeout = route.timeout
    this.dangerousAcceptAnyServerCertificateValidator = route.dangerousAcceptAnyServerCertificateValidator
    this.downstreamHttpVersion = route.downstreamHttpVersion
    this.downstreamHttpMethod = route.downstreamHttpMethod
    this.securityOptions = route.securityOptions
    this.qoSOptions = route.qoSOptions
    this.rateLimitOptions = route.rateLimitOptions
    this.loadBalancerOptions = route.loadBalancerOptions
    this.fileCacheOptions = route.fileCacheOptions
    this.authenticationOptions = route.authenticationOptions
    this.httpHandlerOptions = route.httpHandlerOptions
  }
}

export class ReRouteDto extends ReRouteBase {
  id!: number
  appId!: string
  reRouteId!: string
  concurrencyStamp!: string
}

export class ReRouteCreateDto extends ReRouteBase {
  appId = ''
}

export class ReRouteUpdateDto extends ReRouteBase {
  reRouteId = ''
  concurrencyStamp!: string
}

export class ReRouteGetByPagedDto extends PagedAndSortedResultRequestDto {
  appId!: string
  filter?: string
  constructor() {
    super()
    this.filter = ''
    this.sorting = 'ReRouteName'
  }
}

export class AggregateReRouteConfig {
  reRouteKey = ''
  parameter = ''
  jsonPath = ''
}

export class AggregateReRouteConfigCreate extends AggregateReRouteConfig {
  routeId = ''

  public static empty() {
    return new AggregateReRouteConfigCreate()
  }
}

export class AggregateReRouteBase {
  reRouteKeys!: string[]
  upstreamPathTemplate = ''
  upstreamHost = ''
  reRouteIsCaseSensitive = true
  aggregator = ''
  priority?: number
  upstreamHttpMethod!: string[]

  constructor() {
    this.reRouteKeys = new Array<string>()
    this.upstreamHttpMethod = new Array<string>()
  }
}

export class AggregateReRoute extends AggregateReRouteBase {
  appId = ''
  name = ''
  reRouteId = ''
  concurrencyStamp = ''
  reRouteKeysConfig!: AggregateReRouteConfig[]

  constructor() {
    super()
    this.reRouteKeysConfig = new Array<AggregateReRouteConfig>()
  }

  public static empty() {
    return new AggregateReRoute()
  }
}

export class AggregateReRouteCreate extends AggregateReRouteBase {
  appId = ''
  name = ''

  public static empty() {
    return new AggregateReRouteCreate()
  }

  public static create(route: AggregateReRoute) {
    const aggregateRoute = new AggregateReRouteCreate()
    aggregateRoute.appId = route.appId
    aggregateRoute.name = route.name
    aggregateRoute.aggregator = route.aggregator
    aggregateRoute.priority = route.priority
    aggregateRoute.reRouteIsCaseSensitive = route.reRouteIsCaseSensitive
    aggregateRoute.reRouteKeys = route.reRouteKeys
    aggregateRoute.upstreamHost = route.upstreamHost
    aggregateRoute.upstreamHttpMethod = route.upstreamHttpMethod
    aggregateRoute.upstreamPathTemplate = route.upstreamPathTemplate
    return aggregateRoute
  }
}

export class AggregateReRouteUpdate extends AggregateReRouteBase {
  routeId = ''
  concurrencyStamp = ''

  public static empty() {
    return new AggregateReRouteUpdate()
  }

  public static create(route: AggregateReRoute) {
    const aggregateRoute = new AggregateReRouteUpdate()
    aggregateRoute.aggregator = route.aggregator
    aggregateRoute.concurrencyStamp = route.concurrencyStamp
    aggregateRoute.priority = route.priority
    aggregateRoute.reRouteIsCaseSensitive = route.reRouteIsCaseSensitive
    aggregateRoute.reRouteKeys = route.reRouteKeys
    aggregateRoute.routeId = route.reRouteId
    aggregateRoute.upstreamHost = route.upstreamHost
    aggregateRoute.upstreamHttpMethod = route.upstreamHttpMethod
    aggregateRoute.upstreamPathTemplate = route.upstreamPathTemplate
    return aggregateRoute
  }
}

export class AggregateReRouteGetByPaged extends PagedAndSortedResultRequestDto {
  appId = ''
  filter = ''

  public static empty() {
    return new AggregateReRouteGetByPaged()
  }
}

export class LoadBalancerDescriptor {
  type!: string
  displayName!: string
}
