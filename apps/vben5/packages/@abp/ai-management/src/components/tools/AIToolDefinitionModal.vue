<script setup lang="ts">
import type { PropertyInfo } from '@abp/ui';
import type { CheckboxChangeEvent } from 'ant-design-vue/es/checkbox/interface';
import type { FormInstance } from 'ant-design-vue/lib/form';
import type {
  DefaultOptionType,
  SelectProps,
  SelectValue,
} from 'ant-design-vue/lib/select';

import type {
  AIToolDefinitionRecordDto,
  AIToolProviderDto,
} from '../../types/tools';

import { computed, nextTick, ref, useTemplateRef, watch } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAuthorization } from '@abp/core';
import { LocalizableInput, PropertyTable, useMessage } from '@abp/ui';
import {
  Checkbox,
  Form,
  FormItemRest,
  Input,
  InputNumber,
  Select,
  Tabs,
} from 'ant-design-vue';

import { useAIToolsApi } from '../../api/useAIToolsApi';
import { AIToolDefinitionPermissions } from '../../constants/permissions';

const emit = defineEmits<{
  (event: 'change', data: AIToolDefinitionVto): void;
}>();
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type AIToolDefinitionVto = Omit<AIToolDefinitionRecordDto, 'creationTime'> & {
  apiKey?: string;
};

// 默认表单数据
const defaultModel: AIToolDefinitionVto = {
  id: '',
  name: '',
  isEnabled: true,
  isGlobal: false,
  isSystem: false,
  provider: '',
  extraProperties: {},
};

const message = useMessage();
const { isGranted } = useAuthorization();
const { createApi, updateApi, getApi, getAvailableProviderListApi } =
  useAIToolsApi();
// 表单
const form = useTemplateRef<FormInstance>('formEl');
// 表单数据
const formModel = ref<AIToolDefinitionVto>({ ...defaultModel });
// 工具提供者选项
const aiToolProviders = ref<AIToolProviderDto[]>([]);
const currentAIToolProvider = ref<AIToolProviderDto>();
const getAIToolOptions = computed<NonNullable<SelectProps['options']>>(() => {
  return aiToolProviders.value.map((provider) => {
    return {
      label: provider.name,
      value: provider.name,
      properties: provider.properties,
    };
  });
});
// 表单允许编辑
const getIsAllowUpdate = computed<boolean>(() => {
  if (formModel.value.id) {
    if (formModel.value.isSystem) {
      return false;
    }
    return isGranted(AIToolDefinitionPermissions.Update);
  }
  return isGranted(AIToolDefinitionPermissions.Create);
});

watch(
  () => getIsAllowUpdate.value,
  (allowUpdate) => {
    nextTick(() => {
      modalApi.setState({ showConfirmButton: allowUpdate });
    });
  },
);

// 工作区编辑模态框
const [Modal, modalApi] = useVbenModal({
  title: $t('AIManagement.Tools:New'),
  class: 'w-1/2',
  closeOnClickModal: false,
  closeOnPressEscape: false,
  fullscreenButton: false,
  onConfirm: onSubmit,
  async onOpenChange(isOpen) {
    isOpen && (await onInit());
  },
});
/** 初始化工作区数据 */
async function onInit() {
  try {
    modalApi.setState({
      loading: true,
      title: $t('AIManagement.Tools:New'),
    });
    await onInitProviderOptions();
    const { id } = modalApi.getData<AIToolDefinitionVto>();
    if (!id) {
      formModel.value = { ...defaultModel };
      currentAIToolProvider.value = undefined;
      return;
    }
    const dto = await getApi(id);
    formModel.value = dto;
    currentAIToolProvider.value = aiToolProviders.value.find(
      (p) => p.name === dto.provider,
    );
    modalApi.setState({
      title: $t('AIManagement.Tools:Edit'),
    });
  } finally {
    modalApi.setState({
      loading: false,
    });
  }
}
/** 初始化模型选项 */
async function onInitProviderOptions() {
  const { items } = await getAvailableProviderListApi();
  aiToolProviders.value = items;
}

function onProviderChange(_: SelectValue, option: DefaultOptionType) {
  const provider = option as AIToolProviderDto;
  currentAIToolProvider.value = provider;
  if (provider) {
    const propertites: Record<string, any> = {};
    provider.properties.forEach((prop) => {
      propertites[prop.name] = '';
    });
    formModel.value.extraProperties ??= {};
    formModel.value.extraProperties = propertites;
  }
}

function onBoolPropChange(key: string, e: CheckboxChangeEvent) {
  const propertites = formModel.value.extraProperties ?? {};
  propertites[key] = {};
  propertites[key] = e.target.checked;
  formModel.value.extraProperties = propertites;
}

function onPropChange(key: string, prop: PropertyInfo) {
  const propertites = formModel.value.extraProperties ?? {};
  propertites[key] = {};
  propertites[key][prop.key] = prop.value;
  formModel.value.extraProperties = propertites;
}
function onPropDelete(key: string, prop: PropertyInfo) {
  const propertites = formModel.value.extraProperties ?? {};
  propertites[key] = {};
  delete propertites[key][prop.key];
  formModel.value.extraProperties = propertites;
}

/** 提交表单 */
async function onSubmit() {
  await form.value?.validate();
  try {
    modalApi.setState({ submitting: true });
    const vto = await (formModel.value.id
      ? updateApi(formModel.value.id, formModel.value)
      : createApi(formModel.value));
    formModel.value = vto;
    message.success($t('AbpUi.SavedSuccessfully'));
    emit('change', vto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal>
    <Form
      :model="formModel"
      ref="formEl"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
      :disabled="!getIsAllowUpdate"
      scroll-to-first-error
    >
      <Tabs>
        <TabPane key="basic" :tab="$t('AIManagement.BasicInfo')" force-render>
          <FormItem
            :label="$t('AIManagement.DisplayName:IsEnabled')"
            name="isEnabled"
          >
            <Checkbox v-model:checked="formModel.isEnabled">
              {{ $t('AIManagement.DisplayName:IsEnabled') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :label="$t('AIManagement.DisplayName:IsGlobal')"
            name="isEnabled"
          >
            <Checkbox v-model:checked="formModel.isGlobal">
              {{ $t('AIManagement.DisplayName:IsGlobal') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :label="$t('AIManagement.DisplayName:ToolProvider')"
            name="provider"
            required
          >
            <Select
              :options="getAIToolOptions"
              v-model:value="formModel.provider"
              @change="onProviderChange"
            />
          </FormItem>
          <FormItem
            :label="$t('AIManagement.DisplayName:Name')"
            name="name"
            required
          >
            <Input v-model:value="formModel.name" />
          </FormItem>
          <FormItem
            :label="$t('AIManagement.DisplayName:Description')"
            name="description"
          >
            <LocalizableInput v-model:value="formModel.description" />
          </FormItem>
        </TabPane>
        <TabPane
          v-if="currentAIToolProvider"
          key="propertites"
          :tab="$t('AIManagement.Propertites')"
          force-render
        >
          <div class="h-[500px] overflow-y-auto rounded bg-gray-50 p-4">
            <template
              v-for="prop in currentAIToolProvider.properties"
              :key="prop.name"
            >
              <FormItem
                v-if="prop.valueType === 'String'"
                :extra="prop.description"
                :label="prop.displayName"
                :name="['extraProperties', prop.name]"
                :required="prop.required"
              >
                <Input v-model:value="formModel.extraProperties[prop.name]" />
              </FormItem>
              <FormItem
                v-if="prop.valueType === 'Number'"
                :extra="prop.description"
                :label="prop.displayName"
                :name="['extraProperties', prop.name]"
                :required="prop.required"
              >
                <InputNumber
                  class="w-full"
                  :min="0"
                  v-model:value="formModel.extraProperties[prop.name]"
                />
              </FormItem>
              <FormItem
                v-else-if="prop.valueType === 'Boolean'"
                :extra="prop.description"
                :label="prop.displayName"
                :name="['extraProperties', prop.name]"
                :required="prop.required"
              >
                <Checkbox
                  :checked="formModel.extraProperties[prop.name] === true"
                  @change="onBoolPropChange(prop.name, $event)"
                >
                  {{ prop.displayName }}
                </Checkbox>
              </FormItem>
              <FormItem
                v-else-if="prop.valueType === 'Select'"
                :extra="prop.description"
                :label="prop.displayName"
                :name="['extraProperties', prop.name]"
                :required="prop.required"
              >
                <Select
                  class="w-full"
                  :options="prop.options"
                  :field-names="{ label: 'name', value: 'value' }"
                  v-model:value="formModel.extraProperties[prop.name]"
                />
              </FormItem>
              <FormItem
                v-else-if="prop.valueType === 'Dictionary'"
                :extra="prop.description"
                :label="prop.displayName"
                :name="['extraProperties', prop.name]"
              >
                <FormItemRest>
                  <PropertyTable
                    :data="formModel.extraProperties[prop.name]"
                    @change="onPropChange(prop.name, $event)"
                    @delete="onPropDelete(prop.name, $event)"
                  />
                </FormItemRest>
              </FormItem>
            </template>
          </div>
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
