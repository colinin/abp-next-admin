interface GetCacheKeysInput {
  filter?: string;
  marker?: string;
  prefix?: string;
}

interface CacheKeyInput {
  key: string;
}

interface CacheSetInput {
  absoluteExpiration?: Date;
  key: string;
  slidingExpiration?: Date;
  value?: string;
}

interface CacheRefreshInput {
  absoluteExpiration?: Date;
  key: string;
  slidingExpiration?: Date;
}

interface CacheKeysDto {
  keys: string[];
  nextMarker: string;
}

interface CacheValueDto {
  expiration?: Date;
  size: number;
  type: string;
  values: Record<string, any>;
}

interface CacheRemoveKeysInput {
  keys: string[];
}

export type {
  CacheKeyInput,
  CacheKeysDto,
  CacheRefreshInput,
  CacheRemoveKeysInput,
  CacheSetInput,
  CacheValueDto,
  GetCacheKeysInput,
};
