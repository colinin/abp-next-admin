<script setup lang="ts">
import type { MenuSubject } from './types';

import { ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { listToTree } from '@abp/core';
import { message } from 'ant-design-vue';

import {
  useDataDictionariesApi,
  useMenusApi,
  useRoleMenusApi,
  useUserMenusApi,
} from '../../api';

interface ModalState {
  identity: string;
}

defineOptions({
  name: 'MenuAllotModal',
});

const props = defineProps<{
  subject: MenuSubject;
}>();

const checkedMenuIds = ref<string[]>([]);

const { getAllApi: getAllMenusApi } = useMenusApi();
const { getAllApi: getUserMenusApi, setMenusApi: setUserMenusApi } =
  useUserMenusApi();
const { getAllApi: getRoleMenusApi, setMenusApi: setRoleMenusApi } =
  useRoleMenusApi();
const { getByNameApi: getDataDictionaryByNameApi } = useDataDictionariesApi();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'ApiSelect',
      componentProps: {
        allowClear: true,
        api: () => getDataDictionaryByNameApi('UI Framework'),
        labelField: 'displayName',
        onChange: onInitMenus,
        resultField: 'items',
        valueField: 'name',
      },
      dependencies: {
        if: (values) => {
          return !values?.id;
        },
        triggerFields: ['id'],
      },
      fieldName: 'framework',
      label: $t('AppPlatform.DisplayName:UIFramework'),
      rules: 'selectRequired',
    },
    {
      component: 'TreeSelect',
      componentProps: {
        allowClear: true,
        blockNode: true,
        fieldNames: {
          label: 'displayName',
          value: 'id',
        },
      },
      dependencies: {
        if(values) {
          return !!values.framework;
        },
        triggerFields: ['framework'],
      },
      fieldName: 'startupMenuId',
      label: $t('AppPlatform.Menu:SetStartup'),
    },
    {
      component: 'Tree',
      componentProps: {
        allowClear: true,
        blockNode: true,
        checkable: true,
        checkStrictly: true,
        fieldNames: {
          key: 'id',
          title: 'displayName',
        },
        onCheck: onMenuChecked,
      },
      dependencies: {
        if(values) {
          return !!values.framework;
        },
        triggerFields: ['framework'],
      },
      fieldName: 'menuIds',
      label: $t('AppPlatform.DisplayName:Menus'),
      modelPropName: 'checkedKeys',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
});

function onMenuChecked(checkedKeys: { checked: string[] }) {
  checkedMenuIds.value = checkedKeys.checked;
}

async function onInitMenus(framework?: string) {
  if (!framework) {
    formApi.updateSchema([
      {
        componentProps: {
          treeData: [],
        },
        fieldName: 'menuIds',
      },
      {
        componentProps: {
          treeData: [],
        },
        fieldName: 'startupMenuId',
      },
    ]);
    formApi.setFieldValue('menuIds', []);
    return;
  }
  const state = modalApi.getData<ModalState>();
  const api =
    props.subject === 'user'
      ? getUserMenusApi({
          framework,
          userId: state.identity,
        })
      : getRoleMenusApi({
          framework,
          role: state.identity,
        });
  const [allMenus, identityMenus] = await Promise.all([
    getAllMenusApi({
      framework,
    }),
    api,
  ]);
  const treeData = listToTree(allMenus.items, {
    id: 'id',
    pid: 'parentId',
  });
  formApi.updateSchema([
    {
      componentProps: {
        treeData,
      },
      fieldName: 'menuIds',
    },
    {
      componentProps: {
        treeData,
      },
      fieldName: 'startupMenuId',
    },
  ]);
  checkedMenuIds.value = identityMenus.items.map((item) => item.id);
  formApi.setFieldValue('menuIds', checkedMenuIds.value);
  const startupMenu = identityMenus.items.find((item) => item.startup);
  formApi.setFieldValue('startupMenuId', startupMenu?.id);
}

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    const state = modalApi.getData<ModalState>();
    const api =
      props.subject === 'user'
        ? setUserMenusApi({
            framework: values.framework,
            menuIds: checkedMenuIds.value,
            startupMenuId: values.startupMenuId,
            userId: state.identity,
          })
        : setRoleMenusApi({
            framework: values.framework,
            menuIds: checkedMenuIds.value,
            roleName: state.identity,
            startupMenuId: values.startupMenuId,
          });
    await api;
    message.success($t('AbpUi.SavedSuccessfully'));
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('AppPlatform.Menu:Manage')">
    <Form />
  </Modal>
</template>

<style scoped></style>
