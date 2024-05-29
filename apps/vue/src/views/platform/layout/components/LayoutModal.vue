<template>
  <BasicModalForm
    @register="register"
    :form-items="formItems"
    :save-changes="handleSaveChanges"
    :title="title"
    :width="650"
  />
</template>

<script lang="ts" setup>
  import { ref, computed } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useModalInner } from '/@/components/Modal';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { getModalFormSchemas } from './ModalData';
  import { Layout } from '/@/api/platform/layouts/model';
  import { create, update } from '/@/api/platform/layouts';

  const emits = defineEmits(['change', 'register']);

  const { L } = useLocalization('AppPlatform');
  const layout = ref<Layout>({} as Layout);
  const formItems = getModalFormSchemas(layout.value);
  const [register, { closeModal }] = useModalInner((dataVal) => {
    layout.value = dataVal;
  });

  const title = computed(() => {
    if (layout.value.id) {
      return L('Layout:EditByName', [layout.value.displayName] as Recordable);
    }
    return L('Layout:AddNew');
  });

  function handleSaveChanges(data) {
    const api =
      data.id === undefined
        ? create({
            dataId: data.dataId,
            framework: data.framework,
            name: data.name,
            displayName: data.displayName,
            description: data.description,
            redirect: data.redirect,
            path: data.path,
          })
        : update(data.id, {
            name: data.name,
            displayName: data.displayName,
            description: data.description,
            redirect: data.redirect,
            path: data.path,
          });
    return api.then(() => {
      emits('change');
      closeModal();
    });
  }
</script>
