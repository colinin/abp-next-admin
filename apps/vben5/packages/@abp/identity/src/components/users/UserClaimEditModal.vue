<script setup lang="ts">
import type { IdentityUserClaimDto } from '../../types/users';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getAssignableClaimsApi } from '../../api/claim-types';
import { createClaimApi, updateClaimApi } from '../../api/users';

interface IdentityUserClaimVto extends IdentityUserClaimDto {
  userId: string;
}

defineOptions({
  name: 'UserClaimEditModal',
});

const emits = defineEmits<{
  (event: 'change', data: IdentityUserClaimDto): void;
}>();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Select',
      fieldName: 'claimType',
      label: $t('AbpIdentity.DisplayName:ClaimType'),
      rules: 'required',
    },
    {
      component: 'Textarea',
      fieldName: 'claimValue',
      label: $t('AbpIdentity.DisplayName:ClaimValue'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen: boolean) {
    if (isOpen) {
      try {
        modalApi.setState({ loading: true });
        const claimVto = modalApi.getData<IdentityUserClaimVto>();
        formApi.setValues({
          ...claimVto,
          newClaimValue: claimVto.claimValue,
        });
        // 新增可选, 修改不可选
        formApi.updateSchema([
          {
            disabled: !!claimVto.id,
            fieldName: 'claimType',
          },
        ]);
        // 新增时初始化可选声明
        !claimVto.id && (await initAssignableClaims());
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpIdentity.ManageClaim'),
});
/** 初始化可用声明类型 */
async function initAssignableClaims() {
  const { items } = await getAssignableClaimsApi();
  formApi.updateSchema([
    {
      componentProps: {
        fieldNames: {
          label: 'name',
          value: 'name',
        },
        options: items,
      },
      fieldName: 'claimType',
    },
  ]);
}
/** 提交声明类型变更 */
async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ confirmLoading: true });
    const claimVto = modalApi.getData<IdentityUserClaimVto>();
    const api = claimVto.id
      ? updateClaimApi(claimVto.userId, {
          claimType: claimVto.claimType,
          claimValue: claimVto.claimValue,
          newClaimValue: values.claimValue,
        })
      : createClaimApi(claimVto.userId, {
          claimType: values.claimType,
          claimValue: values.claimValue,
        });
    await api;
    emits('change', values as IdentityUserClaimDto);
    modalApi.close();
  } finally {
    modalApi.setState({ confirmLoading: false });
  }
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
