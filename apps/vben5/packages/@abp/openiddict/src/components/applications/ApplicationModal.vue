<script setup lang="ts">
import type { OpenIdConfiguration } from '@abp/core';
import type { FormInstance } from 'ant-design-vue';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';
import type { DefaultOptionType } from 'ant-design-vue/es/select';
import type { TransferItem } from 'ant-design-vue/es/transfer';

import type { OpenIddictApplicationDto } from '../../types/applications';
import type { DisplayNameInfo } from '../display-names/types';
import type { PropertyInfo } from '../properties/types';

import {
  type Component,
  computed,
  defineAsyncComponent,
  defineEmits,
  defineOptions,
  reactive,
  ref,
  shallowRef,
  toValue,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { DownOutlined } from '@ant-design/icons-vue';
import {
  Dropdown,
  Form,
  Input,
  InputPassword,
  Menu,
  message,
  Select,
  Tabs,
  Transfer,
} from 'ant-design-vue';

import { createApi, getApi, updateApi } from '../../api/applications';
import { discoveryApi } from '../../api/openid';
import DisplayNameTable from '../display-names/DisplayNameTable.vue';
import PropertyTable from '../properties/PropertyTable.vue';

defineOptions({
  name: 'ApplicationModal',
});
const emits = defineEmits<{
  (event: 'change', data: OpenIddictApplicationDto): void;
}>();

const FormItem = Form.Item;
const MenuItem = Menu.Item;
const TabPane = Tabs.TabPane;

type TabKeys =
  | 'authorize'
  | 'basic'
  | 'dispalyName'
  | 'endpoint'
  | 'props'
  | 'scope';

const defaultModel = {
  applicationType: 'web',
  clientType: 'public',
  consentType: 'explicit',
} as OpenIddictApplicationDto;

const form = ref<FormInstance>();
const formModel = ref<OpenIddictApplicationDto>({ ...defaultModel });
const openIdConfiguration = ref<OpenIdConfiguration>();
const activeTab = ref<TabKeys>('basic');

const uriComponentState = reactive<{
  component: string;
  title: string;
  uris?: string[];
}>({
  component: 'RedirectUris',
  title: $t('AbpOpenIddict.DisplayName:RedirectUris'),
  uris: [],
});
const uriComponentMap = shallowRef<{
  [key: string]: Component;
}>({
  PostLogoutRedirectUris: defineAsyncComponent(
    () => import('../uris/UriTable.vue'),
  ),
  RedirectUris: defineAsyncComponent(() => import('../uris/UriTable.vue')),
});
const clientTypes = reactive<DefaultOptionType[]>([
  { label: 'public', value: 'public' },
  { label: 'confidential', value: 'confidential' },
]);
const applicationTypes = reactive<DefaultOptionType[]>([
  { label: 'Web', value: 'web' },
  { label: 'Native', value: 'native' },
]);
const consentTypes = reactive<DefaultOptionType[]>([
  { label: 'explicit', value: 'explicit' },
  { label: 'external', value: 'external' },
  { label: 'implicit', value: 'implicit' },
  { label: 'systematic', value: 'systematic' },
]);
const endpoints = reactive<DefaultOptionType[]>([
  { label: 'authorization', value: 'authorization' },
  { label: 'token', value: 'token' },
  { label: 'logout', value: 'logout' },
  { label: 'device', value: 'device' },
  { label: 'revocation', value: 'revocation' },
  { label: 'introspection', value: 'introspection' },
]);
const getGrantTypes = computed(() => {
  const types = openIdConfiguration.value?.grant_types_supported ?? [];
  return types.map((type) => {
    return {
      label: type,
      value: type,
    };
  });
});
const getResponseTypes = computed(() => {
  const types = openIdConfiguration.value?.response_types_supported ?? [];
  return types.map((type) => {
    return {
      label: type,
      value: type,
    };
  });
});
const getSupportScopes = computed((): TransferItem[] => {
  const types = openIdConfiguration.value?.scopes_supported ?? [];
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
      uriComponentState.uris = [];
      uriComponentState.component = 'RedirectUris';
      modalApi.setState({
        title: $t('AbpOpenIddict.Applications:AddNew'),
      });
      try {
        modalApi.setState({ loading: true });
        await onDiscovery();
        const claimTypeDto = modalApi.getData<OpenIddictApplicationDto>();
        if (claimTypeDto?.id) {
          const dto = await getApi(claimTypeDto.id);
          formModel.value = dto;
          uriComponentState.uris = dto.redirectUris;
          modalApi.setState({
            title: `${$t('AbpOpenIddict.Applications')} - ${dto.clientId}`,
          });
        }
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: 'ClaimType',
});
async function onDiscovery() {
  openIdConfiguration.value = await discoveryApi();
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
function onSwitchUri(info: MenuInfo) {
  activeTab.value = 'endpoint';
  const eventKey = String(info.key);
  switch (eventKey) {
    case 'PostLogoutRedirectUris': {
      uriComponentState.uris = formModel.value.postLogoutRedirectUris;
      uriComponentState.title = $t(
        'AbpOpenIddict.DisplayName:PostLogoutRedirectUris',
      );
      break;
    }
    case 'RedirectUris': {
      uriComponentState.uris = formModel.value.redirectUris;
      uriComponentState.title = $t('AbpOpenIddict.DisplayName:RedirectUris');
      break;
    }
  }
  uriComponentState.component = eventKey;
}
function onUriChange(uri: string) {
  switch (uriComponentState.component) {
    case 'PostLogoutRedirectUris': {
      formModel.value.postLogoutRedirectUris ??= [];
      formModel.value.postLogoutRedirectUris.push(uri);
      break;
    }
    case 'RedirectUris': {
      formModel.value.redirectUris ??= [];
      formModel.value.redirectUris.push(uri);
      break;
    }
  }
}
function onUriDelete(uri: string) {
  switch (uriComponentState.component) {
    case 'PostLogoutRedirectUris': {
      formModel.value.postLogoutRedirectUris ??= [];
      formModel.value.postLogoutRedirectUris =
        formModel.value.postLogoutRedirectUris.filter((item) => item !== uri);
      uriComponentState.uris = formModel.value.postLogoutRedirectUris;
      break;
    }
    case 'RedirectUris': {
      formModel.value.redirectUris ??= [];
      formModel.value.redirectUris = formModel.value.redirectUris.filter(
        (item) => item !== uri,
      );
      uriComponentState.uris = formModel.value.redirectUris;
      break;
    }
  }
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
            :label="$t('AbpOpenIddict.DisplayName:ApplicationType')"
            name="applicationType"
            required
          >
            <Select
              v-model:value="formModel.applicationType"
              :options="applicationTypes"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:ClientId')"
            name="clientId"
            required
          >
            <Input v-model:value="formModel.clientId" autocomplete="off" />
          </FormItem>
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:ClientType')"
            name="clientType"
          >
            <Select
              v-model:value="formModel.clientType"
              :options="clientTypes"
            />
          </FormItem>
          <FormItem
            v-if="!formModel.id && formModel.clientType === 'confidential'"
            :label="$t('AbpOpenIddict.DisplayName:ClientSecret')"
            name="clientSecret"
          >
            <InputPassword
              v-model:value="formModel.clientSecret"
              autocomplete="off"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:ClientUri')"
            name="clientUri"
          >
            <Input v-model:value="formModel.clientUri" />
          </FormItem>
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:LogoUri')"
            name="logoUri"
          >
            <Input v-model:value="formModel.logoUri" autocomplete="off" />
          </FormItem>
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:ConsentType')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
            name="consentType"
          >
            <Select
              v-model:value="formModel.consentType"
              :options="consentTypes"
              default-value="explicit"
            />
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
        <!-- 端点 -->
        <TabPane key="endpoint">
          <template #tab>
            <Dropdown>
              <span>
                {{ $t('AbpOpenIddict.Endpoints') }}
                <DownOutlined />
              </span>
              <template #overlay>
                <Menu @click="onSwitchUri">
                  <MenuItem key="RedirectUris">
                    {{ $t('AbpOpenIddict.DisplayName:RedirectUris') }}
                  </MenuItem>
                  <MenuItem key="PostLogoutRedirectUris">
                    {{ $t('AbpOpenIddict.DisplayName:PostLogoutRedirectUris') }}
                  </MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component
            :is="uriComponentMap[uriComponentState.component]"
            :title="uriComponentState.title"
            :uris="uriComponentState.uris"
            @change="onUriChange"
            @delete="onUriDelete"
          />
        </TabPane>
        <!-- 范围 -->
        <TabPane key="scope" :tab="$t('AbpOpenIddict.Scopes')">
          <Transfer
            v-model:target-keys="formModel.scopes"
            :data-source="getSupportScopes"
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
        <!-- 授权 -->
        <TabPane key="authorize" :tab="$t('AbpOpenIddict.Authorizations')">
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:Endpoints')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
            name="endpoints"
          >
            <Select
              v-model:value="formModel.endpoints"
              :options="endpoints"
              mode="tags"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:GrantTypes')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
            name="grantTypes"
          >
            <Select
              v-model:value="formModel.grantTypes"
              :options="getGrantTypes"
              mode="tags"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpOpenIddict.DisplayName:ResponseTypes')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
            name="responseTypes"
          >
            <Select
              v-model:value="formModel.responseTypes"
              :options="getResponseTypes"
              mode="tags"
            />
          </FormItem>
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
