<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('TaskManagement.BackgroundJobs.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('BackgroundJobs:AddNew') }}</a-button
        >
      </template>
      <template #enable="{ record }">
        <Switch :checked="record.isEnabled" disabled />
      </template>
      <template #status="{ record }">
        <Tooltip v-if="record.isAbandoned" color="orange" :title="L('Description:IsAbandoned')">
          <Tag :color="JobStatusColor[record.status]">{{ JobStatusMap[record.status] }}</Tag>
        </Tooltip>
        <Tag v-else :color="JobStatusColor[record.status]">{{ JobStatusMap[record.status] }}</Tag>
      </template>
      <template #type="{ record }">
        <Tag color="blue">{{ JobTypeMap[record.jobType] }}</Tag>
      </template>
      <template #priority="{ record }">
        <Tag :color="JobPriorityColor[record.priority]">{{ JobPriorityMap[record.priority] }}</Tag>
      </template>
      <template #action="{ record }">
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
              ifShow: [JobStatus.Running, JobStatus.FailedRetry].includes(record.status),
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
              ifShow: [JobStatus.Running, JobStatus.Completed, JobStatus.FailedRetry].includes(record.status),
              onClick: handleTrigger.bind(null, record),
            },
            {
              auth: 'TaskManagement.BackgroundJobs.Stop',
              label: L('BackgroundJobs:Stop'),
              ifShow: [JobStatus.Running, JobStatus.FailedRetry].includes(record.status),
              onClick: handleStop.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <BackgroundJobInfoModal @change="handleChange" @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { Switch, Modal, Tag, Tooltip, message } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { getList, deleteById, pause, resume, trigger, stop } from '/@/api/task-management/backgroundJobInfo';
  import { JobStatus } from '/@/api/task-management/model/backgroundJobInfoModel';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { JobStatusMap, JobStatusColor, JobTypeMap, JobPriorityMap, JobPriorityColor } from '../datas/typing';
  import BackgroundJobInfoModal from './BackgroundJobInfoModal.vue';

  const { L } = useLocalization('TaskManagement');
  const { hasPermission } = usePermission();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { reload }] = useTable({
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
    rowSelection: { type: 'radio' },
    formConfig: getSearchFormSchemas(),
    actionColumn: {
      width: 220,
      title: L('Actions'),
      dataIndex: 'action',
      slots: { customRender: 'action' },
    },
  });

  function handleChange() {
    reload();
  }

  function handleAddNew() {
    openModal(true, { id: null });
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handlePause(record) {
    pause(record.id).then(() => {
      message.success(L('Successful'));
      reload();
    });
  }

  function handleResume(record) {
    resume(record.id).then(() => {
      message.success(L('Successful'));
      reload();
    });
  }

  function handleTrigger(record) {
    trigger(record.id).then(() => {
      message.success(L('Successful'));
      reload();
    });
  }

  function handleStop(record) {
    stop(record.id).then(() => {
      message.success(L('Successful'));
      reload();
    });
  }

  function handleDelete(record) {
    Modal.warning({
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        deleteById(record.id).then(() => {
          reload();
        });
      },
    });
  }
</script>
