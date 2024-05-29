import { defHttp } from '/@/utils/http/axios';
import { ChangeAvatarInput } from './model';

export const changeAvatar = (input: ChangeAvatarInput) => {
  return defHttp.post<void>({
    url: '/api/account/my-claim/change-avatar',
    data: input,
  });
};