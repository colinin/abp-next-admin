<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue/lib/form';
import type {
  DefaultOptionType,
  SelectProps,
  SelectValue,
} from 'ant-design-vue/lib/select';

import type {
  AIToolDefinitionRecordDto,
  AIToolPropertyDescriptorDto,
  AIToolProviderDto,
} from '../../types/tools';

import {
  computed,
  defineAsyncComponent,
  nextTick,
  ref,
  useTemplateRef,
  watch,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAuthorization } from '@abp/core';
import { LocalizableInput, useMessage } from '@abp/ui';
import { Checkbox, Form, Input, Select, Tabs } from 'ant-design-vue';

import { useAIToolsApi } from '../../api/useAIToolsApi';
import { AIToolDefinitionPermissions } from '../../constants/permissions';

const emit = defineEmits<{
  (event: 'change', data: AIToolDefinitionVto): void;
}>();

const AIToolProperty = defineAsyncComponent(
  () => import('./AIToolProperty.vue'),
);

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
const getParentProperties = computed<AIToolPropertyDescriptorDto[]>(() => {
  if (!currentAIToolProvider.value) {
    return [];
  }
  return currentAIToolProvider.value.properties.filter(
    (p) => p.dependencies.length === 0,
  );
});
const getDependencyProperties = computed<AIToolPropertyDescriptorDto[]>(() => {
  if (!currentAIToolProvider.value || !formModel.value.extraProperties) {
    return [];
  }
  const properties = currentAIToolProvider.value.properties.filter(
    (p) => p.dependencies.length,
  );
  return properties.filter((p) => {
    for (let index = 0; index < p.dependencies.length; index++) {
      const depend = p.dependencies[index]!;
      if (formModel.value.extraProperties[depend.name] === depend.value) {
        return true;
      }
    }
    return false;
  });
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
      switch (prop.valueType) {
        case 'Boolean': {
          propertites[prop.name] = false;
          break;
        }
        case 'Dictionary': {
          propertites[prop.name] = {};
          break;
        }
        case 'Number':
        case 'Select':
        case 'String': {
          propertites[prop.name] = undefined;
          break;
        }
      }
    });
    formModel.value.extraProperties ??= {};
    formModel.value.extraProperties = propertites;
  }
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
            :extra="$t('AIManagement.Description:IsGlobal')"
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
            <LocalizableInput
              :disabled="!getIsAllowUpdate"
              v-model:value="formModel.description"
            />
          </FormItem>
        </TabPane>
        <TabPane
          v-if="currentAIToolProvider"
          key="propertites"
          :tab="$t('AIManagement.Propertites')"
          force-render
        >
          <div class="h-[500px] overflow-y-auto rounded bg-gray-50 p-4">
            <template v-for="prop in getParentProperties" :key="prop.name">
              <FormItem
                :extra="prop.description"
                :label="prop.displayName"
                :name="['extraProperties', prop.name]"
                :required="prop.required"
              >
                <AIToolProperty
                  :property="prop"
                  v-model:model="formModel.extraProperties"
                />
              </FormItem>
            </template>
            <template v-for="prop in getDependencyProperties" :key="prop.name">
              <FormItem
                :extra="prop.description"
                :label="prop.displayName"
                :name="['extraProperties', prop.name]"
                :required="prop.required"
              >
                <AIToolProperty
                  :property="prop"
                  v-model:model="formModel.extraProperties"
                />
              </FormItem>
            </template>
          </div>
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
