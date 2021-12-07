<template>
  <div :class="getClass">
    <div ref="chatBoxRef" class="chat-box">
      <div
        ref="chatMsgRef"
        class="msg-box"
        :style="chatMsgStyle"
        v-loading="loadingRef"
        @scroll="handleScroll"
      >
        <div v-for="message in messages">
          <div v-if="message.source === MessageSourceTye.System" class="msg-item sys">
            <span class="content">{{ getContent(message) }}</span>
          </div>
          <div v-else-if="message.formUserId !== currentUser.userId" class="msg-item">
            <div class="avatar">
              <Avatar class="icon" :src="currentChat.avatar ?? undefinedAvatar" />
            </div>
            <div class="main">
              <div class="title">
                <span>{{ message.formUserName }}</span>
                <span class="time">{{ formatToDateTime(message.sendTime) }}</span>
              </div>
              <span class="content left">{{ message.content }}</span>
            </div>
          </div>
          <div v-else class="msg-item">
            <div class="main right">
              <div class="title right">
                <span class="time">{{ formatToDateTime(message.sendTime) }}</span>
                <span>{{ message.formUserName }}</span>
              </div>
              <span class="content right" @contextmenu="(e) => handleContext(e, message)">{{
                message.content
              }}</span>
            </div>
            <div class="avatar">
              <Avatar class="icon" :src="currentUser.avatar ?? undefinedAvatar" />
            </div>
          </div>
        </div>
      </div>
      <div ref="chatSendRef" class="send-box">
        <div class="toobar">
          <Icon class="icon" icon="bi:emoji-smile" />
          <Icon class="icon" icon="bi:folder" />
          <Icon class="icon" icon="bi:scissors" />
          <Icon class="icon" icon="bi:chat-dots" />
        </div>
        <div class="text">
          <TextArea v-model:value="sendMsgRef" :auto-size="{ minRows: 6, maxRows: 8 }" />
        </div>
        <div class="button">
          <Button :disabled="sendMsgRef === ''" style="width: 100px" @click="handleSendMessage"
            >发送</Button
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref, nextTick, onMounted, onUnmounted } from 'vue';
  import { Avatar, Button, Input } from 'ant-design-vue';
  import { Icon } from '/@/components/Icon';
  import { Loading, useLoading } from '/@/components/Loading';
  import { SizeEnum } from '/@/enums/sizeEnum';
  import emitter from '/@/utils/eventBus';
  import { ChatEventEnum } from '/@/enums/imEnum';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { useContentHeight } from '/@/hooks/web/useContentHeight';
  import { useContextMenu } from '/@/hooks/web/useContextMenu';
  import { useExtraPropTranslation } from '/@/hooks/web/useExtraPropTranslation';

  import { useUserStoreWithOut } from '/@/store/modules/user';
  import undefinedAvatar from '/@/assets/icons/64x64/color-user.png';

  import { useDesign } from '/@/hooks/web/useDesign';
  import { useRootSetting } from '/@/hooks/setting/useRootSetting';

  import {
    ChatMessage,
    ChatMessagePagedResult,
    MessageSourceTye,
  } from '/@/api/messages/model/messagesModel';

  export default defineComponent({
    name: 'ChatMessagePanel',
    props: {
      /** 新消息事件名称 */
      eventName: {
        type: String,
        required: false,
      },
      currentChat: {
        type: Object as PropType<Recordable>,
        required: true,
      },
      fetchApi: {
        type: Function as PropType<(input) => Promise<ChatMessagePagedResult>>,
        required: true,
      },
      fetchParams: {
        type: Object as PropType<Recordable>,
        required: true,
        default: {},
      },
      contentFullHeight: {
        type: Boolean,
        default: true,
      },
    },
    components: {
      Avatar,
      Button,
      Icon,
      Loading,
      TextArea: Input.TextArea,
    },
    emits: ['send'],
    setup(props, { emit }) {
      let fetchFlag = true;
      const requestRef = ref<any>({
        skipCount: 1,
        maxResultCount: 25,
        sorting: 'messageId desc',
      });
      const { tryLocalize } = useExtraPropTranslation();
      const sendMsgRef = ref('');
      const loadingRef = ref<any>(null);
      const chatBoxRef = ref<any>(null);
      const chatMsgRef = ref<any>(null);
      const chatSendRef = ref<any>(null);
      const messages = ref<ChatMessage[]>([]);
      const currentUser = computed(() => {
        const userStore = useUserStoreWithOut();
        return userStore.getUserInfo;
      });
      const getIsContentFullHeight = computed(() => {
        return props.contentFullHeight;
      });

      const [openWrapLoading, closeWrapLoading] = useLoading({
        target: chatMsgRef,
        props: {
          size: SizeEnum.SMALL,
          tip: '加载中...',
          absolute: true,
        },
      });

      const { prefixCls } = useDesign('im-chat-panel');
      const { getDarkMode } = useRootSetting();
      const getClass = computed(() => {
        return [prefixCls, `${prefixCls}--${unref(getDarkMode)}`];
      });

      const { contentHeight } = useContentHeight(
        getIsContentFullHeight,
        chatBoxRef,
        [chatSendRef],
        [chatMsgRef],
      );

      const chatMsgStyle = computed(() => {
        const height = `${unref(contentHeight)}px`;
        return {
          minHeight: height,
          height: height,
        };
      });

      const getContent = computed(() => {
        return (message: ChatMessage) => {
          // return message.content
          return tryLocalize('content', message, message.content);
        };
      });

      onMounted(() => {
        props.eventName && emitter.on(props.eventName, _newMessageReceived);
        emitter.on(ChatEventEnum.USER_MESSAGE_RECALL, _recallMessage);
        _fetchChatMessage();
      });

      onUnmounted(() => {
        props.eventName && emitter.off(props.eventName, _newMessageReceived);
        emitter.off(ChatEventEnum.USER_MESSAGE_RECALL, _recallMessage);
      });

      function handleScroll(e) {
        if (e.target.scrollHeight > e.target.clientHeight && e.target.scrollTop === 0) {
          _fetchChatMessage();
          if (e.target.scrollTop === 0) {
            // 预留空间保持刷新
            nextTick(() => {
              const chatMsgEl = unref(chatMsgRef);
              chatMsgEl.scrollTop = 10;
            });
          }
        }
      }

      function handleSendMessage() {
        emit('send', sendMsgRef.value);
        sendMsgRef.value = '';
      }

      const [createContextMenu] = useContextMenu();
      function handleContext(e: MouseEvent, message) {
        createContextMenu({
          event: e,
          items: [
            {
              label: '撤回',
              handler: () => {
                emitter.emit(ChatEventEnum.USER_MESSAGE_RECALL, message);
              },
            },
          ],
        });
      }

      function _fetchChatMessage() {
        if (!fetchFlag) {
          return;
        }
        fetchFlag = false;
        openWrapLoading();
        const request = {
          ...requestRef.value,
          ...props.fetchParams,
        };
        formatPagedRequest(request);
        props
          .fetchApi(request)
          .then((res) => {
            messages.value.unshift(
              ...res.items.sort((a, b) => {
                return a.messageId < b.messageId ? -1 : 0;
              }),
            );
            fetchFlag = res.items.length === requestRef.value.maxResultCount;
            requestRef.value.skipCount === 1 && _scrollBottom();
            requestRef.value.skipCount += 1;
          })
          .catch(() => {
            fetchFlag = true;
          })
          .finally(() => {
            closeWrapLoading();
          });
      }

      function _recallMessage(chatMessage: ChatMessage) {
        console.log('recall', chatMessage);
        if (chatMessage.source === MessageSourceTye.System) {
          const index = messages.value.findIndex(
            (m) => m.messageId === chatMessage.extraProperties.MessageId,
          );
          if (index >= 0) {
            messages.value.splice(index, 1);
          }
          messages.value.push(chatMessage);
        }
      }

      function _newMessageReceived(chatMessage: ChatMessage) {
        messages.value.push(chatMessage);
        _scrollBottom();
        if (chatMessage.messageId) {
          emitter.emit(ChatEventEnum.USER_MESSAGE_READ, chatMessage);
        }
      }

      function _scrollBottom() {
        nextTick(() => {
          const chatMsgEl = unref(chatMsgRef);
          chatMsgEl.scrollTop = chatMsgEl.scrollHeight;
        });
      }

      return {
        getClass,
        messages,
        chatBoxRef,
        sendMsgRef,
        chatMsgRef,
        loadingRef,
        currentUser,
        chatSendRef,
        chatMsgStyle,
        getContent,
        undefinedAvatar,
        formatToDateTime,
        handleContext,
        handleScroll,
        handleSendMessage,
        MessageSourceTye,
      };
    },
  });
</script>

<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-im-chat-panel';

  .@{prefix-cls} {
    display: flex;
    flex-direction: column;

    &--dark {
      background: rgb(22 22 21);
      color: rgb(255 255 255);

      .chat-box .send-box {
        background: rgb(10 8 8) !important;
      }

      .chat-box .msg-box .msg-item .main .content {
        color: black;
      }
    }

    .chat-box {
      flex-direction: column;

      .msg-box {
        overflow-y: scroll;

        .msg-item {
          display: flex;
          flex-direction: row;
          margin: 18px 5px;

          &.sys {
            height: 33px;
            justify-content: center;

            .content {
              cursor: pointer;
              color: rgb(63 88 139);
              font-size: 8pt;
            }
          }

          .avatar {
            width: 60px;
            height: 60px;
            justify-content: center;
            align-items: center;
            display: flex;

            .icon {
              width: 50px;
              height: 50px;
            }
          }

          .main {
            display: flex;
            flex-direction: column;

            &.right {
              margin-left: auto;
            }

            .title {
              display: flex;
              font-size: 12pt;
              color: rgb(128 125 125);
              align-items: center;
              margin: 0 5px;
              padding: 0 5px;

              &.right {
                margin-left: auto;
              }

              .time {
                font-size: 10pt;
                color: rgb(136 132 132);
                margin: 0 10px;
              }
            }

            .content {
              width: auto !important;
              float: left;
              max-width: 400px;
              font-size: 12pt;
              margin: 5px;
              margin-right: auto;
              padding: 0 5px;
              background: rgb(255 255 255);
              border-radius: 5px;
              position: relative;

              &.left::after {
                content: ' ';
                position: absolute;
                width: 0;
                height: 0;
                top: 7px;
                left: -6px;
                border-top: 5px solid transparent;
                border-bottom: 5px solid transparent;
                border-right: 6px solid rgb(255 255 255);
              }

              &.right {
                float: right;
                margin-left: auto;
                margin-right: 5px;
                background: rgb(118 216 118);
              }

              &.right::after {
                content: ' ';
                position: absolute;
                width: 0;
                height: 0;
                top: 7px;
                right: -5px;
                border-top: 5px solid transparent;
                border-bottom: 5px solid transparent;
                border-left: 5px solid rgb(118 216 118);
              }
            }
          }
        }
      }

      .send-box {
        width: 100%;
        height: 240px;
        background: rgb(255 255 255);

        .toobar {
          flex: 1;
          height: 35px;
          justify-content: center;

          .icon {
            margin: 10px;

            :active {
              color: seagreen;
            }
          }
        }

        .text {
          flex: 2;
          margin: 10px;
        }

        .button {
          height: 40px;
          text-align: right;
          margin: 10px 10px 5px 0;
        }
      }
    }
  }
</style>
