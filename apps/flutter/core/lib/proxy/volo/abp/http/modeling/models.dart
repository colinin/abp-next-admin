import 'package:json_annotation/json_annotation.dart';

part 'models.g.dart';

@JsonSerializable()
class ActionApiDescriptionModel {
  ActionApiDescriptionModel({
    this.uniqueName,
    this.name,
    this.httpMethod,
    this.url,
    this.supportedVersions,
    this.parametersOnMethod,
    this.parameters,
    this.returnValue,
    this.allowAnonymous,
    this.implementFrom,
  });
  String? uniqueName;
  String? name;
  String? httpMethod;
  String? url;
  Iterable<String>? supportedVersions;
  Iterable<MethodParameterApiDescriptionModel>? parametersOnMethod = [];
  Iterable<ParameterApiDescriptionModel>? parameters = [];
  ReturnValueApiDescriptionModel? returnValue;
  bool? allowAnonymous;
  String? implementFrom;

  factory ActionApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$ActionApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$ActionApiDescriptionModelToJson(this);
}

@JsonSerializable()
class MethodParameterApiDescriptionModel {
  MethodParameterApiDescriptionModel({
    this.name,
    this.typeAsString,
    this.type,
    this.typeSimple,
    this.isOptional,
    this.defaultValue,
  });
  String? name;
  String? typeAsString;
  String? type;
  String? typeSimple;
  bool? isOptional;
  dynamic defaultValue;

  factory MethodParameterApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$MethodParameterApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$MethodParameterApiDescriptionModelToJson(this);
}

@JsonSerializable()
class ParameterApiDescriptionModel {
  ParameterApiDescriptionModel({
    this.name,
    this.nameOnMethod,
    this.jsonName,
    this.type,
    this.typeSimple,
    this.isOptional,
    this.defaultValue,
    this.constraintTypes,
    this.bindingSourceId,
    this.descriptorName,
  });
  String? nameOnMethod;
  String? name;
  String? jsonName;
  String? type;
  String? typeSimple;
  bool? isOptional = false;
  dynamic defaultValue;
  Iterable<String>? constraintTypes = [];
  String? bindingSourceId;
  String? descriptorName;

  factory ParameterApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$ParameterApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$ParameterApiDescriptionModelToJson(this);
}

@JsonSerializable()
class ReturnValueApiDescriptionModel {
  ReturnValueApiDescriptionModel({
    this.type,
    this.typeSimple,
  });
  String? type;
  String? typeSimple;

  factory ReturnValueApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$ReturnValueApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$ReturnValueApiDescriptionModelToJson(this);
}

@JsonSerializable()
class PropertyApiDescriptionModel {
  PropertyApiDescriptionModel({
    this.name,
    this.jsonName,
    this.type,
    this.typeSimple,
    this.isRequired,
    this.minLength,
    this.maxLength,
    this.minimum,
    this.maximum,
    this.regex,
  });
  String? name;
  String? jsonName;
  String? type;
  String? typeSimple;
  bool? isRequired = false;
  int? minLength;
  int? maxLength;
  String? minimum;
  String? maximum;
  String? regex;

  factory PropertyApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$PropertyApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$PropertyApiDescriptionModelToJson(this);
}

@JsonSerializable()
class TypeApiDescriptionModel {
  TypeApiDescriptionModel({
    this.baseType,
    this.isEnum,
    this.enumNames,
    this.enumValues,
    this.genericArguments,
    this.properties,
  });
  String? baseType;
  bool? isEnum = false;
  Iterable<String>? enumNames = [];
  Iterable<dynamic>? enumValues = [];
  Iterable<String>? genericArguments = [];
  Iterable<PropertyApiDescriptionModel>? properties = [];

  factory TypeApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$TypeApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$TypeApiDescriptionModelToJson(this);
}

@JsonSerializable()
class InterfaceMethodApiDescriptionModel {
  InterfaceMethodApiDescriptionModel({
    this.name,
    this.parametersOnMethod,
    this.returnValue,
  });
  String? name;
  Iterable<MethodParameterApiDescriptionModel>? parametersOnMethod = [];
  ReturnValueApiDescriptionModel? returnValue = {} as ReturnValueApiDescriptionModel;

  factory InterfaceMethodApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$InterfaceMethodApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$InterfaceMethodApiDescriptionModelToJson(this);
}

@JsonSerializable()
class ControllerInterfaceApiDescriptionModel {
  ControllerInterfaceApiDescriptionModel({
    this.type,
    this.name,
    this.methods,
  });
  String? type;
  String? name;
  Iterable<InterfaceMethodApiDescriptionModel>? methods = [];

  factory ControllerInterfaceApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$ControllerInterfaceApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$ControllerInterfaceApiDescriptionModelToJson(this);
}

@JsonSerializable()
class ApplicationApiDescriptionModelRequestDto {
  ApplicationApiDescriptionModelRequestDto({
    this.includeTypes,
  });
  bool? includeTypes = false;

  factory ApplicationApiDescriptionModelRequestDto.fromJson(Map<String, dynamic> json) => _$ApplicationApiDescriptionModelRequestDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationApiDescriptionModelRequestDtoToJson(this);
}

@JsonSerializable()
class ModuleApiDescriptionModel {
  ModuleApiDescriptionModel({
    this.rootPath,
    this.remoteServiceName,
    this.controllers,
  });
  String? rootPath;
  String? remoteServiceName;
  Map<String, ControllerApiDescriptionModel>? controllers = {};

  factory ModuleApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$ModuleApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$ModuleApiDescriptionModelToJson(this);
}

@JsonSerializable()
class ControllerApiDescriptionModel {
  ControllerApiDescriptionModel({
    this.controllerName,
    this.controllerGroupName,
    this.isRemoteService,
    this.isIntegrationService,
    this.apiVersion,
    this.type,
    this.interfaces,
    this.actions,
  });
  String? controllerName;
  String? controllerGroupName;
  bool? isRemoteService = true;
  bool? isIntegrationService = false;
  String? apiVersion;
  String? type;
  Iterable<ControllerInterfaceApiDescriptionModel>? interfaces = [];
  Map<String, ActionApiDescriptionModel>? actions = {};

  factory ControllerApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$ControllerApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$ControllerApiDescriptionModelToJson(this);
}

@JsonSerializable()
class ApplicationApiDescriptionModel {
  ApplicationApiDescriptionModel({
    this.modules,
    this.types,
  });
  Map<String, ModuleApiDescriptionModel>? modules = {};
  Map<String, TypeApiDescriptionModel>? types = {};

  factory ApplicationApiDescriptionModel.fromJson(Map<String, dynamic> json) => _$ApplicationApiDescriptionModelFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationApiDescriptionModelToJson(this);
}