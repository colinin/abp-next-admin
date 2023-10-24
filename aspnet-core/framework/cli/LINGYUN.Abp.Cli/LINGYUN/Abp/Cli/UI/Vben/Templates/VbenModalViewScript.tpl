<template>
  <BasicModal
    :title="L('{{model.application}}')"
    :can-fullscreen="false"
    :show-ok-btn="{{ model.has_submit }}"
    :width="800"
    :height="500"
    @register="registerModal"
    {{~ if model.has_submit ~}}
    @ok="handleSubmit"
    {{~ end ~}}
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  {{~ if model.has_submit ~}}
  import { useMessage } from '/@/hooks/web/useMessage';
  {{~ end ~}}
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getModalFormSchemas } from '../datas/ModalData';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { {{ model.get_action }}{{ if model.has_create }}, {{ model.create_action }}{{ end }}{{ if model.has_update }}, {{ model.update_action }}{{ end }} } from '{{model.api_path}}';

  const emits = defineEmits(['change', 'register']);

  {{~ if model.has_submit ~}}
  const { createMessage } = useMessage();
  {{~ end ~}}
  const { L } = useLocalization(['{{model.remote_service}}', 'AbpUi']);
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: getModalFormSchemas(),
    transformDateFunc: (date) => {
      return date ? formatToDateTime(date) : '';
    },
  });
  const [registerModal, { {{ if model.has_submit }}closeModal{{ end }} }] = useModalInner((data) => {
    nextTick(() => {
      resetFields();
      if (data.{{ model.key }}) {
        fetchEntity(data.{{ model.key }});
      }
    });
  });

  function fetchEntity(id: string) {
  {{ model.get_action }}(id).then((dto) => {
      setFieldsValue(dto);
    });
  }

  {{~ if model.has_submit ~}}
  function handleSubmit() {
    validate().then((input) => {
      {{~ if model.has_create && model.has_update ~}}
      const api = input.{{ model.key }}
        ? {{ model.update_action }}(input.{{ model.key }}, input)
        : {{ model.create_action }}(input);
      {{~ else if model.has_create ~}}
      const api = {{ model.create_action }}(input);
      {{~ else if model.has_create ~}}
      const api = {{ model.update_action }}(input.{{ model.key }}, input);
      {{~ end ~}}

      api.then((dto) => {
        createMessage.success(L('SuccessfullySaved'));
        emits('change', dto);
        closeModal();
      });
    });
  }
  {{~ end ~}}
</script>
