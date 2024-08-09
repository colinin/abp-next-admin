import { defHttp } from '/@/utils/http/axios';
import { ThemeSetting } from './model/themeModel';

export const getTheme = () => {
  return defHttp.get<ThemeSetting>({
    url: '/api/platform/theme/vue-vben-admin',
  });
};

export const changeTheme = (themeSetting: ThemeSetting) => {
  return defHttp.put<void>({
    url: '/api/platform/theme/vue-vben-admin/change',
    data: themeSetting,
  });
};
