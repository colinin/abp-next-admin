import { defAbpHttp } from '/@/utils/http/abp';
import { ChangeAvatar } from './model/claimsModel';

enum Api {
  ChangeAvatar = '/api/account/my-claim/change-avatar',
}

export const changeAvatar = (input: ChangeAvatar) => {
  return defAbpHttp.post<void>({
    url: Api.ChangeAvatar,
    data: input,
  });
};