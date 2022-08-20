<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="formTitle"
    :width="1000"
    :min-height="500"
    :mask-closable="false"
    @ok="handleOk"
    @visible-change="handleVisibleModal"
  >
    <Form
      ref="formElRef"
      :model="modelRef"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs
        v-model:activeKey="tabActivedKey"
        :style="tabsStyle.style"
        :tabBarStyle="tabsStyle.tabBarStyle"
        @change="handleChangeTab"
      >
        <!-- 基本信息 -->
        <TabPane key="basic" :tab="L('Basics')">
          <FormItem name="enabled" :label="L('Enabled')">
            <Checkbox v-model:checked="modelRef.enabled">{{ L('Enabled') }}</Checkbox>
          </FormItem>
          <FormItem name="requireRequestObject" :label="L('Client:RequireRequestObject')">
            <Checkbox v-model:checked="modelRef.requireRequestObject">{{
              L('Client:RequireRequestObject')
            }}</Checkbox>
          </FormItem>
          <FormItem name="clientId" required :label="L('Client:Id')">
            <BInput v-model:value="modelRef.clientId" :disabled="isEdit" />
          </FormItem>
          <FormItem name="clientName" required :label="L('Name')">
            <BInput v-model:value="modelRef.clientName" />
          </FormItem>
          <FormItem name="description" :label="L('Description')">
            <TextArea v-model:value="modelRef.description" />
          </FormItem>
          <FormItem name="protocolType" :label="L('Client:ProtocolType')">
            <Select v-model:value="modelRef.protocolType">
              <Option value="oidc">OpenID Connect</Option>
            </Select>
          </FormItem>
          <FormItem
            name="allowedIdentityTokenSigningAlgorithms"
            :label="L('Client:AllowedIdentityTokenSigningAlgorithms')"
          >
            <BInput v-model:value="modelRef.allowedIdentityTokenSigningAlgorithms" />
          </FormItem>
          <FormItem name="requirePkce" :label="L('Client:RequiredPkce')">
            <Checkbox v-model:checked="modelRef.requirePkce">{{
              L('Client:RequiredPkce')
            }}</Checkbox>
          </FormItem>
          <FormItem name="allowPlainTextPkce" :label="L('Client:AllowedPlainTextPkce')">
            <Checkbox v-model:checked="modelRef.allowPlainTextPkce">{{
              L('Client:AllowedPlainTextPkce')
            }}</Checkbox>
          </FormItem>
        </TabPane>

        <!-- Urls -->
        <TabPane key="urls">
          <template #tab>
            <Dropdown>
              <span
                >{{ L('Client:ApplicationUrls') }}
                <DownOutlined />
              </span>
              <template #overlay>
                <Menu @click="handleClickUrlsMenu">
                  <MenuItem key="ClientCallback">{{ L('Client:CallbackUrl') }}</MenuItem>
                  <MenuItem key="ClientCorsOrigins">{{
                    L('Client:AllowedCorsOrigins')
                  }}</MenuItem>
                  <MenuItem key="ClientLogoutRedirectUris">{{
                    L('Client:PostLogoutRedirectUri')
                  }}</MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component :is="componentsRef[urlsComponent]" :modelRef="modelRef" />
        </TabPane>

        <!-- 资源 -->
        <TabPane key="resources">
          <template #tab>
            <Dropdown>
              <span
                >{{ L('Client:Resources') }}
                <DownOutlined />
              </span>
              <template #overlay>
                <Menu @click="handleClickResourcesMenu">
                  <MenuItem key="ClientApiResource">{{ L('Resource:Api') }}</MenuItem>
                  <MenuItem key="ClientIdentityResource">{{ L('Resource:Identity') }}</MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component :is="componentsRef[resourcesComponent]" :modelRef="modelRef" />
        </TabPane>

        <!-- 认证/注销 -->
        <TabPane key="authentication" :tab="L('Authentication')">
          <FormItem
            name="frontChannelLogoutSessionRequired"
            :label="L('Client:FrontChannelLogoutSessionRequired')"
          >
            <Checkbox v-model:checked="modelRef.frontChannelLogoutSessionRequired">{{
              L('Client:FrontChannelLogoutSessionRequired')
            }}</Checkbox>
          </FormItem>
          <!-- :required="modelRef.frontChannelLogoutSessionRequired" -->
          <FormItem name="frontChannelLogoutUri" :label="L('Client:FrontChannelLogoutUri')">
            <BInput v-model:value="modelRef.frontChannelLogoutUri" />
          </FormItem>
          <FormItem name="enabled" :label="L('Client:BackChannelLogoutSessionRequired')">
            <Checkbox v-model:checked="modelRef.backChannelLogoutSessionRequired">{{
              L('Client:BackChannelLogoutSessionRequired')
            }}</Checkbox>
          </FormItem>
          <!-- :required="modelRef.backChannelLogoutSessionRequired" -->
          <FormItem name="backChannelLogoutUri" :label="L('Client:BackChannelLogoutUri')">
            <BInput v-model:value="modelRef.backChannelLogoutUri" />
          </FormItem>
        </TabPane>

        <!-- 令牌 -->
        <TabPane key="token" :tab="L('Token')">
          <FormItem name="identityTokenLifetime" :label="L('Client:IdentityTokenLifetime')">
            <InputNumber class="input-number" v-model:value="modelRef.identityTokenLifetime" />
          </FormItem>
          <FormItem name="accessTokenLifetime" :label="L('Client:AccessTokenLifetime')">
            <InputNumber class="input-number" v-model:value="modelRef.accessTokenLifetime" />
          </FormItem>
          <FormItem name="accessTokenType" :label="L('Client:AccessTokenType')">
            <Select v-model:value="modelRef.accessTokenType">
              <Option :value="0">Jwt</Option>
              <Option :value="1">Reference</Option>
            </Select>
          </FormItem>
          <FormItem name="authorizationCodeLifetime" :label="L('Client:AuthorizationCodeLifetime')">
            <InputNumber class="input-number" v-model:value="modelRef.authorizationCodeLifetime" />
          </FormItem>
          <FormItem
            name="absoluteRefreshTokenLifetime"
            :label="L('Client:AbsoluteRefreshTokenLifetime')"
          >
            <InputNumber
              class="input-number"
              v-model:value="modelRef.absoluteRefreshTokenLifetime"
            />
          </FormItem>
          <FormItem
            name="slidingRefreshTokenLifetime"
            :label="L('Client:SlidingRefreshTokenLifetime')"
          >
            <InputNumber
              class="input-number"
              v-model:value="modelRef.slidingRefreshTokenLifetime"
            />
          </FormItem>
          <FormItem name="refreshTokenUsage" :label="L('Client:RefreshTokenUsage')">
            <Select v-model:value="modelRef.refreshTokenUsage">
              <Option :value="0">ReUse</Option>
              <Option :value="1">OneTimeOnly</Option>
            </Select>
          </FormItem>
          <FormItem name="refreshTokenExpiration" :label="L('Client:RefreshTokenExpiration')">
            <Select v-model:value="modelRef.refreshTokenExpiration">
              <Option :value="0">Sliding</Option>
              <Option :value="1">Absolute</Option>
            </Select>
          </FormItem>
          <FormItem name="userSsoLifetime" :label="L('Client:UserSsoLifetime')">
            <InputNumber class="input-number" v-model:value="modelRef.userSsoLifetime" />
          </FormItem>
          <FormItem name="allowOfflineAccess" :label="L('Client:AllowedOfflineAccess')">
            <Checkbox v-model:checked="modelRef.allowOfflineAccess">{{
              L('Client:AllowedOfflineAccess')
            }}</Checkbox>
          </FormItem>
          <FormItem
            name="allowAccessTokensViaBrowser"
            :label="L('Client:AllowedAccessTokensViaBrowser')"
          >
            <Checkbox v-model:checked="modelRef.allowAccessTokensViaBrowser">{{
              L('Client:AllowedAccessTokensViaBrowser')
            }}</Checkbox>
          </FormItem>
          <FormItem
            name="updateAccessTokenClaimsOnRefresh"
            :label="L('Client:UpdateAccessTokenClaimsOnRefresh')"
          >
            <Checkbox v-model:checked="modelRef.updateAccessTokenClaimsOnRefresh">{{
              L('Client:UpdateAccessTokenClaimsOnRefresh')
            }}</Checkbox>
          </FormItem>
          <FormItem name="includeJwtId" :label="L('Client:IncludeJwtId')">
            <Checkbox v-model:checked="modelRef.includeJwtId">{{
              L('Client:IncludeJwtId')
            }}</Checkbox>
          </FormItem>
          <FormItem name="clientClaimsPrefix" :label="L('Client:ClientClaimsPrefix')">
            <BInput v-model:value="modelRef.clientClaimsPrefix" />
          </FormItem>
          <FormItem name="pairWiseSubjectSalt" :label="L('Client:PairWiseSubjectSalt')">
            <BInput v-model:value="modelRef.pairWiseSubjectSalt" />
          </FormItem>
        </TabPane>

        <!-- 同意屏幕 -->
        <TabPane key="consent" :tab="L('Consent')">
          <FormItem name="requireConsent" :label="L('Client:RequireConsent')">
            <Checkbox v-model:checked="modelRef.requireConsent">{{
              L('Client:RequireConsent')
            }}</Checkbox>
          </FormItem>
          <FormItem name="allowRememberConsent" :label="L('Client:AllowRememberConsent')">
            <Checkbox v-model:checked="modelRef.allowRememberConsent">{{
              L('Client:AllowRememberConsent')
            }}</Checkbox>
          </FormItem>
          <FormItem name="clientUri" :label="L('Client:ClientUri')">
            <BInput v-model:value="modelRef.clientUri" />
          </FormItem>
          <FormItem name="logoUri" :label="L('Client:LogoUri')">
            <BInput v-model:value="modelRef.logoUri" />
          </FormItem>
        </TabPane>

        <!-- 设备流程 -->
        <TabPane key="deviceFlow" :tab="L('DeviceFlow')">
          <FormItem name="userCodeType" :label="L('Client:UserCodeType')">
            <BInput v-model:value="modelRef.userCodeType" />
          </FormItem>
          <FormItem name="deviceCodeLifetime" :label="L('Client:DeviceCodeLifetime')">
            <InputNumber class="input-number" v-model:value="modelRef.deviceCodeLifetime" />
          </FormItem>
        </TabPane>

        <!-- 高级 -->
        <TabPane key="advanced">
          <template #tab>
            <Dropdown>
              <span
                >{{ L('Advanced') }}
                <DownOutlined />
              </span>
              <template #overlay>
                <Menu @click="handleClickAdvancedMenu">
                  <MenuItem key="ClientSecret">{{ L('Secret') }}</MenuItem>
                  <MenuItem key="ClientClaim">{{ L('Claims') }}</MenuItem>
                  <MenuItem key="ClientProperties">{{ L('Propertites') }}</MenuItem>
                  <MenuItem key="ClientGrantType">{{ L('Client:AllowedGrantTypes') }}</MenuItem>
                  <MenuItem key="ClientIdentityProvider">{{
                    L('Client:IdentityProviderRestrictions')
                  }}</MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component :is="componentsRef[advancedComponent]" :modelRef="modelRef" />
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, shallowRef } from 'vue';
  import { useTabsStyle } from '/@/hooks/component/useStyles';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { Checkbox, Dropdown, Menu, Tabs, Form, Input, InputNumber, Select } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useModal } from '../hooks/useModal';
  import ClientCallback from './ClientCallback.vue';
  import ClientCorsOrigins from './ClientCorsOrigins.vue';
  import ClientLogoutRedirectUris from './ClientLogoutRedirectUris.vue';
  import ClientApiResource from './ClientApiResource.vue';
  import ClientIdentityResource from './ClientIdentityResource.vue';
  import ClientSecret from './ClientSecret.vue';
  import ClientClaim from './ClientClaim.vue';
  import ClientProperties from './ClientProperties.vue';
  import ClientGrantType from './ClientGrantType.vue';
  import ClientIdentityProvider from './ClientIdentityProvider.vue';

  const FormItem = Form.Item;
  const MenuItem = Menu.Item;
  const TabPane = Tabs.TabPane;
  const TextArea = Input.TextArea;
  const Option = Select.Option;

  const componentsRef = shallowRef({
    'ClientCallback': ClientCallback,
    'ClientCorsOrigins': ClientCorsOrigins,
    'ClientLogoutRedirectUris': ClientLogoutRedirectUris,
    'ClientApiResource': ClientApiResource,
    'ClientIdentityResource': ClientIdentityResource,
    'ClientSecret': ClientSecret,
    'ClientClaim': ClientClaim,
    'ClientProperties': ClientProperties,
    'ClientGrantType': ClientGrantType,
    'ClientIdentityProvider': ClientIdentityProvider,
  });

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const formElRef = ref<any>(null);
  const modelIdRef = ref('');
  const tabActivedKey = ref('basic');
  const advancedComponent = ref('ClientSecret');
  const urlsComponent = ref('ClientCallback');
  const resourcesComponent = ref('ClientApiResource');
  const [registerModal, { changeOkLoading }] = useModalInner((val) => {
    modelIdRef.value = val.id;
  });
  const {
    isEdit,
    modelRef,
    formRules,
    formTitle,
    handleChangeTab,
    handleVisibleModal,
    handleSubmit,
  } = useModal({
    modelIdRef,
    formElRef,
    tabActivedKey,
  });
  const tabsStyle = useTabsStyle();

  function handleClickAdvancedMenu(e) {
    tabActivedKey.value = 'advanced';
    advancedComponent.value = e.key;
  }

  function handleClickUrlsMenu(e) {
    tabActivedKey.value = 'urls';
    urlsComponent.value = e.key;
  }

  function handleClickResourcesMenu(e) {
    tabActivedKey.value = 'resources';
    resourcesComponent.value = e.key;
  }

  function handleOk() {
    changeOkLoading(true);
    handleSubmit().then(() => {
      createMessage.success(L('Successful'));
      emits('change');
    }).finally(() => {
      changeOkLoading(false);
    });
  }
</script>

<style lang="less" scoped>
  .input-number {
    width: 100%;
  }
</style>
