export interface CacheKeys {
  nextMarker: string;
  keys: string[],
}

export interface CacheValue {
  type: string;
  size: number;
  expiration?: Date;
  values: {[key:string]: any};
}

export interface CacheRefreshRequest {
  key: string;
  absoluteExpiration?: Date;
  slidingExpiration?: Date;
}

export interface GetCacheKeysRequest {
  prefix?: string;
  marker?: string;
  filter?: string;
}
