<template>
  <BasicModal
    v-bind="$attrs"
    :title="title"
    :loading="loading"
    :showOkBtn="!loading"
    :showCancelBtn="!loading"
    :maskClosable="!loading"
    :closable="!loading"
    :width="800"
    :height="500"
    @register="registerModal"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick, ref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getModalFormSchemas } from '../datas//ModalData';
  import { GetAsyncById, CreateAsyncByInput, UpdateAsyncByIdAndInput } from '/@/api/saas/tenant';

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpSaas');
  const loading = ref(false);
  const title = ref('');
  const [registerModal, { closeModal }] = useModalInner((data) => {
    nextTick(() => {
      fetchTenant(data.id);
    });
  });
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    layout: 'vertical',
    schemas: getModalFormSchemas(),
    showActionButtonGroup: false,
  });

  function fetchTenant(id?: string) {
    title.value = L('Edit');
    resetFields();
    if (id) {
      GetAsyncById(id).then((res) => {
        setFieldsValue(res);
        title.value = L('NewTenant');
      });
    }
  }

  function handleSubmit() {
    validate().then((input) => {
      loading.value = true;
      const api = input.id ? UpdateAsyncByIdAndInput(input.id, input) : CreateAsyncByInput(input);
      api
        .then(() => {
          createMessage.success(L('Successful'));
          emits('change');
          closeModal();
        })
        .finally(() => {
          loading.value = false;
        });
    });
  }
</script>
