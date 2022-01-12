<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="title"
    :width="800"
    :min-height="600"
    @ok="handleSubmit"
    @visible-change="handleVisibleChange"
  >
    <Row>
      <Col :span="24">
        <Checkbox
          :checked="permissionTreeCheckState.checked"
          :indeterminate="permissionTreeCheckState.indeterminate"
          @change="handleGrantAllPermission"
          >{{ L('SelectAllInAllTabs') }}</Checkbox
        >
      </Col>
      <Divider />
      <Col :span="24">
        <Tabs v-model="activeKey" tab-position="left" type="card">
          <TabPane
            v-for="permission in permissionTree"
            :key="permission.name"
            :tab="permissionTab(permission)"
          >
            <Card :title="permission.displayName" :bordered="false">
              <Checkbox
                :checked="permissionTabCheckState(permission).checked"
                :indeterminate="permissionTabCheckState(permission).indeterminate"
                @change="(e) => handleGrantPermissions(permission, e)"
                >{{ L('SelectAllInThisTab') }}</Checkbox
              >
              <Divider />
              <BasicTree
                :checkable="true"
                :checkStrictly="true"
                :disabled="permissionTreeDisabled"
                :treeData="permission.children"
                :replaceFields="{
                  key: 'name',
                  title: 'displayName',
                  children: 'children',
                }"
                :checkedKeys="permissionGrantKeys(permission)"
                @check="
                  (selectKeys, event) => handlePermissionGranted(permission, selectKeys, event)
                "
              />
            </Card>
          </TabPane>
        </Tabs>
      </Col>
    </Row>
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { message } from 'ant-design-vue';
  import { Card, Checkbox, Col, Divider, Row, Tabs } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTree } from '/@/components/Tree';
  import { usePermissions } from './hooks/usePermissions';
  import { PermissionProps } from './types/permission';

  const defaultProps: PermissionProps = {
    providerName: 'G',
    providerKey: '',
    readonly: false,
  };

  export default defineComponent({
    name: 'PermissionModal',
    components: {
      BasicModal,
      BasicTree,
      Card,
      Checkbox,
      Divider,
      Col,
      Row,
      Tabs,
      TabPane: Tabs.TabPane,
    },
    setup() {
      const { L } = useLocalization('AbpPermissionManagement');
      const activeKey = ref('');
      const model = ref<PermissionProps>(defaultProps);
      const {
        title,
        permissionTree,
        permissionTab,
        permissionGrantKeys,
        permissionTabCheckState,
        permissionTreeCheckState,
        permissionTreeDisabled,
        handlePermissionGranted,
        handleSavePermission,
        handleGrantAllPermission,
        handleGrantPermissions,
      } = usePermissions({
        getPropsRef: model,
      });
      const [registerModal, { closeModal, changeOkLoading }] = useModalInner((val) => {
        model.value = val;
      });

      function handleVisibleChange(visible: boolean) {
        if (!visible) {
          model.value.providerKey = '';
        }
      }

      function handleSubmit() {
        changeOkLoading(true);
        handleSavePermission()
          .then(() => {
            message.success(L('Successful'));
            closeModal();
          })
          .finally(() => {
            changeOkLoading(false);
          });
      }

      return {
        L,
        activeKey,
        title,
        permissionTab,
        permissionTree,
        permissionGrantKeys,
        permissionTabCheckState,
        permissionTreeCheckState,
        permissionTreeDisabled,
        handlePermissionGranted,
        handleGrantAllPermission,
        handleGrantPermissions,
        registerModal,
        handleSubmit,
        handleVisibleChange,
      };
    },
  });
</script>
