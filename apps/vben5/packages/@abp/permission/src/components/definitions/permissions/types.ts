import { $t } from '@vben/locales';

import { MultiTenancySides } from '../../../types/definitions';

export function useTypesMap() {
  const multiTenancySidesMap: { [key: number]: string } = {
    [MultiTenancySides.Both]: $t(
      'AbpPermissionManagement.MultiTenancySides:Both',
    ),
    [MultiTenancySides.Host]: $t(
      'AbpPermissionManagement.MultiTenancySides:Host',
    ),
    [MultiTenancySides.Tenant]: $t(
      'AbpPermissionManagement.MultiTenancySides:Tenant',
    ),
  };

  const multiTenancySideOptions = [
    {
      label: multiTenancySidesMap[MultiTenancySides.Tenant],
      value: MultiTenancySides.Tenant,
    },
    {
      label: multiTenancySidesMap[MultiTenancySides.Host],
      value: MultiTenancySides.Host,
    },
    {
      label: multiTenancySidesMap[MultiTenancySides.Both],
      value: MultiTenancySides.Both,
    },
  ];

  const providersMap: { [key: string]: string } = {
    C: $t('AbpPermissionManagement.Providers:Client'),
    O: $t('AbpPermissionManagement.Providers:OrganizationUnit'),
    R: $t('AbpPermissionManagement.Providers:Role'),
    U: $t('AbpPermissionManagement.Providers:User'),
  };

  const providerOptions = [
    { label: providersMap.R, value: 'R' },
    { label: providersMap.U, value: 'U' },
    { label: providersMap.O, value: 'O' },
    { label: providersMap.C, value: 'C' },
  ];

  return {
    multiTenancySideOptions,
    multiTenancySidesMap,
    providerOptions,
    providersMap,
  };
}
