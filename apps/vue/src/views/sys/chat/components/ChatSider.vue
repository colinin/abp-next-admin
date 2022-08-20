<template>
  <Menu
    style="width: 100%; height: 100%"
    mode="inline"
    :theme="theme"
    v-model:openKeys="openKeys"
    @click="handleClickMenu"
  >
    <MenuItem key="avatar" disabled>
      <template #icon>
        <Avatar :size="26" :src="avatar" style="margin-left: -6px" />
      </template>
    </MenuItem>
    <MenuItem key="notifications">
      <template #icon>
        <NotificationOutlined />
      </template>
    </MenuItem>
    <MenuItem key="chat-message">
      <template #icon>
        <MailOutlined />
      </template>
    </MenuItem>
    <MenuItem key="friend">
      <template #icon>
        <UserSwitchOutlined />
      </template>
    </MenuItem>
    <MenuItem key="group">
      <template #icon>
        <UsergroupAddOutlined />
      </template>
    </MenuItem>
  </Menu>
</template>

<script lang="ts" setup>
  import { computed } from 'vue';
  import { useUserStoreWithOut } from '/@/store/modules/user';
  import { Avatar, Menu, MenuItem } from 'ant-design-vue';
  import {
    MailOutlined,
    UserSwitchOutlined,
    UsergroupAddOutlined,
    NotificationOutlined,
  } from '@ant-design/icons-vue';

  const emits = defineEmits(['switch']);
  const props = defineProps({
    theme: {
      type: String as PropType<'dark' | 'light'>,
      default: 'dark',
    },
    defaultKey: {
      type: String,
      default: 'chat-message',
    },
  });

  const avatar = computed(() => {
    const userStore = useUserStoreWithOut();
    return userStore.getUserInfo.avatar;
  });
  const openKeys = computed(() => {
    return [...props.defaultKey];
  });

  function handleClickMenu({ key }) {
    emits('switch', key);
  }
</script>
