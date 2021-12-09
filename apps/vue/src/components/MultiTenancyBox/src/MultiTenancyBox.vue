<template>
  <InputSearch
    readonly
    :placeholder="L('SwitchTenantHint')"
    size="large"
    autoComplete="off"
    @search="handleSwitchTenant"
    v-model:value="currentTenant.name"
  >
    <template #enterButton>
      <Button> ({{ L('Switch') }}) </Button>
    </template>
  </InputSearch>
  <BasicModal
    v-bind="$attrs"
    :minHeight="80"
    :closable="false"
    :maskClosable="false"
    :canFullscreen="false"
    :showCancelBtn="false"
    @register="registerModal"
    :title="L('SwitchTenant')"
    @ok="switchToTenant"
  >
    <Form layout="vertical" :model="formModelRef">
      <FormItem :label="L('SwitchTenantHint')">
        <BInput :allowClear="true" v-model:value="formModelRef.name" />
      </FormItem>
    </Form>
  </BasicModal>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref } from 'vue';
  import { Button, Form, Input } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { Persistent } from '/@/utils/cache/persistent';
  import { findTenantByName } from '/@/api/multi-tenancy/tenants';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  import { useAbpStoreWithOut } from '/@/store/modules/abp';

  export default defineComponent({
    name: 'MultiTenancyBox',
    components: {
      BasicModal,
      Button,
      Form,
      FormItem: Form.Item,
      BInput,
      InputSearch: Input.Search,
    },
    setup() {
      const { L } = useLocalization('AbpUiMultiTenancy');
      const { createMessage } = useMessage();
      const [registerModal, { openModal, closeModal }] = useModal();
      const currentTenant = computed(() => {
        const abpStore = useAbpStoreWithOut();
        return abpStore.getApplication.currentTenant;
      });
      const formModelRef = ref({
        id: currentTenant.value.id,
        name: currentTenant.value.name,
      });

      function handleSwitchTenant() {
        openModal(true, {}, true);
      }

      async function switchToTenant() {
        Persistent.setTenant({});
        const formModel = unref(formModelRef);
        if (formModel.name) {
          const result = await findTenantByName(formModel.name);
          Persistent.setTenant({
            id: result.tenantId,
            name: result.name,
          });
          if (!result.success) {
            createMessage.warn(L('GivenTenantIsNotAvailable', [formModel.name]));
          }
        }
        const abpStore = useAbpStoreWithOut();
        await abpStore.initlizeAbpApplication();
        closeModal();
      }

      return {
        L,
        currentTenant,
        formModelRef,
        registerModal,
        switchToTenant,
        handleSwitchTenant,
      };
    },
  });
</script>
