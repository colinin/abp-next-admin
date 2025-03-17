<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import { useMySubscribesApi, useNotificationsApi } from '@abp/notifications';
import {
  Card,
  Collapse,
  CollapsePanel,
  List,
  ListItem,
  ListItemMeta,
  message,
  Switch,
} from 'ant-design-vue';

interface NotificationItem {
  description?: string;
  displayName: string;
  isSubscribe: boolean;
  loading: boolean;
  name: string;
}

interface NotificationGroup {
  displayName: string;
  name: string;
  notifications: NotificationItem[];
}

const notificationGroups = ref<NotificationGroup[]>([]);

const { getMySubscribesApi, subscribeApi, unSubscribeApi } =
  useMySubscribesApi();
const { getAssignableNotifiersApi } = useNotificationsApi();

async function onInit() {
  const subRes = await getMySubscribesApi();
  const notifierRes = await getAssignableNotifiersApi();

  const groups: NotificationGroup[] = [];
  notifierRes.items.forEach((group) => {
    const notifications: NotificationItem[] = [];
    group.notifications.forEach((notification) => {
      notifications.push({
        description: notification.description,
        displayName: notification.displayName,
        isSubscribe: subRes.items.some((x) => x.name === notification.name),
        loading: false,
        name: notification.name,
      });
    });
    groups.push({
      displayName: group.displayName,
      name: group.name,
      notifications,
    });
  });
  notificationGroups.value = groups;
}

async function onSubscribed(notification: NotificationItem, checked: boolean) {
  try {
    notification.loading = true;
    const api = checked
      ? subscribeApi(notification.name)
      : unSubscribeApi(notification.name);
    await api;
    message.success($t('AbpUi.SavedSuccessfully'));
  } catch {
    notification.isSubscribe = !checked;
  } finally {
    notification.loading = false;
  }
}

onMounted(() => {
  onInit();
});
</script>

<template>
  <Card :bordered="false" :title="$t('abp.account.settings.noticeSettings')">
    <Collapse>
      <template v-for="group in notificationGroups" :key="group.name">
        <CollapsePanel :header="group.displayName">
          <List>
            <template
              v-for="notification in group.notifications"
              :key="notification.name"
            >
              <ListItem>
                <template #actions>
                  <Switch
                    v-model:checked="notification.isSubscribe"
                    @change="
                      (checked) => onSubscribed(notification, Boolean(checked))
                    "
                  />
                </template>
                <ListItemMeta>
                  <template #title>
                    {{ notification.displayName }}
                  </template>
                  <template #description>
                    <div>{{ notification.description }}</div>
                  </template>
                </ListItemMeta>
              </ListItem>
            </template>
          </List>
        </CollapsePanel>
      </template>
    </Collapse>
  </Card>
</template>

<style scoped></style>
