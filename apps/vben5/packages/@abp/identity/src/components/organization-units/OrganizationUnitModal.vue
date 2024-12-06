<script setup lang="ts">
import type {
  OrganizationUnitCreateDto,
  OrganizationUnitDto,
  OrganizationUnitUpdateDto,
} from '../../types/organization-units';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useVbenForm } from '@abp/ui';

import { createApi, getApi, updateApi } from '../../api/organization-units';

defineOptions({
  name: 'OrganizationUnitModal',
});
const emits = defineEmits<{
  (event: 'change', data: OrganizationUnitDto): void;
}>();

const defaultModel = {
  displayName: '',
} as OrganizationUnitDto;

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      componentProps: {
        style: {
          display: 'none',
        },
      },
      fieldName: 'id',
    },
    {
      component: 'Input',
      componentProps: {
        style: {
          display: 'none',
        },
      },
      fieldName: 'parentId',
    },
    {
      component: 'Input',
      componentProps: {
        autocomplete: 'off',
      },
      fieldName: 'displayName',
      label: $t('AbpIdentity.OrganizationUnit:DisplayName'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});

const [Modal, modalApi] = useVbenModal({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen: boolean) {
    if (isOpen) {
      const { id, parentId } = modalApi.getData<Record<string, any>>();
      if (id) {
        modalApi.setState({ loading: true });
        try {
          const dto = await getApi(id);
          modalApi.setState({
            title: $t('AbpIdentity.OrganizationUnit', [dto.displayName]),
          });
          formApi.setValues(dto);
        } finally {
          modalApi.setState({ loading: false });
        }
        return;
      }
      formApi.setValues({
        ...defaultModel,
        parentId,
      });
      modalApi.setState({
        title: $t('AbpIdentity.OrganizationUnit:New'),
      });
    }
  },
  title: $t('AbpIdentity.OrganizationUnit:New'),
});

async function onSubmit(input: Record<string, any>) {
  const api = input.id
    ? updateApi(input.id, input as OrganizationUnitUpdateDto)
    : createApi(input as OrganizationUnitCreateDto);
  try {
    modalApi.setState({
      confirmLoading: true,
    });
    const dto = await api;
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({
      confirmLoading: false,
    });
  }
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
