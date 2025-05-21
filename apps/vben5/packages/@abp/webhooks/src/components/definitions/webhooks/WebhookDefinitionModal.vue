<script setup lang="ts">
import type {
  FeatureDefinitionDto,
  FeatureGroupDefinitionDto,
} from '@abp/features';
import type { PropertyInfo } from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type { WebhookDefinitionDto } from '../../../types/definitions';
import type { WebhookGroupDefinitionDto } from '../../../types/groups';

import {
  computed,
  defineEmits,
  defineOptions,
  ref,
  toValue,
  useTemplateRef,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  isNullOrWhiteSpace,
  listToTree,
  useAuthorization,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import {
  useFeatureDefinitionsApi,
  useFeatureGroupDefinitionsApi,
} from '@abp/features';
import { LocalizableInput, PropertyTable, valueTypeSerializer } from '@abp/ui';
import {
  Checkbox,
  Form,
  Input,
  message,
  Select,
  Tabs,
  TreeSelect,
} from 'ant-design-vue';

import { useWebhookGroupDefinitionsApi } from '../../../api';
import { useWebhookDefinitionsApi } from '../../../api/useWebhookDefinitionsApi';

defineOptions({
  name: 'WebhookDefinitionModal',
});
const emits = defineEmits<{
  (event: 'change', data: WebhookDefinitionDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

interface FeatureTreeData {
  checkable?: boolean;
  children: FeatureTreeData[];
  displayName: string;
  groupName: string;
  name: string;
}

type TabKeys = 'basic' | 'props';

const defaultModel: WebhookDefinitionDto = {
  displayName: '',
  extraProperties: {},
  groupName: '',
  isEnabled: true,
  isStatic: false,
  name: '',
  requiredFeatures: [],
};

const isEditModel = ref(false);
const activeTab = ref<TabKeys>('basic');
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<WebhookDefinitionDto>({ ...defaultModel });
const webhookGroups = ref<WebhookGroupDefinitionDto[]>([]);
const features = ref<FeatureDefinitionDto[]>([]);
const featureGroups = ref<FeatureGroupDefinitionDto[]>([]);

const { isGranted } = useAuthorization();
const { cancel, createApi, getApi, updateApi } = useWebhookDefinitionsApi();
const { getListApi: getGroupDefinitionsApi } = useWebhookGroupDefinitionsApi();
const { getListApi: getFeaturesApi } = useFeatureDefinitionsApi();
const { getListApi: getFeatureGroupsApi } = useFeatureGroupDefinitionsApi();
const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();

const getFeatureOptions = computed(() => {
  const featureOptions: FeatureTreeData[] = [];
  featureGroups.value.forEach((group) => {
    featureOptions.push({
      checkable: false,
      displayName: group.displayName,
      groupName: group.name,
      name: group.name,
      children: listToTree(
        features.value.filter((x) => x.groupName === group.name),
        {
          id: 'name',
          pid: 'parentName',
        },
      ),
    });
  });
  return featureOptions;
});
const getRequiredFeatures = computed(() => {
  const featureNames = formModel.value.requiredFeatures ?? [];
  const requiredFeatures = features.value
    .filter((feature) => featureNames.includes(feature.name))
    .map((feature) => {
      return {
        label: feature.displayName,
        value: feature.name,
      };
    });
  return requiredFeatures;
});

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('WebhookDefinitionModal has closed!');
  },
  onConfirm: async () => {
    await form.value?.validate();
    const input = toValue(formModel);
    const api = isEditModel.value
      ? updateApi(formModel.value.name, input)
      : createApi(input);
    try {
      modalApi.setState({ submitting: true });
      const res = await api;
      emits('change', res);
      message.success($t('AbpUi.SavedSuccessfully'));
      modalApi.close();
    } finally {
      modalApi.setState({ submitting: false });
    }
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      isEditModel.value = false;
      activeTab.value = 'basic';
      formModel.value = { ...defaultModel };
      modalApi.setState({
        loading: true,
        showConfirmButton: true,
        title: $t('WebhooksManagement.Webhooks:AddNew'),
      });
      try {
        const { groupName, name } = modalApi.getData<WebhookDefinitionDto>();
        await onInit(groupName);
        name && (await onGet(name));
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('WebhooksManagement.Webhooks:AddNew'),
});
async function onGet(name: string) {
  isEditModel.value = true;
  const dto = await getApi(name);
  formModel.value = dto;
  modalApi.setState({
    showConfirmButton: !dto.isStatic,
    title: `${$t('WebhooksManagement.WebhookDefinitions')} - ${dto.name}`,
  });
}
async function onInit(groupName?: string) {
  const [webhookGroupRes, featureGroupItems, featureItems] = await Promise.all([
    getGroupDefinitionsApi({ filter: groupName }),
    onInitFeatureGroups(),
    onInitFeatures(),
  ]);
  featureGroups.value = featureGroupItems;
  features.value = featureItems;
  webhookGroups.value = webhookGroupRes.items.map((group) => {
    const displayName = deserialize(group.displayName);
    return {
      ...group,
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
  if (webhookGroupRes.items.length === 1) {
    formModel.value.groupName = webhookGroupRes.items[0]!.name;
  }
}
async function onInitFeatures() {
  if (!isGranted(['FeatureManagement.Definitions'])) {
    return [];
  }
  const { items } = await getFeaturesApi();
  return items
    .filter((item) => {
      if (!isNullOrWhiteSpace(item.valueType)) {
        const valueType = valueTypeSerializer.deserialize(item.valueType);
        if (valueType.validator.name !== 'BOOLEAN') {
          return false;
        }
      }
      return true;
    })
    .map((item) => {
      const displayName = deserialize(item.displayName);
      const feature = {
        ...item,
        displayName: Lr(displayName.resourceName, displayName.name),
      };
      return feature;
    });
}
async function onInitFeatureGroups() {
  if (!isGranted(['FeatureManagement.GroupDefinitions'])) {
    return [];
  }
  const { items } = await getFeatureGroupsApi();
  return items.map((item) => {
    const displayName = deserialize(item.displayName);
    return {
      ...item,
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
}
function onFeaturesChange(value: any[]) {
  formModel.value.requiredFeatures = value.map((item) => item.value);
}
function onPropChange(prop: PropertyInfo) {
  formModel.value.extraProperties ??= {};
  formModel.value.extraProperties[prop.key] = prop.value;
}
function onPropDelete(prop: PropertyInfo) {
  formModel.value.extraProperties ??= {};
  delete formModel.value.extraProperties[prop.key];
}
</script>

<template>
  <Modal>
    <Form
      ref="form"
      :label-col="{ span: 6 }"
      :model="formModel"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="activeTab">
        <!-- 基本信息 -->
        <TabPane key="basic" :tab="$t('WebhooksManagement.BasicInfo')">
          <FormItem
            name="isEnabled"
            :label="$t('WebhooksManagement.DisplayName:IsEnabled')"
          >
            <Checkbox
              :disabled="formModel.isStatic"
              v-model:checked="formModel.isEnabled"
            >
              {{ $t('WebhooksManagement.DisplayName:IsEnabled') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="groupName"
            :label="$t('WebhooksManagement.DisplayName:GroupName')"
            required
          >
            <Select
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.groupName"
              :options="webhookGroups"
              :field-names="{ label: 'displayName', value: 'name' }"
            />
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:Name')"
            name="name"
            required
          >
            <Input
              v-model:value="formModel.name"
              :disabled="formModel.isStatic"
              autocomplete="off"
            />
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:DisplayName')"
            name="displayName"
            required
          >
            <LocalizableInput
              v-model:value="formModel.displayName"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            name="description"
            :label="$t('WebhooksManagement.DisplayName:Description')"
          >
            <LocalizableInput
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.description"
            />
          </FormItem>
          <FormItem
            v-if="
              isGranted(
                [
                  'FeatureManagement.GroupDefinitions',
                  'FeatureManagement.Definitions',
                ],
                true,
              )
            "
            name="requiredFeatures"
            :label="$t('WebhooksManagement.DisplayName:RequiredFeatures')"
          >
            <TreeSelect
              :tree-data="getFeatureOptions"
              :disabled="formModel.isStatic"
              allow-clear
              tree-checkable
              tree-check-strictly
              :field-names="{ label: 'displayName', value: 'name' }"
              :value="getRequiredFeatures"
              @change="onFeaturesChange"
            />
          </FormItem>
        </TabPane>
        <!-- 属性 -->
        <TabPane key="props" :tab="$t('WebhooksManagement.Properties')">
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
