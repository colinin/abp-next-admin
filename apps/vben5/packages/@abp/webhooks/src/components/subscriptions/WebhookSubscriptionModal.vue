<script setup lang="ts">
import type { TenantDto } from '@abp/saas';
import type { PropertyInfo } from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type {
  WebhookAvailableGroupDto,
  WebhookSubscriptionDto,
} from '../../types';

import { defineEmits, defineOptions, ref, toValue, useTemplateRef } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { isNullOrWhiteSpace } from '@abp/core';
import { useTenantsApi } from '@abp/saas';
import { PropertyTable } from '@abp/ui';
import {
  Checkbox,
  Form,
  Input,
  InputNumber,
  InputPassword,
  message,
  Select,
  Tabs,
  Textarea,
  Tooltip,
} from 'ant-design-vue';
import debounce from 'lodash.debounce';

import { useSubscriptionsApi } from '../../api/useSubscriptionsApi';

defineOptions({
  name: 'WebhookSubscriptionModal',
});
const emits = defineEmits<{
  (event: 'change', data: WebhookSubscriptionDto): void;
}>();

const FormItem = Form.Item;
const FormItemRest = Form.ItemRest;
const SelectGroup = Select.OptGroup;
const SelectOption = Select.Option;
const TabPane = Tabs.TabPane;

const defaultModel: WebhookSubscriptionDto = {
  creationTime: new Date(),
  displayName: '',
  extraProperties: {},
  id: '',
  isActive: true,
  isStatic: false,
  webhooks: [],
  webhookUri: '',
};
type TabActiveKey = 'basic' | 'headers';

const isEditModel = ref(false);
const activeTabKey = ref<TabActiveKey>('basic');
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<WebhookSubscriptionDto>({ ...defaultModel });
const webhookGroups = ref<WebhookAvailableGroupDto[]>([]);
const tenants = ref<TenantDto[]>([]);

const { hasAccessByCodes } = useAccess();
const { getPagedListApi: getTenantsApi } = useTenantsApi();
const { cancel, createApi, getAllAvailableWebhooksApi, getApi, updateApi } =
  useSubscriptionsApi();

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('WebhookSubscriptionModal has closed!');
  },
  onConfirm: async () => {
    await form.value?.validate();
    const input = toValue(formModel);
    const api = isEditModel.value
      ? updateApi(formModel.value.id, input)
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
      activeTabKey.value = 'basic';
      formModel.value = { ...defaultModel };
      modalApi.setState({
        loading: true,
        showConfirmButton: true,
        title: $t('WebhooksManagement.Subscriptions:AddNew'),
      });
      try {
        const { id } = modalApi.getData<WebhookSubscriptionDto>();
        await onInit();
        !isNullOrWhiteSpace(id) && (await onGet(id));
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('WebhooksManagement.Subscriptions:AddNew'),
});
async function onGet(id: string) {
  isEditModel.value = true;
  const dto = await getApi(id);
  formModel.value = dto;
  modalApi.setState({
    showConfirmButton: !dto.isStatic,
    title: $t('WebhooksManagement.Subscriptions:Edit'),
  });
}
async function onInit() {
  const [webhookGroupRes, tenantRes] = await Promise.all([
    getAllAvailableWebhooksApi(),
    onInitTenants(),
  ]);
  webhookGroups.value = webhookGroupRes.items;
  tenants.value = tenantRes;
}
async function onInitTenants(filter?: string) {
  if (!hasAccessByCodes(['AbpSaas.Tenants'])) {
    return [];
  }
  const { items } = await getTenantsApi({ filter });
  return items;
}
function onWebhooksFilter(onputValue: string, option: any) {
  if (option.displayname) {
    return option.displayname.includes(onputValue);
  }
  if (option.label) {
    return option.label.includes(onputValue);
  }
  return true;
}
const onTenantsSearch = debounce(async (input?: string) => {
  const tenantRes = await onInitTenants(input);
  tenants.value = tenantRes;
}, 500);
function onPropChange(prop: PropertyInfo) {
  formModel.value.headers ??= {};
  formModel.value.headers[prop.key] = prop.value;
}
function onPropDelete(prop: PropertyInfo) {
  formModel.value.headers ??= {};
  delete formModel.value.headers[prop.key];
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
      <Tabs v-model:active-key="activeTabKey">
        <TabPane key="basic" :tab="$t('WebhooksManagement.BasicInfo')">
          <FormItem
            name="isActive"
            :label="$t('WebhooksManagement.DisplayName:IsActive')"
            :extra="$t('WebhooksManagement.Description:IsActive')"
          >
            <Checkbox
              :disabled="formModel.isStatic"
              v-model:checked="formModel.isActive"
            >
              {{ $t('WebhooksManagement.DisplayName:IsActive') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:WebhookUri')"
            :extra="$t('WebhooksManagement.Description:WebhookUri')"
            name="webhookUri"
            required
          >
            <Input
              v-model:value="formModel.webhookUri"
              :disabled="formModel.isStatic"
              autocomplete="off"
              allow-clear
            />
          </FormItem>
          <FormItem
            name="webhooks"
            :label="$t('WebhooksManagement.DisplayName:Webhooks')"
            :extra="$t('WebhooksManagement.Description:Webhooks')"
          >
            <Select
              :disabled="formModel.isStatic"
              allow-clear
              mode="multiple"
              v-model:value="formModel.webhooks"
              :filter-option="onWebhooksFilter"
            >
              <SelectGroup
                v-for="group in webhookGroups"
                :key="group.name"
                :label="group.displayName"
              >
                <SelectOption
                  v-for="option in group.webhooks"
                  :key="option.name"
                  :value="option.name"
                  :displayname="option.displayName"
                >
                  <Tooltip placement="right">
                    <template #title>
                      {{ option.description }}
                    </template>
                    {{ option.displayName }}
                  </Tooltip>
                </SelectOption>
              </SelectGroup>
            </Select>
          </FormItem>
          <FormItem
            v-if="hasAccessByCodes(['AbpSaas.Tenants'])"
            :label="$t('WebhooksManagement.DisplayName:TenantId')"
            :extra="$t('WebhooksManagement.Description:TenantId')"
            name="tenantId"
          >
            <Select
              v-model:value="formModel.tenantId"
              :disabled="formModel.isStatic"
              :options="tenants"
              :field-names="{ label: 'normalizedName', value: 'id' }"
              :filter-option="false"
              @search="onTenantsSearch"
              show-search
              allow-clear
            />
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:TimeoutDuration')"
            :extra="$t('WebhooksManagement.Description:TimeoutDuration')"
            name="timeoutDuration"
          >
            <InputNumber
              class="w-full"
              v-model:value="formModel.timeoutDuration"
              :disabled="formModel.isStatic"
              :min="10"
              :max="300"
              autocomplete="off"
            />
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:Secret')"
            :extra="$t('WebhooksManagement.Description:Secret')"
            name="secret"
          >
            <InputPassword
              v-model:value="formModel.secret"
              :disabled="formModel.isStatic"
              autocomplete="off"
              allow-clear
            />
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:Description')"
            name="description"
          >
            <Textarea
              v-model:value="formModel.description"
              :disabled="formModel.isStatic"
              :auto-size="{ minRows: 3 }"
              autocomplete="off"
              allow-clear
            />
          </FormItem>
        </TabPane>
        <TabPane
          key="headers"
          :tab="$t('WebhooksManagement.DisplayName:Headers')"
        >
          <FormItemRest>
            <PropertyTable
              :data="formModel.headers"
              :disabled="formModel.isStatic"
              @change="onPropChange"
              @delete="onPropDelete"
            />
          </FormItemRest>
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
