interface IRule {
  type?: string;
  trigger?: string;
}

export interface IField extends IRule {
  /** 字段名称 */
  name?: string;
  /** 字段前缀
   * @description 用于本地化字段名称
   * @example DisplayName 参与本地化字段结果 => L('ResourceName.DisplayName:Field')
   */
  prefix?: string;
  /** 连接符
   * @description 用于本地化字段名称时的连接字符
   * @example . 参与本地化字段结果 => L('ResourceName.DisplayName.Field')
   * @example : 参与本地化字段结果 => L('ResourceName.DisplayName:Field')
   */
  connector?: string;
  /** 本地化资源 */
  resourceName?: string;
}

export interface IFieldRequired extends IField {
  /** 是否必须 */
  required?: boolean;
}

export interface IFieldBeetWeen extends IField {
  /** 起始值 */
  start: number;
  /** 结束值 */
  end: number;
}

export interface IFieldLength extends IField {
  /** 长度 */
  length: number;
}

export interface IFieldRange extends IField {
  /** 最小数值 */
  minimum: number;
  /** 最大数值 */
  maximum: number;
}

export interface IFieldRegular extends IField {
  /** 正则表达式 */
  expression: string;
}

export interface IFieldMatch extends IFieldRequired {
  /** 字段名称 */
  name: string;
  /** 对比字段 */
  matchField: string;
  /** 对比字段值 */
  matchValue: string;
}

export interface IFieldContains extends IField {
  /** 验证的值中是否包含在定义的值中 */
  value: string;
}

export interface IFieldValidator extends IFieldRequired {
  /** 值是否有效验证器 */
  validator: (value: any) => boolean;
}
