<template>
  <div :class="getClass">
    <div class="im-chat-left">
      <PageWrapper dense>
        <template #title>
          <div class="title">
            <BasicTitle>
              <template #default>
                <div class="search">
                  <Input :placeholder="t('AbpIdentity.Search')">
                    <template #prefix>
                      <SearchOutlined />
                    </template>
                  </Input>
                </div>
                <div class="button">
                  <Button @click="handleAddFriend">
                    <template #icon>
                      <PlusOutlined />
                    </template>
                  </Button>
                </div>
              </template>
            </BasicTitle>
          </div>
        </template>
        <template #default>
          <div class="subject">
            <template v-for="(chat, index) in chatItems" :key="index">
              <div :class="getChatClass(chat)" :tabindex="index" @click="handleClickChat(chat)">
                <div class="avatar">
                  <Avatar v-if="chat.avatar" :src="chat.avatar" />
                  <Avatar v-else :src="undefinedAvatar" />
                </div>
                <div class="main">
                  <div class="title">
                    <span>{{ chat.name }}</span>
                    <span class="time">{{ formatToDateTime(chat.sendTime, 'HH:mm') }}</span>
                  </div>
                  <div class="content">
                    <span v-if="chat.groupId" class="sort-content">{{
                      `${chat.formUserName}: ${chat.content}`
                    }}</span>
                    <span v-else class="sort-content">{{ chat.content }}</span>
                  </div>
                </div>
              </div>
            </template>
          </div>
        </template>
      </PageWrapper>
    </div>
    <div class="im-chat-right">
      <PageWrapper v-if="swicthToChat !== undefined" :title="swicthToChat.name" dense fixed-height>
        <template #default>
          <ChatMessagePanel
            v-if="swicthToChat.groupId !== ''"
            :event-name="ChatEventEnum.USER_MESSAGE_GROUP_NEW"
            :current-chat="swicthToChat"
            :fetchApi="getGroupMessages"
            :fetch-params="{
              groupId: swicthToChat.groupId,
            }"
            @send="handleSendMessage"
          />
          <ChatMessagePanel
            v-else
            :event-name="ChatEventEnum.USER_MESSAGE_NEW"
            :current-chat="swicthToChat"
            :fetchApi="getChatMessages"
            :fetch-params="{
              receiveUserId: swicthToChat.formUserId,
            }"
            @send="handleSendMessage"
          />
        </template>
      </PageWrapper>
    </div>
    <ChatSearchModal @register="registerModal" />
  </div>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref, onMounted, onUnmounted } from 'vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { Avatar, Button, Input } from 'ant-design-vue';
  import { PlusOutlined, SearchOutlined } from '@ant-design/icons-vue';
  import { useModal } from '/@/components/Modal';
  import { PageWrapper } from '/@/components/Page';
  import { BasicTitle } from '/@/components/Basic/index';
  import { ChatMessage, MessageType } from '/@/api/messages/model/messagesModel';
  import { getChatMessages, getGroupMessages, getLastMessages } from '/@/api/messages/messages';
  import ChatSearchModal from './ChatSearchModal.vue';
  import ChatMessagePanel from './ChatMessagePanel.vue';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { ChatEventEnum } from '/@/enums/imEnum';
  import emitter from '/@/utils/eventBus';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useRootSetting } from '/@/hooks/setting/useRootSetting';
  import { isNullOrWhiteSpace } from '/@/utils/strings';
  import undefinedAvatar from '/@/assets/icons/64x64/color-user.png';

  interface Chat {
    id: string;
    name: string;
    formUserId: string;
    formUserName: string;
    groupId?: string;
    avatar: string;
    content: string;
    sendTime: Date;
  }

  export default defineComponent({
    name: 'ChatMessage',
    components: {
      Avatar,
      BasicTitle,
      Button,
      ChatSearchModal,
      ChatMessagePanel,
      Input,
      PageWrapper,
      PlusOutlined,
      SearchOutlined,
    },
    setup() {
      const { t } = useI18n();
      const chatItems = ref<Chat[]>([]);
      const swicthToChat = ref<Chat>();
      const selectedChats = ref<string[]>();
      const [registerModal, { openModal }] = useModal();

      const { prefixCls } = useDesign('im-chat-container');
      const { getDarkMode } = useRootSetting();
      const getClass = computed(() => {
        return [prefixCls, `${prefixCls}--${unref(getDarkMode)}`];
      });
      const getChatClass = computed(() => {
        return (chat: Chat) => {
          return selectedChats.value?.includes(chat.id) ? 'info selected' : 'info';
        };
      });

      onMounted(() => {
        emitter.on(ChatEventEnum.USER_MESSAGE_NEW, _newMessageReceived);
        emitter.on(ChatEventEnum.USER_MESSAGE_GROUP_NEW, _newMessageReceived);
        emitter.on(ChatEventEnum.USER_MESSAGE_RECALL, _newMessageReceived);
        getLastMessages({
          sorting: '',
          maxResultCount: 25,
        }).then((res) => {
          chatItems.value = res.items.map((x) => {
            return {
              id: x.messageId,
              name: x.object,
              avatar: x.avatar,
              content: x.content,
              groupId: x.groupId,
              sendTime: x.sendTime,
              formUserId: x.formUserId,
              formUserName: x.formUserName,
            };
          });
        });
      });

      onUnmounted(() => {
        emitter.off(ChatEventEnum.USER_MESSAGE_NEW, _newMessageReceived);
        emitter.off(ChatEventEnum.USER_MESSAGE_GROUP_NEW, _newMessageReceived);
        emitter.off(ChatEventEnum.USER_MESSAGE_RECALL, _newMessageReceived);
      });

      function handleClickChat(chat: Chat) {
        swicthToChat.value = chat;
        selectedChats.value = [chat.id];
      }

      function handleAddFriend() {
        openModal(true);
      }

      function handleSendMessage(content: string) {
        const data = {
          content: content,
          type: MessageType.Text,
        };
        if (swicthToChat.value?.groupId) {
          data['groupId'] = swicthToChat.value.groupId;
        } else {
          data['receivedUserId'] = swicthToChat.value?.formUserId;
        }
        emitter.emit(ChatEventEnum.USER_SEND_MESSAGE, data);
      }

      function _newMessageReceived(message: ChatMessage) {
        const msgIndex = isNullOrWhiteSpace(message.groupId)
          ? chatItems.value.findIndex(
              (m) => m.formUserId === message.formUserId || m.formUserId === message.toUserId,
            )
          : chatItems.value.findIndex((m) => m.groupId === message.groupId);
        if (msgIndex >= 0) {
          chatItems.value[msgIndex].content = message.content;
          chatItems.value[msgIndex].formUserName = message.formUserName;
        }
      }

      return {
        t,
        getClass,
        getChatClass,
        chatItems,
        swicthToChat,
        registerModal,
        handleClickChat,
        handleAddFriend,
        formatToDateTime,
        ChatEventEnum,
        getChatMessages,
        getGroupMessages,
        handleSendMessage,
        undefinedAvatar,
      };
    },
  });
</script>

<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-im-chat-container';
  .@{prefix-cls} {
    display: flex;
    height: 100%;

    &--dark {
      .im-chat-left .subject .info {
        &:hover {
          background-color: @trigger-dark-hover-bg-color !important;
        }

        &:focus {
          background-color: @sider-dark-bg-color !important;
        }

        &.selected {
          background: @sider-dark-bg-color !important;
        }
      }
    }

    .im-chat-left {
      display: flex;
      flex-direction: column;
      width: '266px';

      .title {
        .search {
          width: 'auto';
        }

        .button {
          width: '55px';
          margin-left: 5px;
        }
      }

      .subject {
        width: 100%;
        height: 100%;
        margin-top: 5px;
        cursor: pointer;

        .info:hover {
          background: rgb(226 220 220);
        }

        .info:focus {
          background: rgb(182 178 178);
        }

        .info {
          display: flex;

          &.selected {
            background: rgb(167 159 159);
          }

          .avatar {
            display: box;
            width: 60px;
            height: 60px;
            -webkit-box-orient: horizontal;
            -webkit-box-pack: center;
            -webkit-box-align: center;
          }

          .main {
            display: flex;
            flex-direction: column;
            justify-content: center;

            .title {
              display: flex;
              font-size: 12pt;
              font-weight: 500;

              .time {
                margin-left: auto;
                margin-right: 5px;
                margin-top: 3px;
                font-size: 10pt;
                color: rgb(136 132 132);
              }
            }

            .content {
              font-size: 10pt;
              color: rgb(128 125 125);
            }
          }
        }

        .sort-content {
          width: 200px;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
          display: inline-block;
        }
      }
    }

    .im-chat-right {
      width: 100%;
      background-color: rgb(230 231 223);

      .vben-page-wrapper {
        .overflow-hidden &.vben-page-wrapper-content {
          margin: 0;
        }
      }
    }
  }
</style>
