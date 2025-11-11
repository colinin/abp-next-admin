<script setup lang="ts">
import type { MenuDto, UserFavoriteMenuDto } from '../../../types';

import { defineAsyncComponent, ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { useAppConfig } from '@vben/hooks';
import { IconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { listToTree } from '@abp/core';
import { message, TreeSelect } from 'ant-design-vue';

import { useMyFavoriteMenusApi } from '../../../api/useMyFavoriteMenusApi';
import { useMyMenusApi } from '../../../api/useMyMenusApi';

const emits = defineEmits<{
  (event: 'change', data: UserFavoriteMenuDto): void;
}>();

const ColorPicker = defineAsyncComponent(() =>
  import('vue3-colorpicker').then((res) => {
    import('vue3-colorpicker/style.css');
    return res.ColorPicker;
  }),
);

const availableMenus = ref<MenuDto[]>([]);

const { getAllApi } = useMyMenusApi();
const { createApi } = useMyFavoriteMenusApi();
const { uiFramework } = useAppConfig(import.meta.env, import.meta.env.PROD);

const [Form, formApi] = useVbenForm({
  schema: [
    {
      label: $t('workbench.content.favoriteMenu.select'),
      fieldName: 'menuId',
      component: 'TreeSelect',
      rules: 'selectRequired',
    },
    {
      label: $t('workbench.content.favoriteMenu.color'),
      fieldName: 'color',
      component: 'ColorPicker',
      defaultValue: '#000000',
      modelPropName: 'pureColor',
    },
    {
      label: $t('workbench.content.favoriteMenu.alias'),
      fieldName: 'aliasName',
      component: 'Input',
    },
    {
      label: $t('workbench.content.favoriteMenu.icon'),
      fieldName: 'icon',
      component: 'IconPicker',
    },
  ],
  showDefaultActions: false,
  handleSubmit: onSubmit,
  commonConfig: {
    colon: true,
    componentProps: {
      class: 'w-full',
    },
  },
});
const [Modal, modalApi] = useVbenModal({
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      await onInitMenus();
    }
  },
});

async function onInitMenus() {
  const { items } = await getAllApi({
    framework: uiFramework,
  });
  const menus = listToTree<MenuDto>(items, { id: 'id', pid: 'parentId' });
  availableMenus.value = menus;
}

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    const menuDto = await createApi({
      framework: uiFramework,
      menuId: values.menuId,
      color: values.color,
      icon: values.icon,
      aliasName: values.aliasName,
    });
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', menuDto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('workbench.content.favoriteMenu.manage')">
    <Form>
      <template #color="slotProps">
        <div class="flex flex-row items-center">
          <ColorPicker v-bind="slotProps" format="hex" />
          <span>({{ slotProps.value }})</span>
        </div>
      </template>
      <template #menuId="slotProps">
        <TreeSelect
          allow-clear
          class="w-full"
          tree-icon
          v-bind="slotProps"
          :field-names="{ label: 'displayName', value: 'id' }"
          :tree-data="availableMenus"
        >
          <template #title="item">
            <div class="flex flex-row items-center gap-1">
              <IconifyIcon v-if="item.meta?.icon" :icon="item.meta.icon" />
              <span>{{ item.displayName }}</span>
            </div>
          </template>
        </TreeSelect>
      </template>
    </Form>
  </Modal>
</template>

<style scoped></style>
