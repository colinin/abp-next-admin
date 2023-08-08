import 'package:json_annotation/json_annotation.dart';

part 'abp.dto.g.dart';

@JsonSerializable()
class LocalizableStringInfo {
  LocalizableStringInfo({
    required this.resourceName,
    required this.name,
    this.values,
  });
  String resourceName;
  String name;
  Map<String, dynamic>? values;

  factory LocalizableStringInfo.fromJson(Map<String, dynamic> json) => _$LocalizableStringInfoFromJson(json);
  Map<String, dynamic> toJson() => _$LocalizableStringInfoToJson(this);
}

@JsonSerializable()
class ExtensibleObject {
  ExtensibleObject({
    this.extraProperties,
  });
  Map<String, dynamic>? extraProperties;

  factory ExtensibleObject.fromJson(Map<String, dynamic> json) => _$ExtensibleObjectFromJson(json);
  Map<String, dynamic> toJson() => _$ExtensibleObjectToJson(this);
}

@JsonSerializable(genericArgumentFactories: true)
class EntityDto<TPrimaryKey> {
  EntityDto({
    required this.id,
  });
  TPrimaryKey id;

  factory EntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$EntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);
 
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$EntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class CreationAuditedEntityDto<TPrimaryKey> extends EntityDto<TPrimaryKey> {
  CreationAuditedEntityDto({
    required super.id,
    required this.creationTime,
    this.creatorId,
  });
  DateTime creationTime;
  String? creatorId;

  factory CreationAuditedEntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$CreationAuditedEntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);

  @override
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$CreationAuditedEntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends CreationAuditedEntityDto<TPrimaryKey> {
  CreationAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required super.creationTime,
    super.creatorId,
  });
  TUserDto creator;

  factory CreationAuditedEntityWithUserDto.fromJson(
    Map<String, dynamic> json,
    {
      TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey,
      TUserDto Function(Object? json)? fromJsonTUserDto,
    }) => 
      _$CreationAuditedEntityWithUserDtoFromJson(json,
        fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey,
        fromJsonTUserDto ?? (json) => json as TUserDto);

  @override
  Map<String, dynamic> toJson(
    {
      Object? Function(TPrimaryKey value)? toJsonTPrimaryKey,
      Object? Function(TUserDto value)? toJsonTUserDto,
    }) => 
      _$CreationAuditedEntityWithUserDtoToJson(this,
      toJsonTPrimaryKey ?? (instance) => instance,
      toJsonTUserDto ?? (instance) => instance);
}

@JsonSerializable(genericArgumentFactories: true)
class AuditedEntityDto<TPrimaryKey> extends CreationAuditedEntityDto<TPrimaryKey> {
  AuditedEntityDto({
    required super.id,
    required super.creationTime,
    super.creatorId,
    this.lastModificationTime,
    this.lastModifierId,
  });
  DateTime? lastModificationTime;
  String? lastModifierId;

  factory AuditedEntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$AuditedEntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);

  @override
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$AuditedEntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class AuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends AuditedEntityDto<TPrimaryKey> {
  AuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  TUserDto creator;
  TUserDto lastModifier;

  factory AuditedEntityWithUserDto.fromJson(
    Map<String, dynamic> json,
    {
      TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey,
      TUserDto Function(Object? json)? fromJsonTUserDto,
    }) => 
      _$AuditedEntityWithUserDtoFromJson(json,
        fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey,
        fromJsonTUserDto ?? (json) => json as TUserDto);

  @override
  Map<String, dynamic> toJson(
    {
      Object? Function(TPrimaryKey value)? toJsonTPrimaryKey,
      Object? Function(TUserDto value)? toJsonTUserDto,
    }) => 
      _$AuditedEntityWithUserDtoToJson(this,
      toJsonTPrimaryKey ?? (instance) => instance,
      toJsonTUserDto ?? (instance) => instance);
}

@JsonSerializable(genericArgumentFactories: true)
class FullAuditedEntityDto<TPrimaryKey> extends AuditedEntityDto<TPrimaryKey> {
  FullAuditedEntityDto({
    required super.id,
    required this.isDeleted,
    this.deleterId,
    this.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  bool isDeleted;
  String? deleterId;
  DateTime? deletionTime;

  factory FullAuditedEntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$FullAuditedEntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);

  @override
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$FullAuditedEntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends FullAuditedEntityDto<TPrimaryKey> {
  FullAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required this.deleter,
    required super.isDeleted,
    super.deleterId,
    super.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  TUserDto creator;
  TUserDto lastModifier;
  TUserDto deleter;

  factory FullAuditedEntityWithUserDto.fromJson(
    Map<String, dynamic> json,
    {
      TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey,
      TUserDto Function(Object? json)? fromJsonTUserDto,
    }) => 
      _$FullAuditedEntityWithUserDtoFromJson(json,
        fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey,
        fromJsonTUserDto ?? (json) => json as TUserDto);

  @override
  Map<String, dynamic> toJson(
    {
      Object? Function(TPrimaryKey value)? toJsonTPrimaryKey,
      Object? Function(TUserDto value)? toJsonTUserDto,
    }) => 
      _$FullAuditedEntityWithUserDtoToJson(this,
      toJsonTPrimaryKey ?? (instance) => instance,
      toJsonTUserDto ?? (instance) => instance);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleEntityDto<TPrimaryKey> extends ExtensibleObject {
  ExtensibleEntityDto({
    required this.id,
    super.extraProperties,
  });
  TPrimaryKey id;

  factory ExtensibleEntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$ExtensibleEntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);

  @override
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$ExtensibleEntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleCreationAuditedEntityDto<TPrimaryKey> extends ExtensibleEntityDto<TPrimaryKey> {
  ExtensibleCreationAuditedEntityDto({
    required super.id,
    required this.creationTime,
    this.creatorId,
    super.extraProperties,
  });
  DateTime creationTime;
  String? creatorId;

  factory ExtensibleCreationAuditedEntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$ExtensibleCreationAuditedEntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);

  @override
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$ExtensibleCreationAuditedEntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  ExtensibleCreationAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required super.creationTime,
    super.creatorId,
    super.extraProperties,
  });
  TUserDto creator;

  factory ExtensibleCreationAuditedEntityWithUserDto.fromJson(
    Map<String, dynamic> json,
    {
      TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey,
      TUserDto Function(Object? json)? fromJsonTUserDto,
    }) => 
      _$ExtensibleCreationAuditedEntityWithUserDtoFromJson(json,
        fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey,
        fromJsonTUserDto ?? (json) => json as TUserDto);

  @override
  Map<String, dynamic> toJson(
    {
      Object? Function(TPrimaryKey value)? toJsonTPrimaryKey,
      Object? Function(TUserDto value)? toJsonTUserDto,
    }) => 
      _$ExtensibleCreationAuditedEntityWithUserDtoToJson(this,
      toJsonTPrimaryKey ?? (instance) => instance,
      toJsonTUserDto ?? (instance) => instance);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleAuditedEntityDto<TPrimaryKey> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  ExtensibleAuditedEntityDto({
    required super.id,
    required super.creationTime,
    super.creatorId,
    this.lastModificationTime,
    this.lastModifierId,
    super.extraProperties,
  });
  DateTime? lastModificationTime;
  String? lastModifierId;

  factory ExtensibleAuditedEntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$ExtensibleAuditedEntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);

  @override
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$ExtensibleAuditedEntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  ExtensibleAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
    super.extraProperties,
  });
  TUserDto creator;
  TUserDto lastModifier;

  factory ExtensibleAuditedEntityWithUserDto.fromJson(
    Map<String, dynamic> json,
    {
      TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey,
      TUserDto Function(Object? json)? fromJsonTUserDto,
    }) => 
      _$ExtensibleAuditedEntityWithUserDtoFromJson(json,
        fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey,
        fromJsonTUserDto ?? (json) => json as TUserDto);

  @override
  Map<String, dynamic> toJson(
    {
      Object? Function(TPrimaryKey value)? toJsonTPrimaryKey,
      Object? Function(TUserDto value)? toJsonTUserDto,
    }) => 
      _$ExtensibleAuditedEntityWithUserDtoToJson(this,
      toJsonTPrimaryKey ?? (instance) => instance,
      toJsonTUserDto ?? (instance) => instance);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleFullAuditedEntityDto<TPrimaryKey> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  ExtensibleFullAuditedEntityDto({
    required super.id,
    required this.isDeleted,
    this.deleterId,
    this.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
    super.extraProperties,
  });
  bool isDeleted;
  String? deleterId;
  DateTime? deletionTime;

  factory ExtensibleFullAuditedEntityDto.fromJson(
    Map<String, dynamic> json,
    { TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey }) => 
      _$ExtensibleFullAuditedEntityDtoFromJson(json, fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey);

  @override
  Map<String, dynamic> toJson({ Object? Function(TPrimaryKey value)? toJsonTPrimaryKey }) => 
      _$ExtensibleFullAuditedEntityDtoToJson(this, toJsonTPrimaryKey ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends ExtensibleFullAuditedEntityDto<TPrimaryKey> {
  ExtensibleFullAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required this.deleter,
    required super.isDeleted,
    super.deleterId,
    super.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
    super.extraProperties,
  });
  TUserDto creator;
  TUserDto lastModifier;
  TUserDto deleter;

  factory ExtensibleFullAuditedEntityWithUserDto.fromJson(
    Map<String, dynamic> json,
    {
      TPrimaryKey Function(Object? json)? fromJsonTPrimaryKey,
      TUserDto Function(Object? json)? fromJsonTUserDto,
    }) => 
      _$ExtensibleFullAuditedEntityWithUserDtoFromJson(json,
        fromJsonTPrimaryKey ?? (json) => json as TPrimaryKey,
        fromJsonTUserDto ?? (json) => json as TUserDto);

  @override
  Map<String, dynamic> toJson(
    {
      Object? Function(TPrimaryKey value)? toJsonTPrimaryKey,
      Object? Function(TUserDto value)? toJsonTUserDto,
    }) => 
      _$ExtensibleFullAuditedEntityWithUserDtoToJson(this,
      toJsonTPrimaryKey ?? (instance) => instance,
      toJsonTUserDto ?? (instance) => instance);
}

@JsonSerializable(genericArgumentFactories: true)
class ListResultDto<T> {
  ListResultDto({
    required this.items,
  });
  List<T> items;

  factory ListResultDto.fromJson(
    Map<String, dynamic> json,
    { T Function(Object? json)? fromJsonT }) => 
      _$ListResultDtoFromJson(json, fromJsonT ?? (json) => json as T);

  Map<String, dynamic> toJson({ Object? Function(T value)? toJsonT }) => 
      _$ListResultDtoToJson(this, toJsonT ?? (json) => json);
}

@JsonSerializable()
class LimitedResultRequestDto {
  LimitedResultRequestDto({
    this.maxResultCount,
  });
  int? maxResultCount = 10;

  factory LimitedResultRequestDto.fromJson(Map<String, dynamic> json) => _$LimitedResultRequestDtoFromJson(json);

  Map<String, dynamic> toJson() => _$LimitedResultRequestDtoToJson(this);
}

@JsonSerializable()
class ExtensibleLimitedResultRequestDto extends LimitedResultRequestDto {
  ExtensibleLimitedResultRequestDto({
    super.maxResultCount,
    this.extraProperties,
  });
  Map<String, dynamic>? extraProperties;

  factory ExtensibleLimitedResultRequestDto.fromJson(Map<String, dynamic> json) => _$ExtensibleLimitedResultRequestDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$ExtensibleLimitedResultRequestDtoToJson(this);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensibleListResultDto<T> extends ListResultDto<T> {
  ExtensibleListResultDto({
    required super.items,
    this.extraProperties,
  });
  Map<String, dynamic>? extraProperties;

  factory ExtensibleListResultDto.fromJson(
    Map<String, dynamic> json,
    { T Function(Object? json)? fromJsonT }) => 
      _$ExtensibleListResultDtoFromJson(json, fromJsonT ?? (json) => json as T);

  @override
  Map<String, dynamic> toJson({ Object? Function(T value)? toJsonT }) => 
      _$ExtensibleListResultDtoToJson(this, toJsonT ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class PagedResultDto<T> extends ListResultDto<T> {
  PagedResultDto({
    required this.totalCount,
    required super.items,
  });
  int totalCount;

  factory PagedResultDto.fromJson(
    Map<String, dynamic> json,
    { T Function(Object? json)? fromJsonT }) => 
      _$PagedResultDtoFromJson(json, fromJsonT ?? (json) => json as T);

  @override
  Map<String, dynamic> toJson({ Object? Function(T value)? toJsonT }) => 
      _$PagedResultDtoToJson(this, toJsonT ?? (json) => json);
}

@JsonSerializable(genericArgumentFactories: true)
class ExtensiblePagedResultDto<T> extends PagedResultDto<T> {
  ExtensiblePagedResultDto({
    required super.totalCount,
    required super.items,
    this.extraProperties,
  });
  Map<String, dynamic>? extraProperties;

  factory ExtensiblePagedResultDto.fromJson(
    Map<String, dynamic> json,
    { T Function(Object? json)? fromJsonT }) => 
      _$ExtensiblePagedResultDtoFromJson(json, fromJsonT ?? (json) => json as T);

  @override
  Map<String, dynamic> toJson({ Object? Function(T value)? toJsonT }) => 
      _$ExtensiblePagedResultDtoToJson(this, toJsonT ?? (json) => json);
}

@JsonSerializable()
class SortedResultRequest {
  SortedResultRequest({
    this.sorting,
  });
  String? sorting;

  factory SortedResultRequest.fromJson(Map<String, dynamic> json) => _$SortedResultRequestFromJson(json);

  Map<String, dynamic> toJson() => _$SortedResultRequestToJson(this);
}

@JsonSerializable()
class PagedResultRequestDto extends LimitedResultRequestDto {
  PagedResultRequestDto({
    this.skipCount,
    super.maxResultCount,
  });
  int? skipCount = 0;

  factory PagedResultRequestDto.fromJson(Map<String, dynamic> json) => _$PagedResultRequestDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$PagedResultRequestDtoToJson(this);
}

@JsonSerializable()
class PagedAndSortedResultRequestDto extends PagedResultRequestDto {
  PagedAndSortedResultRequestDto({
    super.skipCount,
    super.maxResultCount,
    this.sorting,
  });
  String? sorting;

  factory PagedAndSortedResultRequestDto.fromJson(Map<String, dynamic> json) => _$PagedAndSortedResultRequestDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$PagedAndSortedResultRequestDtoToJson(this);
}

@JsonSerializable()
class ExtensiblePagedAndSortedResultRequestDto extends PagedAndSortedResultRequestDto {
  ExtensiblePagedAndSortedResultRequestDto({
    super.skipCount,
    super.maxResultCount,
    super.sorting,
    this.extraProperties,
  });
  Map<String, dynamic>? extraProperties;

  factory ExtensiblePagedAndSortedResultRequestDto.fromJson(Map<String, dynamic> json) => _$ExtensiblePagedAndSortedResultRequestDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$ExtensiblePagedAndSortedResultRequestDtoToJson(this);
}

@JsonSerializable()
class ExtensiblePagedResultRequestDto extends PagedResultRequestDto {
  ExtensiblePagedResultRequestDto({
    super.skipCount,
    super.maxResultCount,
    this.extraProperties,
  });
  Map<String, dynamic>? extraProperties;

  factory ExtensiblePagedResultRequestDto.fromJson(Map<String, dynamic> json) => _$ExtensiblePagedResultRequestDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$ExtensiblePagedResultRequestDtoToJson(this);
}
