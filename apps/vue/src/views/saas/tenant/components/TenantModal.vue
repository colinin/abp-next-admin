<template>
  <BasicModal
    v-bind="$attrs"
    :title="modalTitle"
    :loading="loading"
    :showOkBtn="!loading"
    :showCancelBtn="!loading"
    :maskClosable="!loading"
    :closable="!loading"
    :width="800"
    :height="500"
    @register="registerModal"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref, watch } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getModalFormSchemas } from '../datas//ModalData';
  import { getById, create, update } from '/@/api/saas/tenant';
  export default defineComponent({
    components: { BasicForm, BasicModal },
    emits: ['change', 'register'],
    setup(_props, { emit }) {
      const { L } = useLocalization('AbpTenantManagement');
      const loading = ref(false);
      const tenantIdRef = ref('');
      const [registerModal, { closeModal }] = useModalInner((data) => {
        tenantIdRef.value = data.id;
      });
      const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
        schemas: getModalFormSchemas(),
        showActionButtonGroup: false,
      });
      const modalTitle = computed(() => {
        return unref(tenantIdRef) ? L('Edit') : L('NewTenant');
      });

      function handleSubmit() {
        validate().then((input) => {
          loading.value = true;

          const api = input.id
            ? update(input.id, {
                name: input.name,
              })
            : create(input);

          api
            .then(() => {
              emit('change');
              closeModal();
            })
            .finally(() => {
              loading.value = false;
            });
        });
      }

      watch(
        () => unref(tenantIdRef),
        (id) => {
          if (id) {
            getById(id).then((res) => {
              setFieldsValue(res);
            });
          } else {
            resetFields();
          }
        },
      );

      return {
        loading,
        modalTitle,
        registerModal,
        handleSubmit,
        registerForm,
      };
    },
  });
</script>
