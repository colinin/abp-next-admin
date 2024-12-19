import type {
  Field,
  FieldBeetWeen,
  FieldContains,
  FieldDefineValidator,
  FieldLength,
  FieldMatch,
  FieldRange,
  FieldRegular,
  FieldValidator,
  Rule,
} from './validations';

/** 规则创建器 */
interface RuleCreator {
  /** 自定义验证器 */
  defineValidator(field: FieldDefineValidator): Rule[];
  /** input 与 value 是否匹配 */
  doNotMatch(field: FieldMatch): Rule[];
  /** 字段{0}不是有效的信用卡号码 */
  fieldDoNotValidCreditCardNumber(field: Field): Rule[];
  /** 字段{0}不是有效的邮箱地址 */
  fieldDoNotValidEmailAddress(field: Field): Rule[];
  /** 字段{0}不是有效的完全限定的http,https或ftp URL. */
  fieldDoNotValidFullyQualifiedUrl(field: Field): Rule[];
  /** 字段{0}不是有效的手机号码 */
  fieldDoNotValidPhoneNumber(field: Field): Rule[];
  /** 字段是无效值 */
  fieldInvalid(field: FieldValidator): Rule[];
  /** 验证未通过 */
  fieldIsNotValid(field: FieldValidator): Rule[];
  /** 字段{0}值必须在{1}和{2}范围内 */
  fieldMustBeetWeen(field: FieldBeetWeen): Rule[];
  /** 字段{0}必须是最大长度为'{1}'的字符串或数组 */
  fieldMustBeStringOrArrayWithMaximumLength(field: FieldLength): Rule[];
  /** 字段{0}必须是最小长度为'{1}'的字符串或数组 */
  fieldMustBeStringOrArrayWithMinimumLength(field: FieldLength): Rule[];
  /** 字段{0}必须是最大长度为{1}的字符串 */
  fieldMustBeStringWithMaximumLength(field: FieldLength): Rule[];
  /** 字段{0}必须是最小长度为{2}并且最大长度{1}的字符串 */
  fieldMustBeStringWithMinimumLengthAndMaximumLength(field: FieldRange): Rule[];
  /** 字段{0}与正则表达式不匹配 */
  fieldMustMatchRegularExpression(field: FieldRegular): Rule[];
  /** {0}字段只允许以下扩展名的文件: {1} */
  fieldOnlyAcceptsFilesExtensions(field: FieldContains): Rule[];
  /** 字段{0}不可为空 */
  fieldRequired(field: Field): Rule[];
}

export type { RuleCreator };
