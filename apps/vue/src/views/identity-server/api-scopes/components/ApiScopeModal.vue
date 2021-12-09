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
      :model="modelRef"
      :rules="formRules"
      :label-col="labelCol"
      :wrapper-col="wrapperCol"
    >
      <Tabs v-model:activeKey="tabActivedKey" @change="handleChangeTab">
        <!-- Api 资源基本信息 -->
        <TabPane key="basic" :tab="L('Basics')">
          <FormItem name="enabled" :label="L('Enabled')">
            <Checkbox v-model:checked="modelRef.enabled">{{ L('Enabled') }}</Checkbox>
          </FormItem>
          <FormItem name="required" :label="L('Required')">
            <Checkbox v-model:checked="modelRef.required">{{ L('Required') }}</Checkbox>
          </FormItem>
          <FormItem name="emphasize" :label="L('Emphasize')">
            <Checkbox v-model:checked="modelRef.emphasize">{{ L('Emphasize') }}</Checkbox>
          </FormItem>
          <FormItem name="showInDiscoveryDocument" :label="L('ShowInDiscoveryDocument')">
            <Checkbox v-model:checked="modelRef.showInDiscoveryDocument">{{
              L('ShowInDiscoveryDocument')
            }}</Checkbox>
          </FormItem>
          <FormItem name="name" required :label="L('Name')">
            <Input v-model:value="modelRef.name" :disabled="isEdit" />
          </FormItem>
          <FormItem name="displayName" :label="L('DisplayName')">
            <Input v-model:value="modelRef.displayName" />
          </FormItem>
          <FormItem name="description" :label="L('Description')">
            <Input v-model:value="modelRef.description" />
          </FormItem>
        </TabPane>

        <!-- Api 资源用户声明 -->
        <TabPane key="claim" :tab="L('UserClaim')">
          <UserClaim :target-claims="targetClaims" @change="handleClaimChange" />
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
                  <MenuItem key="properties">{{ L('Propertites') }}</MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component
            :is="advancedComponent"
            :properties="modelRef.properties"
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
  import { useClaim } from '../hooks/useClaim';
  import { useProperty } from '../hooks/useProperty';
  import UserClaim from '../../components/UserClaim.vue';
  import Properties from '../../components/Properties.vue';
  export default defineComponent({
    name: 'ApiScopeModal',
    components: {
      UserClaim,
      Properties,
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
      const modelIdRef = ref('');
      const tabActivedKey = ref('basic');
      const advancedComponent = ref('properties');
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
      const { handleNewProperty, handleDeleteProperty } = useProperty({ modelRef });
      const { targetClaims, handleClaimChange } = useClaim({ modelRef });

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
        modelRef,
        advancedComponent,
        labelCol: { span: 6 },
        wrapperCol: { span: 18 },
        handleClickMenu,
        handleNewProperty,
        handleDeleteProperty,
        handleChangeTab,
        handleVisibleModal,
        handleOk,
        targetClaims,
        handleClaimChange,
      };
    },
  });
</script>
