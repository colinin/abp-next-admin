<script setup lang="ts">
import type {
  PropertyInfo,
  SelectionStringValueItem,
  StringValueTypeInstance,
} from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type { FeatureDefinitionDto } from '../../../types/definitions';
import type { FeatureGroupDefinitionDto } from '../../../types/groups';

import {
  defineEmits,
  defineOptions,
  reactive,
  ref,
  toValue,
  unref,
  useTemplateRef,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { isBoolean, isString } from '@vben/utils';

import {
  listToTree,
  useLocalization,
  useLocalizationSerializer,
  useValidation,
  ValidationEnum,
} from '@abp/core';
import { LocalizableInput, PropertyTable, StringValueTypeInput } from '@abp/ui';
import {
  Checkbox,
  Form,
  Input,
  InputNumber,
  message,
  Select,
  Tabs,
  Textarea,
  TreeSelect,
} from 'ant-design-vue';

import { useFeatureDefinitionsApi } from '../../../api/useFeatureDefinitionsApi';
import { useFeatureGroupDefinitionsApi } from '../../../api/useFeatureGroupDefinitionsApi';

defineOptions({
  name: 'FeatureDefinitionModal',
});
const emits = defineEmits<{
  (event: 'change', data: FeatureDefinitionDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

interface DefinitionTreeVo {
  children: DefinitionTreeVo[];
  displayName: string;
  groupName: string;
  name: string;
}

type TabKeys = 'basic' | 'props';

const defaultModel = {
  allowedProviders: [],
  displayName: '',
  extraProperties: {},
  groupName: '',
  isAvailableToHost: true,
  isEnabled: true,
  isStatic: false,
  isVisibleToClients: false,
  name: '',
  valueType: '',
} as FeatureDefinitionDto;

const isEditModel = ref(false);
const validatorNameRef = ref<string>('NULL');
const valueTypeNameRef = ref<string>('FreeTextStringValueType');
const activeTab = ref<TabKeys>('basic');
const form = useTemplateRef<FormInstance>('form');
const valueTypeInput = useTemplateRef<StringValueTypeInstance>('valueType');
const formModel = ref<FeatureDefinitionDto>({ ...defaultModel });
const availableGroups = ref<FeatureGroupDefinitionDto[]>([]);
const availableDefinitions = ref<DefinitionTreeVo[]>([]);
const selectionDataSource = ref<{ label: string; value: string }[]>([]);

const { Lr } = useLocalization();
const { deserialize, validate } = useLocalizationSerializer();
const { getListApi: getGroupsApi } = useFeatureGroupDefinitionsApi();
const { defineValidator, fieldRequired, mapEnumValidMessage } = useValidation();
const {
  createApi,
  getApi,
  getListApi: getDefinitionsApi,
  updateApi,
} = useFeatureDefinitionsApi();
const formRules = reactive({
  defaultValue: defineValidator({
    trigger: 'change',
    validator(_rule, value) {
      const valueType = unref(valueTypeInput);
      if (valueType) {
        return valueType.validate(value);
      }
      return Promise.resolve();
    },
  }),
  // description: defineValidator({
  //   trigger: 'blur',
  //   validator(_rule, value) {
  //     if (!validate(value, { required: false })) {
  //       return Promise.reject(
  //         $t(ValidationEnum.FieldRequired, [$t('DisplayName:Description')]),
  //       );
  //     }
  //     return Promise.resolve();
  //   },
  // }),
  displayName: defineValidator({
    required: true,
    trigger: 'blur',
    validator(_rule, value) {
      if (!validate(value)) {
        return Promise.reject(
          mapEnumValidMessage(ValidationEnum.FieldRequired, [
            $t('AbpFeatureManagement.DisplayName:DisplayName'),
          ]),
        );
      }
      return Promise.resolve();
    },
  }),
  groupName: fieldRequired({
    name: 'GroupName',
    prefix: 'DisplayName',
    resourceName: 'AbpFeatureManagement',
    trigger: 'blur',
  }),
  name: fieldRequired({
    name: 'Name',
    prefix: 'DisplayName',
    resourceName: 'AbpFeatureManagement',
    trigger: 'blur',
  }),
});

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    await form.value?.validate();
    const input = toValue(formModel);
    if (input.defaultValue && !isString(input.defaultValue)) {
      input.defaultValue = String(input.defaultValue);
    }
    const api = isEditModel.value
      ? updateApi(formModel.value.name, input)
      : createApi(input);
    modalApi.setState({ submitting: true });
    api
      .then((res) => {
        message.success($t('AbpUi.SavedSuccessfully'));
        emits('change', res);
        modalApi.close();
      })
      .finally(() => {
        modalApi.setState({ submitting: false });
      });
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      isEditModel.value = false;
      activeTab.value = 'basic';
      formModel.value = { ...defaultModel };
      availableDefinitions.value = [];
      availableGroups.value = [];
      modalApi.setState({
        showConfirmButton: true,
        title: $t('AbpFeatureManagement.FeatureDefinitions:AddNew'),
      });
      try {
        modalApi.setState({ loading: true });
        const { groupName, name } = modalApi.getData<FeatureDefinitionDto>();
        name && (await onGet(name));
        await onInitGroups(groupName);
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpFeatureManagement.FeatureDefinitions:AddNew'),
});
async function onInitGroups(name?: string) {
  const { items } = await getGroupsApi({ filter: name });
  availableGroups.value = items.map((group) => {
    const localizableGroup = deserialize(group.displayName);
    return {
      ...group,
      displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
    };
  });
  if (name) {
    formModel.value.groupName = name;
    await onGroupChange(name);
  }
}
async function onGet(name: string) {
  isEditModel.value = true;
  const dto = await getApi(name);
  formModel.value = dto;
  modalApi.setState({
    showConfirmButton: !dto.isStatic,
    title: `${$t('AbpFeatureManagement.FeatureDefinitions')} - ${dto.name}`,
  });
}
async function onGroupChange(name?: string) {
  const { items } = await getDefinitionsApi({
    groupName: name,
  });
  const features = items.map((item) => {
    const displayName = deserialize(item.displayName);
    const description = deserialize(item.description);
    return {
      ...item,
      description: Lr(description.resourceName, description.name),
      disabled: item.name === formModel.value.name,
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
  availableDefinitions.value = listToTree(features, {
    id: 'name',
    pid: 'parentName',
  });
}
function onPropChange(prop: PropertyInfo) {
  formModel.value.extraProperties ??= {};
  formModel.value.extraProperties[prop.key] = prop.value;
}
function onPropDelete(prop: PropertyInfo) {
  formModel.value.extraProperties ??= {};
  delete formModel.value.extraProperties[prop.key];
}
function onValueTypeNameChange(valueTypeName: string) {
  valueTypeNameRef.value = valueTypeName;
  switch (valueTypeName) {
    case 'ToggleStringValueType': {
      if (!isBoolean(formModel.value.defaultValue)) {
        formModel.value.defaultValue ??= 'false';
      }
      break;
    }
    default: {
      formModel.value.defaultValue ??= undefined;
      break;
    }
  }
  form.value?.clearValidate();
}

function onValidatorNameChange(validatorName: string) {
  validatorNameRef.value = validatorName;
}

function onSelectionChange(items: SelectionStringValueItem[]) {
  if (items.length === 0) {
    formModel.value.defaultValue = undefined;
    selectionDataSource.value = [];
    return;
  }
  selectionDataSource.value = items.map((item) => {
    return {
      label: Lr(item.displayText.resourceName, item.displayText.name),
      value: item.value,
    };
  });
  if (!items.some((item) => item.value === formModel.value.defaultValue)) {
    formModel.value.defaultValue = undefined;
  }
}
</script>

<template>
  <Modal>
    <Form
      ref="form"
      :model="formModel"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="activeTab">
        <!-- 基本信息 -->
        <TabPane key="basic" :tab="$t('AbpFeatureManagement.BasicInfo')">
          <FormItem
            :label="$t('AbpFeatureManagement.DisplayName:GroupName')"
            name="groupName"
          >
            <Select
              v-model:value="formModel.groupName"
              :allow-clear="true"
              :disabled="formModel.isStatic"
              :field-names="{
                label: 'displayName',
                value: 'name',
              }"
              :options="availableGroups"
              @change="(e) => onGroupChange(e?.toString())"
            />
          </FormItem>
          <FormItem
            v-if="availableDefinitions.length > 0"
            :label="$t('AbpFeatureManagement.DisplayName:ParentName')"
            name="parentName"
          >
            <TreeSelect
              v-model:value="formModel.parentName"
              :allow-clear="true"
              :disabled="formModel.isStatic"
              :field-names="{
                label: 'displayName',
                value: 'name',
                children: 'children',
              }"
              :tree-data="availableDefinitions"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpFeatureManagement.DisplayName:Name')"
            name="name"
          >
            <Input
              v-model:value="formModel.name"
              :disabled="formModel.isStatic"
              autocomplete="off"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpFeatureManagement.DisplayName:DisplayName')"
            name="displayName"
          >
            <LocalizableInput
              v-model:value="formModel.displayName"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpFeatureManagement.DisplayName:Description')"
            name="description"
          >
            <LocalizableInput
              v-model:value="formModel.description"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            name="defaultValue"
            :label="$t('AbpFeatureManagement.DisplayName:DefaultValue')"
          >
            <Select
              v-if="valueTypeNameRef === 'SelectionStringValueType'"
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.defaultValue"
              :options="selectionDataSource"
            />
            <Textarea
              v-else-if="
                valueTypeNameRef === 'FreeTextStringValueType' &&
                (validatorNameRef === 'NULL' || validatorNameRef === 'STRING')
              "
              :disabled="formModel.isStatic"
              :allow-clear="true"
              :auto-size="{ minRows: 3 }"
              v-model:value="formModel.defaultValue"
            />
            <InputNumber
              v-else-if="
                valueTypeNameRef === 'FreeTextStringValueType' &&
                validatorNameRef === 'NUMERIC'
              "
              style="width: 100%"
              :disabled="formModel.isStatic"
              v-model:value="formModel.defaultValue"
            />
            <Checkbox
              v-else-if="
                valueTypeNameRef === 'ToggleStringValueType' &&
                validatorNameRef === 'BOOLEAN'
              "
              :disabled="formModel.isStatic"
              :checked="formModel.defaultValue === 'true'"
              @change="
                (e) =>
                  (formModel.defaultValue = String(
                    e.target.checked,
                  ).toLowerCase())
              "
            >
              {{ $t('AbpFeatureManagement.DisplayName:DefaultValue') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="isVisibleToClients"
            :label="$t('AbpFeatureManagement.DisplayName:IsVisibleToClients')"
            :extra="$t('AbpFeatureManagement.Description:IsVisibleToClients')"
          >
            <Checkbox v-model:checked="formModel.isVisibleToClients">
              {{ $t('AbpFeatureManagement.DisplayName:IsVisibleToClients') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="isAvailableToHost"
            :label="$t('AbpFeatureManagement.DisplayName:IsAvailableToHost')"
            :extra="$t('AbpFeatureManagement.Description:IsAvailableToHost')"
          >
            <Checkbox v-model:checked="formModel.isAvailableToHost">
              {{ $t('AbpFeatureManagement.DisplayName:IsAvailableToHost') }}
            </Checkbox>
          </FormItem>
        </TabPane>
        <TabPane
          key="valueType"
          :tab="$t('AbpFeatureManagement.ValueValidator')"
          force-render
        >
          <FormItem
            name="valueType"
            label=""
            :label-col="{ span: 0 }"
            :wrapper-col="{ span: 24 }"
          >
            <StringValueTypeInput
              ref="valueType"
              :disabled="formModel.isStatic"
              :allow-delete="true"
              :allow-edit="true"
              v-model:value="formModel.valueType"
              @change:value-type="onValueTypeNameChange"
              @change:validator="onValidatorNameChange"
              @change:selection="onSelectionChange"
            />
          </FormItem>
        </TabPane>
        <!-- 属性 -->
        <TabPane key="props" :tab="$t('AbpFeatureManagement.Properties')">
          <PropertyTable
            :data="formModel.extraProperties"
            :disabled="formModel.isStatic"
            @change="onPropChange"
            @delete="onPropDelete"
          />
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
