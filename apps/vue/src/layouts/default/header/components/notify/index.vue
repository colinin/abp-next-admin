<template>
  <div :class="prefixCls">
    <Badge :count="count" dot :numberStyle="numberStyle">
      <BellOutlined @click="showDrawer" />
    </Badge>
    <Drawer
      v-model:visible="open"
      title=""
      :class="`${prefixCls}__overlay`"
      placement="right"
      :closable="false"
    >
      <Tabs>
        <TabPane :key="notifierRef.key" :tab="notifierRef.name">
          <NoticeList
            :list="notifierRef.list"
            @title-click="readNotifer"
            @content-click="handleShowNotifications"
          />
        </TabPane>
        <TabPane :key="messageRef.key" :tab="messageRef.name">
          <NoticeList :list="messageRef.list">
            <template #footer>
              <ButtonGroup style="width: 100%">
                <Button
                  :disabled="messageRef.list.length === 0"
                  style="width: 50%"
                  type="link"
                  @click="clearMessage"
                  >清空消息</Button
                >
                <Button style="width: 50%" type="link" @click="handleShowMessages">查看更多</Button>
              </ButtonGroup>
            </template>
          </NoticeList>
        </TabPane>
        <TabPane :key="tasksRef.key" :tab="tasksRef.name">
          <NoticeList :list="tasksRef.list" />
        </TabPane>
      </Tabs>
    </Drawer>
    <!-- <Popover title="" trigger="click" :overlayClassName="`${prefixCls}__overlay`">
      <Badge :count="count" dot :numberStyle="numberStyle">
        <BellOutlined />
      </Badge>
      <template #content>
        <Tabs>
          <TabPane :key="notifierRef.key" :tab="notifierRef.name">
            <NoticeList :list="notifierRef.list" @title-click="readNotifer" />
          </TabPane>
          <TabPane :key="messageRef.key" :tab="messageRef.name">
            <NoticeList :list="messageRef.list">
              <template #footer>
                <ButtonGroup style="width: 100%">
                  <Button
                    :disabled="messageRef.list.length === 0"
                    style="width: 50%"
                    type="link"
                    @click="clearMessage"
                    >清空消息</Button
                  >
                  <Button style="width: 50%" type="link" @click="handleShowMessages"
                    >查看更多</Button
                  >
                </ButtonGroup>
              </template>
            </NoticeList>
          </TabPane>
          <TabPane :key="tasksRef.key" :tab="tasksRef.name">
            <NoticeList :list="tasksRef.list" />
          </TabPane>
        </Tabs>
      </template>
    </Popover> -->
  </div>
</template>
<script lang="ts">
  import { computed, defineComponent, ref } from 'vue';
  import { Button, Drawer, Popover, Tabs, Badge } from 'ant-design-vue';
  import { BellOutlined } from '@ant-design/icons-vue';
  import NoticeList from './NoticeList.vue';
  import { useGo } from '/@/hooks/web/usePage';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useTasks } from './useTasks';
  import { useMessages } from './useMessages';
  import { useNotifications } from './useNotifications';

  export default defineComponent({
    components: {
      Button,
      ButtonGroup: Button.Group,
      Drawer,
      Popover,
      BellOutlined,
      Tabs,
      TabPane: Tabs.TabPane,
      Badge,
      NoticeList,
    },
    setup() {
      const { prefixCls } = useDesign('header-notify');
      const go = useGo();
      const open = ref(false);
      const { tasksRef } = useTasks();
      const { messageRef, clearMessage } = useMessages();
      const { notifierRef, readNotifer } = useNotifications();
      // const listData = ref(tabListData);

      const count = computed(() => {
        let count = 0;
        count += notifierRef.value.list.length;
        count += messageRef.value.list.length;
        count += tasksRef.value.list.length;
        return count;
      });

      function handleShowMessages() {
        console.log('handleShowMessages');
        go('/sys/chat?type=chat-message');
      }

      function handleShowNotifications() {
        console.log('handleShowNotifications');
        open.value = false;
        go('/messages/notifications');
      }

      function showDrawer() {
        console.log('showDrawer');
        open.value = true;
      }

      return {
        prefixCls,
        count,
        numberStyle: {},
        open,
        showDrawer,
        notifierRef,
        readNotifer,
        messageRef,
        clearMessage,
        tasksRef,
        handleShowMessages,
        handleShowNotifications,
      };
    },
  });
</script>
<style lang="less">
  @prefix-cls: ~'@{namespace}-header-notify';

  .@{prefix-cls} {
    padding-top: 2px;

    .ant-badge {
      font-size: 18px;

      .ant-badge-multiple-words {
        padding: 0 4px;
      }

      svg {
        width: 0.9em;
      }
    }
  }
</style>
