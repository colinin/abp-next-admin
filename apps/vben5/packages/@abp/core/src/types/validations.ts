type RuleType =
  | 'boolean'
  | 'date'
  | 'email'
  | 'enum'
  | 'float'
  | 'hex'
  | 'integer'
  | 'method'
  | 'number'
  | 'object'
  | 'regexp'
  | 'string'
  | 'url';

interface Rule {
  [key: string]: any;
  trigger?: 'blur' | 'change' | ['change', 'blur'];
  type?: 'array' | RuleType;
}

interface Field extends Rule {
  /** 连接符
   * @description 用于本地化字段名称时的连接字符
   * @example . => L('ResourceName.DisplayName.Field')
   * @example : => L('ResourceName.DisplayName:Field')
   */
  connector?: string;
  /** 字段名称 */
  name?: string;
  /** 字段前缀
   * @description 用于本地化字段名称
   * @example DisplayName => L('ResourceName.DisplayName:Field')
   */
  prefix?: string;
  /** 本地化资源 */
  resourceName?: string;
}

interface FieldRequired extends Field {
  /** 是否必须 */
  required?: boolean;
}

interface FieldBeetWeen extends Field {
  /** 结束值 */
  end: number;
  /** 起始值 */
  start: number;
}

interface FieldLength extends Field {
  /** 长度 */
  length: number;
}

interface FieldRange extends Field {
  /** 最大数值 */
  maximum: number;
  /** 最小数值 */
  minimum: number;
}

interface FieldRegular extends Field {
  /** 正则表达式 */
  expression: string;
}

interface FieldMatch extends FieldRequired {
  /** 对比字段 */
  matchField: string;
  /** 对比字段值 */
  matchValue: string;
  /** 字段名称 */
  name: string;
}

interface FieldContains extends Field {
  /** 验证的值中是否包含在定义的值中 */
  value: string;
}

interface FieldValidator extends FieldRequired {
  /** 值是否有效验证器 */
  validator: (value: any) => boolean;
}

interface FieldDefineValidator extends FieldRequired {
  message?: string;
  validator: (
    rule: any,
    value: any,
    callback: any,
    source?: any,
    options?: any,
  ) => Promise<void> | void;
}

export type {
  Field,
  FieldBeetWeen,
  FieldContains,
  FieldDefineValidator,
  FieldLength,
  FieldMatch,
  FieldRange,
  FieldRegular,
  FieldRequired,
  FieldValidator,
  Rule,
  RuleType,
};
