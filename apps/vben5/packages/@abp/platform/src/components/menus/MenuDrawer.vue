<script setup lang="ts">
import type { MenuDto } from '../../types';
import type { DataItemDto } from '../../types/dataDictionaries';

import { ref } from 'vue';

import { useVbenDrawer, useVbenForm } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  formatToDate,
  formatToDateTime,
  isNullOrWhiteSpace,
  listToTree,
} from '@abp/core';
import { Button, Card, message, Steps } from 'ant-design-vue';

import { useMenusApi } from '../../api';
import { useDataDictionariesApi } from '../../api/useDataDictionariesApi';
import { useLayoutsApi } from '../../api/useLayoutsApi';
import { ValueType } from '../../types/dataDictionaries';

const emits = defineEmits<{
  (event: 'change', data: MenuDto): void;
}>();

const Step = Steps.Step;

type TabKey = 'basic' | 'meta';
type RenderComponentContentType = (
  value: Partial<Record<string, any>>,
) => Record<string, any>;

const currentStep = ref(0);
const submiting = ref(false);
const showMetaForm = ref(false);
const parentMenu = ref<MenuDto>();
const menuMetas = ref<DataItemDto[]>([]);
const activeTabKey = ref<TabKey>('basic');

const { createApi, getAllApi, getApi, updateApi } = useMenusApi();
const { getApi: getLayoutApi, getPagedListApi: getLayoutsApi } =
  useLayoutsApi();
const { getApi: getDataDictionaryApi } = useDataDictionariesApi();

const [BasicForm, basicFormApi] = useVbenForm({
  commonConfig: {
    colon: true,
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onNextStep,
  schema: [
    {
      component: 'Input',
      dependencies: {
        show: false,
        triggerFields: ['name'],
      },
      fieldName: 'id',
    },
    {
      component: 'ApiSelect',
      componentProps: {
        allowClear: true,
        api: getLayoutsApi,
        labelField: 'displayName',
        onChange: onLayoutChange,
        resultField: 'items',
        valueField: 'id',
      },
      fieldName: 'layoutId',
      label: $t('AppPlatform.DisplayName:Layout'),
      rules: 'selectRequired',
    },
    {
      component: 'Checkbox',
      fieldName: 'isPublic',
      label: $t('AppPlatform.DisplayName:IsPublic'),
      renderComponentContent: () => {
        return {
          default: () => [$t('AppPlatform.DisplayName:IsPublic')],
        };
      },
    },
    {
      component: 'ApiTreeSelect',
      componentProps: {
        allowClear: true,
        api: async () => {
          const { items } = await getAllApi();
          return listToTree(items, {
            id: 'id',
            pid: 'parentId',
          });
        },
        labelField: 'displayName',
        onChange: onParentIdChange,
        valueField: 'id',
        childrenField: 'children',
      },
      fieldName: 'parentId',
      label: $t('AppPlatform.DisplayName:ParentMenu'),
    },
    {
      component: 'Input',
      fieldName: 'name',
      label: $t('AppPlatform.DisplayName:Name'),
      rules: 'required',
    },
    {
      component: 'Input',
      fieldName: 'displayName',
      label: $t('AppPlatform.DisplayName:DisplayName'),
      rules: 'required',
    },
    {
      component: 'Input',
      fieldName: 'path',
      label: $t('AppPlatform.DisplayName:Path'),
      rules: 'required',
    },
    {
      component: 'Input',
      fieldName: 'component',
      label: $t('AppPlatform.DisplayName:Component'),
      rules: 'required',
    },
    {
      component: 'Input',
      fieldName: 'redirect',
      label: $t('AppPlatform.DisplayName:Redirect'),
    },
    {
      component: 'Textarea',
      componentProps: {
        autoSize: {
          minRows: 3,
        },
      },
      fieldName: 'description',
      label: $t('AppPlatform.DisplayName:Description'),
    },
  ],
  showDefaultActions: false,
  submitButtonOptions: {
    content: $t('AppPlatform.NextStep'),
  },
});
const [MetaForm, metaFormApi] = useVbenForm({
  commonConfig: {
    colon: true,
    componentProps: {
      class: 'w-full',
    },
    labelWidth: 150,
  },
  handleSubmit: onSubmit,
  resetButtonOptions: {
    content: $t('AppPlatform.PreStep'),
  },
  schema: [],
  showDefaultActions: false,
});
const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-2/3',
  onConfirm: onSubmit,
  onOpenChange: async (isOpen) => {
    if (isOpen) {
      await onInit();
    }
  },
});

function onPreStep() {
  currentStep.value = 0;
}

async function onNextStep() {
  currentStep.value = 1;
}

async function onInit() {
  metaFormApi.removeSchemaByFields(menuMetas.value.map((x) => x.name));
  activeTabKey.value = 'basic';
  parentMenu.value = undefined;
  showMetaForm.value = false;
  submiting.value = false;
  menuMetas.value = [];
  currentStep.value = 0;
  const { id, layoutId, parentId } = drawerApi.getData<MenuDto>();
  if (!isNullOrWhiteSpace(layoutId)) {
    await onLayoutChange(layoutId);
    await basicFormApi.setFieldValue('layoutId', layoutId);
  }
  if (isNullOrWhiteSpace(id)) {
    await basicFormApi.setFieldValue('parentId', parentId);
    await onParentIdChange(parentId);
    drawerApi.setState({ title: $t('AppPlatform.Menu:AddNew') });
    return;
  }
  const dto = await getApi(id);
  await basicFormApi.setValues(dto);
  // 编辑模式无需使用前缀
  await onParentIdChange(undefined);
  onInitMetaFormValues(dto.meta);
  drawerApi.setState({ title: `${$t('AppPlatform.Menu:Edit')} - ${dto.name}` });
}

async function onParentIdChange(menuId?: string) {
  basicFormApi.updateSchema([
    {
      componentProps: {
        addonBefore: '',
      },
      fieldName: 'path',
    },
  ]);
  if (menuId) {
    const parentMenuDto = await getApi(menuId);
    parentMenu.value = parentMenuDto;
    basicFormApi.updateSchema([
      {
        componentProps: {
          addonBefore: parentMenuDto.path,
        },
        fieldName: 'path',
      },
    ]);
  }
}

async function onLayoutChange(layoutId?: string) {
  metaFormApi.removeSchemaByFields(menuMetas.value.map((x) => x.name));
  if (!layoutId) {
    showMetaForm.value = false;
    menuMetas.value = [];
    return;
  }
  try {
    drawerApi.setState({ loading: true });
    const layoutDto = await getLayoutApi(layoutId);
    const dataDto = await getDataDictionaryApi(layoutDto.dataId);
    basicFormApi.setFieldValue('component', layoutDto.path);
    menuMetas.value = dataDto.items.sort();
    onInitMetaFormSchemas();
  } finally {
    drawerApi.setState({ loading: false });
  }
}

function onInitMetaFormSchemas() {
  metaFormApi.removeSchemaByFields(menuMetas.value.map((x) => x.name));
  const metaValues: Record<string, any> = {};
  menuMetas.value.forEach((dataItem) => {
    metaFormApi.setState((pre) => {
      let component = 'Input';
      let defaultValue: any | undefined;
      let componentProps: Record<string, any> | undefined;
      let renderComponentContent: RenderComponentContentType | undefined;
      switch (dataItem.valueType) {
        case ValueType.Array: {
          component = 'Select';
          componentProps = {
            mode: 'tags',
          };
          if (!isNullOrWhiteSpace(dataItem.defaultValue)) {
            defaultValue = dataItem.defaultValue.split(',');
          }
          break;
        }
        case ValueType.Boolean: {
          component = 'Checkbox';
          defaultValue = dataItem.defaultValue === 'true';
          renderComponentContent = () => {
            return {
              default: () => [dataItem.displayName],
            };
          };
          break;
        }
        case ValueType.Date:
        case ValueType.DateTime: {
          component = 'DatePicker';
          defaultValue = dataItem.defaultValue;
          componentProps = {
            showTime: dataItem.valueType === ValueType.DateTime,
            valueFormat:
              dataItem.valueType === ValueType.DateTime
                ? 'YYYY-MM-DD HH:mm:ss'
                : 'YYYY-MM-DD',
          };
          break;
        }
        case ValueType.Numeic: {
          component = 'InputNumber';
          defaultValue = dataItem.defaultValue
            ? Number(dataItem.defaultValue)
            : undefined;
          break;
        }
        case ValueType.String: {
          // 约定名称包含icon时使用IconPicker组件
          const itemNameLowerCase = dataItem.name.toLocaleLowerCase();
          if (itemNameLowerCase.includes('icon')) {
            component = 'IconPicker';
            componentProps = {
              style: {
                width: '100%',
              },
            };
          } else {
            componentProps = {
              autocomplete: 'off',
            };
          }
          defaultValue = dataItem.defaultValue;
          break;
        }
      }
      metaValues[dataItem.name] = defaultValue;
      return {
        schema: [
          ...(pre?.schema ?? []),
          {
            component,
            componentProps,
            fieldName: dataItem.name,
            help: dataItem.description,
            label: dataItem.displayName,
            renderComponentContent,
            rules: dataItem.allowBeNull ? undefined : 'required',
          },
        ],
      };
    });
  });
  metaFormApi.setValues(metaValues);
  showMetaForm.value = true;
}

function onInitMetaFormValues(meta?: Record<string, any>) {
  metaFormApi.resetForm();
  if (!meta) {
    return;
  }
  const values: Record<string, any> = {};
  menuMetas.value.forEach((dataItem) => {
    const metaValue = meta[dataItem.name];
    if (isNullOrWhiteSpace(metaValue)) {
      return;
    }
    switch (dataItem.valueType) {
      case ValueType.Array: {
        const metaValueArr = String(metaValue);
        values[dataItem.name] = metaValueArr.split(',');
        break;
      }
      case ValueType.Boolean: {
        values[dataItem.name] = String(metaValue) === 'true';
        break;
      }
      case ValueType.Date: {
        values[dataItem.name] = formatToDate(metaValue);
        break;
      }
      case ValueType.DateTime: {
        values[dataItem.name] = formatToDateTime(metaValue);
        break;
      }
      case ValueType.Numeic: {
        values[dataItem.name] = Number(metaValue);
        break;
      }
      case ValueType.String: {
        values[dataItem.name] = String(metaValue);
        break;
      }
    }
  });
  metaFormApi.setValues(values);
}

async function onSubmit() {
  try {
    submiting.value = true;
    drawerApi.setState({ loading: true });
    const basicInput = await basicFormApi.getValues();
    const metaInput = await metaFormApi.getValues();
    let combPath = String(basicInput.path);
    if (!combPath.startsWith('/')) {
      combPath = `/${combPath}`;
    }
    if (parentMenu.value && !combPath.startsWith(parentMenu.value.path)) {
      combPath = `${parentMenu.value?.path ?? ''}${combPath}`;
    }
    const api = basicInput.id
      ? updateApi(basicInput.id, {
          component: basicInput.component,
          description: basicInput.description,
          displayName: basicInput.displayName,
          isPublic: basicInput.isPublic,
          meta: metaInput,
          name: basicInput.name,
          parentId: basicInput.parentId,
          path: combPath,
          redirect: basicInput.redirect,
        })
      : createApi({
          component: basicInput.component,
          description: basicInput.description,
          displayName: basicInput.displayName,
          isPublic: basicInput.isPublic,
          layoutId: basicInput.layoutId,
          meta: metaInput,
          name: basicInput.name,
          parentId: basicInput.parentId,
          path: combPath,
          redirect: basicInput.redirect,
        });
    const dto = await api;
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    drawerApi.close();
  } finally {
    submiting.value = false;
    drawerApi.setState({ loading: false });
  }
}
</script>

<template>
  <Drawer>
    <Card>
      <div class="mx-auto max-w-lg">
        <Steps :current="currentStep">
          <Step :title="$t('AppPlatform.DisplayName:Basic')" />
          <Step :title="$t('AppPlatform.DisplayName:Meta')" />
        </Steps>
      </div>
      <div class="p-20">
        <BasicForm v-show="currentStep === 0" />
        <MetaForm v-show="currentStep === 1" />
      </div>
    </Card>

    <template #footer>
      <Button v-if="currentStep === 1" @click="onPreStep">
        {{ $t('AppPlatform.PreStep') }}
      </Button>
      <Button
        v-if="currentStep === 0"
        type="primary"
        @click="basicFormApi.validateAndSubmitForm"
        :loading="submiting"
      >
        {{ $t('AppPlatform.NextStep') }}
      </Button>
      <Button
        v-if="currentStep === 1"
        type="primary"
        @click="metaFormApi.validateAndSubmitForm"
        :loading="submiting"
      >
        {{ $t('AbpUi.Submit') }}
      </Button>
    </template>
  </Drawer>
</template>

<style scoped lang="scss">
:deep(.grid-cols-1) {
  .flex-shrink-0 {
    .relative {
      button {
        width: 100%;
      }
    }
  }
}
</style>
