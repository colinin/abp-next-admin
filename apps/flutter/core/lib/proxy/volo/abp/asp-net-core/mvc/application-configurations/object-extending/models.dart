import 'package:json_annotation/json_annotation.dart';

part 'models.g.dart';

@JsonSerializable()
class EntityExtensionDto {
  EntityExtensionDto({
    this.properties,
    this.configuration,
  });
  Map<String, ExtensionPropertyDto>? properties;
  Map<String, dynamic>? configuration;

  factory EntityExtensionDto.fromJson(Map<String, dynamic> json) => _$EntityExtensionDtoFromJson(json);
  Map<String, dynamic> toJson() => _$EntityExtensionDtoToJson(this);
}

@JsonSerializable()
class ExtensionEnumDto {
  ExtensionEnumDto({
    this.localizationResource,
    this.fields,
  });
  String? localizationResource;
  Iterable<ExtensionEnumFieldDto>? fields = [];

  factory ExtensionEnumDto.fromJson(Map<String, dynamic> json) => _$ExtensionEnumDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionEnumDtoToJson(this);
}

@JsonSerializable()
class ExtensionEnumFieldDto {
  ExtensionEnumFieldDto({
    this.name,
    this.value,
  });
  String? name;
  dynamic value;

  factory ExtensionEnumFieldDto.fromJson(Map<String, dynamic> json) => _$ExtensionEnumFieldDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionEnumFieldDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyApiCreateDto {
  ExtensionPropertyApiCreateDto({
    required this.isAvailable,
  });
  final bool isAvailable;

  factory ExtensionPropertyApiCreateDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyApiCreateDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyApiCreateDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyApiDto {
  ExtensionPropertyApiDto({
    this.onGet,
    this.onCreate,
    this.onUpdate,
  });
  ExtensionPropertyApiGetDto? onGet;
  ExtensionPropertyApiCreateDto? onCreate;
  ExtensionPropertyApiUpdateDto? onUpdate;

  factory ExtensionPropertyApiDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyApiDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyApiDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyApiGetDto {
  ExtensionPropertyApiGetDto({
    required this.isAvailable,
  });
  final bool isAvailable;

  factory ExtensionPropertyApiGetDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyApiGetDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyApiGetDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyApiUpdateDto {
  ExtensionPropertyApiUpdateDto({
    required this.isAvailable,
  });
  final bool isAvailable;

  factory ExtensionPropertyApiUpdateDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyApiUpdateDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyApiUpdateDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyAttributeDto {
  ExtensionPropertyAttributeDto({
    this.typeSimple,
    this.config,
  });
  String? typeSimple;
  Map<String, dynamic>? config;

  factory ExtensionPropertyAttributeDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyAttributeDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyAttributeDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyDto {
  ExtensionPropertyDto({
    this.type,
    this.typeSimple,
    this.displayName,
    this.api,
    this.ui,
    this.attributes,
    this.configuration,
    this.defaultValue,
  });
  String? type;
  String? typeSimple;
  LocalizableStringDto? displayName;
  ExtensionPropertyApiDto? api;
  ExtensionPropertyUiDto? ui;
  Iterable<ExtensionPropertyAttributeDto>? attributes = [];
  Map<String, dynamic>? configuration;
  dynamic defaultValue;

  factory ExtensionPropertyDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyUiDto {
  ExtensionPropertyUiDto({
    this.onTable,
    this.onCreateForm,
    this.onEditForm,
    this.lookup,
  });
  ExtensionPropertyUiTableDto? onTable;
  ExtensionPropertyUiFormDto? onCreateForm;
  ExtensionPropertyUiFormDto? onEditForm;
  ExtensionPropertyUiLookupDto? lookup;

  factory ExtensionPropertyUiDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyUiDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyUiDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyUiFormDto {
  ExtensionPropertyUiFormDto({
    required this.isAvailable,
  });
  final bool isAvailable;

  factory ExtensionPropertyUiFormDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyUiFormDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyUiFormDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyUiLookupDto {
  ExtensionPropertyUiLookupDto({
    this.url,
    this.resultListPropertyName,
    this.displayPropertyName,
    this.valuePropertyName,
    this.filterParamName,
  });
  String? url;
  String? resultListPropertyName;
  String? displayPropertyName;
  String? valuePropertyName;
  String? filterParamName;

  factory ExtensionPropertyUiLookupDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyUiLookupDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyUiLookupDtoToJson(this);
}

@JsonSerializable()
class ExtensionPropertyUiTableDto {
  ExtensionPropertyUiTableDto({
    required this.isAvailable,
  });
  final bool isAvailable;

  factory ExtensionPropertyUiTableDto.fromJson(Map<String, dynamic> json) => _$ExtensionPropertyUiTableDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensionPropertyUiTableDtoToJson(this);
}

@JsonSerializable()
class LocalizableStringDto {
  LocalizableStringDto({
    this.name,
    this.resource,
  });
  String? name;
  String? resource;

  factory LocalizableStringDto.fromJson(Map<String, dynamic> json) => _$LocalizableStringDtoFromJson(json);
  Map<String, dynamic> toJson() => _$LocalizableStringDtoToJson(this);
}

@JsonSerializable()
class ModuleExtensionDto {
  ModuleExtensionDto({
    this.entities,
    this.configuration,
  });
  Map<String, EntityExtensionDto>? entities;
  Map<String, dynamic>? configuration;

  factory ModuleExtensionDto.fromJson(Map<String, dynamic> json) => _$ModuleExtensionDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ModuleExtensionDtoToJson(this);
}

@JsonSerializable()
class ObjectExtensionsDto {
  ObjectExtensionsDto({
    this.modules,
    this.enums,
  });
  Map<String, ModuleExtensionDto>? modules;
  Map<String, ExtensionEnumDto>? enums;

  factory ObjectExtensionsDto.fromJson(Map<String, dynamic> json) => _$ObjectExtensionsDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ObjectExtensionsDtoToJson(this);
}