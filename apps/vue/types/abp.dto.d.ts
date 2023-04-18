declare interface ExtensibleObject {
  extraProperties: ExtraPropertyDictionary;
}

declare interface EntityDto<TPrimaryKey> {
  id: TPrimaryKey;
}

declare interface CreationAuditedEntityDto<TPrimaryKey> extends EntityDto<TPrimaryKey> {
  creationTime: Date;
  creatorId?: string;
}

declare interface CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends CreationAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
}

declare interface AuditedEntityDto<TPrimaryKey> extends CreationAuditedEntityDto<TPrimaryKey> {
  lastModificationTime?: Date;
  lastModifierId?: string;
}

declare interface AuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends AuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
}

declare interface ExtensibleEntityDto<TKey> extends ExtensibleObject {
  id: TKey;
}

declare interface ExtensibleCreationAuditedEntityDto<TPrimaryKey>
  extends ExtensibleEntityDto<TPrimaryKey> {
  creationTime: Date;
  creatorId?: string;
}

declare interface ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
}

declare interface ExtensibleAuditedEntityDto<TPrimaryKey>
  extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  lastModificationTime?: Date;
  lastModifierId?: string;
}

declare interface ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
}

declare interface ExtensibleFullAuditedEntityDto<TPrimaryKey>
  extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  isDeleted: boolean;
  deleterId?: string;
  deletionTime?: Date;
}

declare interface ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends ExtensibleFullAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
  deleter: TUserDto;
}

declare interface FullAuditedEntityDto<TPrimaryKey> extends AuditedEntityDto<TPrimaryKey> {
  isDeleted: boolean;
  deleterId?: string;
  deletionTime?: Date;
}

declare interface FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends FullAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
  deleter: TUserDto;
}

declare interface LimitedResultRequestDto {
  maxResultCount?: number;
}

declare interface ExtensibleLimitedResultRequestDto
  extends LimitedResultRequestDto,
    ExtensibleObject {}

declare interface ListResultDto<T> {
  items: T[];
}

declare interface ExtensibleListResultDto<T> extends ListResultDto<T>, ExtensibleObject {}

declare interface PagedResultDto<T> extends ListResultDto<T> {
  totalCount: number;
}

declare interface ExtensiblePagedResultDto<T> extends PagedResultDto<T>, ExtensibleObject {}

declare interface SortedResultRequest {
  sorting?: string;
}

declare interface PagedAndSortedResultRequestDto
  extends PagedResultRequestDto,
    SortedResultRequest {}

declare interface ExtensiblePagedAndSortedResultRequestDto
  extends PagedAndSortedResultRequestDto,
    ExtensibleObject {}

declare interface PagedResultRequestDto extends LimitedResultRequestDto {
  skipCount?: number;
}

declare interface ExtensiblePagedResultRequestDto extends PagedResultRequestDto, ExtensibleObject {}

declare interface ApplicationLocalizationResourceDto {
  texts: Dictionary<string, string>;
  baseResources: string[];
}

declare interface ApplicationLocalizationDto {
  resources: Dictionary<string, ApplicationLocalizationResourceDto>;
}

declare interface ApplicationLocalizationConfigurationDto {
  values: Dictionary<string, Dictionary<string, string>>;
  resources: Dictionary<string, ApplicationLocalizationResourceDto>;
  languages: LanguageInfo[];
  currentCulture: CurrentCulture;
  defaultResourceName: string;
  languagesMap: Dictionary<string, NameValue[]>;
  languageFilesMap: Dictionary<string, NameValue[]>;
}

declare interface ApplicationAuthConfigurationDto {
  grantedPolicies: Dictionary<string, boolean>;
}

declare interface ApplicationSettingConfigurationDto {
  values: Dictionary<string, string>;
}

declare interface ApplicationFeatureConfigurationDto {
  values: Dictionary<string, string>;
}

declare interface ApplicationGlobalFeatureConfigurationDto {
  enabledFeatures: string[];
}

declare interface TimingDto {
  timeZone: TimeZone;
}

declare interface LocalizableStringDto {
  name: string;
  resource?: string;
}

declare interface ExtensionPropertyApiGetDto {
  isAvailable: boolean;
}

declare interface ExtensionPropertyApiCreateDto {
  isAvailable: boolean;
}

declare interface ExtensionPropertyApiUpdateDto {
  isAvailable: boolean;
}

declare interface ExtensionPropertyUiTableDto {
  isAvailable: boolean;
}

declare interface ExtensionPropertyUiFormDto {
  isAvailable: boolean;
}

declare interface ExtensionPropertyUiLookupDto {
  url: string;
  resultListPropertyName: string;
  displayPropertyName: string;
  valuePropertyName: string;
  filterParamName: string;
}

declare interface ExtensionPropertyApiDto {
  onGet: ExtensionPropertyApiGetDto;
  onCreate: ExtensionPropertyApiCreateDto;
  onUpdate: ExtensionPropertyApiUpdateDto;
}

declare interface ExtensionPropertyUiDto {
  onTable: ExtensionPropertyUiTableDto;
  onCreateForm: ExtensionPropertyUiFormDto;
  onEditForm: ExtensionPropertyUiFormDto;
  lookup: ExtensionPropertyUiLookupDto;
}

declare interface ExtensionPropertyAttributeDto {
  typeSimple: string;
  config: Dictionary<string, any>;
}

declare interface ExtensionPropertyDto {
  type: string;
  typeSimple: string;
  displayName?: LocalizableStringDto;
  api: ExtensionPropertyApiDto;
  ui: ExtensionPropertyUiDto;
  attributes: ExtensionPropertyAttributeDto[];
  configuration: Dictionary<string, any>;
  defaultValue: any;
}

declare interface EntityExtensionDto {
  properties: Dictionary<string, ExtensionPropertyDto>;
  configuration: Dictionary<string, any>;
}

declare interface ModuleExtensionDto {
  entities: Dictionary<string, EntityExtensionDto>;
  configuration: Dictionary<string, any>;
}

declare interface ExtensionEnumFieldDto {
  name: string;
  value: any;
}

declare interface ExtensionEnumDto {
  fields: ExtensionEnumFieldDto[];
  localizationResource: string;
}

declare interface ObjectExtensionsDto {
  modules: Dictionary<string, ModuleExtensionDto>;
  enums: Dictionary<string, ExtensionEnumDto>;
}

declare interface ApplicationConfigurationDto extends IHasExtraProperties {
  localization: ApplicationLocalizationConfigurationDto;
  auth: ApplicationAuthConfigurationDto;
  setting: ApplicationSettingConfigurationDto;
  currentUser: CurrentUser;
  features: ApplicationFeatureConfigurationDto;
  globalFeatures: ApplicationGlobalFeatureConfigurationDto;
  multiTenancy: MultiTenancyInfo;
  currentTenant: CurrentTenant;
  timing: Timing;
  clock: Clock;
  objectExtensions: ObjectExtensionsDto;
}
