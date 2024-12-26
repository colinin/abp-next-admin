<script setup lang="ts">
import type { PropertyInfo } from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type { PermissionDefinitionDto } from '../../../types/definitions';
import type { PermissionGroupDefinitionDto } from '../../../types/groups';

import { defineEmits, defineOptions, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  listToTree,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { LocalizableInput, PropertyTable } from '@abp/ui';
import {
  Checkbox,
  Form,
  Input,
  message,
  Select,
  Tabs,
  TreeSelect,
} from 'ant-design-vue';

import {
  createApi,
  getApi,
  getListApi as getPermissionsApi,
  updateApi,
} from '../../../api/definitions';
import { getListApi as getGroupsApi } from '../../../api/groups';
import { useTypesMap } from './types';

defineOptions({
  name: 'PermissionDefinitionModal',
});
const emits = defineEmits<{
  (event: 'change', data: PermissionDefinitionDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

interface PermissionTreeVo {
  children: PermissionTreeVo[];
  displayName: string;
  groupName: string;
  name: string;
}

type TabKeys = 'basic' | 'props';

const defaultModel = {
  isEnabled: true,
} as PermissionDefinitionDto;

const isEditModel = ref(false);
const activeTab = ref<TabKeys>('basic');
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<PermissionDefinitionDto>({ ...defaultModel });
const { multiTenancySideOptions, providerOptions } = useTypesMap();
const availableGroups = ref<PermissionGroupDefinitionDto[]>([]);
const availablePermissions = ref<PermissionTreeVo[]>([]);

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();
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
      availablePermissions.value = [];
      availableGroups.value = [];
      modalApi.setState({
        showConfirmButton: true,
        title: $t('AbpPermissionManagement.PermissionDefinitions:AddNew'),
      });
      try {
        modalApi.setState({ loading: true });
        const { groupName, name } = modalApi.getData<PermissionDefinitionDto>();
        name && (await onGet(name));
        await onInitGroups(groupName);
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpPermissionManagement.PermissionDefinitions:AddNew'),
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
    title: `${$t('AbpPermissionManagement.PermissionDefinitions')} - ${dto.name}`,
  });
}
async function onGroupChange(name?: string) {
  const { items } = await getPermissionsApi({
    groupName: name,
  });
  const permissions = items.map((permission) => {
    const localizablePermission = deserialize(permission.displayName);
    return {
      ...permission,
      disabled: permission.name === formModel.value.name,
      displayName: Lr(
        localizablePermission.resourceName,
        localizablePermission.name,
      ),
    };
  });
  availablePermissions.value = listToTree(permissions, {
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
        <TabPane key="basic" :tab="$t('AbpPermissionManagement.BasicInfo')">
          <FormItem
            :label="$t('AbpPermissionManagement.DisplayName:IsEnabled')"
            name="isEnabled"
          >
            <Checkbox
              v-model:checked="formModel.isEnabled"
              :disabled="formModel.isStatic"
            >
              {{ $t('AbpPermissionManagement.DisplayName:IsEnabled') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :label="$t('AbpPermissionManagement.DisplayName:GroupName')"
            name="groupName"
            required
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
            v-if="availablePermissions.length > 0"
            :label="$t('AbpPermissionManagement.DisplayName:ParentName')"
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
              :tree-data="availablePermissions"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpPermissionManagement.DisplayName:Name')"
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
            :label="$t('AbpPermissionManagement.DisplayName:DisplayName')"
            name="displayName"
            required
          >
            <LocalizableInput
              v-model:value="formModel.displayName"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpPermissionManagement.DisplayName:MultiTenancySide')"
            name="multiTenancySide"
          >
            <Select
              v-model:value="formModel.multiTenancySide"
              :disabled="formModel.isStatic"
              :options="multiTenancySideOptions"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpPermissionManagement.DisplayName:Providers')"
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
import { useLocalization, useLocalizationSerializer } from '@abp/core';
