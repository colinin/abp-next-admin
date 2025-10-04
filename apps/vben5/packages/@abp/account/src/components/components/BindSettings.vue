<script setup lang="ts">
import type { BindItem } from '../../types/bind';

import { onMounted } from 'vue';

import { Button, Card, Empty, List } from 'ant-design-vue';

defineProps<{
  items?: BindItem[];
}>();
const emits = defineEmits<{
  (event: 'onInit'): void;
}>();
const ListItem = List.Item;
const ListItemMeta = List.Item.Meta;

onMounted(() => {
  setTimeout(() => {
    emits('onInit');
  }, 200);
});
</script>

<template>
  <Card :bordered="false" :title="$t('abp.account.settings.bindSettings')">
    <Empty v-if="items?.length === 0" />
    <List v-else item-layout="horizontal">
      <ListItem v-for="item in items" :key="item.title">
        <template v-if="item.buttons?.length" #extra>
          <Button
            v-for="button in item.buttons"
            :type="button.type"
            @click="button.click"
            :key="button.title"
          >
            {{ button.title }}
          </Button>
        </template>
        <ListItemMeta :description="item.description" :title="item.title" />
      </ListItem>
    </List>
  </Card>
</template>

<style scoped></style>
