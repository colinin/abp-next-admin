<script setup lang="ts">
import { computed, defineAsyncComponent } from 'vue';

import { useVbenModal } from '@vben/common-ui';

import { useAbpStore } from '@abp/core';
import { Button, InputSearch } from 'ant-design-vue';

defineOptions({
  name: 'TenantSelect',
});

const emits = defineEmits<{
  (event: 'change', data?: { id?: string; name?: string }): void;
}>();

const abpStore = useAbpStore();

const getCurrentTenant = computed(() => {
  return abpStore.application?.currentTenant;
});

const [Modal, modapApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./TenantSelectModal.vue'),
  ),
});

function onSwitchClick() {
  modapApi.setData({
    name: getCurrentTenant.value?.name,
  });
  modapApi.open();
}

function onChange(tenant?: { id?: string; name?: string }) {
  emits('change', tenant);
}
</script>

<template>
  <div class="w-full">
    <InputSearch
      readonly
      :value="getCurrentTenant?.name"
      :placeholder="$t('AbpUiMultiTenancy.NotSelected')"
    >
      <template #enterButton>
        <Button @click="onSwitchClick">
          ({{ $t('AbpUiMultiTenancy.Switch') }})
        </Button>
      </template>
    </InputSearch>
    <Modal @change="onChange" />
  </div>
</template>

<style scoped></style>
