// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'models.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

EntityExtensionDto _$EntityExtensionDtoFromJson(Map<String, dynamic> json) =>
    EntityExtensionDto(
      properties: (json['properties'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(
            k, ExtensionPropertyDto.fromJson(e as Map<String, dynamic>)),
      ),
      configuration: json['configuration'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$EntityExtensionDtoToJson(EntityExtensionDto instance) =>
    <String, dynamic>{
      'properties': instance.properties,
      'configuration': instance.configuration,
    };

ExtensionEnumDto _$ExtensionEnumDtoFromJson(Map<String, dynamic> json) =>
    ExtensionEnumDto(
      localizationResource: json['localizationResource'] as String?,
      fields: (json['fields'] as List<dynamic>?)?.map(
          (e) => ExtensionEnumFieldDto.fromJson(e as Map<String, dynamic>)),
    );

Map<String, dynamic> _$ExtensionEnumDtoToJson(ExtensionEnumDto instance) =>
    <String, dynamic>{
      'localizationResource': instance.localizationResource,
      'fields': instance.fields?.toList(),
    };

ExtensionEnumFieldDto _$ExtensionEnumFieldDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionEnumFieldDto(
      name: json['name'] as String?,
      value: json['value'],
    );

Map<String, dynamic> _$ExtensionEnumFieldDtoToJson(
        ExtensionEnumFieldDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'value': instance.value,
    };

ExtensionPropertyApiCreateDto _$ExtensionPropertyApiCreateDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyApiCreateDto(
      isAvailable: json['isAvailable'] as bool,
    );

Map<String, dynamic> _$ExtensionPropertyApiCreateDtoToJson(
        ExtensionPropertyApiCreateDto instance) =>
    <String, dynamic>{
      'isAvailable': instance.isAvailable,
    };

ExtensionPropertyApiDto _$ExtensionPropertyApiDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyApiDto(
      onGet: json['onGet'] == null
          ? null
          : ExtensionPropertyApiGetDto.fromJson(
              json['onGet'] as Map<String, dynamic>),
      onCreate: json['onCreate'] == null
          ? null
          : ExtensionPropertyApiCreateDto.fromJson(
              json['onCreate'] as Map<String, dynamic>),
      onUpdate: json['onUpdate'] == null
          ? null
          : ExtensionPropertyApiUpdateDto.fromJson(
              json['onUpdate'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$ExtensionPropertyApiDtoToJson(
        ExtensionPropertyApiDto instance) =>
    <String, dynamic>{
      'onGet': instance.onGet,
      'onCreate': instance.onCreate,
      'onUpdate': instance.onUpdate,
    };

ExtensionPropertyApiGetDto _$ExtensionPropertyApiGetDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyApiGetDto(
      isAvailable: json['isAvailable'] as bool,
    );

Map<String, dynamic> _$ExtensionPropertyApiGetDtoToJson(
        ExtensionPropertyApiGetDto instance) =>
    <String, dynamic>{
      'isAvailable': instance.isAvailable,
    };

ExtensionPropertyApiUpdateDto _$ExtensionPropertyApiUpdateDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyApiUpdateDto(
      isAvailable: json['isAvailable'] as bool,
    );

Map<String, dynamic> _$ExtensionPropertyApiUpdateDtoToJson(
        ExtensionPropertyApiUpdateDto instance) =>
    <String, dynamic>{
      'isAvailable': instance.isAvailable,
    };

ExtensionPropertyAttributeDto _$ExtensionPropertyAttributeDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyAttributeDto(
      typeSimple: json['typeSimple'] as String?,
      config: json['config'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ExtensionPropertyAttributeDtoToJson(
        ExtensionPropertyAttributeDto instance) =>
    <String, dynamic>{
      'typeSimple': instance.typeSimple,
      'config': instance.config,
    };

ExtensionPropertyDto _$ExtensionPropertyDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyDto(
      type: json['type'] as String?,
      typeSimple: json['typeSimple'] as String?,
      displayName: json['displayName'] == null
          ? null
          : LocalizableStringDto.fromJson(
              json['displayName'] as Map<String, dynamic>),
      api: json['api'] == null
          ? null
          : ExtensionPropertyApiDto.fromJson(
              json['api'] as Map<String, dynamic>),
      ui: json['ui'] == null
          ? null
          : ExtensionPropertyUiDto.fromJson(json['ui'] as Map<String, dynamic>),
      attributes: (json['attributes'] as List<dynamic>?)?.map((e) =>
          ExtensionPropertyAttributeDto.fromJson(e as Map<String, dynamic>)),
      configuration: json['configuration'] as Map<String, dynamic>?,
      defaultValue: json['defaultValue'],
    );

Map<String, dynamic> _$ExtensionPropertyDtoToJson(
        ExtensionPropertyDto instance) =>
    <String, dynamic>{
      'type': instance.type,
      'typeSimple': instance.typeSimple,
      'displayName': instance.displayName,
      'api': instance.api,
      'ui': instance.ui,
      'attributes': instance.attributes?.toList(),
      'configuration': instance.configuration,
      'defaultValue': instance.defaultValue,
    };

ExtensionPropertyUiDto _$ExtensionPropertyUiDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyUiDto(
      onTable: json['onTable'] == null
          ? null
          : ExtensionPropertyUiTableDto.fromJson(
              json['onTable'] as Map<String, dynamic>),
      onCreateForm: json['onCreateForm'] == null
          ? null
          : ExtensionPropertyUiFormDto.fromJson(
              json['onCreateForm'] as Map<String, dynamic>),
      onEditForm: json['onEditForm'] == null
          ? null
          : ExtensionPropertyUiFormDto.fromJson(
              json['onEditForm'] as Map<String, dynamic>),
      lookup: json['lookup'] == null
          ? null
          : ExtensionPropertyUiLookupDto.fromJson(
              json['lookup'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$ExtensionPropertyUiDtoToJson(
        ExtensionPropertyUiDto instance) =>
    <String, dynamic>{
      'onTable': instance.onTable,
      'onCreateForm': instance.onCreateForm,
      'onEditForm': instance.onEditForm,
      'lookup': instance.lookup,
    };

ExtensionPropertyUiFormDto _$ExtensionPropertyUiFormDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyUiFormDto(
      isAvailable: json['isAvailable'] as bool,
    );

Map<String, dynamic> _$ExtensionPropertyUiFormDtoToJson(
        ExtensionPropertyUiFormDto instance) =>
    <String, dynamic>{
      'isAvailable': instance.isAvailable,
    };

ExtensionPropertyUiLookupDto _$ExtensionPropertyUiLookupDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyUiLookupDto(
      url: json['url'] as String?,
      resultListPropertyName: json['resultListPropertyName'] as String?,
      displayPropertyName: json['displayPropertyName'] as String?,
      valuePropertyName: json['valuePropertyName'] as String?,
      filterParamName: json['filterParamName'] as String?,
    );

Map<String, dynamic> _$ExtensionPropertyUiLookupDtoToJson(
        ExtensionPropertyUiLookupDto instance) =>
    <String, dynamic>{
      'url': instance.url,
      'resultListPropertyName': instance.resultListPropertyName,
      'displayPropertyName': instance.displayPropertyName,
      'valuePropertyName': instance.valuePropertyName,
      'filterParamName': instance.filterParamName,
    };

ExtensionPropertyUiTableDto _$ExtensionPropertyUiTableDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensionPropertyUiTableDto(
      isAvailable: json['isAvailable'] as bool,
    );

Map<String, dynamic> _$ExtensionPropertyUiTableDtoToJson(
        ExtensionPropertyUiTableDto instance) =>
    <String, dynamic>{
      'isAvailable': instance.isAvailable,
    };

LocalizableStringDto _$LocalizableStringDtoFromJson(
        Map<String, dynamic> json) =>
    LocalizableStringDto(
      name: json['name'] as String?,
      resource: json['resource'] as String?,
    );

Map<String, dynamic> _$LocalizableStringDtoToJson(
        LocalizableStringDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'resource': instance.resource,
    };

ModuleExtensionDto _$ModuleExtensionDtoFromJson(Map<String, dynamic> json) =>
    ModuleExtensionDto(
      entities: (json['entities'] as Map<String, dynamic>?)?.map(
        (k, e) =>
            MapEntry(k, EntityExtensionDto.fromJson(e as Map<String, dynamic>)),
      ),
      configuration: json['configuration'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ModuleExtensionDtoToJson(ModuleExtensionDto instance) =>
    <String, dynamic>{
      'entities': instance.entities,
      'configuration': instance.configuration,
    };

ObjectExtensionsDto _$ObjectExtensionsDtoFromJson(Map<String, dynamic> json) =>
    ObjectExtensionsDto(
      modules: (json['modules'] as Map<String, dynamic>?)?.map(
        (k, e) =>
            MapEntry(k, ModuleExtensionDto.fromJson(e as Map<String, dynamic>)),
      ),
      enums: (json['enums'] as Map<String, dynamic>?)?.map(
        (k, e) =>
            MapEntry(k, ExtensionEnumDto.fromJson(e as Map<String, dynamic>)),
      ),
    );

Map<String, dynamic> _$ObjectExtensionsDtoToJson(
        ObjectExtensionsDto instance) =>
    <String, dynamic>{
      'modules': instance.modules,
      'enums': instance.enums,
    };
