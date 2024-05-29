import type { Ref } from 'vue';
import type { TabFormSchema, TabFormActionType } from '/@/components/Form/src/types/form';

import { unref, computed, watch, createVNode } from 'vue';
import { Checkbox } from 'ant-design-vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { cloneDeep } from 'lodash-es';
import { get as getLayout, getAll as getAllLayout } from '/@/api/platform/layouts';
import { get as getData } from '/@/api/platform/datas';
import { getAll as getAllMenu, create, update } from '/@/api/platform/menus';
import { DataItem, ValueType } from '/@/api/platform/datas/model';
import { listToTree } from '/@/utils/helper/treeHelper';
import { Menu, UpdateMenu, CreateMenu } from '/@/api/platform/menus/model';

interface UseMenuFormContext {
  menuModel: Ref<Menu>;
  formElRef: Ref<Nullable<TabFormActionType>>;
  framework: Ref<string | undefined>,
}

export function useMenuFormContext({ menuModel, formElRef, framework }: UseMenuFormContext) {
  const { L } = useLocalization('AppPlatform');

  function getMetaFormSchemas(meta: DataItem[]): TabFormSchema[] {
    return meta.sort((pre, next) => pre.name.localeCompare(next.name))
    .map((item) => {
      const schema: TabFormSchema = {
        tab: L('DisplayName:Meta'),
        field: 'meta.'.concat(item.name),
        label: item.displayName,
        colProps: { span: 24 },
        required: !item.allowBeNull,
        component: 'Input',
        componentProps: {
          style: {
            width: '100%',
          },
        }
      };
      switch (item.valueType) {
        case ValueType.Boolean:
          schema.component = 'Checkbox';
          let checked = item.defaultValue === 'true';
          schema.render = ({ model, field }) => {
            if (model[field]) {
              checked = model[field] === 'true';
            }
            return createVNode(Checkbox, {
              checked: checked,
              value: item.displayName,
              onChange: (e: ChangeEvent) => {
                model[field] = e.target.checked.toString();
              },
            }, () => item.displayName);
          };
          break;
        case ValueType.Date:
          schema.component = 'DatePicker';
          if (item.defaultValue) {
            schema.defaultValue = new Date(item.defaultValue);
          }
          break;
        case ValueType.DateTime:
          schema.component = 'TimePicker';
          if (item.defaultValue) {
            schema.defaultValue = new Date(item.defaultValue);
          }
          break;
        case ValueType.Numeic:
          schema.component = 'InputNumber';
          if (item.defaultValue) {
            schema.defaultValue = Number(item.defaultValue);
          }
          break;
        case ValueType.Array:
          schema.component = 'Input';
          schema.componentProps = {
            placeholder: item.description,
          };
          break;
        default:
        case ValueType.String:
        case ValueType.Object:
          if (item.name === 'icon') {
            schema.component = 'IconPicker';
            schema.componentProps = {
              onlyDefineIcons: false,
              style: {
                width: '100%',
              },
            };
          } else {
            schema.component = 'Input';
            schema.componentProps = {
              placeholder: item.description,
            };
          }
          schema.defaultValue = item.defaultValue;
          break;
      }
      return schema;
    });
  }

  function getBasicFormSchemas(): TabFormSchema[] {
    return [
      {
        tab: L('DisplayName:Basic'),
        field: 'id',
        component: 'Input',
        label: 'id',
        colProps: { span: 24 },
        ifShow: false,
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'layoutId',
        component: 'ApiSelect',
        label: L('DisplayName:Layout'),
        colProps: { span: 24 },
        required: true,
        componentProps: {
          api: getAllLayout,
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'id',
          onChange(val) {
            fetchLayoutResource(val);
          },
          // onOptionsChange() {
          //   const menu = unref(menuModel);
          //   fetchLayoutResource(menu.layoutId);
          // },
        },
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'isPublic',
        component: 'Checkbox',
        label: L('DisplayName:IsPublic'),
        colProps: { span: 24 },
        renderComponentContent: L('DisplayName:IsPublic'),
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'parentId',
        label: L('DisplayName:ParentMenu'),
        component: 'TreeSelect',
        colProps: { span: 24 },
        componentProps: {
          fieldNames: {
            label: 'displayName',
            key: 'id',
            value: 'id',
          },
          getPopupContainer: () => document.body,
        },
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'name',
        component: 'Input',
        label: L('DisplayName:Name'),
        colProps: { span: 24 },
        required: true,
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'displayName',
        component: 'Input',
        label: L('DisplayName:DisplayName'),
        colProps: { span: 24 },
        required: true,
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'path',
        component: 'Input',
        label: L('DisplayName:Path'),
        colProps: { span: 24 },
        required: true,
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'component',
        component: 'Input',
        label: L('DisplayName:Component'),
        colProps: { span: 24 },
        required: true,
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'redirect',
        component: 'Input',
        label: L('DisplayName:Redirect'),
        colProps: { span: 24 },
      },
      {
        tab: L('DisplayName:Basic'),
        field: 'description',
        component: 'InputTextArea',
        label: L('DisplayName:Description'),
        colProps: { span: 24 },
      },
    ];
  }

  const getFormSchemas = computed((): TabFormSchema[] => {
    return [...getBasicFormSchemas()];
  });

  const formTitle = computed((): string => {
    const menu = unref(menuModel);
    if (menu.id) {
      return L('Menu:EditByName', [menu.displayName] as Recordable);
    }
    return L('Menu:AddNew');
  });

  function removeAllMetaSchemas() {
    const tabKey = L('DisplayName:Meta');
    const formEl = unref(formElRef);
    const schemas = unref(getFormSchemas);
    const metaSchemas= schemas.filter((x) => x.tab === tabKey);
    metaSchemas.forEach((x) => {
      formEl?.removeSchemaByField(x.field);
      const index = schemas.findIndex((s) => s.field === x.field);
      if (index) {
        schemas.splice(index, 1);
      }
    });
  }

  function removeAllParentMenus() {
    const formEl = unref(formElRef);
    formEl?.updateSchema({
      field: 'parentId',
      componentProps: { treeData: [] },
    });
  }

  async function fetchLayoutResource(layoutId?: string) {
    removeAllMetaSchemas();
    if (layoutId) {
      await warpParentRootMenu(layoutId);
      const formEl = unref(formElRef);
      const layout = await getLayout(layoutId);
      const { items } = await getData(layout.dataId);
      const metaSchemas = getMetaFormSchemas(items);
      const schemas = unref(getFormSchemas);
      metaSchemas.forEach((x) => {
        schemas.push(x);
        formEl?.appendSchemaByField(x, '');
      });
      formEl?.setFieldsValue({
        ...unref(menuModel),
        layoutId: layoutId,
      });
    }
  }

  async function warpParentRootMenu(layoutId?: string) {
    removeAllParentMenus();
    const formEl = unref(formElRef);
    const { items } = await getAllMenu({
      filter: '',
      sorting: 'name',
      framework: framework.value ?? '',
      layoutId: layoutId,
    });
    const treeData = listToTree(items, { id: 'id', pid: 'parentId' });
    const menu = unref(menuModel);
    formEl?.updateSchema({
      field: 'parentId',
      defaultValue: menu.parentId,
      componentProps: { treeData },
    });
  }

  function handleFormSubmit() {
    const formEl = unref(formElRef);
    return formEl?.validate().then(() => {
      const model = unref(menuModel);
      const input = formEl?.getFieldsValue();
      return model.id
        ? update(model.id, cloneDeep(input) as UpdateMenu)
        : create(cloneDeep(input) as CreateMenu);
    });
  }

  watch(
    () => unref(menuModel),
    (model) => {
      const formEl = unref(formElRef);
      formEl?.resetFields();
      formEl?.setFieldsValue(model);
    },
    { immediate: true },
  );

  return {
    formTitle,
    getFormSchemas,
    handleFormSubmit,
    fetchLayoutResource,
  };
}
