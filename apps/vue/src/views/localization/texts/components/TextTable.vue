<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('LocalizationManagement.Text.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('Text:AddNew') }}</a-button
        >
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'LocalizationManagement.Text.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <TextModal @change="handleChange" @register="registerModal" />
  </div>
</template>

<script lang="ts">
  import { cloneDeep } from 'lodash-es';
  import { defineComponent } from 'vue';
  import { Switch } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getList,  } from '/@/api/localization/texts';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import TextModal from './TextModal.vue';

  export default defineComponent({
    name: 'TextTable',
    components: {
      BasicTable,
      Switch,
      TableAction,
      TextModal,
    },
    setup() {
      const { L } = useLocalization(['LocalizationManagement', 'AbpUi']);
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const [registerTable, { setTableData, setPagination, getForm }] = useTable({
        rowKey: 'id',
        title: L('Texts'),
        columns: getDataColumns(),
        pagination: true,
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,
        immediate: false,
        formConfig: getSearchFormSchemas(fetchTexts),
        actionColumn: {
          width: 120,
          title: L('Actions'),
          dataIndex: 'action',
        },
      });

      function fetchTexts() {
        setPagination({
          current: 1,
        });
        const form = getForm();
        return form.validate().then((input) => {
          const request = cloneDeep(input);
          request.onlyNull = input.onlyNull === 1;
          return getList(request).then((res) => {
            return setTableData(res.items);
          });
        })
      }

      function handleChange() {
        fetchTexts();
      }

      function handleAddNew() {
        openModal(true, { id: null });
      }

      function handleEdit(record) {
        openModal(true, {...{ id: 1 }, ...record});
      }

      return {
        L,
        hasPermission,
        registerTable,
        registerModal,
        openModal,
        handleChange,
        handleAddNew,
        handleEdit,
      };
    },
  });
</script>
