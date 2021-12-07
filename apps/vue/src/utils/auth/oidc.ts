import oidc from 'oidc-client';
import { useGlobSetting } from '/@/hooks/setting';
const glob = useGlobSetting();
const location = window.location;
// 预留oidc接口
// 用于可能的SSO
// yarn add oidc-client
export const mgr = new oidc.UserManager({
  authority: glob.authority,
  client_id: glob.clientId,
  client_secret: glob.clientSecret,
  response_type: 'id_token token',
  scope: 'openid profile email address offline_access lingyun-abp-application',
  redirect_uri: `${location.protocol}//${location.host}/signin-callback`,
  post_logout_redirect_uri: `${location.protocol}//${location.host}/signout-callback`,
});
