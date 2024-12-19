import type { RuleCreator } from '../types/rules';
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
  RuleType,
} from '../types/validations';

import { ValidationEnum } from '../constants';
import { isEmail, isPhone } from '../utils/regex';
import { useLocalization } from './useLocalization';

export function useValidation() {
  const { L } = useLocalization(['AbpValidation']);
  function _getFieldName(field: Field) {
    return __getFieldName(
      field.name ?? '',
      field.resourceName,
      field.prefix,
      field.connector,
    );
  }

  function __getFieldName(
    fieldName: string,
    resourceName?: string,
    prefix?: string,
    connector?: string,
  ) {
    if (fieldName && resourceName) {
      fieldName = prefix
        ? `${prefix}${connector ?? ':'}${fieldName}`
        : fieldName;
      const { L: l } = useLocalization(resourceName);
      return l(fieldName);
    }
    return fieldName;
  }

  function _createRule(options: {
    len?: number;
    max?: number;
    message?: string;
    min?: number;
    required?: boolean;
    trigger?: 'blur' | 'change' | ['change', 'blur'];
    type?: 'array' | RuleType;
    validator?: (
      rule: any,
      value: any,
      callback: any,
      source?: any,
      options?: any,
    ) => Promise<void> | void;
  }): Rule[] {
    return [
      {
        len: options.len,
        max: options.max,
        message: options.message,
        min: options.min,
        required: options.required,
        trigger: options.trigger,
        type: options.type,
        validator: options.validator,
      },
    ];
  }

  function _createValidator(
    field: Field,
    useNameEnum: string,
    notNameEnum: string,
    required?: boolean,
  ): Rule {
    const message = field.name
      ? L(useNameEnum, [_getFieldName(field)])
      : L(notNameEnum);
    return {
      message,
      required,
      trigger: field.trigger,
      type: field.type,
    };
  }

  function _createLengthValidator(
    field: FieldLength,
    checkMaximum: boolean,
    useNameEnum: string,
    notNameEnum: string,
    required?: boolean,
  ): Rule {
    const message = field.name
      ? L(useNameEnum, [_getFieldName(field), field.length])
      : L(notNameEnum, [field.length]);

    function checkLength(value: any[] | string) {
      return checkMaximum
        ? field.length > value.length
        : value.length > field.length;
    }

    return {
      message,
      required,
      trigger: field.trigger,
      type: field.type,
      validator: (_: any, value: string) => {
        if (!checkLength(value)) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  function _createLengthRangValidator(
    field: FieldRange,
    useNameEnum: string,
    notNameEnum: string,
    required?: boolean,
  ): Rule {
    const message = field.name
      ? L(useNameEnum, [_getFieldName(field), field.minimum, field.maximum])
      : L(notNameEnum, [field.minimum, field.maximum]);
    return {
      message,
      required,
      trigger: field.trigger,
      type: field.type,
      validator: (_: any, value: string) => {
        if (value.length < field.minimum || value.length > field.maximum) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  function _createBeetWeenValidator(field: FieldBeetWeen): Rule {
    const message = field.name
      ? L(ValidationEnum.FieldMustBeetWeen, [
          _getFieldName(field),
          field.start,
          field.end,
        ])
      : L(ValidationEnum.ThisFieldMustBeBetween, [field.start, field.end]);
    return {
      message,
      trigger: field.trigger,
      validator: (_: any, value: number) => {
        // beetween不在进行必输检查, 改为数字有效性检查
        if (Number.isNaN(value)) {
          return Promise.reject(message);
        }
        return value < field.start || value > field.end
          ? Promise.reject(message)
          : Promise.resolve();
      },
    };
  }

  function _createRegularExpressionValidator(
    field: FieldRegular,
    required?: boolean,
  ): Rule {
    const message = field.name
      ? L(ValidationEnum.FieldMustMatchRegularExpression, [
          _getFieldName(field),
          field.expression,
        ])
      : L(ValidationEnum.ThisFieldMustMatchTheRegularExpression, [
          field.expression,
        ]);
    return {
      message,
      pattern: new RegExp(field.expression),
      required,
      trigger: field.trigger,
      type: field.type,
    };
  }

  function _createEmailValidator(field: Field, required?: boolean): Rule {
    const message = field.name
      ? L(ValidationEnum.FieldDoNotValidEmailAddress, [_getFieldName(field)])
      : L(ValidationEnum.ThisFieldIsNotAValidEmailAddress);
    return {
      message,
      required,
      trigger: field.trigger,
      type: field.type,
      validator: (_: any, value: string) => {
        if (!isEmail(value)) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  function _createPhoneValidator(field: Field, required?: boolean): Rule {
    const message = field.name
      ? L(ValidationEnum.FieldDoNotValidPhoneNumber, [_getFieldName(field)])
      : L(ValidationEnum.ThisFieldIsNotAValidPhoneNumber);
    return {
      message,
      required,
      trigger: field.trigger,
      type: field.type,
      validator: (_: any, value: string) => {
        if (!isPhone(value)) {
          return Promise.reject(message);
        }
        return Promise.resolve();
      },
    };
  }

  const ruleCreator: RuleCreator = {
    defineValidator(field: FieldDefineValidator) {
      return _createRule(field);
    },
    doNotMatch(field: FieldMatch) {
      const message = L(ValidationEnum.DoNotMatch, [
        __getFieldName(field.name, field.resourceName, field.prefix),
        __getFieldName(field.matchField, field.resourceName, field.prefix),
      ]);
      return _createRule({
        message,
        required: field.required,
        trigger: field.trigger,
        type: field.type,
        validator: (_, value: string) => {
          if (value !== field.matchValue) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
    fieldDoNotValidCreditCardNumber(field: Field) {
      if (field.name) {
        return _createRule({
          message: L(ValidationEnum.FieldDoNotValidCreditCardNumber, [
            _getFieldName(field),
          ]),
          trigger: field.trigger,
          type: field.type,
        });
      }
      return _createRule({
        message: L(ValidationEnum.ThisFieldIsNotAValidCreditCardNumber),
        trigger: field.trigger,
        type: field.type,
      });
    },
    fieldDoNotValidEmailAddress(field: Field) {
      return [_createEmailValidator(field)];
    },
    fieldDoNotValidFullyQualifiedUrl(field: Field) {
      if (field.name) {
        return _createRule({
          message: L(ValidationEnum.FieldDoNotValidFullyQualifiedUrl, [
            _getFieldName(field),
          ]),
          trigger: field.trigger,
          type: field.type,
        });
      }
      return _createRule({
        message: L(
          ValidationEnum.ThisFieldIsNotAValidFullyQualifiedHttpHttpsOrFtpUrl,
        ),
        trigger: field.trigger,
        type: field.type,
      });
    },
    fieldDoNotValidPhoneNumber(field: Field) {
      return [_createPhoneValidator(field)];
    },
    fieldInvalid(field: FieldValidator) {
      const message = field.name
        ? L(ValidationEnum.FieldInvalid, [_getFieldName(field)])
        : L(ValidationEnum.ThisFieldIsInvalid);
      return _createRule({
        message,
        required: field.required,
        trigger: field.trigger,
        type: field.type,
        validator: (_, value: any) => {
          if (!field.validator(value)) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
    fieldIsNotValid(field: FieldValidator) {
      const message = field.name
        ? L(ValidationEnum.FieldIsNotValid, [_getFieldName(field)])
        : L(ValidationEnum.ThisFieldIsNotValid);
      return _createRule({
        message,
        required: field.required,
        trigger: field.trigger,
        type: field.type,
        validator: (_, value: any) => {
          if (field.validator(value)) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
    fieldMustBeetWeen(field: FieldBeetWeen) {
      return [_createBeetWeenValidator(field)];
    },
    fieldMustBeStringOrArrayWithMaximumLength(field: FieldLength) {
      return [
        _createLengthValidator(
          field,
          true,
          ValidationEnum.FieldMustBeStringOrArrayWithMaximumLength,
          ValidationEnum.ThisFieldMustBeAStringOrArrayTypeWithAMaximumLength,
        ),
      ];
    },
    fieldMustBeStringOrArrayWithMinimumLength(field: FieldLength) {
      return [
        _createLengthValidator(
          field,
          false,
          ValidationEnum.FieldMustBeStringOrArrayWithMinimumLength,
          ValidationEnum.ThisFieldMustBeAStringOrArrayTypeWithAMinimumLength,
        ),
      ];
    },
    fieldMustBeStringWithMaximumLength(field: FieldLength) {
      return [
        _createLengthValidator(
          field,
          true,
          ValidationEnum.FieldMustBeStringWithMaximumLength,
          ValidationEnum.ThisFieldMustBeAStringWithAMaximumLength,
        ),
      ];
    },
    fieldMustBeStringWithMinimumLengthAndMaximumLength(field: FieldRange) {
      return [
        _createLengthRangValidator(
          field,
          ValidationEnum.FieldMustBeStringWithMinimumLengthAndMaximumLength,
          ValidationEnum.ThisFieldMustBeAStringWithAMinimumLengthAndAMaximumLength,
        ),
      ];
    },
    fieldMustMatchRegularExpression(field: FieldRegular) {
      return [_createRegularExpressionValidator(field)];
    },
    fieldOnlyAcceptsFilesExtensions(field: FieldContains) {
      const message = field.name
        ? L(ValidationEnum.FieldOnlyAcceptsFilesExtensions, [
            _getFieldName(field),
            field.value,
          ])
        : L(ValidationEnum.ThisFieldMustMatchTheRegularExpression, [
            field.value,
          ]);
      return _createRule({
        message,
        trigger: field.trigger,
        type: field.type,
        validator: (_, value: string) => {
          if (!field.value.includes(value)) {
            return Promise.reject(message);
          }
          return Promise.resolve();
        },
      });
    },
    fieldRequired(field: Field) {
      return [
        _createValidator(
          field,
          ValidationEnum.FieldRequired,
          ValidationEnum.ThisFieldIsRequired,
          true,
        ),
      ];
    },
  };

  return {
    ruleCreator,
  };
}
