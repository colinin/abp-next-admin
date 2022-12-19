export interface BasicPageParams {
  page: number;
  pageSize: number;
}

export interface BasicFetchResult<T> {
  items: T[];
  total: number;
}

export interface IAvailable {
  isAvailable: boolean;
}

export class Available implements IAvailable {
  isAvailable!: boolean;
}

/** 授权接口 */
export interface IPermission {
  /** 权限名称 */
  name: string;
  /** 是否授权 */
  isGranted: boolean;
}

/** 作用于接口 */
export interface IScope {
  /** 作用域 */
  scope: string;
}

/** 密钥 */
export interface ISecret {
  /** 密钥类型 */
  type: string;
  /** 密钥值 */
  value: string;
  /** 密钥说明 */
  description: string | undefined;
  /** 过期日期 */
  expiration: Date | undefined;
}

/** 令牌 */
export interface IClaim {
  /** 类型 */
  type: string;
  /** 数值 */
  value: string;
}

export class SecretBase implements ISecret {
  type = '';
  value = '';
  description = '';
  expiration: Date | undefined;
}

export enum HashType {
  Sha256,
  Sha512,
}

export class Claim implements IClaim {
  type = '';
  value = '';
}

export class KeyValue<TKey, TValue> {
  key!: TKey;
  value?: TValue;
}
