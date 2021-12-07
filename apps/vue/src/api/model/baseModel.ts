export interface BasicPageParams {
  page: number;
  pageSize: number;
}

export interface BasicFetchResult<T> {
  items: T[];
  total: number;
}

export interface INameValue<T> {
  name: string;
  value: T;
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

/** 分页排序查询对象 */
export class PagedAndSortedResultRequestDto implements IPagedResultRequest, ISortedResultRequest {
  /** 查询页码 */
  skipCount: number;
  /** 单页最大返回数量 */
  maxResultCount: number;
  /** 排序字段 */
  sorting: string | undefined;

  constructor() {
    this.sorting = '';
    this.skipCount = 1;
    this.maxResultCount = 10;
  }
}

/** 创建实体审计对象 */
export class CreationAuditedEntityDto implements IMayHaveCreator {
  creatorId: string | undefined;
  creationTime!: Date;
}

/** 实体审计对象 */
export class AuditedEntityDto implements CreationAuditedEntityDto, IModificationAuditedObject {
  lastModifierId: string | undefined;
  lastModificationTime: Date | undefined;
  creatorId: string | undefined;
  creationTime!: Date;
}

/** 所有实体审计对象 */
export class FullAuditedEntityDto implements AuditedEntityDto, IDeletionAuditedObject {
  lastModifierId: string | undefined;
  lastModificationTime: Date | undefined;
  creatorId: string | undefined;
  creationTime!: Date;
  deleterId!: string;
  deletionTime: Date | undefined;
  isDeleted!: boolean;
}

/** 通用分页请求接口 */
export class PagedResultRequestDto implements IPagedResultRequest {
  skipCount: number;
  maxResultCount: number;

  constructor() {
    this.skipCount = 1;
    this.maxResultCount = 25;
  }
}

export class PagedResultDto<T> implements ListResultDto<T>, IPagedResult<T> {
  items!: T[];
  totalCount!: number;
}

/** 列表接口Dto */
export class ListResultDto<T> implements IListResult<T> {
  items!: T[];
}

export class ExtensibleObject implements IHasExtraProperties {
  extraProperties!: { [key: string]: any };
}

export class ExtensibleEntity<TKey> extends ExtensibleObject {
  id!: TKey;
}

export class ExtensibleCreationAuditedEntity<TKey> extends ExtensibleEntity<TKey> {
  creationTime!: Date;
  creatorId?: string;
}

export class ExtensibleAuditedEntity<TKey> extends ExtensibleCreationAuditedEntity<TKey> {
  lastModificationTime?: Date;
  lastModifierId?: string;
}

export interface IHasConcurrencyStamp {
  concurrencyStamp: string;
}

export interface IHasExtraProperties {
  extraProperties: { [key: string]: any };
}

/** 分页请求接口 */
export interface IPagedResultRequest extends ILimitedResultRequest {
  /** 当前页 */
  skipCount: number;
}

/** 最大请求接口 */
export interface ILimitedResultRequest {
  /** 当前页 */
  maxResultCount: number;
}

/** 返回列表接口 */
export interface IListResult<T> {
  /** 列表项 */
  items: T[];
}

/** 分页请求接口 */
export interface IPagedResult<T> extends IListResult<T> {
  /** 最大数据大小 */
  totalCount: number;
}

/** 排序请求接口 */
export interface ISortedResultRequest {
  /** 排序字段 */
  sorting: string | undefined;
}

/** 分页排序请求接口 */
export interface IPagedAndSortedResultRequest extends IPagedResultRequest, ISortedResultRequest {}

/** 创建人接口 */
export interface IMayHaveCreator extends IHasCreationTime {
  /** 创建人标识 */
  creatorId: string | undefined;
}

/** 创建时间接口 */
export interface IHasCreationTime {
  /** 创建时间 */
  creationTime: Date;
}

/** 修改人接口 */
export interface IModificationAuditedObject extends IHasModificationTime {
  /** 上次修改人标识 */
  lastModifierId: string | undefined;
}

/** 修改时间接口 */
export interface IHasModificationTime {
  /** 上次修改时间 */
  lastModificationTime: Date | undefined;
}

/** 删除人接口 */
export interface IDeletionAuditedObject extends IHasDeletionTime {
  /** 删除人标识 */
  deleterId: string;
}

/** 删除时间接口 */
export interface IHasDeletionTime extends ISoftDelete {
  /** 删除时间 */
  deletionTime: Date | undefined;
}

/** 软删除接口 */
export interface ISoftDelete {
  /** 是否已删除 */
  isDeleted: boolean;
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
