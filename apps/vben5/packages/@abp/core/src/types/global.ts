/** 名称/值键值对 */
interface NameValue<T> {
  /** 名称 */
  name: string;
  /** 值 */
  value: T;
}
interface AuthConfig {
  audience?: string;
  authority: string;
  clientId: string;
  clientSecret: string;
}
interface Environment {
  authConfig: AuthConfig;
  dev: boolean;
  prod: boolean;
}
/** 本地化文本数据对象 */
interface LocalizableStringInfo {
  /** 文本名称 */
  name: string;
  /** 资源名称 */
  resourceName: string;
}
/** 键值对字典对象 */
type Dictionary<TKey extends number | string | symbol = any, KValue = any> = {
  [key in TKey]: KValue;
};
/** 扩展属性字典 */
interface ExtraPropertyDictionary {
  [key: string]: any;
}
/** 并发接口 */
interface IHasConcurrencyStamp {
  /** 并发令牌，更新数据时需携带 */
  concurrencyStamp?: string;
}
/** 扩展属性接口 */
interface IHasExtraProperties {
  /** 扩展属性 */
  extraProperties: ExtraPropertyDictionary;
}
/** 选择项 */
interface ISelectionStringValueItem {
  /** 选择项显示文本多语言对象 */
  displayText: LocalizableStringInfo;
  /** 选择项值 */
  value: string;
}
/** 选择项数据源 */
interface ISelectionStringValueItemSource {
  /** 选择项列表 */
  items: ISelectionStringValueItem[];
}
/** 验证器 */
interface Validator {
  /** 名称 */
  name: string;
  /** 属性 */
  properties: { [key: string]: string };
}
/** 值类型 */
interface ValueType {
  /** 名称 */
  name: string;
  /** 属性 */
  properties: { [key: string]: string };
  /** 验证器 */
  validator: Validator;
}
/** 选择项值类型 */
interface SelectionStringValueType extends ValueType {
  /** 选择项数据源 */
  itemSource: ISelectionStringValueItemSource;
}
/** 多语言描述 */
interface LanguageInfo {
  /**
   * 文化名称
   * @summary 简体中文：zh-Hans
   * @summary English：en-Us
   */
  cultureName: string;
  /** 显示名称 */
  displayName: string;
  /**
   * 两个字符描述名称
   * @summary 简体中文：zh
   * @summary English：en
   */
  twoLetterISOLanguageName: string;
  /** 用于UI的文化名称（仅后端） */
  uiCultureName: string;
}
/** 时间日期格式化描述 */
interface DateTimeFormat {
  /** 日历算法类型 */
  calendarAlgorithmType: string;
  /** 日期分隔符 */
  dateSeparator: string;
  /** 长时间日期格式 */
  dateTimeFormatLong: string;
  /** 完整时间日期格式 */
  fullDateTimePattern: string;
  /** 长时间格式 */
  longTimePattern: string;
  /** 短日期格式 */
  shortDatePattern: string;
  /** 短时间格式 */
  shortTimePattern: string;
}
/** 当前语言描述 */
interface CurrentCulture {
  /** 文化名称 */
  cultureName: string;
  /** 时间日期格式化配置 */
  dateTimeFormat: DateTimeFormat;
  /** 显示名称 */
  displayName: string;
  /** 英文名称 */
  englishName: string;
  /** 是否从右到左书写习惯 */
  isRightToLeft: boolean;
  /** 名称 */
  name: string;
  /** 本地名称 */
  nativeName: string;
  /** 三个字符描述名称 */
  threeLetterIsoLanguageName: string;
  /** 两个字符描述名称 */
  twoLetterIsoLanguageName: string;
}
/** Windows时区 */
interface WindowsTimeZone {
  /** 时区标识 */
  timeZoneId: string;
}
/** IANA时区 */
interface IanaTimeZone {
  /** 时区名称 */
  timeZoneName: string;
}
/** 时区描述 */
interface TimeZone {
  /** IANA时区配置 */
  iana: IanaTimeZone;
  /** Windows时区配置 */
  windows: WindowsTimeZone;
}
/** 时钟描述 */
interface Clock {
  /** 时钟类型
   * @summary UTC 格林威治时间
   * @summary Local 本地时间
   */
  kind: string;
}
/** 多语言描述 */
interface MultiTenancyInfo {
  /** 是否启用多租户 */
  isEnabled: boolean;
}
/** 当前租户 */
interface CurrentTenant {
  /** 租户标识 */
  id?: string;
  /** 租户是否可用 */
  isAvailable: boolean;
  /** 租户名称 */
  name?: string;
}
/** 当前用户 */
interface CurrentUser {
  /** 邮件地址 */
  email: string;
  /** 邮件是否已确认 */
  emailVerified: boolean;
  /** 用户标识 */
  id?: string;
  /** 模拟租户标识 */
  impersonatorTenantId?: string;
  /** 模拟租户名称 */
  impersonatorTenantName?: string;
  /** 模拟用户标识 */
  impersonatorUserId?: string;
  /** 模拟用户名称 */
  impersonatorUserName?: string;
  /** 是否已认证 */
  isAuthenticated: boolean;
  /** 名称 */
  name?: string;
  /** 手机号 */
  phoneNumber?: string;
  /** 手机号是否已确认 */
  phoneNumberVerified: boolean;
  /** 角色列表 */
  roles: string[];
  /** 会话标识 */
  sessionId?: string;
  /** 姓氏 */
  surName?: string;
  /** 租户标识 */
  tenantId?: string;
  /** 用户名 */
  userName: string;
}

interface IHasSimpleStateCheckers<
  TState extends IHasSimpleStateCheckers<TState>,
> {
  // eslint-disable-next-line no-use-before-define
  stateCheckers: ISimpleStateChecker<TState>[];
}

type SimpleStateRecord<
  TState extends IHasSimpleStateCheckers<TState>,
  TValue,
> = {
  [P in TState]: TValue;
};

type SimpleStateCheckerResult<TState extends IHasSimpleStateCheckers<TState>> =
  SimpleStateRecord<TState, boolean>;

interface SimpleStateCheckerContext<
  TState extends IHasSimpleStateCheckers<TState>,
> {
  state: TState;
}

interface SimpleBatchStateCheckerContext<
  TState extends IHasSimpleStateCheckers<TState>,
> {
  states: TState[];
}

interface ISimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>> {
  isEnabled(context: SimpleStateCheckerContext<TState>): boolean;
  serialize(): string | undefined;
}

interface ISimpleBatchStateChecker<
  TState extends IHasSimpleStateCheckers<TState>,
> {
  isEnabled(
    context: SimpleBatchStateCheckerContext<TState>,
  ): SimpleStateCheckerResult<TState>;
  serialize(): string | undefined;
}

interface ISimpleStateCheckerSerializer {
  deserialize<TState extends IHasSimpleStateCheckers<TState>>(
    jsonObject: any,
    state: TState,
  ): ISimpleStateChecker<TState> | undefined;
  deserializeArray<TState extends IHasSimpleStateCheckers<TState>>(
    value: string,
    state: TState,
  ): ISimpleStateChecker<TState>[];
  serialize<TState extends IHasSimpleStateCheckers<TState>>(
    checker: ISimpleStateChecker<TState>,
  ): string | undefined;
  serializeArray<TState extends IHasSimpleStateCheckers<TState>>(
    stateCheckers: ISimpleStateChecker<TState>[],
  ): string | undefined;
}

export type {
  Clock,
  CurrentCulture,
  CurrentTenant,
  CurrentUser,
  DateTimeFormat,
  Dictionary,
  Environment,
  ExtraPropertyDictionary,
  IanaTimeZone,
  IHasConcurrencyStamp,
  IHasExtraProperties,
  IHasSimpleStateCheckers,
  ISelectionStringValueItem,
  ISelectionStringValueItemSource,
  ISimpleBatchStateChecker,
  ISimpleStateChecker,
  ISimpleStateCheckerSerializer,
  LanguageInfo,
  LocalizableStringInfo,
  MultiTenancyInfo,
  NameValue,
  SelectionStringValueType,
  SimpleBatchStateCheckerContext,
  SimpleStateCheckerContext,
  SimpleStateCheckerResult,
  TimeZone,
  Validator,
  ValueType,
  WindowsTimeZone,
};
