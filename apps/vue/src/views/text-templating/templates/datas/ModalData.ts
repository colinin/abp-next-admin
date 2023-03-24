import { useAbpStoreWithOut } from '/@/store/modules/abp';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';
import { getList as getResources } from '/@/api/localization/resources';
import { getList as getLanguages } from '/@/api/localization/languages';
import { getList as getTexts } from '/@/api/localization/texts';
import { GetListAsyncByInput as getTemplateDefintions } from '/@/api/text-templating/definitions';

const abpStore = useAbpStoreWithOut();
const { L } = useLocalization(['AbpTextTemplating', 'AbpLocalization', 'AbpUi']);

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 24 },
      },
    ],
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'concurrencyStamp',
      component: 'Input',
      label: 'concurrencyStamp',
      colProps: { span: 24 },
      show: false,
    },
    {
      field: 'isInlineLocalized',
      component: 'Checkbox',
      label: L('DisplayName:IsInlineLocalized'),
      colProps: { span: 24 },
      defaultValue: false,
      renderComponentContent: L('DisplayName:IsInlineLocalized'),
    },
    {
      field: 'defaultCultureName',
      component: 'ApiSelect',
      label: L('DisplayName:DefaultCultureName'),
      colProps: { span: 24 },
      componentProps: {
        api: getLanguages,
        params: {
          input: {
            skipCount: 0,
            maxResultCount: 100,
          },
        },
        resultField: 'items',
        labelField: 'displayName',
        valueField: 'cultureName',
      },
    },
    {
      field: 'isLayout',
      component: 'Checkbox',
      label: L('DisplayName:IsLayout'),
      colProps: { span: 24 },
      defaultValue: false,
      renderComponentContent: L('DisplayName:IsLayout'),
      componentProps: ({ formActionType }) => {
        return {
          onChange: (e) => {
            if (e.target.checked) {
              formActionType.setFieldsValue({
                layout: '',
              });
            }
          }
        }
      },
    },
    {
      field: 'layout',
      component: 'ApiSelect',
      label: L('DisplayName:Layout'),
      colProps: { span: 24 },
      ifShow: ({ values }) => {
        return values.isLayout === false;
      },
      componentProps: {
        api: getTemplateDefintions,
        params: {
          input: {
            skipCount: 0,
            maxResultCount: 100,
            isLayout: true,
          },
        },
        resultField: 'items',
        labelField: 'displayName',
        valueField: 'name',
      },
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'resource',
      component: 'ApiSelect',
      label: L('DisplayName:ResourceName'),
      colProps: { span: 12 },
      required: true,
      componentProps: ({ formActionType }) => {
        return {
          api: getResources,
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'name',
          showSearch: true,
          filterOption: (input: string, option: any) => {
            return option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0;
          },
          onChange: (val) => {
            formActionType.setFieldsValue({
              text: '',
            });
            getTexts({
              cultureName: 'en',
              targetCultureName: abpStore.getApplication.localization.currentCulture.cultureName,
              resourceName: val,
              filter: '',
              onlyNull: false,
            }).then((res) => {
              formActionType.updateSchema({
                field: 'text',
                componentProps: {
                  options: res.items.map((item) => {
                    return {
                      label: item.targetValue,
                      value: item.key,
                    }
                  }),
                }
              });
            });
          },
          getPopupContainer: (triggerNode) => triggerNode.parentNode,
          style: {
            width: '100%',
          },
        }
      },
    },
    {
      field: 'text',
      component: 'Select',
      label: L('DisplayName:DisplayName'),
      colProps: { span: 12 },
      required: true,
      componentProps: {
        showSearch: true,
        filterOption: (input: string, option: any) => {
          return option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0;
        },
        style: {
          width: '100%',
        },
      },
    },
  ];
}
