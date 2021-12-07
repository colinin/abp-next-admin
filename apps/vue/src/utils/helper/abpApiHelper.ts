import {
  ActionApiDescriptionModel,
  ApiVersionInfo,
  ParameterApiDescriptionModel,
} from '/@/api/abp/model/apiDefinition';

export enum ParameterBindingSources {
  modelBinding = 'ModelBinding',
  query = 'Query',
  body = 'Body',
  path = 'Path',
  form = 'Form',
  header = 'Header',
  custom = 'Custom',
  services = 'Services',
}

const bindSources = [ParameterBindingSources.modelBinding, ParameterBindingSources.query];

export class UrlBuilder {
  public static generateUrlWithParameters(
    action: ActionApiDescriptionModel,
    methodArguments: any,
    apiVersion: ApiVersionInfo
  ) {
    // fix: 需要前缀，可能引起api调用错误
    let urlBuilder = action.url.startsWith('/') ? action.url : `/${action.url}`;
    urlBuilder = this.replacePathVariables(
      urlBuilder,
      action.parameters,
      methodArguments,
      apiVersion
    );
    urlBuilder = this.addQueryStringParameters(
      urlBuilder,
      action.parameters,
      methodArguments,
      apiVersion
    );
    return urlBuilder;
  }

  private static replacePathVariables(
    urlBuilder: string,
    actionParameters: ParameterApiDescriptionModel[],
    methodArguments: any,
    apiVersion: ApiVersionInfo
  ) {
    const pathParameters = actionParameters.filter(
      (x) => x.bindingSourceId === ParameterBindingSources.path
    );
    if (pathParameters.length === 0) {
      return urlBuilder;
    }
    if (pathParameters.some((x) => x.name === 'apiVersion')) {
      urlBuilder = urlBuilder.replace('{apiVersion}', apiVersion.version);
    }
    pathParameters
      .filter((x) => x.name !== 'apiVersion')
      .forEach((pathParameter) => {
        const value = HttpActionParameterHelper.findParameterValue(methodArguments, pathParameter);
        if (!value) {
          if (pathParameter.isOptional) {
            urlBuilder = urlBuilder.replace(`{${pathParameter.name}}`, '');
          } else if (pathParameter.defaultValue) {
            urlBuilder = urlBuilder.replace(
              `{${pathParameter.name}}`,
              String(pathParameter.defaultValue)
            );
          } else {
            throw new Error(
              `Missing path parameter value for ${pathParameter.name} (${pathParameter.nameOnMethod})`
            );
          }
        } else {
          urlBuilder = urlBuilder.replace(`{${pathParameter.name}}`, String(value));
        }
      });
    return urlBuilder;
  }

  private static addQueryStringParameters(
    urlBuilder: string,
    actionParameters: ParameterApiDescriptionModel[],
    methodArguments: any,
    apiVersion: ApiVersionInfo
  ) {
    const queryStringParameters = actionParameters.filter((x) =>
      bindSources.some((b) => b === x.bindingSourceId)
    );
    let isFirstParam = true;
    queryStringParameters.forEach((queryStringParameter) => {
      const value = HttpActionParameterHelper.findParameterValue(
        methodArguments,
        queryStringParameter
      );
      if (!value) {
        return;
      }
      urlBuilder = this.addQueryStringParameter(
        urlBuilder,
        isFirstParam,
        queryStringParameter.name,
        value
      );
      isFirstParam = false;
    });
    if (apiVersion.shouldSendInQueryString()) {
      urlBuilder = this.addQueryStringParameter(
        urlBuilder,
        isFirstParam,
        'api-version',
        apiVersion.version
      );
    }
    return urlBuilder;
  }

  private static addQueryStringParameter(
    urlBuilder: string,
    isFirstParam: boolean,
    name: string,
    value: any
  ) {
    urlBuilder += isFirstParam ? '?' : '&';
    if (Array.isArray(value)) {
      let index = 0;
      value.forEach((val) => {
        urlBuilder += `${name}[${index++}]=` + encodeURI(val);
      });
      urlBuilder.substring(0, urlBuilder.length - 1);
    } else {
      urlBuilder += `${name}=` + encodeURI(value);
    }
    return urlBuilder;
  }
}

export class HttpActionParameterHelper {
  public static findParameterValue(
    methodArguments: any,
    apiParameter: ParameterApiDescriptionModel
  ) {
    const methodArgKeys = Object.keys(methodArguments);
    const keyIndex =
      apiParameter.name === apiParameter.nameOnMethod
        ? methodArgKeys.findIndex((key) => apiParameter.name.toLowerCase() === key.toLowerCase())
        : methodArgKeys.findIndex(
            (key) => apiParameter.nameOnMethod.toLowerCase() === key.toLowerCase()
          );

    let value = methodArguments[methodArgKeys[keyIndex]];
    if (!value) {
      // fix bug: 请求参数为空时返回空字符串而不是null
      return '';
    }

    if (apiParameter.name === apiParameter.nameOnMethod) {
      return value;
    }

    const inputKeys = Object.keys(value);
    const inputKeyIndex = inputKeys.findIndex(
      (key) => key.toLowerCase() === apiParameter.name.toLowerCase()
    );
    // fix bug: 请求参数为空时返回空字符串而不是null
    value = inputKeyIndex < 0 ? '' : value[inputKeys[inputKeyIndex]] ?? '';

    return String(value);
  }
}
