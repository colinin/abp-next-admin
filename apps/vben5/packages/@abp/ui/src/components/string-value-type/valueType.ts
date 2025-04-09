import type { Dictionary, LocalizableStringInfo } from '@abp/core';

import type { ValueValidator } from './validator';

import {
  AlwaysValidValueValidator,
  BooleanValueValidator,
  NumericValueValidator,
  StringValueValidator,
} from './validator';

export interface StringValueType {
  name: string;
  properties: Dictionary<string, any>;
  validator: ValueValidator;
}

export interface SelectionStringValueItem {
  displayText: LocalizableStringInfo;
  value: string;
}

export interface SelectionStringValueItemSource {
  items: SelectionStringValueItem[];
}

export class FreeTextStringValueType implements StringValueType {
  name = 'FreeTextStringValueType';
  properties: Dictionary<string, any>;
  validator: ValueValidator;
  constructor(validator?: ValueValidator) {
    this.properties = {};
    this.validator = validator ?? new AlwaysValidValueValidator();
  }
}

export class ToggleStringValueType implements StringValueType {
  name = 'ToggleStringValueType';
  properties: Dictionary<string, any>;
  validator: ValueValidator;
  constructor(validator?: ValueValidator) {
    this.properties = {};
    this.validator = validator ?? new BooleanValueValidator();
  }
}

export class SelectionStringValueType implements StringValueType {
  itemSource: SelectionStringValueItemSource;
  name = 'SelectionStringValueType';
  properties: Dictionary<string, any>;
  validator: ValueValidator;
  constructor(validator?: ValueValidator) {
    this.properties = {};
    this.itemSource = {
      items: [],
    };
    this.validator = validator ?? new AlwaysValidValueValidator();
  }
}

class StringValueTypeSerializer {
  _deserializeValidator(validator: any): ValueValidator {
    let convertValidator: ValueValidator = new AlwaysValidValueValidator();
    if (validator.name) {
      switch (validator.name) {
        case 'BOOLEAN': {
          convertValidator = new BooleanValueValidator();
          break;
        }
        case 'NULL': {
          convertValidator = new AlwaysValidValueValidator();
          break;
        }
        case 'NUMERIC': {
          convertValidator = new NumericValueValidator();
          break;
        }
        case 'STRING': {
          convertValidator = new StringValueValidator();
          break;
        }
      }
    }
    convertValidator.properties = validator.properties;
    return convertValidator;
  }

  deserialize(value: string): StringValueType {
    let valueType: StringValueType;
    const valueTypeObj = JSON.parse(value);
    switch (valueTypeObj.name) {
      case 'SELECTION':
      case 'SelectionStringValueType': {
        valueType = new SelectionStringValueType();
        (valueType as SelectionStringValueType).itemSource =
          valueTypeObj.itemSource;
        break;
      }
      case 'TOGGLE':
      case 'ToggleStringValueType': {
        valueType = new ToggleStringValueType();
        break;
      }
      default: {
        valueType = new FreeTextStringValueType();
        break;
      }
    }
    valueType.properties = valueTypeObj.properties;
    valueType.validator = this._deserializeValidator(valueTypeObj.validator);
    return valueType;
  }

  serialize(value: StringValueType): string {
    const valueTypeString = JSON.stringify(value);
    return valueTypeString;
  }
}

export const valueTypeSerializer = new StringValueTypeSerializer();
