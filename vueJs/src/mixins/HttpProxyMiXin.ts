import ApiService from '@/api/serviceBase'
import { PagedResultDto } from '@/api/types'
import { Component, Vue } from 'vue-property-decorator'
import { HttpProxyModule } from '@/store/modules/http-proxy'
import {
  ActionApiDescriptionModel,
  ApiVersionInfo,
  ParameterBindingSources,
  UrlBuilder,
  ApplicationApiDescriptionModel
} from '@/api/dynamic-api'
/**
 * 动态Http代理组件
 */
@Component({
  name: 'HttpProxyMiXin'
})
export default class HttpProxyMiXin extends Vue {
  protected pagedRequest<TResult>(options: {
    service: string,
    controller: string,
    action: string,
    data?: any,
    params?: any
  }) {
    return this.request<PagedResultDto<TResult>>(options)
  }

  protected request<TResult>(options: {
    service: string,
    controller: string,
    action: string,
    data?: any
  }) {
    const action = ApplicationApiDescriptionModel
      .getAction(
        options.service, options.controller, options.action,
        HttpProxyModule.applicationApiDescriptionModel.modules)
    const apiVersion = this.getApiVersionInfo(action)
    let url = process.env.REMOTE_SERVICE_BASE_URL || ''
    url = this.ensureEndsWith(url, '/') + UrlBuilder.generateUrlWithParameters(action, options.data, apiVersion)
  
    return ApiService.HttpRequest<TResult>({
      url: url,
      method: action?.httpMethod,
      data: options.data
    })
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
