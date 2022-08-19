<template>
  <BasicModal
    @register="registerModal"
    :title="L('TokenInfo')"
    :can-fullscreen="false"
    :show-ok-btn="false"
    :width="800"
    :height="500"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getModalFormSchemas } from '../datas/ModalData';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { getById } from '/@/api/openiddict/tokens';

  const { L } = useLocalization('AbpOpenIddict');
  const [registerForm, { setFieldsValue, resetFields }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: getModalFormSchemas(),
    transformDateFunc: (date) => {
      return date ? formatToDateTime(date) : '';
    },
  });
  const [registerModal] = useModalInner((data) => {
    nextTick(() => {
      resetFields();
      fetchToken(data.id);
    });
  });

  function fetchToken(id: string) {
    getById(id).then((token) => {
      setFieldsValue(token);
    });
  }
</script>
