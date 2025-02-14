import type { LocalizableStringInfo } from '../types';

import { isNullOrWhiteSpace } from '../utils/string';

interface ValidateOptions {
  required?: boolean;
}

interface ILocalizableStringSerializer {
  deserialize(value?: string): LocalizableStringInfo;
  serialize(value?: LocalizableStringInfo): string;
  validate(value?: string, opt?: ValidateOptions): boolean;
}

export function useLocalizationSerializer(): ILocalizableStringSerializer {
  function Validate(value?: string, opt?: ValidateOptions): boolean {
    if (!value || isNullOrWhiteSpace(value)) {
      if (!opt || opt.required === undefined || opt.required === true) {
        return false;
      }
      return true;
    }
    if (value.length < 3 || value[1] !== ':') {
      return false;
    }
    const type = value[0];
    switch (type) {
      case 'F': {
        return !isNullOrWhiteSpace(value.slice(2).trim());
      }
      case 'L': {
        const commaPosition = value.indexOf(',', 2);
        if (commaPosition === -1) {
          return false;
        }
        const name = value.slice(Math.max(0, commaPosition + 1));
        if (isNullOrWhiteSpace(name)) {
          return false;
        }
        return true;
      }
      default: {
        return false;
      }
    }
  }

  function Serialize(value?: LocalizableStringInfo): string {
    if (!value) return '';
    return `L:${value.resourceName},${value.name}`;
  }

  function Deserialize(value?: string): LocalizableStringInfo {
    if (!value || isNullOrWhiteSpace(value)) {
      return {
        name: '',
        resourceName: '',
      };
    }
    if (value.length < 2 || value[1] !== ':') {
      return {
        name: value,
        resourceName: '',
      };
    }
    const type = value[0];
    switch (type) {
      case 'F': {
        return {
          name: value.slice(2),
          resourceName: 'Fixed',
        };
      }
      case 'L': {
        const commaPosition = value.indexOf(',', 2);
        if (commaPosition === -1) {
          return {
            name: value,
            resourceName: 'Default',
          };
        }
        const resourceName = value.slice(2, commaPosition);
        const name = value.slice(Math.max(0, commaPosition + 1));
        if (isNullOrWhiteSpace(resourceName)) {
          return {
            name: value,
            resourceName: 'Default',
          };
        }
        return {
          name,
          resourceName,
        };
      }
      default: {
        return {
          name: value,
          resourceName: 'Default',
        };
      }
    }
  }

  return {
    deserialize: Deserialize,
    serialize: Serialize,
    validate: Validate,
  };
}
