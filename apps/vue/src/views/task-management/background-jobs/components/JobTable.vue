<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('TaskManagement.BackgroundJobs.Start')"
          :disabled="!isMultiSelected"
          @click="handleStart"
          >{{ L('BackgroundJobs:Start') }}</a-button
        >
        <a-button
          v-if="hasPermission('TaskManagement.BackgroundJobs.Stop')"
          type="primary"
          danger
          :disabled="!isMultiSelected"
          @click="handleStop"
          >{{ L('BackgroundJobs:Stop') }}</a-button
        >
        <a-button
          v-if="hasPermission('TaskManagement.BackgroundJobs.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('BackgroundJobs:AddNew') }}</a-button
        >
        <a-button
          v-if="hasPermission('TaskManagement.BackgroundJobs.Delete')"
          type="primary"
          danger
          :disabled="!isMultiSelected"
          @click="handleDeleteMany"
          >{{ L('Delete') }}</a-button
        >
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'isEnabled'">
          <Switch :checked="record.isEnabled" readonly />
        </template>
        <template v-else-if="column.key === 'name'">
          <a href="javascript:(0);" @click="handleDetail(record)">{{ record.name }}</a>
        </template>
        <template v-else-if="column.key === 'status'">
          <Tooltip v-if="record.isAbandoned" color="orange" :title="L('Description:IsAbandoned')">
            <Tag :color="JobStatusColor[record.status]">{{ JobStatusMap[record.status] }}</Tag>
          </Tooltip>
          <Tag v-else :color="JobStatusColor[record.status]">{{ JobStatusMap[record.status] }}</Tag>
        </template>
        <template v-else-if="column.key === 'jobType'">
          <Tag color="blue">{{ JobTypeMap[record.jobType] }}</Tag>
        </template>
        <template v-else-if="column.key === 'priority'">
          <Tag :color="JobPriorityColor[record.priority]">{{
            JobPriorityMap[record.priority]
          }}</Tag>
        </template>
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'TaskManagement.BackgroundJobs.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'TaskManagement.BackgroundJobs.Delete',
                color: 'error',
                label: L('Delete'),
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'TaskManagement.BackgroundJobs.Pause',
                label: L('BackgroundJobs:Pause'),
                ifShow: [JobStatus.Queuing, JobStatus.Running, JobStatus.FailedRetry].includes(
                  record.status,
                ),
                onClick: handlePause.bind(null, record),
              },
              {
                auth: 'TaskManagement.BackgroundJobs.Resume',
                label: L('BackgroundJobs:Resume'),
                ifShow: [JobStatus.Paused, JobStatus.Stopped].includes(record.status),
                onClick: handleResume.bind(null, record),
              },
              {
                auth: 'TaskManagement.BackgroundJobs.Trigger',
                label: L('BackgroundJobs:Trigger'),
                ifShow: [
                  JobStatus.Queuing,
                  JobStatus.Running,
                  JobStatus.Completed,
                  JobStatus.FailedRetry,
                ].includes(record.status),
                onClick: handleTrigger.bind(null, record),
              },
              {
                auth: 'TaskManagement.BackgroundJobs.Create',
                label: L('BackgroundJobs:Copy'),
                onClick: handleCopy.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <JobModal @change="reloadTable" @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, ref } from 'vue';
  import { Switch, Tag, Tooltip } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useGo } from '/@/hooks/web/usePage';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import {
    getList,
    pause,
    resume,
    trigger,
    deleteById,
    bulkStop,
    bulkStart,
    bulkDelete,
    getAvailableFields,
    advancedSearch,
  } from '/@/api/task-management/jobs';
  import { JobStatus } from '/@/api/task-management/jobs/model';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import {
    JobStatusMap,
    JobStatusColor,
    JobTypeMap,
    JobPriorityMap,
    JobPriorityColor,
  } from '../datas/typing';
  import JobModal from './JobModal.vue';

  const go = useGo();
  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['TaskManagement', 'AbpUi']);
  const { hasPermission } = usePermission();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { reload, clearSelectedRowKeys }] = useTable({
    rowKey: 'id',
    title: L('BackgroundJobs'),
    columns: getDataColumns(),
    api: getList,
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: true,
    clickToRowSelect: false,
    formConfig: getSearchFormSchemas(),
    advancedSearchConfig: {
      useAdvancedSearch: true,
      defineFieldApi: getAvailableFields,
      fetchApi: advancedSearch,
    },
    rowSelection: {
      type: 'checkbox',
      onChange: handleSelectChange,
    },
    actionColumn: {
      width: 220,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const selectedRowKeys = ref<string[]>([]);
  const isMultiSelected = computed(() => {
    return selectedRowKeys.value.length;
  });

  function handleSelectChange(keys) {
    selectedRowKeys.value = keys;
  }

  function handleAddNew() {
    openModal(true, { id: null });
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleDetail(record) {
    go(`/task-management/background-jobs/${record.id}`);
  }

  function handlePause(record) {
    pause(record.id).then(() => {
      createMessage.success(L('Successful'));
      reloadTable();
    });
  }

  function handleResume(record) {
    resume(record.id).then(() => {
      createMessage.success(L('Successful'));
      reloadTable();
    });
  }

  function handleTrigger(record) {
    trigger(record.id).then(() => {
      createMessage.success(L('Successful'));
      reloadTable();
    });
  }

  function handleStart() {
    bulkStart(selectedRowKeys.value).then(() => {
      createMessage.success(L('Successful'));
      reloadTable();
    });
  }

  function handleStop() {
    bulkStop(selectedRowKeys.value).then(() => {
      createMessage.success(L('Successful'));
      reloadTable();
    });
  }

  function handleDeleteMany() {
    if (selectedRowKeys.value.length <= 0) {
      return;
    }
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('MultipleSelectJobsWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        return bulkDelete(selectedRowKeys.value).then(() => {
          reloadTable();
        });
      },
    });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('MultipleSelectJobsWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        return deleteById(record.id).then(() => {
          reloadTable();
        });
      },
    });
  }

  function handleCopy(record) {
    openModal(true, { id: record.id, copy: true });
  }

  function reloadTable() {
    clearSelectedRowKeys();
    reload();
  }
</script>
