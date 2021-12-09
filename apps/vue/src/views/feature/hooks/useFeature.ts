import type { Ref } from 'vue';

import { watch, ref, unref } from 'vue';
import { message } from 'ant-design-vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';
import { FeatureGroup } from '/@/api/feature/model/featureModel';
import { get, update } from '/@/api/feature/feature';
import { ReturnInnerMethods } from '/@/components/Modal';

interface UseFeature {
  providerName: Ref<string>;
  providerKey: Ref<string | null>;
  formRel: Ref<any>;
  modalMethods: ReturnInnerMethods;
}

export function useFeature({ providerName, providerKey, formRel, modalMethods }: UseFeature) {
  const { L } = useLocalization('AbpFeatureManagement');
  const { ruleCreator } = useValidation();
  const featureGroup = ref<{ groups: FeatureGroup[] }>({
    groups: [],
  });
  const featureGroupKey = ref(0);

  watch(
    () => unref(providerKey),
    (key) => {
      if (key !== undefined) {
        const form = unref(formRel);
        form.resetFields();
        onGroupChange(0);
        get({
          providerName: unref(providerName),
          providerKey: key,
        }).then((res) => {
          featureGroup.value.groups = mapFeatures(res.groups);
        });
      }
    },
  );

  function getFeatures(groups: FeatureGroup[]) {
    const features: { name: string; value: string }[] = [];
    groups.forEach((g) => {
      g.features.forEach((f) => {
        if (f.value !== null) {
          features.push({
            name: f.name,
            value: String(f.value),
          });
        }
      });
    });
    return features;
  }

  function mapFeatures(groups: FeatureGroup[]) {
    groups.forEach((g) => {
      g.features.forEach((f) => {
        switch (f.valueType?.validator.name) {
          case 'BOOLEAN':
            f.value = String(f.value).toLocaleLowerCase() === 'true';
            break;
          case 'NUMERIC':
            f.value = Number(f.value);
            break;
        }
      });
    });
    return groups;
  }

  function validator(validator: Validator) {
    const featureRules: { [key: string]: any }[] = new Array<{ [key: string]: any }>();
    if (validator.properties) {
      switch (validator.name) {
        case 'NUMERIC':
          featureRules.push(
            ...ruleCreator.fieldMustBeetWeen({
              start: Number(validator.properties.MinValue),
              end: Number(validator.properties.MaxValue),
              trigger: 'change',
            }),
          );
          break;
        case 'STRING':
          if (
            validator.properties.AllowNull &&
            validator.properties.AllowNull.toLowerCase() === 'true'
          ) {
            featureRules.push(
              ...ruleCreator.fieldRequired({
                trigger: 'blur',
              }),
            );
          }
          featureRules.push(
            ...ruleCreator.fieldMustBeStringWithMinimumLengthAndMaximumLength({
              minimum: Number(validator.properties.MinValue),
              maximum: Number(validator.properties.MaxValue),
              trigger: 'blur',
            }),
          );
          break;
        default:
          break;
      }
    }
    return featureRules;
  }

  function onGroupChange(activeKey) {
    featureGroupKey.value = activeKey;
  }

  function handleSubmit() {
    const form = unref(formRel);
    form.validate().then(() => {
      update(
        {
          providerName: unref(providerName),
          providerKey: unref(providerKey),
        },
        {
          features: getFeatures(unref(featureGroup).groups),
        },
      ).then(() => {
        modalMethods.closeModal();
        message.success(L('Successful'));
      });
    });
  }

  return {
    featureGroup,
    featureGroupKey,
    validator,
    handleSubmit,
    onGroupChange,
  };
}
