import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization(['{{model.remote_service}}', 'AbpUi']);

export function getSearchFormProps(): Partial<FormProps> {
  return {
    {{~ if model.exists_search_models ~}}
    labelWidth: 100,
    schemas: [
      {{~ for searchModel in model.search_models ~}}
      {
        field: '{{ searchModel.name }}',
        component: '{{ searchModel.component }}',
        label: L('{{ searchModel.display_name }}'),
        colProps: {{ searchModel.col_props }},
      },
      {{~ end ~}}
    ],
    {{~ end ~}}
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {{~ for inputModel in model.input_models ~}}
    {
      field: '{{ inputModel.name }}',
      component: '{{ inputModel.component }}',
      label: L('{{ inputModel.display_name }}'),
      {{~ if !inputModel.show ~}}
      show: false,
      {{~ end ~}}
      {{~ if inputModel.disabled ~}}
      dynamicDisabled: true,
      {{~ end ~}}
      colProps: {{ inputModel.col_props }},
      componentProps: {{ inputModel.component_props }},
    },
    {{~ end ~}}
  ];
}
