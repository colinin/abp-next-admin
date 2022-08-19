<template>
  <BasicModal
    @register="registerModal"
    :title="title"
    :can-fullscreen="false"
    :show-ok-btn="false"
    :width="800"
    :height="500"
  >

  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed } from 'vue';
  import { Form } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  const FormItem = Form.Item;

  const { L } = useLocalization('AbpOpenIddict');
  const [registerModal] = useModalInner((data) => {
    console.log(data);
  });
  const title = computed(() => {

  });
</script>
