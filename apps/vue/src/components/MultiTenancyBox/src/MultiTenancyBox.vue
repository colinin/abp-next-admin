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

<script lang="ts">
  import { computed, defineComponent } from 'vue';
  import { Button, Form, Input } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import MultiTenancyModal from './MultiTenancyModal.vue';

  export default defineComponent({
    name: 'MultiTenancyBox',
    components: {
      BasicModal,
      Button,
      Form,
      FormItem: Form.Item,
      BInput,
      InputSearch: Input.Search,
      MultiTenancyModal,
    },
    setup() {
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

      return {
        L,
        multiTenancyEnabled,
        currentTenant,
        registerModal,
        handleSwitchTenant,
      };
    },
  });
</script>
