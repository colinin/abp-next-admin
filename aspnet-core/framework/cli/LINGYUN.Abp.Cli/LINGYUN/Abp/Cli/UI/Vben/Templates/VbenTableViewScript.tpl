<template>
  <div :class="getWrapperClass">
    <BasicTable @register="registerTable">
      <template #toolbar>
        {{~ if model.has_create ~}}
        <Button
          {{~ if model.create_permission ~}}
          v-auth="['{{ model.create_permission_name }}']"
          {{~ end ~}}
          type="primary"
          @click="handleAddNew"
        >
         {%{ {{ L('}%}{{ model.application }}{%{:AddNew') }} }%}
        </Button>
        {{~ end ~}}
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {{~ if model.has_update ~}}
              {
                {{~ if model.update_permission ~}}
                auth: '{{ model.update_permission_name }}',
                {{~ end ~}}
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {{~ end ~}}
              {{~ if model.has_delete ~}}
              {
                {{~ if model.delete_permission ~}}
                auth: '{{ model.delete_permission_name }}',
                {{~ end ~}}
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
              {{~ end ~}}
            ]"
          />
        </template>
      </template>
    </BasicTable>
    {{~ if model.has_create || model.has_update ~}}
    <{{model.modal_name}} @register="registerModal" @change="reload" />
    {{~ end ~}}
  </div>
</template>

<script lang="ts" setup>
  import { computed, useAttrs } from 'vue';
  {{~ if model.has_create ~}}
  import { Button } from 'ant-design-vue';
  {{~ end ~}}
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormProps } from '../datas/ModalData';
  {{~ if model.has_create || model.has_update ~}}
  import { useModal } from '/@/components/Modal';
  import { useDesign } from '/@/hooks/web/useDesign';
  {{~ end ~}}
  {{~ if model.has_delete ~}}
  import { useMessage } from '/@/hooks/web/useMessage';
  {{~ end ~}}
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { {{ model.get_list_action }}{{ if model.has_delete }}, {{ model.delete_action }}{{ end }}{{ if model.has_advanced_search }}, {{ model.available_fields_action }}, {{ model.advanced_search_action }}{{ end }} } from '{{model.api_path}}';
  {{~ if model.paged_request ~}}
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  {{~ end ~}}
  import {{model.modal_name}} from './{{model.modal_name}}.vue';

  const attrs = useAttrs();
  const { prefixCls } = useDesign('{{model.application}}');
  const getWrapperClass = computed(() => {
    return [
      prefixCls,
      attrs.class,
      {
        [`${prefixCls}-container`]: true,
      },
    ];
  });
  const { L } = useLocalization(['{{model.remote_service}}', 'AbpUi']);
  {{~ if model.has_delete ~}}
  const { createConfirm, createMessage } = useMessage();
  {{~ end ~}}
  {{~ if model.has_update ~}}
  const [registerModal, { openModal }] = useModal();
  {{~ end ~}}
  const [registerTable, { {{ if model.has_delete || model.has_create || model.has_update }}reload{{ end }} }] = useTable({
    rowKey: '{{ model.key }}',
    title: L('{{model.application}}'),
    api: {{ model.get_list_action }},
    columns: getDataColumns(),
    {{~ if model.paged_request ~}}
    beforeFetch: formatPagedRequest,
    {{~ end ~}}
    pagination: true,
    striped: false,
    useSearchForm: true,
    showIndexColumn: false,
    showTableSetting: true,
    formConfig: getSearchFormProps(),
    {{~ if model.has_advanced_search ~}}
    advancedSearchConfig: {
      useAdvancedSearch: true,
      defineFieldApi: {{ model.available_fields_action }},
      fetchApi: {{ model.advanced_search_action }},
    },
    {{~ end ~}}
    bordered: true,
    canResize: true,
    immediate: true,
    {{~ if model.has_update || model.has_delete ~}}
    actionColumn: {
      width: 150,
      title: L('Actions'),
      dataIndex: 'action',
    },
    {{~ end ~}}
  });

  {{~ if model.has_create ~}}
  function handleAddNew() {
    openModal(true, {});
  }
  {{~ end ~}}

  {{~ if model.has_update ~}}
  function handleEdit(record) {
    openModal(true, record);
  }
  {{~ end ~}}

  {{~ if model.has_delete ~}}
  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return {{ model.delete_action }}(record.{{ model.key }}).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
  {{~ end ~}}
</script>

<style lang="less" scoped>
    @prefix-cls: ~'@{namespace}-{{model.application}}';
    .@{prefix-cls} {
        max-width: 100%;
        height: 100%;
    }
</style>