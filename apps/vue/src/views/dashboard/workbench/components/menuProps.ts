import { useI18n } from '/@/hooks/web/useI18n';

export interface Menu {
  id: string,
  title: string;
  desc?: string;
  icon?: string;
  color?: string;
  size?: number;
  path?: string;
  hasDefault?: boolean;
  click?: Function;
}

export function useDefaultMenus() {
  const { t } = useI18n();

  const defaultMenus: Menu[] = [{
      id: '0',
      title: t('layout.header.home'),
      icon: 'ion:home-outline',
      color: '#1fdaca',
      path: '/',
      hasDefault: true,
    },{
      id: '1',
      title: t('routes.dashboard.dashboard'),
      icon: 'ion:grid-outline',
      color: '#bf0c2c',
      path: '/dashboard/workbench',
      hasDefault: true,
    },{
      id: '2',
      title: t('routes.basic.accountSetting'),
      icon: 'ant-design:setting-outlined',
      color: '#3fb27f',
      path: '/account/settings',
      hasDefault: true,
    },{
      id: '3',
      title: t('routes.basic.accountCenter'),
      icon: 'ant-design:profile-outlined',
      color: '#4daf1bc9',
      path: '/account/center',
      hasDefault: true,
    }
  ];

  return defaultMenus;
}
