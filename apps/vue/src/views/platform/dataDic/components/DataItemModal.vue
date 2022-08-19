<template>
  <BasicModal v-bind="$attrs" @register="register" :title="title" :width="600" @ok="handleSubmit">
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref, watch } from 'vue';

  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  import { DataItem } from '/@/api/platform/model/dataItemModel';
  import { getDataItemFormSchemas } from './ModalData';

  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form/index';

  import { createItem, updateItem } from '/@/api/platform/dataDic';
  export default defineComponent({
    name: 'DataItemModal',
    components: {
      BasicForm,
      BasicModal,
    },
    emits: ['change', 'register'],
    setup(_, { emit }) {
      const dataItem = ref<DataItem>();
      const { createMessage } = useMessage();
      const { L } = useLocalization(['AppPlatform', 'AbpUi']);
      const schemas = getDataItemFormSchemas();

      watch(
        () => unref(dataItem),
        (item) => {
          resetFields();
          setFieldsValue(item);
        },
        {
          immediate: false,
        },
      )

      const [registerForm, { validate, setFieldsValue, resetFields }] = useForm({
        labelWidth: 120,
        schemas,
        showActionButtonGroup: false,
        actionColOptions: {
          span: 24,
        },
      });

      const [register, { changeLoading, closeModal }] = useModalInner((dataVal) => {
        dataItem.value = dataVal;
      });

      const title = computed(() => {
        if (unref(dataItem)?.id) {
          return L('Data:EditItem');
        }
        return L('Data:AppendItem');
      });

      function handleSubmit() {
        validate().then((input) => {
          changeLoading(true);
          const api = input.id
              ? updateItem(input.dataId, input.name, input)
              : createItem(input.dataId, input);
          api.then(() => {
            createMessage.success(L('Successful'));
            emit('change', input.dataId);
            closeModal();
          })
          .finally(() => {
            changeLoading(false);
          });
        });
      }

      return {
        L,
        title,
        registerForm,
        register,
        dataItem,
        handleSubmit,
      };
    },
  });
</script>
