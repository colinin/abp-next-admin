import { defAbpHttp } from '/@/utils/http/abp';
import { OpenIdConfiguration } from './model/basicModel';

enum Api {
  Discovery = '/.well-known/openid-configuration',
}

export const discovery = () => {
  return defAbpHttp.get<OpenIdConfiguration>({
    url: Api.Discovery,
  });
};
