<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('Objects:UploadFile')"
    :width="800"
    :min-height="300"
  >
    <BasicTable @register="registerTable">
      <template #toolbar>
        <input ref="btnRef" style="display: none" />
        <a-button type="primary" @click="handleSelect">{{ L('Upload:SelectFile') }}</a-button>
      </template>
      <template #size="{ record }">
        <span>{{ fileSize(record.size) }}</span>
      </template>
      <template #status="{ record }">
        <Tag v-if="record.completed" color="green">{{ L('Upload:Completed') }}</Tag>
        <Tooltip v-else-if="record.error" :title="record.errorMsg">
          <Tag color="red">{{ L('Upload:Error') }}</Tag>
        </Tooltip>
        <Tag v-else-if="record.paused" color="orange">{{ L('Upload:Pause') }}</Tag>
        <span v-else>{{ record.progress }} {{ averageSpeed(record.averageSpeed) }}</span>
      </template>
      <template #action="{ record }">
        <TableAction
          :stop-button-propagation="true"
          :actions="[
            {
              ifShow: !record.completed,
              color: 'warning',
              label: '',
              icon: record.paused ? 'ant-design:caret-right-outlined' : 'ant-design:pause-outlined',
              onClick: record.paused
                ? handleResume.bind(null, record)
                : handlePause.bind(null, record),
            },
            {
              color: 'error',
              label: '',
              icon: 'ant-design:delete-outlined',
              onClick: handleCancel.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
  </BasicModal>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref, onMounted, onUnmounted, watch } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Tag, Tooltip } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { uploadUrl } from '/@/api/oss-management/oss';
  import { useUserStoreWithOut } from '/@/store/modules/user';
  import Uploader from 'simple-uploader.js';

  export default defineComponent({
    name: 'OssUploadModal',
    components: { BasicModal, BasicTable, TableAction, Tag, Tooltip },
    setup() {
      let uploader: any = null;
      const { L } = useLocalization('AbpOssManagement', 'AbpUi');
      const bucket = ref('');
      const path = ref('');
      const btnRef = ref<any>();
      const [registerModal] = useModalInner((data) => {
        path.value = data.path;
        bucket.value = data.bucket;
      });
      const fileList = ref<Recordable[]>([]);
      const [registerTable] = useTable({
        rowKey: 'id',
        columns: [
          {
            title: 'id',
            dataIndex: 'id',
            width: 1,
            ifShow: false,
          },
          {
            title: L('DisplayName:Name'),
            dataIndex: 'name',
            align: 'left',
            width: 300,
            sorter: true,
          },
          {
            title: L('DisplayName:Size'),
            dataIndex: 'size',
            align: 'left',
            width: 100,
            sorter: true,
            slots: {
              customRender: 'size',
            },
          },
          {
            title: L('DisplayName:Status'),
            dataIndex: 'status',
            align: 'left',
            width: 'auto',
            sorter: true,
            slots: {
              customRender: 'status',
            },
          },
        ],
        dataSource: fileList,
        pagination: false,
        striped: false,
        useSearchForm: false,
        showTableSetting: false,
        bordered: false,
        showIndexColumn: false,
        canResize: false,
        immediate: false,
        actionColumn: {
          width: 120,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      onMounted(() => {
        const userStore = useUserStoreWithOut();
        uploader = new Uploader({
          target: uploadUrl,
          testChunks: false,
          // 加重试机制防止网络波动
          maxChunkRetries: 3,
          chunkRetryInterval: null,
          successStatuses: [200, 201, 202, 204, 205],
          permanentErrors: [400, 401, 403, 404, 415, 500, 501],
          headers: {
            // TODO: 使用缓存存储令牌类型?
            Authorization: `Bearer ${userStore.getToken}`,
          },
          processParams: (params: any) => {
            params.bucket = unref(bucket);
            params.path = unref(path);
            return params;
          },
        });
        // uploader.on('fileSuccess', _fileSuccess);
        uploader.on('filesSubmitted', _filesSubmitted);
        uploader.on('fileError', _fileError);
        uploader.on('fileProgress', _fileProgress);
      });

      onUnmounted(() => {
        // uploader.off('fileSuccess', _fileSuccess);
        uploader.off('filesSubmitted', _filesSubmitted);
        uploader.off('fileError', _fileError);
        uploader.off('fileProgress', _fileProgress);
        uploader = null;
      });

      watch(
        () => unref(btnRef),
        (btn) => {
          uploader.assignBrowse(btn);
        },
      );

      function _filesSubmitted(_, files) {
        files.forEach((f) => {
          f.paused = true;
          f.progress = '0 %';
        });
        fileList.value.push(...files);
      }

      function _fileProgress(_, file) {
        if (file._prevUploadedSize) {
          file.progress = `${Math.floor((file._prevUploadedSize / file.size) * 100)} %`;
        }
      }

      function _fileError(_, file, message) {
        file.paused = true;
        if (message) {
          const formatedError = JSON.parse(message);
          file.errorMsg = formatedError.error?.message;
        }
      }

      function handleSelect() {
        unref(btnRef)?.click();
      }

      function handleResume(record) {
        if (record.error) {
          record.retry();
          record.errorMsg = '';
        }
        record.resume();
      }

      function handlePause(record) {
        record.pause();
      }

      function handleCancel(record) {
        record.cancel();
        uploader.removeFile(record);
        fileList.value = fileList.value.filter((f) => f.id !== record.id);
      }

      const fileSize = computed(() => {
        return (size) => {
          return Uploader.utils.formatSize(size);
        };
      });

      const averageSpeed = computed(() => {
        return (speed) => {
          return `${Uploader.utils.formatSize(speed)} / s`;
        };
      });

      return {
        L,
        btnRef,
        fileSize,
        averageSpeed,
        registerModal,
        registerTable,
        handleSelect,
        handleResume,
        handlePause,
        handleCancel,
      };
    },
  });
</script>
