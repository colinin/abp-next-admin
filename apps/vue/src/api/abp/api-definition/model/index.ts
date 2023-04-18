import { Method } from 'axios';

export class ApiVersionInfo {
  bindingSource?: string;
  version!: string;

  constructor(version: string, bindingSource?: string) {
    this.version = version;
    this.bindingSource = bindingSource;
  }

  public shouldSendInQueryString() {
    return ['Path'].some((x) => x === this.bindingSource);
  }
}

export class ControllerInterfaceApiDescriptionModel {
  type!: string;
}

export class ReturnValueApiDescriptionModel {
  type!: string;
  typeSimple!: string;
}

export class MethodParameterApiDescriptionModel {
  name!: string;
  typeAsString!: string;
  type!: string;
  typeSimple!: string;
  isOptional!: boolean;
  defaultValue?: any;
}

export class ParameterApiDescriptionModel {
  nameOnMethod!: string;
  name!: string;
  type!: string;
  typeSimple!: string;
  isOptional!: boolean;
  defaultValue?: any;
  constraintTypes = new Array<string>();
  bindingSourceId?: string;
  descriptorName?: string;
}

export class PropertyApiDescriptionModel {
  name!: string;
  type!: string;
  typeSimple!: string;
  isRequired!: boolean;
}

export class ActionApiDescriptionModel {
  uniqueName!: string;
  name!: string;
  httpMethod!: Method;
  url!: string;
  supportedVersions = new Array<string>();
  parametersOnMethod = new Array<MethodParameterApiDescriptionModel>();
  parameters = new Array<ParameterApiDescriptionModel>();
  returnValue = new ReturnValueApiDescriptionModel();
}

export class ControllerApiDescriptionModel {
  controllerName!: string;
  type!: string;
  interfaces = new Array<ControllerInterfaceApiDescriptionModel>();
  actions: { [key: string]: ActionApiDescriptionModel } = {};
}

export class ModuleApiDescriptionModel {
  rootPath = 'app';
  remoteServiceName = 'Default';
  controllers: { [key: string]: ControllerApiDescriptionModel } = {};
}

export class TypeApiDescriptionModel {
  baseType!: string;
  isEnum!: boolean;
  enumNames = new Array<string>();
  enumValues = new Array<any>();
  genericArguments = new Array<string>();
  properties = new Array<PropertyApiDescriptionModel>();
}

export class ApplicationApiDescriptionModel {
  modules: { [key: string]: ModuleApiDescriptionModel } = {};
  types: { [key: string]: TypeApiDescriptionModel } = {};
}
