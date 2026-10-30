<script setup lang="ts">
import type { AntFlowAddKind } from './types';

import { ref } from 'vue';

import { $t } from '@vben/locales';

interface AddAction {
  color?: string;
  icon?: string;
  key: AntFlowAddKind;
  label: string;
}

const props = withDefaults(
  defineProps<{
    actions?: AddAction[];
    readonly?: boolean;
  }>(),
  {
    actions: () => [
      {
        key: 'approval',
        label: $t('antflow.addNode.approval'),
        icon: '✓',
        color: '#f2994a',
      },
      {
        key: 'cc',
        label: $t('antflow.addNode.cc'),
        icon: 'CC',
        color: '#2f80ed',
      },
      {
        key: 'condition',
        label: $t('antflow.addNode.condition'),
        icon: '◇',
        color: '#27ae60',
      },
    ],
    readonly: false,
  },
);

const emit = defineEmits<{
  add: [kind: AntFlowAddKind];
}>();

const menuOpen = ref(false);

function handleAdd(kind: AntFlowAddKind) {
  if (props.readonly) {
    return;
  }
  menuOpen.value = false;
  emit('add', kind);
}
</script>

<template>
  <div
    v-if="!readonly"
    class="add-node"
    @click.stop
    @mouseenter="menuOpen = true"
    @mouseleave="menuOpen = false"
  >
    <div
      class="add-btn"
      role="button"
      tabindex="0"
      :title="$t('antflow.addNode.add')"
    >
      <span class="add-plus">＋</span>
    </div>
    <div class="add-menu" :class="{ 'is-open': menuOpen }" @click.stop>
      <button
        v-for="item in actions"
        :key="item.key"
        class="menu-item"
        type="button"
        @click.stop="handleAdd(item.key)"
      >
        <span class="menu-icon" :style="{ color: item.color ?? '#3296fa' }">
          {{ item.icon ?? item.label.slice(0, 1) }}
        </span>
        <span class="menu-label">{{ item.label }}</span>
      </button>
    </div>
  </div>
</template>

<style scoped>
.add-node {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.add-node::after {
  position: absolute;
  top: 50%;
  left: 100%;
  width: 16px;
  height: 56px;
  content: '';
  transform: translateY(-50%);
}

.add-btn {
  position: relative;
  z-index: 2;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 30px;
  height: 30px;
  color: #fff;
  cursor: pointer;
  background-color: #3296fa;
  border-radius: 50%;
  box-shadow: 0 4px 10px rgb(50 150 250 / 30%);
  transition:
    transform 0.2s ease,
    box-shadow 0.2s ease;
}

.add-btn:hover {
  box-shadow: 0 8px 18px rgb(50 150 250 / 35%);
  transform: scale(1.12);
}

.add-plus {
  font-size: 20px;
  font-weight: 500;
  line-height: 1;
}

.add-menu {
  position: absolute;
  top: 50%;
  left: 42px;
  z-index: 20;
  display: grid;
  visibility: hidden;
  grid-template-columns: repeat(3, 1fr);
  gap: 8px;
  min-width: 260px;
  padding: 10px 12px;
  pointer-events: none;
  background-color: #fff;
  border: 1px solid #edf0f5;
  border-radius: 14px;
  box-shadow: 0 12px 30px rgb(20 40 80 / 16%);
  opacity: 0;
  transform: translate3d(0, -50%, 0);
  transition:
    opacity 0.15s ease,
    visibility 0.15s ease;
}

.add-menu.is-open {
  visibility: visible;
  pointer-events: auto;
  opacity: 1;
  transform: translate3d(0, -50%, 0);
}

.add-menu::before {
  position: absolute;
  top: 50%;
  left: -8px;
  content: '';
  border-color: transparent #edf0f5 transparent transparent;
  border-style: solid;
  border-width: 8px 8px 8px 0;
  transform: translateY(-50%);
}

.add-menu::after {
  position: absolute;
  top: 50%;
  left: -7px;
  content: '';
  border-color: transparent #fff transparent transparent;
  border-style: solid;
  border-width: 7px 7px 7px 0;
  transform: translateY(-50%);
}

.menu-item {
  display: flex;
  gap: 6px;
  align-items: center;
  padding: 8px 10px;
  cursor: pointer;
  background: #fff;
  border: 1px solid #e7e9ef;
  border-radius: 10px;
  transition:
    transform 0.15s ease,
    box-shadow 0.15s ease,
    border-color 0.15s ease;
}

.menu-item:hover {
  border-color: #cfd7e6;
  box-shadow: 0 6px 14px rgb(20 40 80 / 12%);
  transform: translateY(-1px);
}

.menu-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 30px;
  height: 30px;
  font-size: 14px;
  font-weight: 600;
  background: #f5f7fb;
  border-radius: 50%;
}

.menu-label {
  font-size: 13px;
  font-weight: 500;
  color: #3f4754;
  white-space: nowrap;
}
</style>
