import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { getAll as getAllLayout } from '/@/api/platform/layout';
import { getByName as getDataByName } from '/@/api/platform/dataDic';

const { L } = useLocalization('AppPlatform');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('DisplayName:Filter'),
        colProps: { span: 8 },
      },
      {
        field: 'framework',
        component: 'ApiSelect',
        label: L('DisplayName:UIFramework'),
        colProps: { span: 8 },
        componentProps: {
          api: () => getDataByName('UI Framework'),
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'name',
        },
      },
      {
        field: 'layoutId',
        component: 'ApiSelect',
        label: L('DisplayName:Layout'),
        colProps: { span: 8 },
        componentProps: {
          api: () => getAllLayout(),
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'id',
        },
      },
    ],
  };
}
