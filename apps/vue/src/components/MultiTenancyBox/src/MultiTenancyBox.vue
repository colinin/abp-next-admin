<template>
  <div v-if="multiTenancyEnabled">
    <InputSearch
      readonly
      :placeholder="L('SwitchTenantHint')"
      size="large"
      autoComplete="off"
      @search="handleSwitchTenant"
      :value="currentTenant.name"
    >
      <template #enterButton>
        <Button> ({{ L('Switch') }}) </Button>
      </template>
    </InputSearch>
    <MultiTenancyModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { computed } from 'vue';
  import { Button, Input } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import MultiTenancyModal from './MultiTenancyModal.vue';

  const InputSearch = Input.Search;

  const { L } = useLocalization('AbpUiMultiTenancy');
  const [registerModal, { openModal }] = useModal();
  const multiTenancyEnabled = computed(() => {
    const abpStore = useAbpStoreWithOut();
    return abpStore.getApplication.multiTenancy.isEnabled;
  });
  const currentTenant = computed(() => {
    const abpStore = useAbpStoreWithOut();
    return abpStore.getApplication.currentTenant;
  });

  function handleSwitchTenant() {
    openModal(true, currentTenant.value.name);
  }
</script>
