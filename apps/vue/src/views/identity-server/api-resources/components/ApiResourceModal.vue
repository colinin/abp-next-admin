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
      :label-col="labelCol"
      :wrapper-col="wrapperCol"
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
            <Input v-model:value="resourceRef.name" :disabled="isEdit" />
          </FormItem>
          <FormItem name="displayName" :label="L('DisplayName')">
            <Input v-model:value="resourceRef.displayName" />
          </FormItem>
          <FormItem name="description" :label="L('Description')">
            <Input v-model:value="resourceRef.description" />
          </FormItem>
          <FormItem
            name="allowedAccessTokenSigningAlgorithms"
            :label="L('AllowedAccessTokenSigningAlgorithms')"
          >
            <Input v-model:value="resourceRef.allowedAccessTokenSigningAlgorithms" />
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
                  <MenuItem key="api-resource-secret">{{ L('Secret') }}</MenuItem>
                  <MenuItem key="properties">{{ L('Propertites') }}</MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component
            :is="advancedComponent"
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

<script lang="ts">
  import { defineComponent, ref } from 'vue';
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
  export default defineComponent({
    name: 'ApiResourceModal',
    components: {
      UserClaim,
      Properties,
      ApiResourceScope,
      ApiResourceSecret,
      BasicModal,
      DownOutlined,
      Form,
      FormItem: Form.Item,
      Dropdown,
      Menu,
      MenuItem: Menu.Item,
      Tabs,
      TabPane: Tabs.TabPane,
      Input,
      Checkbox,
    },
    emits: ['change', 'register'],
    setup(_, { emit }) {
      const { L } = useLocalization('AbpIdentityServer');
      const formElRef = ref<any>(null);
      const resourceIdRef = ref('');
      const tabActivedKey = ref('basic');
      const advancedComponent = ref('api-resource-secret');
      const [registerModal, { changeOkLoading }] = useModalInner((val) => {
        resourceIdRef.value = val.id;
      });
      const {
        isEdit,
        resourceRef,
        formRules,
        formTitle,
        handleChangeTab,
        handleVisibleModal,
        handleSubmit,
      } = useModal({
        resourceIdRef,
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
        handleSubmit()
          .then(() => {
            emit('change');
          })
          .finally(() => {
            changeOkLoading(false);
          });
      }

      return {
        L,
        isEdit,
        formElRef,
        formRules,
        formTitle,
        tabActivedKey,
        registerModal,
        resourceRef,
        advancedComponent,
        labelCol: { span: 6 },
        wrapperCol: { span: 18 },
        handleClickMenu,
        handleNewSecret,
        handleDeleteSecret,
        handleNewProperty,
        handleDeleteProperty,
        handleChangeTab,
        handleVisibleModal,
        handleOk,
        targetScopes,
        handleScopeChange,
        targetClaims,
        handleClaimChange,
      };
    },
  });
</script>
