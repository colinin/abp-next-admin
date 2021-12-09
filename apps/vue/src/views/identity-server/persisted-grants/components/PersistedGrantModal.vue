<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('DisplayName:PersistedGrants')"
    :width="660"
    :min-height="400"
  >
    <BasicForm
      ref="formElRef"
      :model="persistedGrantRef"
      :colon="true"
      :schemas="formSchemas"
      :label-width="120"
      :show-action-button-group="false"
      :action-col-options="{
        span: 24,
      }"
    />
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref, watch, unref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, FormActionType } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getModalFormSchemas } from './ModalData';
  import { get } from '/@/api/identity-server/persistedGrants';
  import { PersistedGrant } from '/@/api/identity-server/model/persistedGrantsModel';
  export default defineComponent({
    name: 'PersistedGrantModal',
    components: { BasicForm, BasicModal },
    emits: ['register'],
    setup() {
      const { L } = useLocalization('AbpIdentityServer');
      const persistedGrantIdRef = ref('');
      const persistedGrantRef = ref<PersistedGrant | null>(null);
      const formElRef = ref<Nullable<FormActionType>>(null);
      const [registerModal] = useModalInner((val) => {
        persistedGrantIdRef.value = val.id;
      });
      const formSchemas = getModalFormSchemas();

      watch(
        () => unref(persistedGrantIdRef),
        (id) => {
          const formEl = unref(formElRef);
          formEl?.resetFields();
          get(id).then((res) => {
            persistedGrantRef.value = res;
          });
        },
      );

      return {
        L,
        formElRef,
        formSchemas,
        registerModal,
        persistedGrantRef,
      };
    },
  });
</script>
