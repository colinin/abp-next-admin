import { useI18n } from './useI18n';
import { IHasExtraProperties } from '/@/api/model/baseModel';

export function useExtraPropTranslation() {
  const { t } = useI18n();

  function tryLocalize(key: string, extraProp: IHasExtraProperties, defaultValue: string = '') {
    if (extraProp.extraProperties.L !== true) {
      return defaultValue;
    }
    const getProp = _findProperty(key, extraProp);
    if (!getProp) {
      return defaultValue;
    }
    const mapItems: Recordable = {};
    Object.keys(getProp).forEach((key) => {
      mapItems[key.toLocaleLowerCase()] = getProp[key];
    });
    const { resourcename, name, values } = mapItems;
    return resourcename && name ? t(`${resourcename}.${name}`, values) : defaultValue;
  }

  function _findProperty(key: string, extraProp: IHasExtraProperties) {
    const findKey = Object.keys(extraProp.extraProperties).find(
      (k) => k.toLocaleLowerCase() === key.toLocaleLowerCase(),
    );
    return (findKey && extraProp.extraProperties[findKey]) || undefined;
  }

  return {
    tryLocalize,
  };
}
