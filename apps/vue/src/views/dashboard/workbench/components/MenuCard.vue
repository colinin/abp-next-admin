<template>
  <div>
    <Card :title="title" class="menu-card">
      <template #extra>
        <Button type="link" @click="handleManageMenu">{{ t('routes.dashboard.workbench.menus.more') }}</Button>
      </template>
      <CardGrid v-for="menu in menus" :key="menu.title" class="menu-card-grid">
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
  import { Button, Card } from 'ant-design-vue';
  import { Icon } from '/@/components/Icon';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useModal } from '/@/components/Modal';
  import MenuReference from './MenuReference.vue';

  const CardGrid = Card.Grid;

  interface Menu {
    title: string;
    desc?: string;
    icon?: string;
    color?: string;
    size?: number;
  }

  const emits = defineEmits(['change', 'register']);
  const props = defineProps({
    title: {
      type: String,
      required: true,
    },
    menus: {
      type: Array as PropType<Menu[]>,
      default: [],
    }
  });

  const { t } = useI18n();
  const [registerReference, { openModal: openReferenceModal }] = useModal();

  function handleManageMenu() {
    openReferenceModal(true, { defaultCheckedKeys: props.menus });
  }

  function handleAddNew() {
    openReferenceModal(true, { radio: true });
  }

  function handleChange(menus) {
    console.log(menus);
    emits('change', menus);
  }
</script>

<style lang="less" scoped>
  .menu-card-grid {
    margin: 10px;
    width: 30%;
    cursor: pointer;
  }
</style>
