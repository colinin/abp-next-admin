/*
  Reference: 
*/

import ApiService from './serviceBase'
import { Method } from 'axios'

const sourceUrl = '/api/abp/api-definition'
const serviceUrl = process.env.VUE_APP_BASE_API

export enum ParameterBindingSources {
  modelBinding = 'ModelBinding',
  query = 'Query',
  body = 'Body',
  path = 'Path',
  form = 'Form',
  header = 'Header',
  custom = 'Custom',
  services = 'Services'
}

const bindSources = [ParameterBindingSources.modelBinding, ParameterBindingSources.query]

export default class DynamicApiService {
  /** 获取api代理信息
   * @param includeTypes 包括类型信息
  */
  public static get(includeTypes = false) {
    const _url = sourceUrl + '?includeTypes=' + includeTypes
    return ApiService.Get<ApplicationApiDescriptionModel>(_url, serviceUrl)
  }
}

export class UrlBuilder {
  public static generateUrlWithParameters(action: ActionApiDescriptionModel, methodArguments: any, apiVersion: ApiVersionInfo) {
    let urlBuilder = action.url
    urlBuilder = this.replacePathVariables(urlBuilder, action.parameters, methodArguments, apiVersion)
    urlBuilder = this.addQueryStringParameters(urlBuilder, action.parameters, methodArguments, apiVersion)
    return urlBuilder
  }

  private static replacePathVariables(
    urlBuilder: string,
    actionParameters: ParameterApiDescriptionModel[],
    methodArguments: any,
    apiVersion: ApiVersionInfo) {
      const pathParameters = actionParameters.filter(x => x.bindingSourceId === ParameterBindingSources.path)
      if (pathParameters.length === 0) {
        return urlBuilder
      }
      if (pathParameters.some(x => x.name === 'apiVersion')) {
        urlBuilder = urlBuilder.replace("{apiVersion}", apiVersion.version)
      }
      pathParameters
        .filter(x => x.name !== 'apiVersion')
        .forEach(pathParameter => {
          const value = HttpActionParameterHelper.findParameterValue(methodArguments, pathParameter)
          if (!value) {
            if (pathParameter.isOptional) {
              urlBuilder = urlBuilder.replace(`{${pathParameter.name}}`, '')
            } else if (pathParameter.defaultValue) {
              urlBuilder = urlBuilder.replace(`{${pathParameter.name}}`, String(pathParameter.defaultValue))
            } else {
              throw new Error(`Missing path parameter value for ${pathParameter.name} (${pathParameter.nameOnMethod})`)
            }
          } else {
            urlBuilder = urlBuilder.replace(`{${pathParameter.name}}`, String(value))
          }
        })
      return urlBuilder
  }

  private static addQueryStringParameters(
    urlBuilder: string,
    actionParameters: ParameterApiDescriptionModel[],
    methodArguments: any,
    apiVersion: ApiVersionInfo
  ) {
    const queryStringParameters = actionParameters.filter(x => bindSources.some(b => b === x.bindingSourceId))
    let isFirstParam = true
    queryStringParameters
      .forEach(queryStringParameter => {
        const value = HttpActionParameterHelper.findParameterValue(methodArguments, queryStringParameter)
        if (!value) {
          return
        }
        urlBuilder = this.addQueryStringParameter(urlBuilder, isFirstParam, queryStringParameter.name, value)
        isFirstParam = false
      })
    if (apiVersion.shouldSendInQueryString()) {
      urlBuilder = this.addQueryStringParameter(urlBuilder, isFirstParam, 'api-version', apiVersion.version)
    }
    return urlBuilder
  }

  private static addQueryStringParameter(
    urlBuilder: string,
    isFirstParam: boolean,
    name: string,
    value: any
  ) {
    urlBuilder += isFirstParam ? '?' : '&'
    if (Array.isArray(value)) {
      let index = 0
      value.forEach(val => {
        urlBuilder += `${name}[${index++}]=` + encodeURI(val)
      })
      urlBuilder.substring(0, urlBuilder.length - 1)
    } else {
      urlBuilder += `${name}=` + encodeURI(value)
    }
    return urlBuilder
  }
}

export class HttpActionParameterHelper {
  public static findParameterValue(methodArguments: any, apiParameter: ParameterApiDescriptionModel) {
    const methodArgKeys = Object.keys(methodArguments)
    const keyIndex = apiParameter.name === apiParameter.nameOnMethod ?
      methodArgKeys.findIndex(key => apiParameter.name.toLowerCase() === key.toLowerCase()) :
      methodArgKeys.findIndex(key => apiParameter.nameOnMethod.toLowerCase() === key.toLowerCase())

    let value = methodArguments[methodArgKeys[keyIndex]]
    if (!value) {
      return null
    }

    if (apiParameter.name === apiParameter.nameOnMethod) {
      return value
    }

    const inputKeys = Object.keys(value)
    const inputKeyIndex = inputKeys.findIndex(key => key.toLowerCase() === apiParameter.name.toLowerCase())
    value = inputKeyIndex < 0 ? null : value[inputKeys[inputKeyIndex]]

    return String(value)
  }
}

export class ApiVersionInfo {
  bindingSource?: string
  version!: string

  constructor (
    version: string,
    bindingSource?: string
  ) {
    this.version = version
    this.bindingSource = bindingSource
  }

  public shouldSendInQueryString() {
    return ['Path'].some(x => x === this.bindingSource)
  }
}

export class ControllerInterfaceApiDescriptionModel {
  type!: string
}

export class ReturnValueApiDescriptionModel {
  type!: string
  typeSimple!: string
}

export class MethodParameterApiDescriptionModel {
  name!: string
  typeAsString!: string
  type!: string
  typeSimple!: string
  isOptional!: boolean
  defaultValue?: any
}

export class ParameterApiDescriptionModel {
  nameOnMethod!: string
  name!: string
  type!: string
  typeSimple!: string
  isOptional!: boolean
  defaultValue?: any
  constraintTypes = new Array<string>()
  bindingSourceId?: string
  descriptorName?: string
}

export class PropertyApiDescriptionModel {
  name!: string
  type!: string
  typeSimple!: string
  isRequired!: boolean
}

export class ActionApiDescriptionModel {
  uniqueName!: string
  name!: string
  httpMethod!: Method
  url!: string
  supportedVersions = new Array<string>()
  parametersOnMethod = new Array<MethodParameterApiDescriptionModel>()
  parameters = new Array<ParameterApiDescriptionModel>()
  returnValue = new ReturnValueApiDescriptionModel()
}

export class ControllerApiDescriptionModel {
  controllerName!: string
  type!: string
  interfaces = new Array<ControllerInterfaceApiDescriptionModel>()
  actions: {[key: string]: ActionApiDescriptionModel} = {}
}

export class ModuleApiDescriptionModel {
  rootPath = 'app'
  remoteServiceName = 'Default'
  controllers: {[key: string]: ControllerApiDescriptionModel} = {}
}

export class TypeApiDescriptionModel {
  baseType!: string
  isEnum!: boolean
  enumNames =  new Array<string>()
  enumValues =  new Array<any>()
  genericArguments =  new Array<string>()
  properties =  new Array<PropertyApiDescriptionModel>() 
}

export class ApplicationApiDescriptionModel {
  modules: {[key: string]: ModuleApiDescriptionModel} = {}
  types: {[key: string]: TypeApiDescriptionModel} = {}
}
