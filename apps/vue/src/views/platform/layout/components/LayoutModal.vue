<template>
  <BasicModalForm
    @register="register"
    :form-items="formItems"
    :save-changes="handleSaveChanges"
    :title="title"
    :width="650"
  />
</template>

<script lang="ts">
  import { defineComponent, ref, computed } from 'vue';

  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useModalInner } from '/@/components/Modal';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { getModalFormSchemas } from './ModalData';
  import { Layout } from '/@/api/platform/model/layoutModel';
  import { create, update } from '/@/api/platform/layout';

  export default defineComponent({
    name: 'LayoutModal',
    components: {
      BasicModalForm,
    },
    emits: ['change', 'register'],
    setup() {
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

      return {
        L,
        layout,
        register,
        closeModal,
        formItems,
        title,
      };
    },
    methods: {
      handleSaveChanges(data) {
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
          this.$emit('change');
        });
      },
    },
  });
</script>
