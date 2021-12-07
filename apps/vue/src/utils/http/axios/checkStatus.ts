import type { ErrorMessageMode } from '/#/axios';
import { useMessage } from '/@/hooks/web/useMessage';
import { useI18n } from '/@/hooks/web/useI18n';
// import router from '/@/router';
// import { PageEnum } from '/@/enums/pageEnum';
import { useUserStoreWithOut } from '/@/store/modules/user';
import projectSetting from '/@/settings/projectSetting';
import { SessionTimeoutProcessingEnum } from '/@/enums/appEnum';

const { createMessage, createErrorModal } = useMessage();
const error = createMessage.error!;
const stp = projectSetting.sessionTimeoutProcessing;

export function checkStatus(
  status: number,
  msg: string,
  errorMessageMode: ErrorMessageMode = 'message',
): void {
  const { t } = useI18n();
  const userStore = useUserStoreWithOut();
  let errMessage = '';

  switch (status) {
    case 400:
      errMessage = `${msg}`;
      break;
    // 401: Not logged in
    // Jump to the login page if not logged in, and carry the path of the current page
    // Return to the current page after successful login. This step needs to be operated on the login page.
    case 401:
      userStore.setToken(undefined);
      errMessage = msg || t('sys.api.errMsg401');
      if (stp === SessionTimeoutProcessingEnum.PAGE_COVERAGE) {
        userStore.setSessionTimeout(true);
      } else {
        userStore.logout(true);
      }
      break;
    case 403:
      errMessage = t('sys.api.errMsg403');
      break;
    // 404请求不存在
    case 404:
      errMessage = t('sys.api.errMsg404');
      break;
    case 405:
      errMessage = t('sys.api.errMsg405');
      break;
    case 408:
      errMessage = t('sys.api.errMsg408');
      break;
    case 500:
      errMessage = t('sys.api.errMsg500');
      break;
    case 501:
      errMessage = t('sys.api.errMsg501');
      break;
    case 502:
      errMessage = t('sys.api.errMsg502');
      break;
    case 503:
      errMessage = t('sys.api.errMsg503');
      break;
    case 504:
      errMessage = t('sys.api.errMsg504');
      break;
    case 505:
      errMessage = t('sys.api.errMsg505');
      break;
    default:
  }

  if (errMessage) {
    if (errorMessageMode === 'modal') {
      createErrorModal({ title: t('sys.api.errorTip'), content: errMessage });
    } else if (errorMessageMode === 'message') {
      error({ content: errMessage, key: `global_error_message_status_${status}` });
    }
  }
}

export function checkResponse(response: any): void {
  if (!response.data) {
    // 都没捕获到则提示默认错误信息
    const { t } = useI18n();
    checkStatus(response.status, t('sys.api.apiRequestFailed'));
    return;
  }

  let errorJson = response.data.error;

  // 会话超时
  if (response.status === 401) {
    const userStore = useUserStoreWithOut();
    userStore.setToken(undefined);
    userStore.setSessionTimeout(true);
    return;
  }

  // abp框架抛出异常信息
  if (response.headers['_abperrorformat'] === 'true') {
    if (errorJson === undefined && response.data.type === 'application/json') {
      const reader = new FileReader();
      reader.onload = function (e) {
        errorJson = JSON.parse(e.target?.result as string);
        console.log(errorJson);
        error(errorJson.error.message);
      };
      reader.readAsText(response.data);
    } else {
      let errorMessage = errorJson.message;
      if (errorJson.validationErrors) {
        errorMessage += errorJson.validationErrors.map((v) => v.message).join('\n');
      }
      error(errorMessage);
    }
    return;
  }

  // oauth错误信息
  if (response.data.error_description) {
    error(response.data.error_description);
    return;
  }

  // 其他错误
  if (response.data.error.details) {
    error(response.data.error.details);
    return;
  }

  if (response.data.error.message) {
    error(response.data.error.message);
    return;
  }
}
