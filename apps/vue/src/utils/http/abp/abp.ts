import { defHttp } from '/@/utils/http/axios';

import { useAbpStoreWithOut } from '/@/store/modules/abp';
import {
  ActionApiDescriptionModel,
  ApiVersionInfo,
  ControllerApiDescriptionModel,
  ModuleApiDescriptionModel,
} from '/@/api/abp/model/apiDefinition';
import { ParameterBindingSources, UrlBuilder } from '/@/utils/helper/abpApiHelper';
import { ListResultDto, PagedResultDto } from '/@/api/model/baseModel';

import { useI18n } from '/@/hooks/web/useI18n';
import { AxiosRequestConfig } from 'axios';
import { RequestOptions, UploadFileParams } from '/#/axios';

const { t } = useI18n();

export class abpRequest {
  pagedRequest<TResult>(options: {
    service: string;
    controller: string;
    action: string;
    data?: any;
    params?: any;
  }) {
    return this.request<PagedResultDto<TResult>>(options);
  }

  listRequest<TResult>(options: {
    service: string;
    controller: string;
    action: string;
    data?: any;
    params?: any;
  }) {
    return this.request<ListResultDto<TResult>>(options);
  }

  get<T = any>(config: AxiosRequestConfig, options?: RequestOptions): Promise<T> {
    return defHttp.request({ ...config, method: 'GET' }, options);
  }

  post<T = any>(config: AxiosRequestConfig, options?: RequestOptions): Promise<T> {
    return defHttp.request({ ...config, method: 'POST' }, options);
  }

  put<T = any>(config: AxiosRequestConfig, options?: RequestOptions): Promise<T> {
    return defHttp.request({ ...config, method: 'PUT' }, options);
  }

  delete<T = any>(config: AxiosRequestConfig, options?: RequestOptions): Promise<T> {
    return defHttp.request({ ...config, method: 'DELETE' }, options);
  }

  uploadFile<T = any>(config: AxiosRequestConfig, params: UploadFileParams) {
    return defHttp.uploadFile<T>(config, params);
  }

  request<TResult>(options: {
    service: string;
    controller: string;
    action: string;
    data?: any;
    params?: any;
  }) {
    const abpStore = useAbpStoreWithOut();
    const module = this.getModule(options.service, abpStore.apidefinition.modules);
    const controller = this.getController(options.controller, module.controllers);
    const action = this.getAction(options.action, controller.actions);
    const apiVersion = this.getApiVersionInfo(action);
    const url = UrlBuilder.generateUrlWithParameters(action, options.params, apiVersion);

    return defHttp.request<TResult>({
      url: url,
      method: action?.httpMethod,
      data: options.data,
    });
  }

  private getModule(remoteService: string, modules: { [key: string]: ModuleApiDescriptionModel }) {
    const moduleKeys = Object.keys(modules);
    const index = moduleKeys.findIndex((key) => {
      const m = modules[key];
      if (m.remoteServiceName.toLowerCase() === remoteService.toLowerCase()) {
        return m;
      }
    });
    if (index < 0) {
      throw new Error(t('sys.abp.remoteServiceNotFound', { name: remoteService }));
    }
    return modules[moduleKeys[index]];
  }

  private getController(
    controllerName: string,
    controllers: { [key: string]: ControllerApiDescriptionModel },
  ) {
    const controllerKeys = Object.keys(controllers);
    const index = controllerKeys.findIndex((key) => {
      const c = controllers[key];
      if (c.controllerName.toLowerCase() === controllerName.toLowerCase()) {
        return c;
      }
    });
    if (index < 0) {
      throw new Error(t('sys.abp.controllerNotFound', { name: controllerName }));
    }
    return controllers[controllerKeys[index]];
  }

  private getAction(actionName: string, actions: { [key: string]: ActionApiDescriptionModel }) {
    const actionKeys = Object.keys(actions);
    const index = actionKeys.findIndex((key) => {
      const a = actions[key];
      if (a.name.toLowerCase() === actionName.toLowerCase()) {
        return a;
      }
    });
    if (index < 0) {
      throw new Error(t('sys.abp.actionNotFound', { name: actionName }));
    }
    return actions[actionKeys[index]];
  }

  private getApiVersionInfo(action: ActionApiDescriptionModel) {
    const apiVersion = this.findBestApiVersion(action);
    const versionParam =
      action.parameters.find(
        (p) => p.name === 'apiVersion' || p.bindingSourceId === ParameterBindingSources.path,
      ) ??
      action.parameters.find(
        (p) => p.name === 'api-version' || p.bindingSourceId === ParameterBindingSources.query,
      );
    return new ApiVersionInfo(apiVersion, versionParam?.bindingSourceId);
  }

  private findBestApiVersion(action: ActionApiDescriptionModel) {
    if (action.supportedVersions.length === 0) {
      return '1.0';
    }
    return action.supportedVersions[action.supportedVersions.length - 1];
  }
}
