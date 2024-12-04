import {
  type Clock,
  type CurrentCulture,
  type CurrentTenant,
  type CurrentUser,
  type Dictionary,
  type ExtraPropertyDictionary,
  type IHasExtraProperties,
  type LanguageInfo,
  type MultiTenancyInfo,
  type NameValue,
  type TimeZone,
} from './global';
/** 包装器数据传输对象 */
interface WrapResult<T> {
  /**
   * 错误代码
   * @summary '0' 表示正常
   */
  code: string;
  /** 错误详情 */
  details?: string;
  /** 错误消息 */
  message: string;
  /** 返回数据 */
  result: T;
}
/** 远程服务验证错误描述 */
interface RemoteServiceValidationErrorInfo {
  /** 字段名称列表 */
  members: string[];
  /** 错误消息 */
  message: string;
}
/** 远程服务错误描述（仅适用于接口代理） */
interface RemoteServiceErrorInfo {
  /** 错误代码 */
  code?: string;
  /** 异常数据 */
  data?: { [key: string]: any };
  /** 错误详情 */
  details?: string;
  /** 错误消息 */
  message?: string;
  /** 验证错误列表 */
  validationErrors: RemoteServiceValidationErrorInfo[];
}
/** 扩展属性数据传输对象 */
interface ExtensibleObject {
  /** 扩展属性 */
  extraProperties: ExtraPropertyDictionary;
}
/** 实体数据传输对象 */
interface EntityDto<TPrimaryKey> {
  /** 实体标识 */
  id: TPrimaryKey;
}
/** 实体新增属性数据传输对象 */
interface CreationAuditedEntityDto<TPrimaryKey> extends EntityDto<TPrimaryKey> {
  /** 创建时间 */
  creationTime: Date;
  /** 创建人标识 */
  creatorId?: string;
}
/** 实体新增用户属性数据传输对象 */
interface CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends CreationAuditedEntityDto<TPrimaryKey> {
  /** 创建人实体数据传输对象 */
  creator: TUserDto;
}
/** 实体审计属性数据传输对象 */
interface AuditedEntityDto<TPrimaryKey>
  extends CreationAuditedEntityDto<TPrimaryKey> {
  /** 上次变更过时间 */
  lastModificationTime?: Date;
  /** 上次变更人 */
  lastModifierId?: string;
}
/** 实体审计用户属性数据传输对象 */
interface AuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends AuditedEntityDto<TPrimaryKey> {
  /** 创建人实体数据传输对象 */
  creator: TUserDto;
  /** 变更人实体数据传输对象 */
  lastModifier: TUserDto;
}
/** 实体审计全属性数据传输对象 */
interface FullAuditedEntityDto<TPrimaryKey>
  extends AuditedEntityDto<TPrimaryKey> {
  /** 删除人标识 */
  deleterId?: string;
  /** 删除时间 */
  deletionTime?: Date;
  /** 是否已删除 */
  isDeleted: boolean;
}
/** 实体审计用户全属性数据传输对象 */
interface FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends AuditedEntityWithUserDto<TPrimaryKey, TUserDto>,
    FullAuditedEntityDto<TPrimaryKey> {
  /** 删除人实体数据传输对象 */
  deleter: TUserDto;
}
/** 实体扩展属性数据传输对象 */
interface ExtensibleEntityDto<TKey> extends ExtensibleObject, EntityDto<TKey> {}
/** 实体新增扩展属性数据传输对象 */
interface ExtensibleCreationAuditedEntityDto<TPrimaryKey>
  extends CreationAuditedEntityDto<TPrimaryKey>,
    ExtensibleEntityDto<TPrimaryKey> {}
/** 实体新增用户扩展属性数据传输对象 */
interface ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>,
    ExtensibleEntityDto<TPrimaryKey> {}
/** 实体审计扩展属性数据传输对象 */
interface ExtensibleAuditedEntityDto<TPrimaryKey>
  extends AuditedEntityDto<TPrimaryKey>,
    ExtensibleEntityDto<TPrimaryKey> {}
/** 实体审计用户扩展属性数据传输对象 */
interface ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends AuditedEntityWithUserDto<TPrimaryKey, TUserDto>,
    ExtensibleEntityDto<TPrimaryKey> {}
/** 实体审计全属性扩展数据传输对象 */
interface ExtensibleFullAuditedEntityDto<TPrimaryKey>
  extends FullAuditedEntityDto<TPrimaryKey>,
    ExtensibleEntityDto<TPrimaryKey> {}
/** 实体审计用户全属性扩展数据传输对象 */
interface ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>,
    ExtensibleEntityDto<TPrimaryKey> {}
/** 最大请求数据传输对象 */
interface LimitedResultRequestDto {
  /** 最大返回数据大小 */
  maxResultCount?: number;
}
/** 最大请求扩展数据传输对象 */
interface ExtensibleLimitedResultRequestDto
  extends LimitedResultRequestDto,
    ExtensibleObject {}
/** 排序请求数据传输对象 */
interface SortedResultRequest {
  /** 排序字段
   * @summary 升序: 字段名称 + asc；降序: 字段名称 + desc
   * @example fieldA asc
   * @example fieldB desc
   */
  sorting?: string;
}
/** 分页请求数据传输对象 */
interface PagedResultRequestDto extends LimitedResultRequestDto {
  /**
   * 跳过数据大小
   * @summary 分页时跳过的数据大小，计算方式：（当前页 - 1）* 最大返回结果，不能小于0
   * @example (pageNum - 1) * pageSize
   */
  skipCount?: number;
}
/** 分页排序请求数据传输对象 */
interface PagedAndSortedResultRequestDto
  extends PagedResultRequestDto,
    SortedResultRequest {}
/** 分页排序请求扩展数据传输对象 */
interface ExtensiblePagedAndSortedResultRequestDto
  extends PagedAndSortedResultRequestDto,
    ExtensibleObject {}
/** 列表数据传输对象 */
interface ListResultDto<T> {
  /** 返回项目列表 */
  items: T[];
}
/** 列表扩展数据传输对象 */
interface ExtensibleListResultDto<T>
  extends ListResultDto<T>,
    ExtensibleObject {}
/** 分页列表数据传输对象 */
interface PagedResultDto<T> extends ListResultDto<T> {
  /** 符合条件的最大数量 */
  totalCount: number;
}
/** 分页列表扩展数据传输对象 */
interface ExtensiblePagedResultDto<T>
  extends PagedResultDto<T>,
    ExtensibleObject {}
/** 分页列表扩展数据传输对象 */
interface ExtensiblePagedResultRequestDto
  extends PagedResultRequestDto,
    ExtensibleObject {}
/** 应用程序本地化资源数据传输对象 */
interface ApplicationLocalizationResourceDto {
  /** 继承资源名称列表 */
  baseResources: string[];
  /** 本地化文本字典列表 */
  texts: Dictionary<string, string>;
}
/** 应用程序本地化数据传输对象 */
interface ApplicationLocalizationDto {
  /** 当前语言环境 */
  currentCulture: CurrentCulture;
  /** 资源字典列表 */
  resources: Dictionary<string, ApplicationLocalizationResourceDto>;
}
/** 应用程序本地化配置数据传输对象 */
interface ApplicationLocalizationConfigurationDto {
  /** 当前语言环境 */
  currentCulture: CurrentCulture;
  /** 默认本地化资源名称 */
  defaultResourceName: string;
  /** 多语言文件配置映射（仅后端） */
  languageFilesMap: Dictionary<string, NameValue<any>[]>;
  /** 支持的多语言列表 */
  languages: LanguageInfo[];
  /** 多语言资源映射（仅后端） */
  languagesMap: Dictionary<string, NameValue<any>[]>;
  /** 本地化资源字典列表 */
  resources: Dictionary<string, ApplicationLocalizationResourceDto>;
  /** 本地化资源文本字典列表 */
  values: Dictionary<string, Dictionary<string, string>>;
}
/** 应用程序授权数据传输对象 */
interface ApplicationAuthConfigurationDto {
  /** 已授权字典列表 */
  grantedPolicies: Dictionary<string, boolean>;
}
/** 应用程序设置数据传输对象 */
interface ApplicationSettingConfigurationDto {
  /** 设置值字典列表 */
  values: Dictionary<string, string>;
}
/** 应用程序功能数据传输对象 */
interface ApplicationFeatureConfigurationDto {
  /** 功能值字典列表 */
  values: Dictionary<string, string>;
}
/** 应用程序全局功能数据传输对象 */
interface ApplicationGlobalFeatureConfigurationDto {
  /** 已启用功能名称列表 */
  enabledFeatures: string[];
}
/** 应用程序时区数据传输对象 */
interface TimingDto {
  /** 时区配置定义 */
  timeZone: TimeZone;
}
/** 本地化文本数据对象 */
interface LocalizableStringDto {
  /** 文本名称 */
  name: string;
  /** 资源名称 */
  resource?: string;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyApiGetDto {
  isAvailable: boolean;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyApiCreateDto {
  isAvailable: boolean;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyApiUpdateDto {
  isAvailable: boolean;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyUiTableDto {
  isAvailable: boolean;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyUiFormDto {
  isAvailable: boolean;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyUiLookupDto {
  displayPropertyName: string;
  filterParamName: string;
  resultListPropertyName: string;
  url: string;
  valuePropertyName: string;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyApiDto {
  onCreate: ExtensionPropertyApiCreateDto;
  onGet: ExtensionPropertyApiGetDto;
  onUpdate: ExtensionPropertyApiUpdateDto;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyUiDto {
  lookup: ExtensionPropertyUiLookupDto;
  onCreateForm: ExtensionPropertyUiFormDto;
  onEditForm: ExtensionPropertyUiFormDto;
  onTable: ExtensionPropertyUiTableDto;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyAttributeDto {
  config: Dictionary<string, any>;
  typeSimple: string;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionPropertyDto {
  api: ExtensionPropertyApiDto;
  attributes: ExtensionPropertyAttributeDto[];
  configuration: Dictionary<string, any>;
  defaultValue: any;
  displayName?: LocalizableStringDto;
  type: string;
  typeSimple: string;
  ui: ExtensionPropertyUiDto;
}
/** 扩展属性（用于请求代理，忽略） */
interface EntityExtensionDto {
  configuration: Dictionary<string, any>;
  properties: Dictionary<string, ExtensionPropertyDto>;
}
/** 扩展属性（用于请求代理，忽略） */
interface ModuleExtensionDto {
  configuration: Dictionary<string, any>;
  entities: Dictionary<string, EntityExtensionDto>;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionEnumFieldDto {
  name: string;
  value: any;
}
/** 扩展属性（用于请求代理，忽略） */
interface ExtensionEnumDto {
  fields: ExtensionEnumFieldDto[];
  localizationResource: string;
}
/** 扩展属性（用于请求代理，忽略） */
interface ObjectExtensionsDto {
  enums: Dictionary<string, ExtensionEnumDto>;
  modules: Dictionary<string, ModuleExtensionDto>;
}
/** 应用程序配置数据传输对象 */
interface ApplicationConfigurationDto extends IHasExtraProperties {
  /** 授权配置 */
  auth: ApplicationAuthConfigurationDto;
  /** 时钟配置 */
  clock: Clock;
  /** 当前租户 */
  currentTenant: CurrentTenant;
  /** 当前用户 */
  currentUser: CurrentUser;
  /** 功能配置 */
  features: ApplicationFeatureConfigurationDto;
  /** 全局功能配置 */
  globalFeatures: ApplicationGlobalFeatureConfigurationDto;
  /** 本地化配置 */
  localization: ApplicationLocalizationConfigurationDto;
  /** 多租户配置 */
  multiTenancy: MultiTenancyInfo;
  /** 实体对象扩展配置 */
  objectExtensions: ObjectExtensionsDto;
  /** 系统设置 */
  setting: ApplicationSettingConfigurationDto;
  // timing: Timing;
}

export type {
  ApplicationAuthConfigurationDto,
  ApplicationConfigurationDto,
  ApplicationFeatureConfigurationDto,
  ApplicationGlobalFeatureConfigurationDto,
  ApplicationLocalizationConfigurationDto,
  ApplicationLocalizationDto,
  ApplicationLocalizationResourceDto,
  ApplicationSettingConfigurationDto,
  AuditedEntityDto,
  AuditedEntityWithUserDto,
  CreationAuditedEntityDto,
  CreationAuditedEntityWithUserDto,
  EntityDto,
  EntityExtensionDto,
  ExtensibleAuditedEntityDto,
  ExtensibleAuditedEntityWithUserDto,
  ExtensibleCreationAuditedEntityDto,
  ExtensibleCreationAuditedEntityWithUserDto,
  ExtensibleEntityDto,
  ExtensibleFullAuditedEntityDto,
  ExtensibleFullAuditedEntityWithUserDto,
  ExtensibleLimitedResultRequestDto,
  ExtensibleListResultDto,
  ExtensibleObject,
  ExtensiblePagedAndSortedResultRequestDto,
  ExtensiblePagedResultDto,
  ExtensiblePagedResultRequestDto,
  ExtensionEnumDto,
  ExtensionEnumFieldDto,
  ExtensionPropertyApiCreateDto,
  ExtensionPropertyApiDto,
  ExtensionPropertyApiGetDto,
  ExtensionPropertyApiUpdateDto,
  ExtensionPropertyAttributeDto,
  ExtensionPropertyDto,
  ExtensionPropertyUiDto,
  ExtensionPropertyUiFormDto,
  ExtensionPropertyUiLookupDto,
  ExtensionPropertyUiTableDto,
  FullAuditedEntityDto,
  FullAuditedEntityWithUserDto,
  LimitedResultRequestDto,
  ListResultDto,
  LocalizableStringDto,
  ModuleExtensionDto,
  NameValue,
  ObjectExtensionsDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
  PagedResultRequestDto,
  RemoteServiceErrorInfo,
  SortedResultRequest,
  TimingDto,
  WrapResult,
};
