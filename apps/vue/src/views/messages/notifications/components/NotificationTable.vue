<template>
  <BasicTable
    @register="registerTable"
    v-model:expandedRowKeys="expandedRowKeys"
    :showExpandColumn="false"
    :rowSelection="{
      type: 'checkbox',
      selectedRowKeys: selectedRowKeys,
      onChange: handleSelectRowChanged,
    }"
    @fetch-success="handleChanged"
  >
    <template #toolbar>
      <Dropdown v-if="selectedRowKeys.length > 0">
        <template #overlay>
          <Menu @click="handleMarkMultipleState">
            <MenuItem key="read">
              <Icon icon="ic:outline-mark-email-read" />
              {{ L('Read') }}
            </MenuItem>
            <MenuItem key="un-read">
              <Icon icon="ic:outline-mark-email-unread" />
              {{ L('UnRead') }}
            </MenuItem>
          </Menu>
        </template>
        <Button>
          <Icon icon="material-symbols:bookmark-outline" />
          {{ L('MarkAs') }}
          <DownOutlined />
        </Button>
      </Dropdown>
    </template>
    <template #expandedRowRender="{ record }">
      <MarkdownViewer :value="getContent(record)" />
    </template>
    <template #bodyCell="{ column, record }">
      <template v-if="column.key === 'title'">
        <Icon
          v-if="record.state === NotificationReadState.Read"
          class="title-icon"
          icon="ic:outline-mark-email-read"
          :size="20"
          color="#00DD00"
        />
        <Icon v-else class="title-icon" icon="ic:outline-mark-email-unread" :size="20" color="#FF7744" />
        <a href="javascript:(0);" @click="handleExpanded(record)">{{ getTitle(record) }}</a>
      </template>
      <template v-else-if="column.key === 'message'">
        <a href="javascript:(0);" @click="handleExpanded(record)">{{ getContent(record) }}</a>
      </template>
      <template v-else-if="column.key === 'type'">
        <span>{{ getType(record) }}</span>
      </template>
      <template v-else-if="column.key === 'action'">
        <TableAction
          :stop-button-propagation="true"
          :actions="[
            {
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
          :drop-down-actions="[
            {
              label: L('MarkAs'),
            }
          ]"
        >
          <template #more>
            <Dropdown>
              <template #overlay>
                <Menu @click="(e) => handleMarkSingleState(e, record)">
                  <MenuItem key="read">
                    {{ L('Read') }}
                  </MenuItem>
                  <MenuItem key="un-read">
                    {{ L('UnRead') }}
                  </MenuItem>
                </Menu>
              </template>
              <Button type="link" size="small">
                {{ L('MarkAs') }}
                <DownOutlined />
              </Button>
            </Dropdown>
          </template>
        </TableAction>
      </template>
    </template>
  </BasicTable>
</template>

<script lang="ts" setup>
  import type { MenuInfo } from 'ant-design-vue/lib/menu/src/interface';
  import { computed, reactive, ref } from 'vue';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { Button, Dropdown, Menu, MenuItem } from 'ant-design-vue';
  import { Icon } from '/@/components/Icon';
  import { MarkdownViewer } from '/@/components/Markdown';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { markReadState, deleteById, getList } from '/@/api/messages/notifications';
  import { NotificationType, NotificationReadState, NotificationInfo } from '/@/api/messages/model/notificationsModel';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  // 由于通知可能需要前端本地化的特性
  // 此模块需要所有的本地化资源
  const { L } = useLocalization();

  const [registerTable, { reload, clearSelectedRowKeys }] = useTable({
    rowKey: 'id',
    title: L('Notifications'),
    columns: getDataColumns(),
    api: getList,
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: true,
    canResize: true,
    immediate: true,
    formConfig: getSearchFormSchemas(),
    actionColumn: {
      width: 150,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const { createConfirm, createMessage } = useMessage();
  const expandedRowKeys = ref<string[]>([]);
  const selectedRowKeys = ref<string[]>([]);
  const typeTitleMap = reactive({
    [NotificationType.System]: L('Notifications:System'),
    [NotificationType.Application]: L('Notifications:Application'),
    [NotificationType.User]: L('Notifications:User'),
  });
  const getTitle = computed(() => {
    return (notifier) => {
      if (notifier.data?.extraProperties?.L === true) {
        return L(notifier.data.extraProperties.title.Name, notifier.data.extraProperties.title.Values);
      }
      return notifier.data.extraProperties.title;
    };
  });
  const getContent = computed(() => {
    return (notifier) => {
      if (notifier.data?.extraProperties?.L === true) {
        return L(notifier.data.extraProperties.message.Name, notifier.data.extraProperties.message.Values);
      }
      return notifier.data.extraProperties.message;
    };
  });
  const getType = computed(() => {
    return (notifier) => {
      return typeTitleMap[notifier.type];
    };
  });

  function handleDelete(record: NotificationInfo) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }

  function handleMarkMultipleState(e: MenuInfo) {
    let state: NotificationReadState;
    switch (e.key) {
      case 'un-read':
        state = NotificationReadState.UnRead;
        break;
      case 'read':
      default:
        state = NotificationReadState.Read;
        break;
    }
    _markReadState(selectedRowKeys.value, state);
  }

  function handleMarkSingleState(e: MenuInfo, record: NotificationInfo) {
    let state: NotificationReadState;
    switch (e.key) {
      case 'un-read':
        state = NotificationReadState.UnRead;
        break;
      case 'read':
      default:
        state = NotificationReadState.Read;
        break;
    }
    _markReadState([record.id], state);
  }

  function _markReadState(ids: string[], state: NotificationReadState) {
    markReadState(ids, state).then(() => {
      createMessage.success(L('Successful'));
      reload();
    });
  }

  function handleSelectRowChanged(rowKeys) {
    selectedRowKeys.value = rowKeys;
  }

  function handleExpanded(record) {
    const index = expandedRowKeys.value.findIndex(key => key === record.id);
    if (index >= 0) {
      expandedRowKeys.value.splice(index, 1);
    } else {
      expandedRowKeys.value.push(record.id);
      if (record.state == NotificationReadState.UnRead) {
        record.state = NotificationReadState.Read;
        markReadState([record.id], record.state);
      }
    }
  }

  function handleChanged() {
    clearSelectedRowKeys();
  }
</script>

<style lang="less" scoped>
  .title-icon {
    margin-right: 5px;
  }
</style>
