import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';
import { HashType } from '/@/api/identity-server/model/apiResourcesModel';

const { L } = useLocalization('AbpIdentityServer');

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

export function getSecretFormSchemas(): FormSchema[] {
  return [
    {
      field: 'hashType',
      component: 'Input',
      label: '',
      colProps: { span: 24 },
      required: true,
      ifShow: false,
      defaultValue: HashType.Sha256,
    },
    {
      field: 'type',
      component: 'Select',
      label: L('Secret:Type'),
      colProps: { span: 24 },
      required: true,
      componentProps: {
        options: [
          { label: 'JsonWebKey', value: 'JWK' },
          { label: 'SharedSecret', value: 'SharedSecret' },
          { label: 'X509CertificateName', value: 'X509Name' },
          { label: 'X509CertificateBase64', value: 'X509CertificateBase64' },
          { label: 'X509CertificateThumbprint', value: 'X509Thumbprint' },
        ],
      },
    },
    {
      field: 'value',
      component: 'Input',
      label: L('Secret:Value'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'description',
      component: 'Input',
      label: L('Description'),
      colProps: { span: 24 },
    },
    {
      field: 'expiration',
      component: 'DatePicker',
      label: L('Expiration'),
      colProps: { span: 24 },
      componentProps: {
        style: {
          width: '100%',
        },
      },
    },
  ];
}

export function getPropertyFormSchemas(): FormSchema[] {
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
