<template>
  <div>
    <Card :title="title" class="menu-card">
      <!-- <template #extra>
        <Button type="link" @click="handleManageMenu">{{ t('routes.dashboard.workbench.menus.more') }}</Button>
      </template> -->
      <CardGrid
        v-for="menu in menus"
        :key="menu.title"
        class="menu-card-grid"
        :style="{ float: menus.length >= 2 ? 'left' : 'right' }"
        @click="handleNavigationTo(menu)"
        @contextmenu="(e) => handleContext(e, menu)"
      >
        <span class="flex">
          <Icon :icon="menu.icon" :color="menu.color" :size="menu.size ?? 30" />
          <span class="text-lg ml-4">{{ menu.title }}</span>
        </span>
        <div v-if="menu.desc" class="flex mt-2 h-10 text-secondary">{{ menu.desc }}</div>
      </CardGrid>
      <CardGrid class="menu-card-grid" @click="handleAddNew">
        <span class="flex">
          <Icon icon="ion:add-outline" color="#00BFFF" :size="30" />
          <span class="text-lg ml-4">{{ t('routes.dashboard.workbench.menus.addMenu') }}</span>
        </span>
      </CardGrid>
    </Card>
    <MenuReference @register="registerReference" @change="handleChange" />
  </div>
</template>
<script lang="ts" setup>
  import { Card } from 'ant-design-vue';
  import { Icon } from '/@/components/Icon';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useGo } from '/@/hooks/web/usePage';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useContextMenu } from '/@/hooks/web/useContextMenu';
  import { useModal } from '/@/components/Modal';
  import { Menu } from './menuProps';
  import MenuReference from './MenuReference.vue';

  const CardGrid = Card.Grid;

  const emits = defineEmits(['change', 'delete', 'register']);
  defineProps({
    title: {
      type: String,
      required: true,
    },
    menus: {
      type: Array as PropType<Menu[]>,
      default: [],
    },
  });

  const go = useGo();
  const { t } = useI18n();
  const { createConfirm } = useMessage();
  const [createContextMenu] = useContextMenu();
  const [registerReference, { openModal: openReferenceModal }] = useModal();

  function handleContext(e: MouseEvent, menu: Menu) {
    createContextMenu({
      event: e,
      items: [
        {
          label: t('routes.dashboard.workbench.menus.deleteMenu'),
          icon: 'ant-design:delete-outlined',
          disabled: menu.hasDefault,
          handler: () => {
            if (!menu.hasDefault) {
              createConfirm({
                iconType: 'warning',
                title: t('AbpUi.AreYouSure'),
                content: t('AbpUi.ItemWillBeDeletedMessage'),
                okCancel: true,
                onOk: () => {
                  emits('delete', menu);
                },
              });
            }
          },
        },
      ],
    });
  }

  function handleAddNew() {
    openReferenceModal(true, {});
  }

  function handleNavigationTo(menu: Menu) {
    if (menu.path) {
      go(menu.path);
    }
  }

  function handleChange(menu) {
    emits('change', menu);
  }
</script>

<style lang="less" scoped>
  .menu-card-grid {
    margin: 10px;
    width: 30%;
    text-align: 'center';
    cursor: pointer;
  }
</style>
