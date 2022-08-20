<template>
  <BasicModal
    @register="registerModal"
    :title="L('ConnectionStrings')"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { setConnectionString } from '/@/api/saas/tenant';
  import { getConnectionFormSchemas } from '../datas//ModalData';

  const emits = defineEmits(['change']);

  const { createMessage } = useMessage();
  const { L } = useLocalization(['AbpSaas']);

  const [registerForm, { resetFields, setFieldsValue, validate }] = useForm({
    colon: true,
    labelWidth: 120,
    layout: 'horizontal',
    schemas: getConnectionFormSchemas(),
    showActionButtonGroup: false,
  });
  const [registerModal, { closeModal }] = useModalInner((data) => {
    nextTick(() => {
      resetFields();
      setFieldsValue(data);
    });
  });

  function handleSubmit() {
    validate().then((input) => {
      setConnectionString(input.id, input).then(() => {
        createMessage.success(L('Successful'));
        closeModal();
        emits('change');
      });
    });
  }
</script>
