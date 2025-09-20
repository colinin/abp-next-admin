<script setup lang="ts">
import type { FavoriteMenu } from '../types';

import { computed, h } from 'vue';

import { $t } from '@vben/locales';

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  VbenIcon,
} from '@vben-core/shadcn-ui';

import { DeleteOutlined } from '@ant-design/icons-vue';
import { Dropdown, Menu, Modal } from 'ant-design-vue';

interface Props {
  items?: FavoriteMenu[];
  title: string;
}

defineOptions({
  name: 'WorkbenchQuickNav',
});

const props = withDefaults(defineProps<Props>(), {
  items: () => [],
});

const emits = defineEmits<{
  (event: 'click', menu: FavoriteMenu): void;
  (event: 'delete', menu: FavoriteMenu): void;
  (event: 'add'): void;
}>();

const MenuItem = Menu.Item;

const getFavoriteMenus = computed(() => {
  const addMenu: FavoriteMenu = {
    id: 'addMenu',
    displayName: $t('workbench.content.favoriteMenu.create'),
    icon: 'ion:add-outline',
    color: '#00bfff',
    isDefault: true,
  };
  return [...props.items, addMenu];
});

function onClick(menu: FavoriteMenu) {
  if (menu.id === 'addMenu') {
    emits('add');
    return;
  }
  emits('click', menu);
}

function onMenuClick(key: string, menu: FavoriteMenu) {
  switch (key) {
    case 'delete': {
      Modal.confirm({
        centered: true,
        iconType: 'warning',
        title: $t('AbpUi.AreYouSure'),
        content: $t('AbpUi.ItemWillBeDeletedMessage'),
        okCancel: true,
        onOk: () => {
          emits('delete', menu);
        },
      });
    }
  }
}
</script>

<template>
  <Card>
    <CardHeader class="py-4">
      <CardTitle class="text-lg">{{ title }}</CardTitle>
    </CardHeader>
    <CardContent class="flex flex-wrap p-0">
      <template
        v-for="(item, index) in getFavoriteMenus"
        :key="item.displayName"
      >
        <Dropdown :trigger="['contextmenu']">
          <div
            :class="{
              'border-r-0': index % 3 === 2,
              'border-b-0': index < 3,
              'pb-4': index > 2,
              'rounded-bl-xl': index === items.length - 3,
              'rounded-br-xl': index === items.length - 1,
            }"
            class="flex-col-center border-border group w-1/3 cursor-pointer border-r border-t py-8 hover:shadow-xl"
            @click="onClick(item)"
          >
            <VbenIcon
              :color="item.color"
              :icon="item.icon"
              class="size-7 transition-all duration-300 group-hover:scale-125"
            />
            <span class="text-md mt-2 truncate">{{ item.displayName }}</span>
          </div>
          <template #overlay>
            <Menu
              v-if="!item.isDefault"
              @click="
                ({ key: menuKey }) => onMenuClick(menuKey.toString(), item)
              "
            >
              <MenuItem key="delete" :icon="h(DeleteOutlined)">
                {{ $t('workbench.content.favoriteMenu.delete') }}
              </MenuItem>
            </Menu>
          </template>
        </Dropdown>
      </template>
    </CardContent>
  </Card>
</template>

<style scoped></style>
