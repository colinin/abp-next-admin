import {
  ValueValidator,
  AlwaysValidValueValidator,
  BooleanValueValidator,
  NumericValueValidator,
  StringValueValidator,
} from "./validator";

export interface StringValueType {
  name: string;
  properties: Dictionary<string, any>;
  validator: ValueValidator;
}

export interface SelectionStringValueItem {
  value: string;
  displayText: LocalizableStringInfo;
}

export interface SelectionStringValueItemSource {
  items: SelectionStringValueItem[];
}

export class FreeTextStringValueType implements StringValueType {
  name = "FreeTextStringValueType";
  properties: Dictionary<string, any>;
  validator: ValueValidator;
  constructor(validator?: ValueValidator) {
    this.properties = {};
    this.validator = validator ?? new AlwaysValidValueValidator();
  }
}

export class ToggleStringValueType implements StringValueType {
  name = "ToggleStringValueType";
  properties: Dictionary<string, any>;
  validator: ValueValidator;
  constructor(validator?: ValueValidator) {
    this.properties = {};
    this.validator = validator ?? new BooleanValueValidator();
  }
}

export class SelectionStringValueType implements StringValueType {
  name = "SelectionStringValueType";
  properties: Dictionary<string, any>;
  validator: ValueValidator;
  itemSource: SelectionStringValueItemSource;
  constructor(validator?: ValueValidator) {
    this.properties = {};
    this.itemSource = {
      items: [],
    };
    this.validator = validator ?? new AlwaysValidValueValidator();
  }
}

class StringValueTypeSerializer {
  serialize(value: StringValueType): string {
    // console.log('serialize', value);
    const valueTypeString = JSON.stringify(value);
    // console.log('deserialize to obj', valueTypeString);
    return valueTypeString;
  }

  deserialize(value: string): StringValueType {
    let valueType: StringValueType;
    const valueTypeObj = JSON.parse(value);
    // console.log('deserialize', value);
    // console.log('deserialize to obj', valueTypeObj);
    switch (valueTypeObj.name) {
      case 'TOGGLE':
      case 'ToggleStringValueType':
        // console.log('deserialize valueType to TOGGLE', valueTypeObj.name);
        valueType = new ToggleStringValueType();
        break;
      case 'SELECTION':
      case 'SelectionStringValueType':
        // console.log('deserialize valueType to SELECTION', valueTypeObj.name);
        valueType = new SelectionStringValueType();
        (valueType as SelectionStringValueType).itemSource = valueTypeObj.itemSource;
        break;
      default:
      case 'FREE_TEXT':
      case 'FreeTextStringValueType':
        // console.log('deserialize valueType to FREE_TEXT or default', valueTypeObj.name);
        valueType = new FreeTextStringValueType();
        break;
    }
    valueType.properties = valueTypeObj.properties;
    valueType.validator = this._deserializeValidator(valueTypeObj.validator);
    return valueType;
  }

  _deserializeValidator(validator: any): ValueValidator {
    let convertValidator: ValueValidator = new AlwaysValidValueValidator();
    if (validator.name) {
      switch (validator.name) {
        case 'BOOLEAN':
          // console.log('deserialize validator to BOOLEAN', validator.name);
          convertValidator = new BooleanValueValidator();
          break;
        case 'NUMERIC':
          // console.log('deserialize validator to NUMERIC', validator.name);
          convertValidator = new NumericValueValidator();
          break;
        case 'STRING':
          // console.log('deserialize validator to STRING', validator.name);
          convertValidator = new StringValueValidator();
          break;
        case 'NULL':
          // console.log('deserialize validator to NULL', validator.name);
          convertValidator = new AlwaysValidValueValidator();
          break;
      }
    }
    convertValidator.properties = validator.properties;
    return convertValidator;
  }
}

export const valueTypeSerializer = new StringValueTypeSerializer();