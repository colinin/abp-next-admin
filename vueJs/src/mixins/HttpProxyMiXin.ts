import ApiService from '@/api/serviceBase'
import { PagedResultDto, ListResultDto } from '@/api/types'
import { Component, Vue } from 'vue-property-decorator'
import { HttpProxyModule } from '@/store/modules/http-proxy'
import {
  ActionApiDescriptionModel,
  ApiVersionInfo,
  ParameterBindingSources,
  UrlBuilder,
  ApplicationApiDescriptionModel,
  ModuleApiDescriptionModel,
  ControllerApiDescriptionModel
} from '@/api/dynamic-api'
/**
 * 动态Http代理组件
 */
@Component({
  name: 'HttpProxyMiXin'
})
export default class HttpProxyMiXin extends Vue {
  private apiDescriptor!: ApplicationApiDescriptionModel

  created() {
    this.apiDescriptor = HttpProxyModule.applicationApiDescriptionModel
  }

  protected pagedRequest<TResult>(options: {
    service: string,
    controller: string,
    action: string,
    data?: any,
    params?: any
  }) {
    return this.request<PagedResultDto<TResult>>(options)
  }

  protected listRequest<TResult>(options: {
    service: string,
    controller: string,
    action: string,
    data?: any,
    params?: any
  }) {
    return this.request<ListResultDto<TResult>>(options)
  }

  protected request<TResult>(options: {
    service: string,
    controller: string,
    action: string,
    data?: any,
    params?: any
  }) {
    const module = this.getModule(options.service, this.apiDescriptor.modules)
    const controller = this.getController(options.controller, module.controllers)
    const action = this.getAction(options.action, controller.actions)
    const apiVersion = this.getApiVersionInfo(action)
    let url = process.env.REMOTE_SERVICE_BASE_URL || ''
    url = this.ensureEndsWith(url, '/') + UrlBuilder.generateUrlWithParameters(action, options.params, apiVersion)
  
    return ApiService.HttpRequest<TResult>({
      url: url,
      method: action?.httpMethod,
      data: options.data
    })
  }

  private getModule(
    remoteService: string,
    modules: {[key: string]: ModuleApiDescriptionModel}) {
    const moduleKeys = Object.keys(modules)
    const index = moduleKeys.findIndex(key => {
      const m = modules[key]
      if (m.remoteServiceName.toLowerCase() === remoteService.toLowerCase()) {
        return m
      }
    })
    if (index < 0) {
      throw new Error(`没有找到名为 ${remoteService} 的服务定义!`)
    }
    return modules[moduleKeys[index]]
  }

  private getController(
    controllerName: string,
    controllers: {[key: string]: ControllerApiDescriptionModel}) {
    const controllerKeys = Object.keys(controllers)
    const index = controllerKeys.findIndex(key => {
      const c = controllers[key]
      if (c.controllerName.toLowerCase() === controllerName.toLowerCase()) {
        return c
      }
    })
    if (index < 0) {
      throw new Error(`没有找到名为 ${controllerName} 的接口定义!`)
    }
    return controllers[controllerKeys[index]]
  }
  
  private getAction(
    actionName: string,
    actions: {[key: string]: ActionApiDescriptionModel}) {
    const actionKeys = Object.keys(actions)
    const index = actionKeys.findIndex(key => {
      const a = actions[key]
      if (a.name.toLowerCase() === actionName.toLowerCase()) {
        return a
      }
    })
    if (index < 0) {
      throw new Error(`没有找到名为 ${actionName} 的方法定义!`)
    }
    return actions[actionKeys[index]]
  }

  private getApiVersionInfo(action: ActionApiDescriptionModel) {
    const apiVersion = this.findBestApiVersion(action)
    const versionParam = 
      action.parameters.find(p => p.name === 'apiVersion' || p.bindingSourceId === ParameterBindingSources.path) ??
      action.parameters.find(p => p.name === 'api-version' || p.bindingSourceId === ParameterBindingSources.query)
    return new ApiVersionInfo(apiVersion, versionParam?.bindingSourceId)
  }

  private findBestApiVersion(action: ActionApiDescriptionModel) {
    if (action.supportedVersions.length === 0) {
      return '1.0'
    }
    return action.supportedVersions[action.supportedVersions.length - 1]
  }

  private ensureEndsWith(str: string, c: string) {
    if (str.lastIndexOf(c) === str.length - c.length) {
      return str
    }
    return str + c
  }
}
