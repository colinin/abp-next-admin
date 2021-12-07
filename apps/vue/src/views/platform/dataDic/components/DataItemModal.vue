<template>
  <BasicModal v-bind="$attrs" @register="register" :title="title()" :width="600" @ok="handleSubmit">
    <BasicForm @register="registerForm" :model="dataItem" />
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';

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
    props: {
      dataId: { type: String, required: true },
    },
    emits: ['change', 'register'],
    setup() {
      const dataItem = ref<DataItem>();
      const { L } = useLocalization('AppPlatform');
      const schemas = getDataItemFormSchemas(dataItem.value);

      const [registerForm, { validate, getFieldsValue }] = useForm({
        labelWidth: 120,
        schemas,
        showActionButtonGroup: false,
        actionColOptions: {
          span: 24,
        },
      });
      const [register, { setModalProps, closeModal }] = useModalInner((dataVal) => {
        dataItem.value = dataVal;
      });

      return {
        L,
        registerForm,
        register,
        dataItem,
        setModalProps,
        validate,
        getFieldsValue,
        closeModal,
      };
    },
    computed: {
      title() {
        return () => {
          if (this.dataItem && this.dataItem.id) {
            return this.L('Data:EditItem');
          }
          return this.L('Data:AppendItem');
        };
      },
    },
    methods: {
      handleSubmit() {
        this.validate().then(() => {
          this.changeModalManeuverability(false);
          const item = this.getFieldsValue();
          const api =
            item.id !== undefined
              ? updateItem(this.dataId, item.name, {
                  defaultValue: item.defaultValue,
                  displayName: item.displayName,
                  description: item.description,
                  allowBeNull: item.allowBeNull,
                  valueType: item.valueType,
                })
              : createItem(this.dataId, {
                  name: item.name,
                  defaultValue: item.defaultValue,
                  displayName: item.displayName,
                  description: item.description,
                  allowBeNull: item.allowBeNull,
                  valueType: item.valueType,
                });
          api
            .then(() => {
              this.$emit('change', this.dataId);
              this.closeModal();
            })
            .finally(() => {
              this.changeModalManeuverability(true);
            });
        });
      },
      changeModalManeuverability(allow: boolean) {
        this.setModalProps({
          loading: !allow,
          confirmLoading: !allow,
          showCancelBtn: allow,
          closable: allow,
          maskClosable: allow,
        });
      },
    },
  });
</script>
