import { defHttp } from '/@/utils/http/axios';
import { OpenIdConfiguration } from './model';

export const discovery = () => {
  return defHttp.get<OpenIdConfiguration>({
    url: '/.well-known/openid-configuration',
  });
};
