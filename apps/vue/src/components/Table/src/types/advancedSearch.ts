/** 高级查询条件属性 */
export interface AdvanceSearchProps {
  /** 使用高级查询 */
  useAdvancedSearch?: boolean;
  /** 允许字段重复搜索 */
  allowDuplicateFieldSearch?: boolean;
  /** 字段列表api */
  defineFieldApi?: () => Promise<any>;
  /**
   * 自定义字段列表
   * @remarks 从服务器返回字段列表后自定义过滤函数,返回字段列表
   */
  defineFieldReplace?: (response: any) => DefineParamter[];
  /**
   * 字段列表api返回结果字段
   * @remarks 从服务器返回字段列表在数据结构中的字段名,默认: items
  */
  listField?: string;
  /** 高级查询api */
  fetchApi?: (...arg: any) => Promise<any>,
}

/** 选项 */
export interface ParamterOption {
  /** 键名 */
  key: string;
  /** 键值 */
  value: any;
}

/** 自定义字段 */
export interface DefineParamter {
  /** 字段名称 */
  name: string;
  /** 字段描述 */
  description?: string;
  /** 数据类型(后端) */
  type: string;
  /** 数据类型(js) */
  javaScriptType: string;
  /** 可用运算条件列表, 用于进一步约束字段可用比较符 */
  availableComparator?: DynamicComparison[];
  /** 选项 */
  options: ParamterOption[];
}

/** 连接条件 */
export enum DynamicLogic {
  /** 且 */
  And = 0,
  /** 或 */
  Or = 1
}

/** 运算条件 */
export enum DynamicComparison {
  /** 等于 */
  Equal = 0,
  /** 不等于 */
  NotEqual = 1,
  /** 小于 */
  LessThan = 2,
  /** 小于等于 */
  LessThanOrEqual = 3,
  /** 大于 */
  GreaterThan = 4,
  /** 大于等于 */
  GreaterThanOrEqual = 5,
  /** 左包含 */
  StartsWith = 6,
  /** 左不包含 */
  NotStartsWith = 7,
  /** 右包含 */
  EndsWith = 8,
  /** 右不包含 */
  NotEndsWith = 9,
  /** 包含 */
  Contains = 10,
  /** 不包含 */
  NotContains = 11,
  /** 空 */
  Null = 12,
  /** 非空 */
  NotNull = 13,
}

/** 动态查询字段 */
export interface DynamicParamter {
  /** 字段名称 */
  field: string;
  /** 连接条件 */
  logic: DynamicLogic;
  /** 运算条件 */
  comparison: DynamicComparison;
  /** 比较值 */
  value: any;
  /** 数据类型(js), 仅作为前端输入控件切换 */
  javaScriptType?: string;
}

/** 动态查询条件 */
export interface DynamicQueryable {
  /** 参数列表 */
  paramters: DynamicParamter[];
}
