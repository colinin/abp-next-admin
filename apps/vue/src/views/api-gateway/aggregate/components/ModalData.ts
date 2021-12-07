import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';
import { getActivedList } from '/@/api/api-gateway/group';
import { getDefinedAggregatorProviders } from '/@/api/api-gateway/basic';

const { L } = useLocalization('ApiGateway');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'appId',
        component: 'ApiSelect',
        label: L('DisplayName:AppId'),
        colProps: { span: 12 },
        required: true,
        componentProps: {
          api: () => getActivedList(),
          resultField: 'items',
          labelField: 'appName',
          valueField: 'appId',
        },
      },
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 12 },
        defaultValue: '',
      },
    ],
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'reRouteId',
      component: 'Input',
      label: 'reRouteId',
      colProps: { span: 24 },
      show: false,
    },
    {
      field: 'concurrencyStamp',
      component: 'Input',
      label: 'concurrencyStamp',
      colProps: { span: 24 },
      show: false,
    },
    {
      field: 'appId',
      component: 'ApiSelect',
      label: L('DisplayName:AppId'),
      colProps: { span: 24 },
      required: true,
      componentProps: {
        api: () => getActivedList(),
        resultField: 'items',
        labelField: 'appName',
        valueField: 'appId',
      },
      dynamicDisabled: ({ values }) => {
        return values.reRouteId !== undefined;
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
      field: 'aggregator',
      component: 'ApiSelect',
      label: L('DisplayName:Aggregator'),
      colProps: { span: 24 },
      componentProps: {
        api: () => getDefinedAggregatorProviders(),
        resultField: 'items',
        labelField: 'provider',
        valueField: 'provider',
      },
    },
    {
      field: 'reRouteKeys',
      component: 'Select',
      label: L('DisplayName:RouteKeys'),
      colProps: { span: 24 },
      required: true,
      componentProps: {
        mode: 'tags',
        open: false,
        tokenSeparators: [','],
      },
    },
    {
      field: 'priority',
      component: 'InputNumber',
      label: L('DisplayName:Priority'),
      colProps: { span: 24 },
      componentProps: {
        style: {
          width: '100%',
        },
      },
    },
    {
      field: 'upstreamHost',
      component: 'Input',
      label: L('DisplayName:UpstreamHost'),
      colProps: { span: 24 },
    },
    {
      field: 'reRouteIsCaseSensitive',
      component: 'Checkbox',
      label: L('DisplayName:CaseSensitive'),
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('DisplayName:CaseSensitive'),
    },
    {
      field: 'upstreamPathTemplate',
      component: 'Input',
      label: L('DisplayName:UpstreamPathTemplate'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'upstreamHttpMethod',
      component: 'Select',
      label: L('DisplayName:UpstreamHttpMethod'),
      colProps: { span: 24 },
      componentProps: {
        mode: 'tags',
        open: false,
        tokenSeparators: [','],
      },
    },
  ];
}

export function getConfigModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'routeId',
      component: 'Input',
      label: 'routeId',
      colProps: { span: 24 },
      show: false,
    },
    {
      field: 'reRouteKey',
      component: 'Input',
      label: L('DisplayName:RouteKey'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'parameter',
      component: 'Input',
      label: L('DisplayName:Parameter'),
      colProps: { span: 24 },
    },
    {
      field: 'jsonPath',
      component: 'Input',
      label: L('DisplayName:JsonPath'),
      colProps: { span: 24 },
    },
  ];
}
