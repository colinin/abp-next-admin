import type { ValidationRule } from 'ant-design-vue/lib/form/Form';
import {
  IField,
  IFieldBeetWeen,
  IFieldContains,
  IFieldLength,
  IFieldRange,
  IFieldMatch,
  IFieldRegular,
  IFieldValidator,
} from './typing';

import { useLocalization } from './useLocalization';
import { isEmail, isPhone } from '/@/utils/is';

export enum ValidationEnum {
  DoNotMatch = "'{0}' and '{1}' do not match.",
  FieldInvalid = 'The field {0} is invalid.',
  FieldIsNotValid = '{0} is not valid.',
  FieldMustBeStringOrArrayWithMaximumLength = "The field {0} must be a string or array type with a maximum length of '{1}'.",
  FieldMustBeStringOrArrayWithMinimumLength = "The field {0} must be a string or array type with a minimum length of '{1}'.",
  FieldMustBeStringWithMaximumLength = 'The field {0} must be a string with a maximum length of {1}.',
  FieldMustBeStringWithMinimumLengthAndMaximumLength = 'The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.',
  FieldMustBeetWeen = 'The field {0} must be between {1} and {2}.',
  FieldMustMatchRegularExpression = "The field {0} must match the regular expression '{1}'.",
  FieldDoNotValidCreditCardNumber = 'The {0} field is not a valid credit card number.',
  FieldDoNotValidEmailAddress = 'The {0} field is not a valid e-mail address.',
  FieldDoNotValidFullyQualifiedUrl = 'The {0} field is not a valid fully-qualified http, https, or ftp URL.',
  FieldDoNotValidPhoneNumber = 'The {0} field is not a valid phone number.',
  FieldRequired = 'The {0} field is required.',
  FieldOnlyAcceptsFilesExtensions = 'The {0} field only accepts files with the following extensions: {1}',
  ThisFieldIsInvalid = 'ThisFieldIsInvalid.',
  ThisFieldIsNotAValidCreditCardNumber = 'ThisFieldIsNotAValidCreditCardNumber.',
  ThisFieldIsNotAValidEmailAddress = 'ThisFieldIsNotAValidEmailAddress.',
  ThisFieldIsNotAValidFullyQualifiedHttpHttpsOrFtpUrl = 'ThisFieldIsNotAValidFullyQualifiedHttpHttpsOrFtpUrl',
  ThisFieldIsNotAValidPhoneNumber = 'ThisFieldIsNotAValidPhoneNumber.',
  ThisFieldIsNotValid = 'ThisFieldIsNotValid.',
  ThisFieldIsRequired = 'ThisFieldIsRequired.',
  ThisFieldMustBeAStringOrArrayTypeWithAMaximumLength = 'ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthOf{0}',
  ThisFieldMustBeAStringOrArrayTypeWithAMinimumLength = 'ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}',
  ThisFieldMustBeAStringWithAMaximumLength = 'ThisFieldMustBeAStringWithAMaximumLengthOf{0}',
  ThisFieldMustBeAStringWithAMinimumLengthAndAMaximumLength = 'ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}',
  ThisFieldMustBeBetween = 'ThisFieldMustBeBetween{0}And{1}',
  ThisFieldMustMatchTheRegularExpression = 'ThisFieldMustMatchTheRegularExpression{0}',
  ThisFieldOnlyAcceptsFilesWithTheFollowingExtensions = 'ThisFieldOnlyAcceptsFilesWithTheFollowingExtensions:{0}',
}

/** 规则创建器 */
interface IRuleCreator {
  /** input 与 value 是否匹配 */
  doNotMatch(field: IFieldMatch): ValidationRule[];
  /** 字段是无效值 */
  fieldInvalid(field: IFieldValidator): ValidationRule[];
  /** 验证未通过 */
  fieldIsNotValid(field: IFieldValidator): ValidationRule[];
  /** 字段{0}必须是最大长度为'{1}'的字符串或数组 */
  fieldMustBeStringOrArrayWithMaximumLength(field: IFieldLength): ValidationRule[];
  /** 字段{0}必须是最小长度为'{1}'的字符串或数组 */
  fieldMustBeStringOrArrayWithMinimumLength(field: IFieldLength): ValidationRule[];
  /** 字段{0}必须是最大长度为{1}的字符串 */
  fieldMustBeStringWithMaximumLength(field: IFieldLength): ValidationRule[];
  /** 字段{0}必须是最小长度为{2}并且最大长度{1}的字符串 */
  fieldMustBeStringWithMinimumLengthAndMaximumLength(field: IFieldRange): ValidationRule[];
  /** 字段{0}值必须在{1}和{2}范围内 */
  fieldMustBeetWeen(field: IFieldBeetWeen): ValidationRule[];
  /** 字段{0}与正则表达式不匹配 */
  fieldMustMatchRegularExpression(field: IFieldRegular): ValidationRule[];
  /** 字段{0}不是有效的信用卡号码 */
  fieldDoNotValidCreditCardNumber(field: IField): ValidationRule[];
  /** 字段{0}不是有效的邮箱地址 */
  fieldDoNotValidEmailAddress(field: IField): ValidationRule[];
  /** 字段{0}不是有效的完全限定的http,https或ftp URL. */
  fieldDoNotValidFullyQualifiedUrl(field: IField): ValidationRule[];
  /** 字段{0}不是有效的手机号码 */
  fieldDoNotValidPhoneNumber(field: IField): ValidationRule[];
  /** 字段{0}不可为空 */
  fieldRequired(field: IField): ValidationRule[];
  /** {0}字段只允许以下扩展名的文件: {1} */
  fieldOnlyAcceptsFilesExtensions(field: IFieldContains): ValidationRule[];
}

export function useValidation() {
  const { L } = useLocalization('AbpValidation');

  function _getFieldName(field: IField) {
    return __getFieldName(field.name ?? '', field.resourceName, field.prefix, field.connector);
  }

  function __getFieldName(
    fieldName: string,
    resourceName?: string,
    prefix?: string,
    connector?: string,
  ) {
    if (fieldName && resourceName) {
      fieldName = prefix ? `${prefix}${connector ?? ':'}${fieldName}` : fieldName;
      const { L: l } = useLocalization(resourceName);
      return l(fieldName);
    }
    return fieldName;
  }

  function _createRule(options: {
    message?: string;
    type?: string;
    required?: boolean;
    len?: number;
    min?: number;
    max?: number;
    trigger?: string;
    validator?: (rule: any, value: any, callback: any, source?: any, options?: any) => any;
  }): ValidationRule[] {
    return [
      {
        message: options.message,
        type: options.type,
        required: options.required,
        len: options.len,
        min: options.min,
        max: options.max,
        validator: options.validator,
        trigger: options.trigger,
      },
    ];
  }

  function _createValidator(
    field: IField,
    useNameEnum: string,
    notNameEnum: string,
    required?: boolean,
  ): ValidationRule {
    const message = field.name ? L(useNameEnum, _getFieldName(field)) : L(notNameEnum);
    return {
      required: required,
      message: message,
      type: field.type,
      trigger: field.trigger,
    };
  }

  function _createLengthValidator(
    field: IFieldLength,
    checkMaximum: boolean,
    useNameEnum: string,
    notNameEnum: string,
    required?: boolean,
  ): ValidationRule {
    const message = field.name
      ? L(useNameEnum, _getFieldName(field), field.length)
      : L(notNameEnum, field.length);

    function checkLength(value: string | any[]) {
      return checkMaximum ? value.length > field.length : value.length < field.length;
    }

    return {
      required: required,
      message: message,
      type: field.type,
      trigger: field.trigger,
      validator: (_, value: string) => {
        if (!checkLength(value)) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  function _createLengthRangValidator(
    field: IFieldRange,
    useNameEnum: string,
    notNameEnum: string,
    required?: boolean,
  ): ValidationRule {
    const message = field.name
      ? L(useNameEnum, _getFieldName(field), field.minimum, field.maximum)
      : L(notNameEnum, field.minimum, field.maximum);
    return {
      required: required,
      message: message,
      type: field.type,
      trigger: field.trigger,
      validator: (_, value: string) => {
        if (value.length < field.minimum || value.length > field.maximum) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  function _createBeetWeenValidator(field: IFieldBeetWeen): ValidationRule {
    const message = field.name
      ? L(ValidationEnum.FieldMustBeetWeen, _getFieldName(field), field.start, field.end)
      : L(ValidationEnum.ThisFieldMustBeBetween, field.start, field.end);
    return {
      message: message,
      trigger: field.trigger,
      validator: (_, value) => {
        // beetween不在进行必输检查, 改为数字有效性检查
        if (isNaN(value)) {
          return Promise.reject(message);
        }
        if (value < field.start || value > field.end) {
          return Promise.reject(message);
        } else {
          return Promise.resolve();
        }
      },
    };
  }

  function _createRegularExpressionValidator(
    field: IFieldRegular,
    required?: boolean,
  ): ValidationRule {
    const message = field.name
      ? L(ValidationEnum.FieldMustMatchRegularExpression, _getFieldName(field), field.expression)
      : L(ValidationEnum.ThisFieldMustMatchTheRegularExpression, field.expression);
    return {
      required: required,
      message: message,
      type: field.type,
      trigger: field.trigger,
      pattern: new RegExp(field.expression),
    };
  }

  function _createEmailValidator(field: IField, required?: boolean): ValidationRule {
    const message = field.name
      ? L(ValidationEnum.FieldDoNotValidEmailAddress, _getFieldName(field))
      : L(ValidationEnum.ThisFieldIsNotAValidEmailAddress);
    return {
      required: required,
      message: message,
      type: field.type,
      trigger: field.trigger,
      validator: (_, value: string) => {
        if (!isEmail(value)) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  function _createPhoneValidator(field: IField, required?: boolean): ValidationRule {
    const message = field.name
      ? L(ValidationEnum.FieldDoNotValidPhoneNumber, _getFieldName(field))
      : L(ValidationEnum.ThisFieldIsNotAValidPhoneNumber);
    return {
      required: required,
      message: message,
      type: field.type,
      trigger: field.trigger,
      validator: (_, value: string) => {
        if (!isPhone(value)) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  const ruleCreator: IRuleCreator = {
    doNotMatch(field: IFieldMatch) {
      const message = L(
        ValidationEnum.DoNotMatch,
        __getFieldName(field.name, field.resourceName, field.prefix),
        __getFieldName(field.matchField, field.resourceName, field.prefix),
      );
      return _createRule({
        required: field.required,
        message: message,
        type: field.type,
        trigger: field.trigger,
        validator: (_, value: string) => {
          if (value !== field.matchValue) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
    fieldInvalid(field: IFieldValidator) {
      const message = field.name
        ? L(ValidationEnum.FieldInvalid, _getFieldName(field))
        : L(ValidationEnum.ThisFieldIsInvalid);
      return _createRule({
        required: field.required,
        message: message,
        type: field.type,
        trigger: field.trigger,
        validator: (_, value: any) => {
          if (!field.validator(value)) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
    fieldIsNotValid(field: IFieldValidator) {
      const message = field.name
        ? L(ValidationEnum.FieldIsNotValid, _getFieldName(field))
        : L(ValidationEnum.ThisFieldIsNotValid);
      return _createRule({
        required: field.required,
        message: message,
        type: field.type,
        trigger: field.trigger,
        validator: (_, value: any) => {
          if (field.validator(value)) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
    fieldMustBeStringOrArrayWithMaximumLength(field: IFieldLength) {
      return [
        _createLengthValidator(
          field,
          true,
          ValidationEnum.FieldMustBeStringOrArrayWithMaximumLength,
          ValidationEnum.ThisFieldMustBeAStringOrArrayTypeWithAMaximumLength,
        ),
      ];
    },
    fieldMustBeStringOrArrayWithMinimumLength(field: IFieldLength) {
      return [
        _createLengthValidator(
          field,
          false,
          ValidationEnum.FieldMustBeStringOrArrayWithMinimumLength,
          ValidationEnum.ThisFieldMustBeAStringOrArrayTypeWithAMinimumLength,
        ),
      ];
    },
    fieldMustBeStringWithMaximumLength(field: IFieldLength) {
      return [
        _createLengthValidator(
          field,
          true,
          ValidationEnum.FieldMustBeStringWithMaximumLength,
          ValidationEnum.ThisFieldMustBeAStringWithAMaximumLength,
        ),
      ];
    },
    fieldMustBeStringWithMinimumLengthAndMaximumLength(field: IFieldRange) {
      return [
        _createLengthRangValidator(
          field,
          ValidationEnum.FieldMustBeStringWithMinimumLengthAndMaximumLength,
          ValidationEnum.ThisFieldMustBeAStringWithAMinimumLengthAndAMaximumLength,
        ),
      ];
    },
    fieldMustBeetWeen(field: IFieldBeetWeen) {
      return [_createBeetWeenValidator(field)];
    },
    fieldMustMatchRegularExpression(field: IFieldRegular) {
      return [_createRegularExpressionValidator(field)];
    },
    fieldDoNotValidCreditCardNumber(field: IField) {
      if (field.name) {
        return _createRule({
          message: L(ValidationEnum.FieldDoNotValidCreditCardNumber, _getFieldName(field)),
          type: field.type,
          trigger: field.trigger,
        });
      }
      return _createRule({
        message: L(ValidationEnum.ThisFieldIsNotAValidCreditCardNumber),
        type: field.type,
        trigger: field.trigger,
      });
    },
    fieldDoNotValidEmailAddress(field: IField) {
      return [_createEmailValidator(field)];
    },
    fieldDoNotValidFullyQualifiedUrl(field: IField) {
      if (field.name) {
        return _createRule({
          message: L(ValidationEnum.FieldDoNotValidFullyQualifiedUrl, _getFieldName(field)),
          type: field.type,
          trigger: field.trigger,
        });
      }
      return _createRule({
        message: L(ValidationEnum.ThisFieldIsNotAValidFullyQualifiedHttpHttpsOrFtpUrl),
        type: field.type,
        trigger: field.trigger,
      });
    },
    fieldDoNotValidPhoneNumber(field: IField) {
      return [_createPhoneValidator(field)];
    },
    fieldRequired(field: IField) {
      return [
        _createValidator(
          field,
          ValidationEnum.FieldRequired,
          ValidationEnum.ThisFieldIsRequired,
          true,
        ),
      ];
    },
    fieldOnlyAcceptsFilesExtensions(field: IFieldContains) {
      const message = field.name
        ? L(ValidationEnum.FieldOnlyAcceptsFilesExtensions, _getFieldName(field), field.value)
        : L(ValidationEnum.ThisFieldMustMatchTheRegularExpression, field.value);
      return _createRule({
        message: message,
        type: field.type,
        trigger: field.trigger,
        validator: (_, value: string) => {
          if (!field.value.includes(value)) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
  };

  return {
    ruleCreator,
  };
}
