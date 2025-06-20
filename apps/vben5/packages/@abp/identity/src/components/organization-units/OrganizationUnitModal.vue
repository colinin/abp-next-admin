<script setup lang="ts">
import type {
  OrganizationUnitCreateDto,
  OrganizationUnitDto,
  OrganizationUnitUpdateDto,
} from '../../types/organization-units';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useOrganizationUnitsApi } from '../../api/useOrganizationUnitsApi';

defineOptions({
  name: 'OrganizationUnitModal',
});
const emits = defineEmits<{
  (event: 'change', data: OrganizationUnitDto): void;
}>();

const defaultModel = {
  displayName: '',
} as OrganizationUnitDto;

const { cancel, createApi, getApi, updateApi } = useOrganizationUnitsApi();
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
  onClosed() {
    cancel('Organization Unit Modal has closed!');
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
  try {
    modalApi.setState({
      submitting: true,
    });
    const dto = input.id
      ? await updateApi(input.id, input as OrganizationUnitUpdateDto)
      : await createApi(input as OrganizationUnitCreateDto);
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({
      submitting: false,
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
