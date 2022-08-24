<template>
  <BasicModal
    ref="modalRef"
    v-bind="$attrs"
    @register="registerModal"
    :title="getIdentity"
    :width="800"
    :min-height="600"
    :mask-closable="false"
    @ok="handleSubmit"
    @visible-change="handleVisibleChange"
  >
    <Row ref="rowRef">
      <Col :span="24" ref="preColRef">
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
                :clickRowToExpand="true"
                :disabled="permissionTreeDisabled"
                :treeData="permission.children"
                :fieldNames="{
                  key: 'name',
                  title: 'displayName',
                  children: 'children',
                }"
                :value="permissionGrantKeys(permission)"
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

<script lang="ts" setup>
  import { computed, ref } from 'vue';
  import { Card, Checkbox, Col, Divider, Row, Tabs } from 'ant-design-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTree } from '/@/components/Tree';
  import { usePermissions } from './hooks/usePermissions';
  import { PermissionProps } from './types/permission';

  const TabPane = Tabs.TabPane;

  const defaultProps: PermissionProps = {
    providerName: 'G',
    providerKey: '',
    readonly: false,
    identity: '',
  };

  const { createMessage } = useMessage();
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
  const getIdentity = computed(() => {
    if (model.value.identity) {
      return `${L('Permissions')} - ${model.value.identity}`;
    }
    return title.value;
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
        createMessage.success(L('Successful'));
        closeModal();
      })
      .finally(() => {
        changeOkLoading(false);
      });
  }
</script>
