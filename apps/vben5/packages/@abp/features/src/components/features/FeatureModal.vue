<script setup lang="ts">
import type { SelectionStringValueType, Validator } from '@abp/core';
import type { FormInstance } from 'ant-design-vue/es/form/Form';

import type { FeatureGroupDto, UpdateFeaturesDto } from '../../types/features';

import { computed, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useValidation } from '@abp/core';
import {
  Card,
  Checkbox,
  Form,
  Input,
  InputNumber,
  message,
  Select,
  Tabs,
} from 'ant-design-vue';

import { useFeaturesApi } from '../../api/useFeaturesApi';

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

interface ModalState {
  displayName?: string;
  providerKey?: string;
  providerName: string;
  readonly?: boolean;
}

interface FormModel {
  groups: FeatureGroupDto[];
}

const activeTabKey = ref('');
const modelState = ref<ModalState>();
const formModel = ref<FormModel>({ groups: [] });
const form = useTemplateRef<FormInstance>('form');

const getModalTitle = computed(() => {
  if (modelState.value?.displayName) {
    return `${$t('AbpFeatureManagement.Features')} - ${modelState.value.displayName}`;
  }
  return $t('AbpFeatureManagement.Features');
});

const { getApi, updateApi } = useFeaturesApi();
const {
  fieldMustBeetWeen,
  fieldMustBeStringWithMinimumLengthAndMaximumLength,
  fieldRequired,
} = useValidation();

const [Modal, modalApi] = useVbenModal({
  centered: true,
  class: 'w-1/2',
  async onConfirm() {
    await form.value?.validate();
    await onSubmit();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      formModel.value = { groups: [] };
      await onGet();
      if (formModel.value?.groups.length > 0) {
        activeTabKey.value = formModel.value.groups[0]?.name!;
      }
    }
  },
});
function mapFeatures(groups: FeatureGroupDto[]) {
  groups.forEach((group) => {
    group.features.forEach((feature) => {
      if (feature.valueType.name === 'SelectionStringValueType') {
        const valueType =
          feature.valueType as unknown as SelectionStringValueType;
        valueType.itemSource.items.forEach((valueItem) => {
          if (valueItem.displayText.resourceName === 'Fixed') {
            valueItem.displayName = valueItem.displayText.name;
            return;
          }
          valueItem.displayName = $t(
            `${valueItem.displayText.resourceName}.${valueItem.displayText.name}`,
          );
        });
      } else {
        switch (feature.valueType?.validator.name) {
          case 'BOOLEAN': {
            feature.value =
              String(feature.value).toLocaleLowerCase() === 'true';
            break;
          }
          case 'NUMERIC': {
            feature.value = Number(feature.value);
            break;
          }
        }
      }
    });
  });
  return groups;
}
function getFeatureInput(groups: FeatureGroupDto[]): UpdateFeaturesDto {
  const input: UpdateFeaturesDto = {
    features: [],
  };
  groups.forEach((g) => {
    g.features.forEach((f) => {
      if (f.value !== null) {
        input.features.push({
          name: f.name,
          value: String(f.value),
        });
      }
    });
  });
  return input;
}
function createRules(field: string, validator: Validator) {
  const featureRules: { [key: string]: any }[] = [];
  if (validator.properties) {
    switch (validator.name) {
      case 'NUMERIC': {
        featureRules.push(
          ...fieldMustBeetWeen({
            end: Number(validator.properties.MaxValue),
            name: field,
            start: Number(validator.properties.MinValue),
            trigger: 'change',
          }),
        );
        break;
      }
      case 'STRING': {
        if (
          validator.properties.AllowNull &&
          validator.properties.AllowNull.toLowerCase() === 'true'
        ) {
          featureRules.push(
            ...fieldRequired({
              name: field,
              trigger: 'blur',
            }),
          );
        }
        featureRules.push(
          ...fieldMustBeStringWithMinimumLengthAndMaximumLength({
            maximum: Number(validator.properties.MaxLength),
            minimum: Number(validator.properties.MinLength),
            name: field,
            trigger: 'blur',
          }),
        );
        break;
      }
      default: {
        break;
      }
    }
  }
  return featureRules;
}
async function onGet() {
  try {
    modalApi.setState({ loading: true });
    const state = modalApi.getData<ModalState>();
    const { groups } = await getApi({
      providerKey: state.providerKey,
      providerName: state.providerName,
    });
    formModel.value = {
      groups: mapFeatures(groups),
    };
    modelState.value = state;
  } finally {
    modalApi.setState({ loading: false });
  }
}
async function onSubmit() {
  try {
    modalApi.setState({ submitting: true });
    const model = toValue(formModel);
    const state = modalApi.getData<ModalState>();
    const input = getFeatureInput(model.groups);
    await updateApi(
      {
        providerKey: state.providerKey,
        providerName: state.providerName,
      },
      input,
    );
    message.success($t('AbpUi.SavedSuccessfully'));
    await onGet();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="getModalTitle">
    <Form :model="formModel" ref="form">
      <Tabs tab-position="left" type="card" v-model:active-key="activeTabKey">
        <TabPane
          v-for="(group, gi) in formModel.groups"
          :key="group.name"
          :tab="group.displayName"
        >
          <Card :bordered="false" :title="group.displayName">
            <template
              v-for="(feature, fi) in group.features"
              :key="feature.name"
            >
              <FormItem
                v-if="feature.valueType !== null"
                :name="['groups', gi, 'features', fi, 'value']"
                :label="feature.displayName"
                :extra="feature.description"
                :rules="
                  createRules(feature.displayName, feature.valueType.validator)
                "
              >
                <Checkbox
                  v-if="
                    feature.valueType.name === 'ToggleStringValueType' &&
                    feature.valueType.validator.name === 'BOOLEAN'
                  "
                  v-model:checked="feature.value"
                >
                  {{ feature.displayName }}
                </Checkbox>
                <div
                  v-else-if="
                    feature.valueType.name === 'FreeTextStringValueType'
                  "
                >
                  <InputNumber
                    v-if="feature.valueType.validator.name === 'NUMERIC'"
                    style="width: 100%"
                    v-model:value="feature.value"
                  />
                  <Input
                    v-else
                    v-model:value="feature.value"
                    autocomplete="off"
                  />
                </div>
                <Select
                  v-else-if="
                    feature.valueType.name === 'SelectionStringValueType'
                  "
                  v-model:value="feature.value"
                  :options="feature.valueType.itemSource.items"
                  :field-names="{ label: 'displayName', value: 'value' }"
                />
              </FormItem>
            </template>
          </Card>
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style lang="scss" scoped>
:deep(.ant-tabs) {
  height: 34rem;

  .ant-tabs-nav {
    width: 14rem;
  }

  .ant-tabs-content-holder {
    overflow: hidden auto !important;
  }
}
</style>
