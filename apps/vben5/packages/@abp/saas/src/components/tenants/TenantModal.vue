<script setup lang="ts">
import type { FormExpose } from 'ant-design-vue/es/form/Form';

import type { EditionDto } from '../../types';
import type {
  TenantConnectionStringDto,
  TenantCreateDto,
  TenantDto,
  TenantUpdateDto,
} from '../../types/tenants';

import { computed, onMounted, ref, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useValidation } from '@abp/core';
import {
  Checkbox,
  DatePicker,
  Form,
  Input,
  InputPassword,
  message,
  Select,
  Textarea,
} from 'ant-design-vue';
import dayjs from 'dayjs';
import debounce from 'lodash.debounce';

import { useEditionsApi } from '../../api/useEditionsApi';
import { useTenantsApi } from '../../api/useTenantsApi';
import ConnectionStringTable from './ConnectionStringTable.vue';

const emits = defineEmits<{
  (event: 'change', val: TenantDto): void;
}>();

const FormItem = Form.Item;

const defaultModel = {
  connectionStrings: [],
  isActive: true,
  useSharedDatabase: true,
} as unknown as TenantDto;

const { fieldDoNotValidEmailAddress, fieldRequired } = useValidation();
const form = useTemplateRef<FormExpose>('form');
const tenant = ref({ ...defaultModel });
const editions = ref<EditionDto[]>([]);
const activeTabKey = ref('basic');

const getFormRules = computed(() => {
  return {
    adminEmailAddress: [
      ...fieldRequired({
        name: 'AdminEmailAddress',
        prefix: 'DisplayName',
        resourceName: 'AbpSaas',
      }),
      ...fieldDoNotValidEmailAddress({
        name: 'AdminEmailAddress',
        prefix: 'DisplayName',
        resourceName: 'AbpSaas',
      }),
    ],
    adminPassword: fieldRequired({
      name: 'AdminPassword',
      prefix: 'DisplayName',
      resourceName: 'AbpSaas',
    }),
    defaultConnectionString: fieldRequired({
      name: 'DefaultConnectionString',
      prefix: 'DisplayName',
      resourceName: 'AbpSaas',
    }),
    name: fieldRequired({
      name: 'TenantName',
      prefix: 'DisplayName',
      resourceName: 'AbpSaas',
    }),
  };
});
/** 启用时间不可晚于禁用时间 */
const getDisabledEnableTime = (current: dayjs.Dayjs) => {
  if (!tenant.value.disableTime) {
    return false;
  }
  return (
    current &&
    current > dayjs(tenant.value.disableTime).add(-1, 'day').endOf('day')
  );
};
/** 禁用时间不可早于启用时间 */
const getDisabledDisableTime = (current: dayjs.Dayjs) => {
  if (!tenant.value.enableTime) {
    return false;
  }
  return current && current < dayjs(tenant.value.enableTime).endOf('day');
};

const { cancel, createApi, getApi, updateApi } = useTenantsApi();
const { getPagedListApi: getEditions } = useEditionsApi();

const [Modal, modalApi] = useVbenModal({
  class: 'w-[600px]',
  onClosed: cancel,
  async onConfirm() {
    await form.value?.validate();
    await onSubmit();
  },
  async onOpenChange(isOpen) {
    activeTabKey.value = 'basic';
    if (isOpen) {
      await onGet();
    }
  },
  title: $t('AbpSaas.Tenants'),
});

async function onGet() {
  const { id } = modalApi.getData<TenantDto>();
  if (!id) {
    tenant.value = { ...defaultModel };
    modalApi.setState({ title: $t('AbpSaas.NewTenant') });
    return;
  }
  try {
    modalApi.setState({ loading: true });
    const editionDto = await getApi(id);
    modalApi.setState({
      title: `${$t('AbpSaas.Tenants')} - ${editionDto.name}`,
    });
    tenant.value = editionDto;
  } finally {
    modalApi.setState({ loading: false });
  }
}

async function onSubmit() {
  try {
    modalApi.setState({ submitting: true });
    const api = tenant.value.id
      ? updateApi(tenant.value.id, tenant.value as TenantUpdateDto)
      : createApi(tenant.value as unknown as TenantCreateDto);
    const dto = await api;
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}

function onNameChange(name?: string) {
  if (
    !tenant.value.id &&
    (!tenant.value.adminEmailAddress ||
      !tenant.value.adminEmailAddress?.endsWith(`@${name}.com`))
  ) {
    tenant.value.adminEmailAddress = `admin@${name}.com`;
  }
}

function onConnectionChange(data: TenantConnectionStringDto) {
  return new Promise<void>((resolve) => {
    tenant.value.connectionStrings ??= [];
    let connectionString = tenant.value.connectionStrings.find(
      (x: TenantConnectionStringDto) => x.name === data.name,
    );
    if (connectionString) {
      connectionString.value = data.value;
    } else {
      connectionString = data;
      tenant.value.connectionStrings = [
        ...tenant.value.connectionStrings,
        data,
      ];
    }
    resolve();
  });
}

function onConnectionDelete(data: TenantConnectionStringDto) {
  return new Promise<void>((resolve) => {
    tenant.value.connectionStrings ??= [];
    tenant.value.connectionStrings = tenant.value.connectionStrings.filter(
      (x: TenantConnectionStringDto) => x.name !== data.name,
    );
    resolve();
  });
}

const onSearchEditions = debounce(async (filter?: string) => {
  const { items } = await getEditions({ filter });
  editions.value = items;
}, 500);

onMounted(onSearchEditions);
</script>

<template>
  <Modal>
    <Form
      ref="form"
      :model="tenant"
      :label-col="{ span: 6 }"
      :wapper-col="{ span: 18 }"
      :rules="getFormRules"
    >
      <FormItem name="isActive" :label="$t('AbpSaas.DisplayName:IsActive')">
        <Checkbox v-model:checked="tenant.isActive">
          {{ $t('AbpSaas.DisplayName:IsActive') }}
        </Checkbox>
      </FormItem>
      <FormItem
        v-if="!tenant.id"
        name="adminEmailAddress"
        :label="$t('AbpSaas.DisplayName:AdminEmailAddress')"
        :label-col="{ span: 8 }"
        :wapper-col="{ span: 16 }"
      >
        <Input
          type="email"
          v-model:value="tenant.adminEmailAddress"
          autocomplete="off"
        />
      </FormItem>
      <FormItem
        v-if="!tenant.id"
        name="adminPassword"
        :label="$t('AbpSaas.DisplayName:AdminPassword')"
      >
        <InputPassword
          v-model:value="tenant.adminPassword"
          autocomplete="off"
        />
      </FormItem>
      <FormItem name="name" :label="$t('AbpSaas.DisplayName:TenantName')">
        <Input
          v-model:value="tenant.name"
          @change="(e) => onNameChange(e.target.value)"
          autocomplete="off"
        />
      </FormItem>
      <FormItem name="editionId" :label="$t('AbpSaas.DisplayName:EditionName')">
        <Select
          :options="editions"
          :field-names="{ label: 'displayName', value: 'id' }"
          v-model:value="tenant.editionId"
          allow-clear
          show-search
          :filter-option="false"
          @search="onSearchEditions"
        />
      </FormItem>
      <FormItem name="enableTime" :label="$t('AbpSaas.DisplayName:EnableTime')">
        <DatePicker
          class="w-full"
          value-format="YYYY-MM-DD"
          :disabled-date="getDisabledEnableTime"
          v-model:value="tenant.enableTime"
        />
      </FormItem>
      <FormItem
        name="disableTime"
        :label="$t('AbpSaas.DisplayName:DisableTime')"
      >
        <DatePicker
          class="w-full"
          value-format="YYYY-MM-DD"
          :disabled-date="getDisabledDisableTime"
          v-model:value="tenant.disableTime"
        />
      </FormItem>
      <FormItem
        v-if="!tenant.id"
        name="useSharedDatabase"
        :label="$t('AbpSaas.DisplayName:UseSharedDatabase')"
      >
        <Checkbox v-model:checked="tenant.useSharedDatabase">
          {{ $t('AbpSaas.DisplayName:UseSharedDatabase') }}
        </Checkbox>
      </FormItem>
      <template v-if="!tenant.id && !tenant.useSharedDatabase">
        <FormItem
          name="defaultConnectionString"
          :label="$t('AbpSaas.DisplayName:DefaultConnectionString')"
        >
          <Textarea
            :auto-size="{ minRows: 2 }"
            v-model:value="tenant.defaultConnectionString"
          />
        </FormItem>
        <ConnectionStringTable
          :connection-strings="tenant.connectionStrings"
          :submit="onConnectionChange"
          :delete="onConnectionDelete"
        />
      </template>
    </Form>
  </Modal>
</template>

<style scoped></style>
