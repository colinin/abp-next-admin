import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';

const { L } = useLocalization('AbpIdentityServer');
export function getFormSchemas(): FormSchema[] {
  return [
    {
      field: 'key',
      component: 'Input',
      label: L('Propertites:Key'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'value',
      component: 'Input',
      label: L('Propertites:Value'),
      colProps: { span: 24 },
      required: true,
    },
  ];
}
