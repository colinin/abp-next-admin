<script setup lang="ts">
import { toRaw } from 'vue';

import { useVbenForm, useVbenModal, z } from '@vben/common-ui';
import { $t } from '@vben/locales';

interface ModalState {
  options: { disabled?: boolean; label: string; value: string }[];
  record: any;
}

const emits = defineEmits(['change']);
const [Modal, modalApi] = useVbenModal({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  onConfirm: onSubmit,
  onOpenChange(isOpen) {
    if (isOpen) {
      onInit();
    }
  },
});

const [Form, formApi] = useVbenForm({
  schema: [
    {
      component: 'Select',
      fieldName: 'name',
      label: $t('component.simple_state_checking.form.name'),
      rules: 'selectRequired',
    },
    {
      component: 'Empty',
      disabledOnChangeListener: false,
      fieldName: 'value',
      label: '',
    },
  ],
  showDefaultActions: false,
});

async function onInit() {
  const state = modalApi.getData<ModalState>();
  formApi.updateSchema([
    {
      componentProps: {
        disabled: !!state.record,
        onChange,
        options: state.options,
      },
      fieldName: 'name',
    },
    {
      component: 'Empty',
      fieldName: 'value',
      label: '',
      rules: z.object({}).optional(),
    },
  ]);
  formApi.setValues({
    name: state.record?.name,
    value: {},
  });
  if (state.record) {
    onRecordChange(state.record);
  }
}

async function onRecordChange(record: any) {
  await formApi.setFieldValue('name', record.name);
  onChange(record.name);
  switch (record.name) {
    case 'F': {
      await formApi.setFieldValue('value', {
        featureNames: record.featureNames,
        requiresAll: record.requiresAll,
      });
      break;
    }
    case 'G': {
      await formApi.setFieldValue('value', {
        globalFeatureNames: record.globalFeatureNames,
        requiresAll: record.requiresAll,
      });
      break;
    }
    case 'P': {
      await formApi.setFieldValue('value', {
        permissions: record.permissions,
        requiresAll: record.requiresAll,
      });
      break;
    }
  }
}

function onChange(value: string) {
  formApi.updateSchema([
    {
      component: 'Empty',
      controlClass: '',
      fieldName: 'value',
      label: '',
      rules: z.object({}).optional(),
    },
  ]);
  formApi.setFieldValue('value', {});
  switch (value) {
    case 'A': {
      formApi.updateSchema([
        {
          controlClass: 'invisible',
          fieldName: 'value',
        },
      ]);
      break;
    }
    case 'F': {
      formApi.updateSchema([
        {
          component: 'FeatureStateCheck',
          fieldName: 'value',
          label: $t(
            'component.simple_state_checking.requireFeatures.featureNames',
          ),
          // TODO: vben应该做到用户能自定义验证器, 在组件的适配器中而不是单独使用时
          rules: z
            .object({
              featureNames: z.array(z.string()),
              requiresAll: z.boolean().optional(),
            })
            .refine((v) => v.featureNames.length > 0, {
              message: `${$t('ui.placeholder.select')}${$t('component.simple_state_checking.requireFeatures.featureNames')}`,
            }),
        },
      ]);
      formApi.setFieldValue('value', {
        featureNames: [],
        requiresAll: false,
      });
      break;
    }
    case 'G': {
      formApi.updateSchema([
        {
          component: 'GlobalFeatureStateCheck',
          fieldName: 'value',
          label: $t(
            'component.simple_state_checking.requireFeatures.featureNames',
          ),
          // TODO: vben应该做到用户能自定义验证器, 在组件的适配器中而不是单独使用时
          rules: z
            .object({
              globalFeatureNames: z.array(z.string()),
              requiresAll: z.boolean().optional(),
            })
            .refine((v) => v.globalFeatureNames.length > 0, {
              message: `${$t('ui.placeholder.select')}${$t('component.simple_state_checking.requireFeatures.featureNames')}`,
            }),
        },
      ]);
      formApi.setFieldValue('value', {
        globalFeatureNames: [],
        requiresAll: false,
      });
      break;
    }
    case 'P': {
      formApi.updateSchema([
        {
          component: 'PermissionStateCheck',
          fieldName: 'value',
          label: $t(
            'component.simple_state_checking.requirePermissions.permissions',
          ),
          // TODO: vben应该做到用户能自定义验证器, 在组件的适配器中而不是单独使用时
          rules: z
            .object({
              permissions: z.array(z.string()),
              requiresAll: z.boolean().optional(),
            })
            .refine((v) => v.permissions.length > 0, {
              message: `${$t('ui.placeholder.select')}${$t('component.simple_state_checking.requirePermissions.permissions')}`,
            }),
        },
      ]);
      formApi.setFieldValue('value', {
        permissions: [],
        requiresAll: false,
      });
      break;
    }
  }
}

async function onSubmit() {
  const result = await formApi.validate();
  if (result.valid) {
    const values = await formApi.getValues();
    const value = toRaw(values.value);
    const stateChecker: { [key: string]: any } = {
      A: value.requiresAll,
      T: values.name,
    };
    switch (values.name) {
      case 'F': {
        stateChecker.N = value.featureNames;
        break;
      }
      case 'G': {
        stateChecker.N = value.globalFeatureNames;
        break;
      }
      case 'P': {
        stateChecker.N = value.permissions;
        break;
      }
    }
    emits('change', stateChecker);
    modalApi.close();
  }
}
</script>

<template>
  <Modal :title="$t('component.simple_state_checking.title')">
    <Form />
  </Modal>
</template>

<style scoped></style>
