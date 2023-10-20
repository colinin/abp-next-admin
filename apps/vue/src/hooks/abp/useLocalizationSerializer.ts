import { isNullOrWhiteSpace } from "/@/utils/strings";

interface ValidateOptions {
  required?: boolean;
}

interface ILocalizableStringSerializer {
  serialize(value?: LocalizableStringInfo): string;
  deserialize(value?: string): LocalizableStringInfo;
  validate(value?: string, opt?: ValidateOptions): boolean;
}

export function useLocalizationSerializer(): ILocalizableStringSerializer {
  function Validate(value?: string, opt?: ValidateOptions): boolean {
    if (!value || isNullOrWhiteSpace(value)) {
      if (!opt || (opt.required === undefined || opt.required === true)) {
        return false;
      }
      return true;
    }
    if (value.length < 3 || value[1] !== ':') {
      return false;
    }
    const type = value[0];
    switch (type) {
      case 'F':
        return !isNullOrWhiteSpace(value.substring(2).trim());
      case 'L':
        const commaPosition = value.indexOf(',', 2);
        if (commaPosition == -1) {
          return false;
        }
        const name = value.substring(commaPosition + 1);
        if (isNullOrWhiteSpace(name)) {
          return false;
        }
        return true;
      default:
        return false;
    }
  }

  function Serialize(value?: LocalizableStringInfo): string {
    if (!value) return '';
    return `L:${value.resourceName},${value.name}`;
  }

  function Deserialize(value?: string): LocalizableStringInfo {
    if (!value || isNullOrWhiteSpace(value)) {
      return {
        resourceName: '',
        name: '',
      };
    }
    if (value.length < 2 || value[1] !== ':') {
      return {
        resourceName: '',
        name: value,
      };
    }
    const type = value[0];
    switch (type) {
      case 'F':
        return {
          resourceName: 'Fixed',
          name: value.substring(2),
        };
      case 'L':
        const commaPosition = value.indexOf(',', 2);
        if (commaPosition == -1) {
          return {
            resourceName: 'Default',
            name: value,
          };
        }
        const resourceName = value.substring(2, commaPosition);
        const name = value.substring(commaPosition + 1);
        if (isNullOrWhiteSpace(resourceName)) {
          return {
            resourceName:  'Default',
            name: value,
          };
        }
        return {
          resourceName: resourceName,
          name: name,
        };
      default:
        return {
          resourceName: 'Default',
          name: value,
        };
    }
  }

  return {
    validate: Validate,
    serialize: Serialize,
    deserialize: Deserialize,
  }
}
