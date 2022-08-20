<template>
  <BasicModal
    v-bind="$attrs"
    :title="modalTitle"
    :loading="loading"
    :showOkBtn="!loading"
    :showCancelBtn="!loading"
    :maskClosable="!loading"
    :closable="!loading"
    :width="500"
    :height="300"
    @register="registerModal"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, unref, nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getModalFormSchemas } from '../datas//ModalData';
  import { getById, create, update } from '/@/api/saas/editions';

  const emits = defineEmits(['change', 'register']);
  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpSaas');
  const loading = ref(false);
  const editionIdRef = ref('');
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner((data) => {
    editionIdRef.value = data.id;
    fetchEdition();
  });
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    layout: 'vertical',
    schemas: getModalFormSchemas(),
    showActionButtonGroup: false,
  });
  const modalTitle = computed(() => {
    return unref(editionIdRef) ? L('Edit') : L('NewEdition');
  });

  function fetchEdition() {
    const editionId = unref(editionIdRef);
    if (!editionId) {
      nextTick(() => {
        resetFields();
      });
      return;
    }
    getById(editionId).then((edition) => {
      nextTick(() => {
        setFieldsValue(edition);
      });
    });
  }

  function handleSubmit() {
    validate().then((input) => {
      changeOkLoading(true);
      const api = input.id ? update(input.id, input) : create(input);
      api.then((edition) => {
        createMessage.success(L('Successful'));
        emits('change', edition);
        closeModal();
      }).finally(() => {
        changeOkLoading(false);
      });
    });
  }
</script>
