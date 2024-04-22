import { isBoolean, isNullOrUnDef, isNumber } from "/@/utils/is";
import { isNullOrWhiteSpace } from "/@/utils/strings";

export interface ValueValidator {
  name: string;
  properties: Dictionary<string, any>;

  isValid(value?: any): boolean;
}

export class AlwaysValidValueValidator implements ValueValidator {
  name = "NULL";
  properties: Dictionary<string, any>;
  constructor() {
    this.properties = {};
  }
  isValid(_value?: any): boolean {
    return true;
  }
}

export class BooleanValueValidator implements ValueValidator {
  name = "BOOLEAN";
  properties: Dictionary<string, any>;
  constructor() {
    this.properties = {};
  }
  isValid(value?: any): boolean {
    if (isNullOrUnDef(value)) return true;
    if (isBoolean(value)) return true;
    const bolString = String(value).toLowerCase();
    if (bolString === 'true' || bolString === 'false') return true;
    return false;
  }
}

export class NumericValueValidator implements ValueValidator {
  name = "NUMERIC";
  properties: Dictionary<string, any>;
  constructor() {
    this.properties = {};
  }

  get minValue(): number | undefined {
    return Number(this.properties['MinValue']);
  }

  set minValue(value: number) {
    this.properties['MinValue'] = value;
  }

  get maxValue(): number | undefined {
    return Number(this.properties['MaxValue']);
  }

  set maxValue(value: number) {
    this.properties['MaxValue'] = value;
  }

  isValid(value?: any): boolean {
    if (isNullOrUnDef(value)) return true;
    if (isNumber(value)) return this._isValidInternal(value);
    const numString = String(value);
    if (!isNullOrUnDef(numString)) {
      const num = Number(numString);
      if (num) return this._isValidInternal(num);
    }
    return false;
  }

  _isValidInternal(value: number): boolean {
    if (this.minValue && value < this.minValue) return false;
    if (this.maxValue && value > this.maxValue) return false;
    return true;
  }
}

export class StringValueValidator implements ValueValidator {
  name = "STRING";
  properties: Dictionary<string, any>;
  constructor() {
    this.properties = {};
  }

  get allowNull(): boolean {
    return String(this.properties['AllowNull'] ?? 'true')?.toLowerCase() === 'true';
  }

  set allowNull(value: boolean) {
    this.properties['AllowNull'] = value;
  }

  get minLength(): number | undefined {
    return Number(this.properties['MinLength']);
  }

  set minLength(value: number) {
    this.properties['MinLength'] = value;
  }

  get maxLength(): number | undefined {
    return Number(this.properties['MaxLength']);
  }

  set maxLength(value: number) {
    this.properties['MaxLength'] = value;
  }

  get regularExpression(): string {
    return String(this.properties['RegularExpression'] ?? '');
  }

  set regularExpression(value: string) {
    this.properties['RegularExpression'] = value
  }

  isValid(value?: any): boolean {
    if (!this.allowNull && isNullOrUnDef(value)) return false;
    const valueString = String(value);
    if (!this.allowNull && isNullOrWhiteSpace(valueString.trim())) return false;
    if (this.minLength && this.minLength > 0 && valueString.length < this.minLength) return false;
    if (this.maxLength && this.maxLength > 0 && valueString.length > this.maxLength) return false;
    if (!isNullOrWhiteSpace(this.regularExpression)) {
      return RegExp(this.regularExpression).test(valueString);
    }
    return true;
  }
}