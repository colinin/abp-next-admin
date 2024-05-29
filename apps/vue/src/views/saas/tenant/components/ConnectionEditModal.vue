<template>
  <BasicModal @register="registerModal" :title="L('ConnectionStrings')" @ok="handleSubmit">
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { SetConnectionStringAsyncByIdAndInput } from '/@/api/saas/tenant';
  import { getConnectionFormSchemas } from '../datas//ModalData';

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization(['AbpSaas']);

  const [registerForm, { resetFields, setFieldsValue, validate }] = useForm({
    colon: true,
    labelWidth: 120,
    layout: 'horizontal',
    schemas: getConnectionFormSchemas(),
    showActionButtonGroup: false,
  });
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner((data) => {
    nextTick(() => {
      resetFields();
      setFieldsValue(data);
    });
  });

  function handleSubmit() {
    validate().then((input) => {
      changeOkLoading(true);
      SetConnectionStringAsyncByIdAndInput(input.id, input)
        .then(() => {
          createMessage.success(L('Successful'));
          closeModal();
          emits('change');
        })
        .finally(() => changeOkLoading(false));
    });
  }
</script>
