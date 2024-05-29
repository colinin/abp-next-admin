<template>
  <BasicModal
    @register="registerModal"
    :title="L('Objects:CreateFolder')"
    :width="466"
    :min-height="66"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, unref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form';
  import { getFolderModalSchemas } from '../datas/ModalData';
  import { createObject } from '/@/api/oss-management/objects';

  const emits = defineEmits(['register', 'change']);

  const { createMessage } = useMessage();
  const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
  const bucket = ref('');
  const path = ref('');
  const [registerForm, { validate, resetFields }] = useForm({
    labelWidth: 120,
    schemas: getFolderModalSchemas(),
    showActionButtonGroup: false,
    actionColOptions: {
      span: 24,
    },
  });
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner((data) => {
    resetFields();
    path.value = data.path;
    bucket.value = data.bucket;
  });

  function handleSubmit() {
    validate().then((input) => {
      changeOkLoading(true);
      const name = input.name.endsWith('/') ? input.name : input.name + '/';
      createObject({
        bucket: unref(bucket),
        path: unref(path),
        object: name,
        overwrite: false,
      })
        .then(() => {
          createMessage.success(L('Successful'));
          closeModal();
          emits('change', unref(bucket), unref(path), name);
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }
</script>
