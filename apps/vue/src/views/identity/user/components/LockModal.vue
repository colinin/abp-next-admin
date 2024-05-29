<template>
  <BasicModalForm
    @register="registerModal"
    :save-changes="handleSaveChanges"
    :form-items="formSchemas"
    :title="L('Lockout')"
  />
</template>

<script lang="ts" setup>
  import { ref, unref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { useModalInner } from '/@/components/Modal';

  import { useLock } from '../hooks/useLock';

  const emits = defineEmits(['change', 'register']);

  const { L } = useLocalization('AbpIdentity');
  const { createMessage } = useMessage();
  const userIdRef = ref('');
  const { formSchemas, handleLock } = useLock({ emit: emits });
  const [registerModal, { closeModal }] = useModalInner((val) => {
    userIdRef.value = val.userId;
  });

  function handleSaveChanges(input) {
    return handleLock(unref(userIdRef), input).then(() => {
      createMessage.success(L('Successful'));
      closeModal();
    });
  }
</script>
