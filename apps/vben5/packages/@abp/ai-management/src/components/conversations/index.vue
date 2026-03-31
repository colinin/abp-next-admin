<script setup lang="ts">
import type {
  BubbleListProps,
  ConversationsProps,
  MessageInfo,
} from 'ant-design-x-vue';

import type { ConversationDto } from '../../types/conversations';

import { computed, h, onMounted, ref, watch } from 'vue';

import { confirm, useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { preferences } from '@vben/preferences';

import { useAuthorization } from '@abp/core';
import { useMessage } from '@abp/ui';
import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { useDebounceFn, useWindowSize } from '@vueuse/core';
import { Button, Space, theme, Typography } from 'ant-design-vue';
import {
  Bubble,
  Conversations,
  Sender,
  useXAgent,
  useXChat,
  Welcome,
} from 'ant-design-x-vue';
import dayJs from 'dayjs';
import markdownit from 'markdown-it';

import { useChatsApi } from '../../api/useChatsApi';
import { useConversationsApi } from '../../api/useConversationsApi';
import { useWorkspaceDefinitionsApi } from '../../api/useWorkspaceDefinitionsApi';
import {
  ChatPermissions,
  ConversationPermissions,
} from '../../constants/permissions';

type BubbleDataType = NonNullable<BubbleListProps['items']>[number];
type Conversation = NonNullable<ConversationsProps['items']>[number];

defineOptions({ name: 'PlaygroundIndependentSetup' });

const { getPagedListApi: getConversationMessagesApi, sendMessageApi } =
  useChatsApi();
const {
  getApi: getConversationApi,
  createApi: createConversationApi,
  deleteApi: deleteConversationApi,
  getPagedListApi: getConversationsApi,
} = useConversationsApi();
const { getPagedListApi: getWorkspaceDefinitionsApi } =
  useWorkspaceDefinitionsApi();

const messageApi = useMessage();
const { token } = theme.useToken();
const { height } = useWindowSize();
const { isGranted } = useAuthorization();
const md = markdownit({ html: true, breaks: true });

const styles = computed(() => {
  return {
    layout: {
      width: '100%',
      'min-width': '970px',
      height: `${height.value - 120}px`,
      'border-radius': `${token.value.borderRadius}px`,
      display: 'flex',
      background: `${token.value.colorBgContainer}`,
      'font-family': `AlibabaPuHuiTi, ${token.value.fontFamily}, sans-serif`,
    },
    menu: {
      background: `${token.value.colorBgLayout}80`,
      width: '280px',
      height: '100%',
      display: 'flex',
      'flex-direction': 'column',
    },
    conversations: {
      padding: '0 12px',
      flex: 1,
      'overflow-y': 'auto',
    },
    chat: {
      height: '100%',
      width: '100%',
      margin: '0 auto',
      'box-sizing': 'border-box',
      display: 'flex',
      'flex-direction': 'column',
      padding: `${token.value.paddingLG}px`,
      gap: '16px',
    },
    messages: {
      flex: 1,
    },
    placeholder: {
      'padding-top': '32px',
      'text-align': 'left',
      flex: 1,
    },
    sender: {
      'box-shadow': token.value.boxShadow,
    },
    logo: {
      display: 'flex',
      height: '72px',
      'align-items': 'center',
      'justify-content': 'start',
      padding: '0 24px',
      'box-sizing': 'border-box',
    },
    'logo-img': {
      width: '24px',
      height: '24px',
      display: 'inline-block',
    },
    'logo-span': {
      display: 'inline-block',
      margin: '0 8px',
      'font-weight': 'bold',
      color: token.value.colorText,
      'font-size': '16px',
    },
    addBtn: {
      background: '#1677ff0f',
      border: '1px solid #1677ff34',
      width: 'calc(100% - 24px)',
      margin: '0 12px 24px 12px',
    },
  } as const;
});

const bubbleRoles: BubbleListProps['roles'] = {
  ai: {
    placement: 'start',
    typing: { step: 5, interval: 20 },
    styles: {
      content: {
        borderRadius: '16px',
      },
    },
  },
  local: {
    placement: 'end',
    variant: 'shadow',
  },
};

// ==================== State ====================
// const headerOpen = ref(false);
const content = ref('');
const activeConversation = ref<ConversationDto>();
// const attachedFiles = ref<AttachmentsProps['items']>([]);
const agentRequestLoading = ref(false);
const agentRequestDisabled = ref(false);
const conversationRequestLoading = ref(false);
const conversationsMenuConfig: NonNullable<ConversationsProps['menu']> = (
  conversation,
) => ({
  items: [
    {
      danger: true,
      disabled: !isGranted(ConversationPermissions.Delete),
      label: $t('AIManagement.Conversations:Delete'),
      key: 'delete',
      icon: h(DeleteOutlined),
    },
  ],
  onClick: async (menuInfo) => {
    switch (menuInfo.key) {
      case 'delete': {
        confirm({
          content: $t('AIManagement.ConversationsDeleteWarnMessage'),
          title: $t('AbpUi.AreYouSure'),
          beforeClose: async ({ isConfirm }) => {
            if (isConfirm) {
              await deleteConversationApi(conversation.key);
              await onInit();
            }
            return true;
          },
          icon: 'warning',
        });
      }
    }
  },
});
const conversationsItems = ref<Conversation[]>([]);
const conversationsCount = ref(0);
// 定义分组优先级
const priorityMap: Record<string, number> = {
  今天: 1,
  昨天: 2,
  '7天内': 3,
  '30天内': 4,
};

const conversationsGroupable = computed<ConversationsProps['groupable']>(() => {
  return {
    sort: (a, b) => {
      if (a in priorityMap && b in priorityMap) {
        return priorityMap[a]! - priorityMap[b]!;
      }

      if (a in priorityMap && !(b in priorityMap)) {
        return -1;
      }

      if (!(a in priorityMap) && b in priorityMap) {
        return 1;
      }

      if (!(a in priorityMap) && !(b in priorityMap)) {
        const [yearA, monthA] = a.split('-').map(Number);
        const [yearB, monthB] = b.split('-').map(Number);

        if (yearA !== yearB) {
          return yearB! - yearA!;
        }
        return monthB! - monthA!;
      }

      return 0;
    },
    title: (group, { components: { GroupTitle } }) =>
      group
        ? h(GroupTitle, null, () => [
            h(Space, null, () => [h('span', null, group)]),
          ])
        : h(GroupTitle),
  };
});

const searchWorkspaces = useDebounceFn((filter?: string) => {
  getWorkspaceDefinitionsApi({ filter }).then((res) => {
    conversationFormApi.updateSchema([
      {
        fieldName: 'workspace',
        componentProps: {
          options: res.items.map((item) => {
            return {
              label: item.displayName,
              value: item.name,
            };
          }),
        },
      },
    ]);
  });
}, 500);

const [NewConversationForm, conversationFormApi] = useVbenForm({
  commonConfig: {
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  schema: [
    {
      label: $t('AIManagement.DisplayName:Workspace'),
      fieldName: 'workspace',
      component: 'Select',
      rules: 'selectRequired',
      componentProps: {
        allowClear: true,
        filterOption: false,
        onSearch: searchWorkspaces,
        showSearch: true,
      },
    },
  ],
  showDefaultActions: false,
  handleSubmit: async (values) => {
    try {
      conversationModalApi.setState({ submitting: true });
      conversationRequestLoading.value = true;
      const conversation = await createConversationApi({
        name: `${$t('AIManagement.Conversations:New')} ${conversationsCount.value + 1}`,
        workspace: values.workspace,
      });
      await onInit(conversation.id);
      activeConversation.value = conversation;
      conversationModalApi.close();
    } finally {
      conversationRequestLoading.value = false;
      conversationModalApi.setState({ submitting: false });
    }
  },
});

const [NewConversationModal, conversationModalApi] = useVbenModal({
  centered: true,
  closeOnClickModal: false,
  closeOnPressEscape: false,
  fullscreenButton: false,
  title: $t('AIManagement.Conversations:New'),
  onConfirm: async () => {
    await conversationFormApi.validateAndSubmitForm();
  },
  onOpenChange(isOpen) {
    isOpen && searchWorkspaces();
  },
});

// ==================== Runtime ====================
const [agent] = useXAgent<string, { message: string }, string>({
  request: async ({ message }, { onSuccess, onUpdate, onError }) => {
    agentRequestLoading.value = true;
    let receivedContent = '';
    try {
      await sendMessageApi(
        {
          conversationId: activeConversation.value!.id,
          workspace: activeConversation.value!.workspace,
          content: message,
        },
        {
          onMessage: (event) => {
            receivedContent += event.content;
            onUpdate(receivedContent);
          },
          onEnd: () => {
            agentRequestLoading.value = false;
            receivedContent && onSuccess([receivedContent]);
          },
        },
      );
    } catch (error: any) {
      agentRequestLoading.value = false;
      onError(new Error(error.message));
      messageApi.warn(error.message);
    }
  },
});

const { onRequest, messages, setMessages } = useXChat({
  agent: agent!.value,
});

watch(
  activeConversation,
  () => {
    if (activeConversation.value !== undefined) {
      setMessages([]);
    }
  },
  { immediate: true },
);

// ==================== Event ====================
async function onSubmit(nextContent: string) {
  if (!nextContent) return;
  if (!activeConversation.value) {
    await onAddConversation();
    setMessages([
      {
        id: 0,
        message: nextContent,
        status: 'local',
      },
    ]);
  }
  onRequest(nextContent);
  content.value = '';
}

async function onAddConversation() {
  conversationModalApi.open();
}

const onConversationClick: ConversationsProps['onActiveChange'] = async (
  key,
) => {
  content.value = '';
  agentRequestDisabled.value = true;
  const conversation = await getConversationApi(key);
  activeConversation.value = conversation;
  const { items } = await getConversationMessagesApi({
    conversationId: conversation.id,
  });
  const messageInfos: MessageInfo<string>[] = [];
  items.forEach((item) => {
    messageInfos.push({
      id: item.id,
      status: item.role === 'user' ? 'local' : 'success',
      message: item.content,
    });
    if (item.replyMessage) {
      messageInfos.push({
        id: `${item.id}_ai`,
        status: 'success',
        message: item.replyMessage,
      });
    }
  });
  setMessages(messageInfos);
  if (dayJs(conversation.expiredAt).isBefore(dayJs())) {
    content.value = $t('AIManagement.ConversationsExpiredWarnMessage');
  } else {
    agentRequestDisabled.value = false;
  }
};

// 附件变更
// const handleFileChange: AttachmentsProps['onChange'] = (info) =>
//   (attachedFiles.value = info.fileList);

// ==================== Nodes ====================
const placeholderNode = computed(() =>
  h(
    Space,
    { direction: 'vertical', size: 16, style: styles.value.placeholder },
    () => [
      h(Welcome, {
        variant: 'borderless',
        icon: 'https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*s5sNRo5LjfQAAAAAAAAAAAAADgCCAQ/fmt.webp',
        title: $t('AIManagement.ChatWelcomeTitle'),
        description: $t('AIManagement.ChatWelcomeMessage'),
      }),
    ],
  ),
);

const bubbleItems = computed<BubbleDataType[]>(() => {
  if (messages.value.length === 0) {
    return [{ content: placeholderNode, variant: 'borderless' }];
  }
  const items: BubbleDataType[] = [];
  messages.value.forEach((message) => {
    const item: BubbleDataType = {
      key: message.id,
      // loading: status === 'loading',
      loading: false,
      role: message.status === 'local' ? 'local' : 'ai',
      content: message.message,
      typing: false,
      messageRender: (content: any) => {
        if (message.status === 'local') {
          return content;
        }
        return h(Typography, null, {
          default: () => h('div', { innerHTML: md.render(content) }),
        });
      },
    };
    if (message.status === 'error') {
      item.placement = 'end';
    }
    items.push(item);
  });
  return items;
});

const onInit = async (activeConversationId?: string) => {
  const { items, totalCount } = await getConversationsApi({
    maxResultCount: 25,
  });
  const nowTime = dayJs();
  conversationsItems.value = items.map((item) => {
    const targetDate = dayJs(item.createdAt);
    const conversation: Conversation = {
      label: item.name,
      key: item.id,
    };
    if (targetDate.format('YYYY-MM-DD') === nowTime.format('YYYY-MM-DD')) {
      conversation.group = '今天';
    } else if (
      targetDate.format('YYYY-MM-DD') ===
      nowTime.subtract(1, 'day').format('YYYY-MM-DD')
    ) {
      conversation.group = '昨天';
    } else {
      const diffDays = nowTime
        .startOf('day')
        .diff(targetDate.startOf('day'), 'day');
      if (diffDays <= 7) {
        conversation.group = '7天内';
      } else if (diffDays <= 30) {
        conversation.group = '30天内';
      } else {
        conversation.group = targetDate.format('YYYY-MM');
      }
    }
    return conversation;
  });
  conversationsCount.value = totalCount;
  activeConversationId && onConversationClick(activeConversationId);
};

onMounted(onInit);
</script>

<template>
  <div :style="styles.layout">
    <div :style="styles.menu">
      <!-- 🌟 Logo -->
      <div :style="styles.logo">
        <img
          :src="preferences.logo.source"
          draggable="false"
          alt="logo"
          :style="styles['logo-img']"
        />
        <span :style="styles['logo-span']">{{ preferences.app.name }}</span>
      </div>

      <!-- 🌟 添加会话 -->
      <Button
        v-access:code="[ConversationPermissions.Create]"
        type="link"
        :style="styles.addBtn"
        @click="onAddConversation"
        :loading="conversationRequestLoading"
      >
        <PlusOutlined />
        {{ $t('AIManagement.Conversations:New') }}
      </Button>

      <!-- 🌟 会话管理 -->
      <Conversations
        :menu="conversationsMenuConfig"
        :groupable="conversationsGroupable"
        :items="conversationsItems"
        :style="styles.conversations"
        :active-key="activeConversation?.id"
        @active-change="onConversationClick"
      />
    </div>

    <div :style="styles.chat">
      <!-- 🌟 消息列表 -->
      <Bubble.List
        :items="bubbleItems"
        :roles="bubbleRoles"
        :style="styles.messages"
      />

      <!-- 🌟 输入框 -->
      <Sender
        :value="content"
        :style="styles.sender"
        :disabled="
          isGranted(ChatPermissions.SendMessage) && agentRequestDisabled
        "
        :loading="agentRequestLoading"
        :auto-size="{ minRows: 3 }"
        :placeholder="$t('AIManagement.ChatSendMessage')"
        @submit="onSubmit"
        @change="(value) => (content = value)"
      >
        <!-- <template #prefix>
          <Badge :dot="attachedFiles.length > 0 && !headerOpen">
            <Button type="text" @click="() => (headerOpen = !headerOpen)">
              <template #icon>
                <PaperClipOutlined />
              </template>
            </Button>
          </Badge>
        </template> -->

        <!-- <template #header>
          <Sender.Header
            title="Attachments"
            :open="headerOpen"
            :styles="{ content: { padding: 0 } }"
            @open-change="(open) => (headerOpen = open)"
          >
            <Attachments
              :before-upload="() => false"
              :items="attachedFiles"
              @change="handleFileChange"
            >
              <template #placeholder="type">
                <Flex
                  v-if="type && type.type === 'inline'"
                  align="center"
                  justify="center"
                  vertical
                  gap="2"
                >
                  <Typography.Text style="font-size: 30px; line-height: 1">
                    <CloudUploadOutlined />
                  </Typography.Text>
                  <Typography.Title
                    :level="5"
                    style="margin: 0; font-size: 14px; line-height: 1.5"
                  >
                    Upload files
                  </Typography.Title>
                  <Typography.Text type="secondary">
                    Click or drag files to this area to upload
                  </Typography.Text>
                </Flex>
                <Typography.Text v-if="type && type.type === 'drop'">
                  Drop file here
                </Typography.Text>
              </template>
            </Attachments>
          </Sender.Header>
        </template> -->
      </Sender>
    </div>
  </div>
  <NewConversationModal>
    <NewConversationForm />
  </NewConversationModal>
</template>

<style scoped lang="scss">
:deep(.ant-typography) {
  p {
    margin-bottom: 0 !important;
  }
}
</style>
