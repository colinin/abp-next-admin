<template>
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
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick, inject } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { findTenantByName } from '/@/api/multi-tenancy/tenants';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import { useGlobSetting } from '/@/hooks/setting';

  const cookies = inject<any>('$cookies');
  const globSetting = useGlobSetting();
  const { L } = useLocalization('AbpUiMultiTenancy');
  const { createMessage } = useMessage();
  const [registerForm, { validate, setFieldsValue }] = useForm({
    showActionButtonGroup: false,
    layout: 'vertical',
    schemas: [
      {
        field: 'name',
        label: L('SwitchTenantHint'),
        component: 'Input',
        colProps: { span: 24 },
      },
    ],
  });
  const [registerModal, { closeModal, changeLoading }] = useModalInner((name) => {
    nextTick(() => {
      setFieldsValue({
        name: name,
      });
    });
  });

  function switchToTenant() {
    validate().then((input) => {
      changeLoading(true);
      const abpStore = useAbpStoreWithOut();
      if (!input.name) {
        cookies?.remove(globSetting.multiTenantKey);
      } else {
        findTenantByName(input.name)
          .then((result) => {
            if (!result.success || !result.tenantId) {
              createMessage.warn(L('GivenTenantIsNotExist', [input.name]));
              return;
            }

            if (!result.isActive) {
              createMessage.warn(L('GivenTenantIsNotAvailable', [input.name]));
              return;
            }

            cookies?.set(globSetting.multiTenantKey, result.tenantId);
          })
          .finally(() => changeLoading(false));
      }
      // 不加延迟在下次请求不会携带租户标识
      setTimeout(() => {
        abpStore
          .initlizeAbpApplication()
          .then(closeModal)
          .finally(() => changeLoading(false));
      }, 100);
    });
  }
</script>
