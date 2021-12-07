<template>
  <Card :title="L('OrganizationUnit:Tree')">
    <template #extra>
      <a-button
        v-if="hasPermission('AbpIdentity.OrganizationUnits.Create')"
        type="primary"
        pre-icon="ant-design:plus-outlined"
        @click="handleAddNew"
      >
        {{ L('OrganizationUnit:AddRoot') }}
      </a-button>
    </template>
    <BasicTree
      ref="treeElRef"
      :draggable="true"
      :click-row-to-expand="false"
      :tree-data="organizationUnitTree"
      :replace-fields="{
        title: 'displayName',
        key: 'id',
      }"
      :before-right-click="getContentMenus"
      defaultExpandLevel="1"
      @select="handleSelect"
      @drop="handleDrop"
    />
    <BasicModalForm
      @register="registerModal"
      :form-items="formSchemas"
      :title="L('OrganizationUnit')"
      :save-changes="handleSubmit"
    />
  </Card>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Card } from 'ant-design-vue';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTree } from '/@/components/Tree';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { useOuTree } from '../hooks/useOuTree';

  export default defineComponent({
    name: 'OrganizationUnitTree',
    components: { BasicModalForm, BasicTree, Card },
    emits: ['change', 'select'],
    setup(_props, { emit }) {
      const { L } = useLocalization('AbpIdentity');
      const {
        formSchemas,
        registerModal,
        organizationUnitTree,
        getContentMenus,
        handleDrop,
        handleAddNew,
        handleSelect,
        handleSubmit,
      } = useOuTree({ emit });
      const { hasPermission } = usePermission();

      return {
        L,
        formSchemas,
        registerModal,
        organizationUnitTree,
        getContentMenus,
        hasPermission,
        handleDrop,
        handleAddNew,
        handleSelect,
        handleSubmit,
      };
    },
  });
</script>
