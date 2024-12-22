<script setup lang="ts">
import type { OpenIdConfiguration } from '@abp/core';
import type { FormInstance } from 'ant-design-vue';
import type { TransferItem } from 'ant-design-vue/es/transfer';

import type { OpenIddictScopeDto } from '../../types/scopes';
import type { DisplayNameInfo } from '../display-names/types';
import type { PropertyInfo } from '../properties/types';

import { computed, defineEmits, defineOptions, ref, toValue } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Form, Input, message, Tabs, Transfer } from 'ant-design-vue';

import { discoveryApi } from '../../api/openid';
import { createApi, getApi, updateApi } from '../../api/scopes';
import DisplayNameTable from '../display-names/DisplayNameTable.vue';
import PropertyTable from '../properties/PropertyTable.vue';

defineOptions({
  name: 'ScopeModal',
});
const emits = defineEmits<{
  (event: 'change', data: OpenIddictScopeDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type TabKeys =
  | 'authorize'
  | 'basic'
  | 'description'
  | 'dispalyName'
  | 'props'
  | 'resource';

const defaultModel = {} as OpenIddictScopeDto;

const form = ref<FormInstance>();
const formModel = ref<OpenIddictScopeDto>({ ...defaultModel });
const openIdConfiguration = ref<OpenIdConfiguration>();
const activeTab = ref<TabKeys>('basic');

const getSupportClaims = computed((): TransferItem[] => {
  const types = openIdConfiguration.value?.claims_supported ?? [];
  return types.map((type) => {
    return {
      key: type,
      title: type,
    };
  });
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
    const api = formModel.value.id
      ? updateApi(formModel.value.id, toValue(formModel))
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
      activeTab.value = 'basic';
      formModel.value = { ...defaultModel };
      modalApi.setState({
        title: $t('AbpOpenIddict.Scopes:AddNew'),
      });
      try {
        modalApi.setState({ loading: true });
        await onDiscovery();
        const { id } = modalApi.getData<OpenIddictScopeDto>();
        id && (await onGet(id));
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpOpenIddict.Scopes:AddNew'),
});
async function onGet(id: string) {
  const dto = await getApi(id);
  formModel.value = dto;
  modalApi.setState({
    title: `${$t('AbpOpenIddict.Scopes')} - ${dto.name}`,
  });
}
async function onDiscovery() {
  openIdConfiguration.value = await discoveryApi();
}
function onDescriptionChange(displayName: DisplayNameInfo) {
  formModel.value.descriptions ??= {};
  formModel.value.descriptions[displayName.culture] = displayName.displayName;
}
function onDescriptionDelete(displayName: DisplayNameInfo) {
  formModel.value.descriptions ??= {};
  delete formModel.value.descriptions[displayName.culture];
}
function onDisplayNameChange(displayName: DisplayNameInfo) {
  formModel.value.displayNames ??= {};
  formModel.value.displayNames[displayName.culture] = displayName.displayName;
}
function onDisplayNameDelete(displayName: DisplayNameInfo) {
  formModel.value.displayNames ??= {};
  delete formModel.value.displayNames[displayName.culture];
}
function onPropChange(prop: PropertyInfo) {
  formModel.value.properties ??= {};
  formModel.value.properties[prop.key] = prop.value;
}
function onPropDelete(prop: PropertyInfo) {
  formModel.value.properties ??= {};
  delete formModel.value.properties[prop.key];
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
        <TabPane key="basic" :tab="$t('AbpOpenIddict.BasicInfo')">
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:Name')"
            name="name"
            required
          >
            <Input v-model:value="formModel.name" autocomplete="off" />
          </FormItem>
        </TabPane>
        <!-- 显示名称 -->
        <TabPane key="dispalyName" :tab="$t('AbpOpenIddict.DisplayNames')">
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:DefaultDisplayName')"
            name="displayName"
          >
            <Input v-model:value="formModel.displayName" autocomplete="off" />
          </FormItem>
          <DisplayNameTable
            :data="formModel.displayNames"
            @change="onDisplayNameChange"
            @delete="onDisplayNameDelete"
          />
        </TabPane>
        <!-- 描述 -->
        <TabPane key="description" :tab="$t('AbpOpenIddict.Descriptions')">
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:DefaultDescription')"
            name="description"
          >
            <Input v-model:value="formModel.description" autocomplete="off" />
          </FormItem>
          <DisplayNameTable
            :data="formModel.descriptions"
            @change="onDescriptionChange"
            @delete="onDescriptionDelete"
          />
        </TabPane>
        <!-- 资源 -->
        <TabPane key="resource" :tab="$t('AbpOpenIddict.Resources')">
          <Transfer
            v-model:target-keys="formModel.resources"
            :data-source="getSupportClaims"
            :list-style="{
              width: '47%',
              height: '338px',
            }"
            :render="(item) => item.title"
            :titles="[
              $t('AbpOpenIddict.Assigned'),
              $t('AbpOpenIddict.Available'),
            ]"
            class="tree-transfer"
          />
        </TabPane>
        <!-- 属性 -->
        <TabPane key="props" :tab="$t('AbpOpenIddict.Propertites')">
          <PropertyTable
            :data="formModel.properties"
            @change="onPropChange"
            @delete="onPropDelete"
          />
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
