import { MultiTenancySides } from '/@/api/permission-management/definitions/permissions/model';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization(['AbpPermissionManagement']);

export const multiTenancySidesMap = {
  [MultiTenancySides.Tenant]: L('MultiTenancySides:Tenant'),
  [MultiTenancySides.Host]: L('MultiTenancySides:Host'),
  [MultiTenancySides.Both]: L('MultiTenancySides:Both'),
};

export const multiTenancySides = [
  { label: multiTenancySidesMap[MultiTenancySides.Tenant], value: MultiTenancySides.Tenant },
  { label: multiTenancySidesMap[MultiTenancySides.Host], value: MultiTenancySides.Host },
  { label: multiTenancySidesMap[MultiTenancySides.Both], value: MultiTenancySides.Both },
];

export const providersMap = {
  'R': L('Providers:Role'),
  'U': L('Providers:User'),
  'O': L('Providers:OrganizationUnit'),
  'C': L('Providers:Client'),
};

export const providers = [
  { label: providersMap['R'], value: 'R' },
  { label: providersMap['U'], value: 'U' },
  { label: providersMap['O'], value: 'O' },
  { label: providersMap['C'], value: 'C' },
];
