<script setup lang="ts">
import type { ApprovalFlowNodeKind } from '../../types/approval-flow';

import { $t } from '@vben/locales';

defineProps<{
  actions: Array<{ color?: string; key: ApprovalFlowNodeKind; label: string }>;
  readonly?: boolean;
}>();

const emit = defineEmits<{
  add: [kind: ApprovalFlowNodeKind];
}>();
</script>

<template>
  <div v-if="!readonly" class="add-node">
    <div class="add-btn" :title="$t('antflow.designer.common.addCondition')">
      ＋
    </div>
    <div class="add-menu">
      <div
        v-for="action in actions"
        :key="action.key"
        class="menu-item"
        @click.stop="emit('add', action.key)"
      >
        <div class="menu-icon" :style="{ color: action.color ?? '#3296fa' }">
          {{ action.label.slice(0, 1) }}
        </div>
        <div class="menu-label">{{ action.label }}</div>
      </div>
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

.add-btn {
  position: relative;
  z-index: 2;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 30px;
  height: 30px;
  font-size: 18px;
  color: #fff;
  cursor: pointer;
  background: #3296fa;
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
  background: #fff;
  border: 1px solid #edf0f5;
  border-radius: 14px;
  box-shadow: 0 12px 30px rgb(20 40 80 / 16%);
  opacity: 0;
  transform: translate3d(0, -50%, 0);
  transition:
    opacity 0.15s ease,
    visibility 0.15s ease;
}

.add-node:hover .add-menu,
.add-node:focus-within .add-menu {
  visibility: visible;
  pointer-events: auto;
  opacity: 1;
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
