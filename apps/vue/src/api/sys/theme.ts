import { defAbpHttp } from '/@/utils/http/abp';
import { ThemeSetting } from './model/themeModel';

export const getTheme = () => {
  return defAbpHttp.get<ThemeSetting>({
    url: '/api/theme/vue-vben-admin',
  });
};

export const changeTheme = (themeSetting: ThemeSetting) => {
  return defAbpHttp.put<void>({
    url: '/api/theme/vue-vben-admin/change',
    data: themeSetting,
  });
};
