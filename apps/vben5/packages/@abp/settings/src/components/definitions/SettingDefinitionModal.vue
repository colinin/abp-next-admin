<script setup lang="ts">
import type { PropertyInfo } from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type { SettingDefinitionDto } from '../../types/definitions';

import {
  defineEmits,
  defineOptions,
  reactive,
  ref,
  toValue,
  useTemplateRef,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { LocalizableInput, PropertyTable } from '@abp/ui';
import {
  Checkbox,
  Form,
  Input,
  message,
  Select,
  Tabs,
  Textarea,
} from 'ant-design-vue';

import { createApi, getApi, updateApi } from '../../api/definitions';

defineOptions({
  name: 'SettingDefinitionModal',
});
const emits = defineEmits<{
  (event: 'change', data: SettingDefinitionDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type TabKeys = 'basic' | 'props';

const defaultModel = {} as SettingDefinitionDto;

const isEditModel = ref(false);
const activeTab = ref<TabKeys>('basic');
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<SettingDefinitionDto>({ ...defaultModel });
const providerOptions = reactive([
  { label: $t('AbpSettingManagement.Providers:Default'), value: 'D' },
  { label: $t('AbpSettingManagement.Providers:Configuration'), value: 'C' },
  { label: $t('AbpSettingManagement.Providers:Global'), value: 'G' },
  { label: $t('AbpSettingManagement.Providers:Tenant'), value: 'T' },
  { label: $t('AbpSettingManagement.Providers:User'), value: 'U' },
]);

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    await form.value?.validate();
    const api = isEditModel.value
      ? updateApi(formModel.value.name, toValue(formModel))
      : createApi(toValue(formModel));
    modalApi.setState({ confirmLoading: true, loading: true });
    api
      .then((res) => {
        message.success($t('AbpUi.SavedSuccessfully'));
        emits('change', res);
        modalApi.close();
      })
      .finally(() => {
        modalApi.setState({ confirmLoading: false, loading: false });
      });
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      isEditModel.value = false;
      activeTab.value = 'basic';
      formModel.value = { ...defaultModel };
      modalApi.setState({
        showConfirmButton: true,
        title: $t('AbpSettingManagement.Definition:AddNew'),
      });
      try {
        modalApi.setState({ loading: true });
        const { name } = modalApi.getData<SettingDefinitionDto>();
        name && (await onGet(name));
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpSettingManagement.Definition:AddNew'),
});
async function onGet(name: string) {
  isEditModel.value = true;
  const dto = await getApi(name);
  formModel.value = dto;
  modalApi.setState({
    showConfirmButton: !dto.isStatic,
    title: `${$t('AbpSettingManagement.Settings')} - ${dto.name}`,
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
        <TabPane key="basic" :tab="$t('AbpSettingManagement.BasicInfo')">
          <FormItem
            :label="$t('AbpSettingManagement.DisplayName:Name')"
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
            :label="$t('AbpSettingManagement.DisplayName:DefaultValue')"
            name="defaultValue"
          >
            <Textarea
              v-model:value="formModel.defaultValue"
              :allow-clear="true"
              :auto-size="{ minRows: 3 }"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpSettingManagement.DisplayName:DisplayName')"
            name="displayName"
            required
          >
            <LocalizableInput
              v-model:value="formModel.displayName"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpSettingManagement.DisplayName:Description')"
            name="description"
          >
            <LocalizableInput
              v-model:value="formModel.description"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            :extra="$t('AbpSettingManagement.Description:Providers')"
            :label="$t('AbpSettingManagement.DisplayName:Providers')"
            name="providers"
          >
            <Select
              v-model:value="formModel.providers"
              :allow-clear="true"
              :disabled="formModel.isStatic"
              :options="providerOptions"
              mode="multiple"
            />
          </FormItem>
          <FormItem
            :extra="$t('AbpSettingManagement.Description:IsInherited')"
            :label="$t('AbpSettingManagement.DisplayName:IsInherited')"
            name="isInherited"
          >
            <Checkbox
              v-model:checked="formModel.isInherited"
              :disabled="formModel.isStatic"
            >
              {{ $t('AbpSettingManagement.DisplayName:IsInherited') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :extra="$t('AbpSettingManagement.Description:IsEncrypted')"
            :label="$t('AbpSettingManagement.DisplayName:IsEncrypted')"
            name="isEncrypted"
          >
            <Checkbox
              v-model:checked="formModel.isEncrypted"
              :disabled="formModel.isStatic"
            >
              {{ $t('AbpSettingManagement.DisplayName:IsEncrypted') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :extra="$t('AbpSettingManagement.Description:IsVisibleToClients')"
            :label="$t('AbpSettingManagement.DisplayName:IsVisibleToClients')"
            name="isVisibleToClients"
          >
            <Checkbox
              v-model:checked="formModel.isVisibleToClients"
              :disabled="formModel.isStatic"
            >
              {{ $t('AbpSettingManagement.DisplayName:IsVisibleToClients') }}
            </Checkbox>
          </FormItem>
        </TabPane>
        <!-- 属性 -->
        <TabPane key="props" :tab="$t('AbpPermissionManagement.Properties')">
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
