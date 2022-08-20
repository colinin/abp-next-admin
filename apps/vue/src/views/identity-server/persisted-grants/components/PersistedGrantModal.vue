<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('DisplayName:PersistedGrants')"
    :width="660"
    :min-height="400"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getModalFormSchemas } from './ModalData';
  import { get } from '/@/api/identity-server/persistedGrants';

  defineEmits(['register']);

  const { L } = useLocalization('AbpIdentityServer');
  const [registerForm, { resetFields, setFieldsValue }] = useForm({
    colon: true,
    labelWidth: 120,
    showActionButtonGroup: false,
    actionColOptions: {
      span: 24,
    },
    schemas: getModalFormSchemas(),
  });
  const [registerModal] = useModalInner((val) => {
    nextTick(() => {
      fetchPersistedGrant(val.id);
    });
  });

  function fetchPersistedGrant(id?: string) {
    resetFields();
    if (id) {
      get(id).then((res) => {
        setFieldsValue(res);
      });
    }
  }
</script>
