<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="formTitle"
    :width="660"
    :min-height="400"
    :mask-closable="false"
    @ok="handleOk"
    @visible-change="handleVisibleModal"
  >
    <Form
      ref="formElRef"
      :model="resourceRef"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:activeKey="tabActivedKey" @change="handleChangeTab">
        <!-- Api 资源基本信息 -->
        <TabPane key="basic" :tab="L('Basics')">
          <FormItem name="enabled" :label="L('Resource:Enabled')">
            <Checkbox v-model:checked="resourceRef.enabled">{{ L('Resource:Enabled') }}</Checkbox>
          </FormItem>
          <FormItem name="showInDiscoveryDocument" :label="L('ShowInDiscoveryDocument')">
            <Checkbox v-model:checked="resourceRef.showInDiscoveryDocument">{{
              L('ShowInDiscoveryDocument')
            }}</Checkbox>
          </FormItem>
          <FormItem name="name" required :label="L('Name')">
            <BInput v-model:value="resourceRef.name" :disabled="isEdit" />
          </FormItem>
          <FormItem name="displayName" :label="L('DisplayName')">
            <BInput v-model:value="resourceRef.displayName" />
          </FormItem>
          <FormItem name="description" :label="L('Description')">
            <BInput v-model:value="resourceRef.description" />
          </FormItem>
          <FormItem
            name="allowedAccessTokenSigningAlgorithms"
            :label="L('AllowedAccessTokenSigningAlgorithms')"
          >
            <BInput v-model:value="resourceRef.allowedAccessTokenSigningAlgorithms" />
          </FormItem>
        </TabPane>

        <!-- Api 资源用户声明 -->
        <TabPane key="claim" :tab="L('UserClaim')">
          <UserClaim :target-claims="targetClaims" @change="handleClaimChange" />
        </TabPane>

        <!-- Api 资源范围 -->
        <TabPane key="scope" :tab="L('Scope')">
          <ApiResourceScope :target-scopes="targetScopes" @change="handleScopeChange" />
        </TabPane>

        <!-- Api 资源密钥/属性 -->
        <TabPane key="advanced">
          <template #tab>
            <Dropdown>
              <span
                >{{ L('Advanced') }}
                <DownOutlined />
              </span>
              <template #overlay>
                <Menu @click="handleClickMenu">
                  <MenuItem key="ApiResourceSecret">{{ L('Secret') }}</MenuItem>
                  <MenuItem key="Properties">{{ L('Propertites') }}</MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component
            :is="componentsRef[advancedComponent]"
            :secrets="resourceRef.secrets"
            :properties="resourceRef.properties"
            @secrets-new="handleNewSecret"
            @secrets-delete="handleDeleteSecret"
            @props-new="handleNewProperty"
            @props-delete="handleDeleteProperty"
          />
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, shallowRef, nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { Checkbox, Dropdown, Menu, Tabs, Form } from 'ant-design-vue';
  import { Input } from '/@/components/Input';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useModal } from '../hooks/useModal';
  import { useSecret } from '../hooks/useSecret';
  import { useScope } from '../hooks/useScope';
  import { useClaim } from '../hooks/useClaim';
  import { useProperty } from '../hooks/useProperty';
  import ApiResourceScope from './ApiResourceScope.vue';
  import ApiResourceSecret from './ApiResourceSecret.vue';
  import UserClaim from '../../components/UserClaim.vue';
  import Properties from '../../components/Properties.vue';

  const FormItem = Form.Item;
  const MenuItem = Menu.Item;
  const TabPane = Tabs.TabPane;
  const BInput = Input!;

  const componentsRef = shallowRef({
    'ApiResourceSecret': ApiResourceSecret,
    'Properties': Properties,
  });

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const formElRef = ref<any>(null);
  const tabActivedKey = ref('basic');
  const advancedComponent = ref('ApiResourceSecret');
  const [registerModal, { changeOkLoading }] = useModalInner((val) => {
    nextTick(() => {
      fetchResource(val.id);
    });
  });
  const {
    isEdit,
    resourceRef,
    formRules,
    formTitle,
    fetchResource,
    handleChangeTab,
    handleVisibleModal,
    handleSubmit,
  } = useModal({
    formElRef,
    tabActivedKey,
  });
  const { handleNewSecret, handleDeleteSecret } = useSecret({ resourceRef });
  const { handleNewProperty, handleDeleteProperty } = useProperty({ resourceRef });
  const { targetClaims, handleClaimChange } = useClaim({ resourceRef });
  const { targetScopes, handleScopeChange } = useScope({ resourceRef });

  function handleClickMenu(e) {
    tabActivedKey.value = 'advanced';
    advancedComponent.value = e.key;
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
