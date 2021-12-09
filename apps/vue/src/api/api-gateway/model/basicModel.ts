export class ServiceDiscoveryProvider {
  host!: string;
  port?: number;
  type!: string;
  token?: string;
  configurationKey?: string;
  pollingInterval?: number;
  namespace?: string;
  scheme?: string;
}

export class RateLimitOptions {
  clientIdHeader?: string;
  httpStatusCode?: number;
  quotaExceededMessage?: string;
  rateLimitCounterPrefix?: string;
  disableRateLimitHeaders!: boolean;
}

export class RateLimitRuleOptions {
  clientWhitelist?: string[] = [];
  enableRateLimiting!: boolean;
  period?: string;
  periodTimespan?: boolean;
  limit?: number;
}

export class QoSOptions {
  timeoutValue?: number = 10000;
  durationOfBreak?: number = 60000;
  exceptionsAllowedBeforeBreaking?: number = 50;
}

export class LoadBalancerOptions {
  type?: string = '';
  key?: string = '';
  expiry?: number = 0;
}

export class HostAndPort {
  host = '';
  port?: number = 80;
}

export class HttpHandlerOptions {
  useProxy = false;
  useTracing = false;
  allowAutoRedirect = false;
  useCookieContainer = false;
  maxConnectionsPerServer?: number = 0;
}

export class FileCacheOptions {
  ttlSeconds?: number = 0;
  region?: string = '';
}

export class AuthenticationOptions {
  authenticationProviderKey?: string = '';
  allowedScopes?: string[] = [];
}

export class SecurityOptions {
  ipAllowedList?: string[] = [];
  ipBlockedList?: string[] = [];
}

export class LoadBalancerDescriptor {
  type!: string;
  displayName!: string;
}

export const HttpMethods: { [key: string]: string } = {
  ['GET']: 'blue',
  ['POST']: 'green',
  ['PUT']: 'orange',
  ['DELETE']: 'red',
  ['OPTIONS']: 'cyan',
  ['PATCH']: 'pink',
};
