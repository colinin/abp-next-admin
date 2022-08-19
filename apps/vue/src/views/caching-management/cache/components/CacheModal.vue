<template>
  <BasicModal
    @register="registerModal"
    :title="L('CacheInfo')"
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
  import { getValue } from '/@/api/caching-management/cache';

  const { L } = useLocalization('CachingManagement');
  const [registerForm, { resetFields, setFieldsValue }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: getModalFormSchemas(),
  });
  const [registerModal] = useModalInner((data) => {
    nextTick(() => {
      fetchCacheInfo(data.key);
    });
  });

  function fetchCacheInfo(key: string) {
    resetFields();
    getValue(key).then((cache) => {
      setFieldsValue({
        key: key,
      });
      setFieldsValue(cache);
    });
  }
</script>
