<template>
  <BasicModalForm
    @register="registerModal"
    :save-changes="handleSaveChanges"
    :form-items="formItems"
    :title="formTitle"
  />
</template>

<script lang="ts">
  import { defineComponent, computed, ref, unref } from 'vue';

  import { useModal } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { getModalFormSchemas } from './ModalData';
  import { RouteGroup } from '/@/api/api-gateway/model/groupModel';
  export default defineComponent({
    name: 'GroupModal',
    components: { BasicModalForm },
    setup() {
      const { L } = useLocalization('ApiGateway');
      const formModel = ref<Nullable<RouteGroup>>(null);
      const formItems = getModalFormSchemas();
      const formTitle = computed(() => {
        const model = unref(formModel);
        if (model && model.id) {
          return L('Group:EditBy', [model.name] as Recordable);
        }
        return L('Group:AddNew');
      });
      const [registerModal, { openModal }] = useModal();

      return {
        formItems,
        formTitle,
        registerModal,
        openModal,
      };
    },
    methods: {
      handleSaveChanges(val) {
        console.log(val);
      },
    },
  });
</script>
