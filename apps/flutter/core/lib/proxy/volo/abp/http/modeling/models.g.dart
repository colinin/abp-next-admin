// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'models.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ActionApiDescriptionModel _$ActionApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    ActionApiDescriptionModel(
      uniqueName: json['uniqueName'] as String?,
      name: json['name'] as String?,
      httpMethod: json['httpMethod'] as String?,
      url: json['url'] as String?,
      supportedVersions: (json['supportedVersions'] as List<dynamic>?)
          ?.map((e) => e as String),
      parametersOnMethod: (json['parametersOnMethod'] as List<dynamic>?)?.map(
          (e) => MethodParameterApiDescriptionModel.fromJson(
              e as Map<String, dynamic>)),
      parameters: (json['parameters'] as List<dynamic>?)?.map((e) =>
          ParameterApiDescriptionModel.fromJson(e as Map<String, dynamic>)),
      returnValue: json['returnValue'] == null
          ? null
          : ReturnValueApiDescriptionModel.fromJson(
              json['returnValue'] as Map<String, dynamic>),
      allowAnonymous: json['allowAnonymous'] as bool?,
      implementFrom: json['implementFrom'] as String?,
    );

Map<String, dynamic> _$ActionApiDescriptionModelToJson(
        ActionApiDescriptionModel instance) =>
    <String, dynamic>{
      'uniqueName': instance.uniqueName,
      'name': instance.name,
      'httpMethod': instance.httpMethod,
      'url': instance.url,
      'supportedVersions': instance.supportedVersions?.toList(),
      'parametersOnMethod': instance.parametersOnMethod?.toList(),
      'parameters': instance.parameters?.toList(),
      'returnValue': instance.returnValue,
      'allowAnonymous': instance.allowAnonymous,
      'implementFrom': instance.implementFrom,
    };

MethodParameterApiDescriptionModel _$MethodParameterApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    MethodParameterApiDescriptionModel(
      name: json['name'] as String?,
      typeAsString: json['typeAsString'] as String?,
      type: json['type'] as String?,
      typeSimple: json['typeSimple'] as String?,
      isOptional: json['isOptional'] as bool?,
      defaultValue: json['defaultValue'],
    );

Map<String, dynamic> _$MethodParameterApiDescriptionModelToJson(
        MethodParameterApiDescriptionModel instance) =>
    <String, dynamic>{
      'name': instance.name,
      'typeAsString': instance.typeAsString,
      'type': instance.type,
      'typeSimple': instance.typeSimple,
      'isOptional': instance.isOptional,
      'defaultValue': instance.defaultValue,
    };

ParameterApiDescriptionModel _$ParameterApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    ParameterApiDescriptionModel(
      name: json['name'] as String?,
      nameOnMethod: json['nameOnMethod'] as String?,
      jsonName: json['jsonName'] as String?,
      type: json['type'] as String?,
      typeSimple: json['typeSimple'] as String?,
      isOptional: json['isOptional'] as bool?,
      defaultValue: json['defaultValue'],
      constraintTypes:
          (json['constraintTypes'] as List<dynamic>?)?.map((e) => e as String),
      bindingSourceId: json['bindingSourceId'] as String?,
      descriptorName: json['descriptorName'] as String?,
    );

Map<String, dynamic> _$ParameterApiDescriptionModelToJson(
        ParameterApiDescriptionModel instance) =>
    <String, dynamic>{
      'nameOnMethod': instance.nameOnMethod,
      'name': instance.name,
      'jsonName': instance.jsonName,
      'type': instance.type,
      'typeSimple': instance.typeSimple,
      'isOptional': instance.isOptional,
      'defaultValue': instance.defaultValue,
      'constraintTypes': instance.constraintTypes?.toList(),
      'bindingSourceId': instance.bindingSourceId,
      'descriptorName': instance.descriptorName,
    };

ReturnValueApiDescriptionModel _$ReturnValueApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    ReturnValueApiDescriptionModel(
      type: json['type'] as String?,
      typeSimple: json['typeSimple'] as String?,
    );

Map<String, dynamic> _$ReturnValueApiDescriptionModelToJson(
        ReturnValueApiDescriptionModel instance) =>
    <String, dynamic>{
      'type': instance.type,
      'typeSimple': instance.typeSimple,
    };

PropertyApiDescriptionModel _$PropertyApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    PropertyApiDescriptionModel(
      name: json['name'] as String?,
      jsonName: json['jsonName'] as String?,
      type: json['type'] as String?,
      typeSimple: json['typeSimple'] as String?,
      isRequired: json['isRequired'] as bool?,
      minLength: json['minLength'] as int?,
      maxLength: json['maxLength'] as int?,
      minimum: json['minimum'] as String?,
      maximum: json['maximum'] as String?,
      regex: json['regex'] as String?,
    );

Map<String, dynamic> _$PropertyApiDescriptionModelToJson(
        PropertyApiDescriptionModel instance) =>
    <String, dynamic>{
      'name': instance.name,
      'jsonName': instance.jsonName,
      'type': instance.type,
      'typeSimple': instance.typeSimple,
      'isRequired': instance.isRequired,
      'minLength': instance.minLength,
      'maxLength': instance.maxLength,
      'minimum': instance.minimum,
      'maximum': instance.maximum,
      'regex': instance.regex,
    };

TypeApiDescriptionModel _$TypeApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    TypeApiDescriptionModel(
      baseType: json['baseType'] as String?,
      isEnum: json['isEnum'] as bool?,
      enumNames: (json['enumNames'] as List<dynamic>?)?.map((e) => e as String),
      enumValues: json['enumValues'] as List<dynamic>?,
      genericArguments:
          (json['genericArguments'] as List<dynamic>?)?.map((e) => e as String),
      properties: (json['properties'] as List<dynamic>?)?.map((e) =>
          PropertyApiDescriptionModel.fromJson(e as Map<String, dynamic>)),
    );

Map<String, dynamic> _$TypeApiDescriptionModelToJson(
        TypeApiDescriptionModel instance) =>
    <String, dynamic>{
      'baseType': instance.baseType,
      'isEnum': instance.isEnum,
      'enumNames': instance.enumNames?.toList(),
      'enumValues': instance.enumValues?.toList(),
      'genericArguments': instance.genericArguments?.toList(),
      'properties': instance.properties?.toList(),
    };

InterfaceMethodApiDescriptionModel _$InterfaceMethodApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    InterfaceMethodApiDescriptionModel(
      name: json['name'] as String?,
      parametersOnMethod: (json['parametersOnMethod'] as List<dynamic>?)?.map(
          (e) => MethodParameterApiDescriptionModel.fromJson(
              e as Map<String, dynamic>)),
      returnValue: json['returnValue'] == null
          ? null
          : ReturnValueApiDescriptionModel.fromJson(
              json['returnValue'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$InterfaceMethodApiDescriptionModelToJson(
        InterfaceMethodApiDescriptionModel instance) =>
    <String, dynamic>{
      'name': instance.name,
      'parametersOnMethod': instance.parametersOnMethod?.toList(),
      'returnValue': instance.returnValue,
    };

ControllerInterfaceApiDescriptionModel
    _$ControllerInterfaceApiDescriptionModelFromJson(
            Map<String, dynamic> json) =>
        ControllerInterfaceApiDescriptionModel(
          type: json['type'] as String?,
          name: json['name'] as String?,
          methods: (json['methods'] as List<dynamic>?)?.map((e) =>
              InterfaceMethodApiDescriptionModel.fromJson(
                  e as Map<String, dynamic>)),
        );

Map<String, dynamic> _$ControllerInterfaceApiDescriptionModelToJson(
        ControllerInterfaceApiDescriptionModel instance) =>
    <String, dynamic>{
      'type': instance.type,
      'name': instance.name,
      'methods': instance.methods?.toList(),
    };

ApplicationApiDescriptionModelRequestDto
    _$ApplicationApiDescriptionModelRequestDtoFromJson(
            Map<String, dynamic> json) =>
        ApplicationApiDescriptionModelRequestDto(
          includeTypes: json['includeTypes'] as bool?,
        );

Map<String, dynamic> _$ApplicationApiDescriptionModelRequestDtoToJson(
        ApplicationApiDescriptionModelRequestDto instance) =>
    <String, dynamic>{
      'includeTypes': instance.includeTypes,
    };

ModuleApiDescriptionModel _$ModuleApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    ModuleApiDescriptionModel(
      rootPath: json['rootPath'] as String?,
      remoteServiceName: json['remoteServiceName'] as String?,
      controllers: (json['controllers'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k,
            ControllerApiDescriptionModel.fromJson(e as Map<String, dynamic>)),
      ),
    );

Map<String, dynamic> _$ModuleApiDescriptionModelToJson(
        ModuleApiDescriptionModel instance) =>
    <String, dynamic>{
      'rootPath': instance.rootPath,
      'remoteServiceName': instance.remoteServiceName,
      'controllers': instance.controllers,
    };

ControllerApiDescriptionModel _$ControllerApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    ControllerApiDescriptionModel(
      controllerName: json['controllerName'] as String?,
      controllerGroupName: json['controllerGroupName'] as String?,
      isRemoteService: json['isRemoteService'] as bool?,
      isIntegrationService: json['isIntegrationService'] as bool?,
      apiVersion: json['apiVersion'] as String?,
      type: json['type'] as String?,
      interfaces: (json['interfaces'] as List<dynamic>?)?.map((e) =>
          ControllerInterfaceApiDescriptionModel.fromJson(
              e as Map<String, dynamic>)),
      actions: (json['actions'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(
            k, ActionApiDescriptionModel.fromJson(e as Map<String, dynamic>)),
      ),
    );

Map<String, dynamic> _$ControllerApiDescriptionModelToJson(
        ControllerApiDescriptionModel instance) =>
    <String, dynamic>{
      'controllerName': instance.controllerName,
      'controllerGroupName': instance.controllerGroupName,
      'isRemoteService': instance.isRemoteService,
      'isIntegrationService': instance.isIntegrationService,
      'apiVersion': instance.apiVersion,
      'type': instance.type,
      'interfaces': instance.interfaces?.toList(),
      'actions': instance.actions,
    };

ApplicationApiDescriptionModel _$ApplicationApiDescriptionModelFromJson(
        Map<String, dynamic> json) =>
    ApplicationApiDescriptionModel(
      modules: (json['modules'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(
            k, ModuleApiDescriptionModel.fromJson(e as Map<String, dynamic>)),
      ),
      types: (json['types'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(
            k, TypeApiDescriptionModel.fromJson(e as Map<String, dynamic>)),
      ),
    );

Map<String, dynamic> _$ApplicationApiDescriptionModelToJson(
        ApplicationApiDescriptionModel instance) =>
    <String, dynamic>{
      'modules': instance.modules,
      'types': instance.types,
    };
