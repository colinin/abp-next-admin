<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue/lib/form';
import type {
  DefaultOptionType,
  SelectProps,
  SelectValue,
} from 'ant-design-vue/lib/select';

import type { WorkspaceDefinitionRecordDto } from '../../types';

import { ref, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  useAuthorization,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useMessage } from '@abp/ui';
import {
  AutoComplete,
  Checkbox,
  Form,
  Input,
  InputNumber,
  Select,
  Tabs,
  Textarea,
} from 'ant-design-vue';
import debounce from 'lodash.debounce';

import { useAIToolsApi } from '../../api/useAIToolsApi';
import { useWorkspaceDefinitionsApi } from '../../api/useWorkspaceDefinitionsApi';
import { WorkspaceDefinitionPermissions } from '../../constants/permissions';

const emit = defineEmits<{
  (event: 'change', data: WorkspaceDefinitionVto): void;
}>();
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type WorkspaceDefinitionVto = Omit<
  WorkspaceDefinitionRecordDto,
  'creationTime'
> & { apiKey?: string };

// 默认表单数据
const defaultModel: WorkspaceDefinitionVto = {
  id: '',
  name: '',
  isEnabled: true,
  displayName: '',
  provider: '',
  modelName: '',
  extraProperties: {},
};

const message = useMessage();
const { isGranted } = useAuthorization();
const { Lr } = useLocalization();
const { deserialize: deserializeLocalizableString } =
  useLocalizationSerializer();
const { getPagedListApi: getAIToolPagedListApi } = useAIToolsApi();
const { createApi, updateApi, getApi, getAvailableProviderListApi } =
  useWorkspaceDefinitionsApi();
// 表单允许编辑
const isAllowUpdate = ref(false);
// 表单
const form = useTemplateRef<FormInstance>('formEl');
// 表单数据
const formModel = ref<WorkspaceDefinitionVto>({ ...defaultModel });
// 模型提供者选项
const providerOptions = ref<NonNullable<SelectProps['options']>>([]);
// 模型选项
const modelOptions = ref<NonNullable<SelectProps['options']>>([]);
// AI工具
const aiToolOptions = ref<NonNullable<SelectProps['options']>>([]);

// 工作区编辑模态框
const [Modal, modalApi] = useVbenModal({
  title: $t('AIManagement.Workspaces:New'),
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
      title: $t('AIManagement.Workspaces:New'),
    });
    await Promise.all([onSearchAIToolOptions(), onInitProviderOptions()]);
    const { id } = modalApi.getData<WorkspaceDefinitionVto>();
    if (!id) {
      isAllowUpdate.value = isGranted(WorkspaceDefinitionPermissions.Create);
      formModel.value = { ...defaultModel };
      return;
    }
    const dto = await getApi(id);
    formModel.value = dto;
    modalApi.setState({
      title: $t('AIManagement.Workspaces:Edit'),
    });
    isAllowUpdate.value = isGranted(WorkspaceDefinitionPermissions.Update);
  } finally {
    modalApi.setState({
      loading: false,
      showConfirmButton: isAllowUpdate.value,
    });
  }
}
/** 初始化模型选项 */
async function onInitProviderOptions() {
  modelOptions.value = [];
  const { items } = await getAvailableProviderListApi();
  providerOptions.value = items.map((provider) => {
    return {
      label: provider.name,
      value: provider.name,
      models: provider.models?.map((model) => {
        return {
          label: model,
          value: model,
        };
      }),
    };
  });
}

/** 初始化AI工具选项 */
const onSearchAIToolOptions = debounce((filter?: string) => {
  getAIToolPagedListApi({ isEnabled: true, filter }).then((res) => {
    aiToolOptions.value = res.items.map((item) => {
      let description = item.description;
      if (description) {
        const localizableString = deserializeLocalizableString(description);
        description = Lr(
          localizableString.resourceName,
          localizableString.name,
        );
      }
      return {
        label: item.name,
        value: item.name,
        description,
      };
    });
  });
}, 500);

function onProviderChange(_: SelectValue, option: DefaultOptionType) {
  modelOptions.value = option.models;
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
      :disabled="!isAllowUpdate"
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
            :label="$t('AIManagement.DisplayName:Name')"
            name="name"
            required
          >
            <Input v-model:value="formModel.name" />
          </FormItem>
          <FormItem
            :label="$t('AIManagement.DisplayName:DisplayName')"
            name="displayName"
            required
          >
            <Input v-model:value="formModel.displayName" />
          </FormItem>
          <FormItem
            :label="$t('AIManagement.DisplayName:Description')"
            name="description"
          >
            <Textarea
              v-model:value="formModel.description"
              :auto-size="{ minRows: 3 }"
            />
          </FormItem>
          <FormItem :label="$t('AIManagement.DisplayName:Tools')" name="tools">
            <Select
              :allow-clear="true"
              :show-search="true"
              :filter-option="false"
              :options="aiToolOptions"
              v-model:value="formModel.tools"
              mode="multiple"
              @search="onSearchAIToolOptions"
            >
              <template #option="{ label, description }">
                {{ label }} ({{ description }})
              </template>
            </Select>
          </FormItem>
        </TabPane>
        <TabPane key="model" :tab="$t('AIManagement.ModelInfo')" force-render>
          <div class="h-[500px] overflow-y-auto rounded bg-gray-50 p-4">
            <FormItem
              :label="$t('AIManagement.DisplayName:Provider')"
              name="provider"
              required
            >
              <Select
                :options="providerOptions"
                v-model:value="formModel.provider"
                @change="onProviderChange"
              />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:ModelName')"
              name="modelName"
              required
            >
              <AutoComplete
                v-model:value="formModel.modelName"
                :options="modelOptions"
              />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:ApiBaseUrl')"
              name="apiBaseUrl"
            >
              <Input v-model:value="formModel.apiBaseUrl" />
            </FormItem>
            <FormItem
              v-if="!formModel.id"
              :label="$t('AIManagement.DisplayName:ApiKey')"
              name="apiKey"
            >
              <Input v-model:value="formModel.apiKey" />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:SystemPrompt')"
              name="systemPrompt"
            >
              <Textarea
                allow-clear
                v-model:value="formModel.systemPrompt"
                :auto-size="{ minRows: 10 }"
                show-count
              />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:Instructions')"
              name="instructions"
            >
              <Textarea
                allow-clear
                v-model:value="formModel.instructions"
                :auto-size="{ minRows: 10 }"
                show-count
              />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:Temperature')"
              :extra="$t('AIManagement.Description:Temperature')"
              name="temperature"
            >
              <InputNumber
                v-model:value="formModel.temperature"
                :max="2.0"
                :min="0.0"
                :step="0.1"
                class="w-full"
              />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:MaxOutputTokens')"
              :extra="$t('AIManagement.Description:MaxOutputTokens')"
              name="maxOutputTokens"
            >
              <InputNumber
                v-model:value="formModel.maxOutputTokens"
                :min="0"
                class="w-full"
              />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:FrequencyPenalty')"
              :extra="$t('AIManagement.Description:FrequencyPenalty')"
              name="frequencyPenalty"
            >
              <InputNumber
                v-model:value="formModel.frequencyPenalty"
                :max="2.0"
                :min="-2.0"
                :step="0.1"
                class="w-full"
              />
            </FormItem>
            <FormItem
              :label="$t('AIManagement.DisplayName:PresencePenalty')"
              :extra="$t('AIManagement.Description:PresencePenalty')"
              name="presencePenalty"
            >
              <InputNumber
                v-model:value="formModel.presencePenalty"
                :max="2.0"
                :min="-2.0"
                :step="0.1"
                class="w-full"
              />
            </FormItem>
          </div>
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
