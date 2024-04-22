// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'abp.dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LocalizableStringInfo _$LocalizableStringInfoFromJson(
        Map<String, dynamic> json) =>
    LocalizableStringInfo(
      resourceName: (json['resourceName'] ?? json['ResourceName']) as String,
      name: (json['name'] ?? json['Name']) as String,
      values: (json['values'] ?? json['Values']) as Map<String, dynamic>?,
    );

Map<String, dynamic> _$LocalizableStringInfoToJson(
        LocalizableStringInfo instance) =>
    <String, dynamic>{
      'resourceName': instance.resourceName,
      'name': instance.name,
      'values': instance.values,
    };

ExtensibleObject _$ExtensibleObjectFromJson(Map<String, dynamic> json) =>
    ExtensibleObject(
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ExtensibleObjectToJson(ExtensibleObject instance) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
    };

EntityDto<TPrimaryKey> _$EntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
    EntityDto<TPrimaryKey>(
      id: fromJsonTPrimaryKey(json['id']),
    );

Map<String, dynamic> _$EntityDtoToJson<TPrimaryKey>(
  EntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'id': toJsonTPrimaryKey(instance.id),
    };

CreationAuditedEntityDto<TPrimaryKey>
    _$CreationAuditedEntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
        CreationAuditedEntityDto<TPrimaryKey>(
          id: fromJsonTPrimaryKey(json['id']),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
        );

Map<String, dynamic> _$CreationAuditedEntityDtoToJson<TPrimaryKey>(
  CreationAuditedEntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'id': toJsonTPrimaryKey(instance.id),
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
    };

CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
    _$CreationAuditedEntityWithUserDtoFromJson<TPrimaryKey, TUserDto>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
  TUserDto Function(Object? json) fromJsonTUserDto,
) =>
        CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>(
          id: fromJsonTPrimaryKey(json['id']),
          creator: fromJsonTUserDto(json['creator']),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
        );

Map<String, dynamic>
    _$CreationAuditedEntityWithUserDtoToJson<TPrimaryKey, TUserDto>(
  CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
  Object? Function(TUserDto value) toJsonTUserDto,
) =>
        <String, dynamic>{
          'id': toJsonTPrimaryKey(instance.id),
          'creationTime': instance.creationTime.toIso8601String(),
          'creatorId': instance.creatorId,
          'creator': toJsonTUserDto(instance.creator),
        };

AuditedEntityDto<TPrimaryKey> _$AuditedEntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
    AuditedEntityDto<TPrimaryKey>(
      id: fromJsonTPrimaryKey(json['id']),
      creationTime: DateTime.parse(json['creationTime'] as String),
      creatorId: json['creatorId'] as String?,
      lastModificationTime: json['lastModificationTime'] == null
          ? null
          : DateTime.parse(json['lastModificationTime'] as String),
      lastModifierId: json['lastModifierId'] as String?,
    );

Map<String, dynamic> _$AuditedEntityDtoToJson<TPrimaryKey>(
  AuditedEntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'id': toJsonTPrimaryKey(instance.id),
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
      'lastModifierId': instance.lastModifierId,
    };

AuditedEntityWithUserDto<TPrimaryKey, TUserDto>
    _$AuditedEntityWithUserDtoFromJson<TPrimaryKey, TUserDto>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
  TUserDto Function(Object? json) fromJsonTUserDto,
) =>
        AuditedEntityWithUserDto<TPrimaryKey, TUserDto>(
          id: fromJsonTPrimaryKey(json['id']),
          creator: fromJsonTUserDto(json['creator']),
          lastModifier: fromJsonTUserDto(json['lastModifier']),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          lastModificationTime: json['lastModificationTime'] == null
              ? null
              : DateTime.parse(json['lastModificationTime'] as String),
          lastModifierId: json['lastModifierId'] as String?,
        );

Map<String, dynamic> _$AuditedEntityWithUserDtoToJson<TPrimaryKey, TUserDto>(
  AuditedEntityWithUserDto<TPrimaryKey, TUserDto> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
  Object? Function(TUserDto value) toJsonTUserDto,
) =>
    <String, dynamic>{
      'id': toJsonTPrimaryKey(instance.id),
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
      'lastModifierId': instance.lastModifierId,
      'creator': toJsonTUserDto(instance.creator),
      'lastModifier': toJsonTUserDto(instance.lastModifier),
    };

FullAuditedEntityDto<TPrimaryKey> _$FullAuditedEntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
    FullAuditedEntityDto<TPrimaryKey>(
      id: fromJsonTPrimaryKey(json['id']),
      isDeleted: json['isDeleted'] as bool,
      deleterId: json['deleterId'] as String?,
      deletionTime: json['deletionTime'] == null
          ? null
          : DateTime.parse(json['deletionTime'] as String),
      creationTime: DateTime.parse(json['creationTime'] as String),
      creatorId: json['creatorId'] as String?,
      lastModificationTime: json['lastModificationTime'] == null
          ? null
          : DateTime.parse(json['lastModificationTime'] as String),
      lastModifierId: json['lastModifierId'] as String?,
    );

Map<String, dynamic> _$FullAuditedEntityDtoToJson<TPrimaryKey>(
  FullAuditedEntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'id': toJsonTPrimaryKey(instance.id),
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
      'lastModifierId': instance.lastModifierId,
      'isDeleted': instance.isDeleted,
      'deleterId': instance.deleterId,
      'deletionTime': instance.deletionTime?.toIso8601String(),
    };

FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
    _$FullAuditedEntityWithUserDtoFromJson<TPrimaryKey, TUserDto>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
  TUserDto Function(Object? json) fromJsonTUserDto,
) =>
        FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>(
          id: fromJsonTPrimaryKey(json['id']),
          creator: fromJsonTUserDto(json['creator']),
          lastModifier: fromJsonTUserDto(json['lastModifier']),
          deleter: fromJsonTUserDto(json['deleter']),
          isDeleted: json['isDeleted'] as bool,
          deleterId: json['deleterId'] as String?,
          deletionTime: json['deletionTime'] == null
              ? null
              : DateTime.parse(json['deletionTime'] as String),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          lastModificationTime: json['lastModificationTime'] == null
              ? null
              : DateTime.parse(json['lastModificationTime'] as String),
          lastModifierId: json['lastModifierId'] as String?,
        );

Map<String, dynamic>
    _$FullAuditedEntityWithUserDtoToJson<TPrimaryKey, TUserDto>(
  FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
  Object? Function(TUserDto value) toJsonTUserDto,
) =>
        <String, dynamic>{
          'id': toJsonTPrimaryKey(instance.id),
          'creationTime': instance.creationTime.toIso8601String(),
          'creatorId': instance.creatorId,
          'lastModificationTime':
              instance.lastModificationTime?.toIso8601String(),
          'lastModifierId': instance.lastModifierId,
          'isDeleted': instance.isDeleted,
          'deleterId': instance.deleterId,
          'deletionTime': instance.deletionTime?.toIso8601String(),
          'creator': toJsonTUserDto(instance.creator),
          'lastModifier': toJsonTUserDto(instance.lastModifier),
          'deleter': toJsonTUserDto(instance.deleter),
        };

ExtensibleEntityDto<TPrimaryKey> _$ExtensibleEntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
    ExtensibleEntityDto<TPrimaryKey>(
      id: fromJsonTPrimaryKey(json['id']),
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ExtensibleEntityDtoToJson<TPrimaryKey>(
  ExtensibleEntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
      'id': toJsonTPrimaryKey(instance.id),
    };

ExtensibleCreationAuditedEntityDto<TPrimaryKey>
    _$ExtensibleCreationAuditedEntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
        ExtensibleCreationAuditedEntityDto<TPrimaryKey>(
          id: fromJsonTPrimaryKey(json['id']),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          extraProperties: json['extraProperties'] as Map<String, dynamic>?,
        );

Map<String, dynamic> _$ExtensibleCreationAuditedEntityDtoToJson<TPrimaryKey>(
  ExtensibleCreationAuditedEntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
      'id': toJsonTPrimaryKey(instance.id),
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
    };

ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
    _$ExtensibleCreationAuditedEntityWithUserDtoFromJson<TPrimaryKey, TUserDto>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
  TUserDto Function(Object? json) fromJsonTUserDto,
) =>
        ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>(
          id: fromJsonTPrimaryKey(json['id']),
          creator: fromJsonTUserDto(json['creator']),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          extraProperties: json['extraProperties'] as Map<String, dynamic>?,
        );

Map<String, dynamic>
    _$ExtensibleCreationAuditedEntityWithUserDtoToJson<TPrimaryKey, TUserDto>(
  ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
  Object? Function(TUserDto value) toJsonTUserDto,
) =>
        <String, dynamic>{
          'extraProperties': instance.extraProperties,
          'id': toJsonTPrimaryKey(instance.id),
          'creationTime': instance.creationTime.toIso8601String(),
          'creatorId': instance.creatorId,
          'creator': toJsonTUserDto(instance.creator),
        };

ExtensibleAuditedEntityDto<TPrimaryKey>
    _$ExtensibleAuditedEntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
        ExtensibleAuditedEntityDto<TPrimaryKey>(
          id: fromJsonTPrimaryKey(json['id']),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          lastModificationTime: json['lastModificationTime'] == null
              ? null
              : DateTime.parse(json['lastModificationTime'] as String),
          lastModifierId: json['lastModifierId'] as String?,
          extraProperties: json['extraProperties'] as Map<String, dynamic>?,
        );

Map<String, dynamic> _$ExtensibleAuditedEntityDtoToJson<TPrimaryKey>(
  ExtensibleAuditedEntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
      'id': toJsonTPrimaryKey(instance.id),
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
      'lastModifierId': instance.lastModifierId,
    };

ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
    _$ExtensibleAuditedEntityWithUserDtoFromJson<TPrimaryKey, TUserDto>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
  TUserDto Function(Object? json) fromJsonTUserDto,
) =>
        ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto>(
          id: fromJsonTPrimaryKey(json['id']),
          creator: fromJsonTUserDto(json['creator']),
          lastModifier: fromJsonTUserDto(json['lastModifier']),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          lastModificationTime: json['lastModificationTime'] == null
              ? null
              : DateTime.parse(json['lastModificationTime'] as String),
          lastModifierId: json['lastModifierId'] as String?,
          extraProperties: json['extraProperties'] as Map<String, dynamic>?,
        );

Map<String, dynamic>
    _$ExtensibleAuditedEntityWithUserDtoToJson<TPrimaryKey, TUserDto>(
  ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
  Object? Function(TUserDto value) toJsonTUserDto,
) =>
        <String, dynamic>{
          'extraProperties': instance.extraProperties,
          'id': toJsonTPrimaryKey(instance.id),
          'creationTime': instance.creationTime.toIso8601String(),
          'creatorId': instance.creatorId,
          'lastModificationTime':
              instance.lastModificationTime?.toIso8601String(),
          'lastModifierId': instance.lastModifierId,
          'creator': toJsonTUserDto(instance.creator),
          'lastModifier': toJsonTUserDto(instance.lastModifier),
        };

ExtensibleFullAuditedEntityDto<TPrimaryKey>
    _$ExtensibleFullAuditedEntityDtoFromJson<TPrimaryKey>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
) =>
        ExtensibleFullAuditedEntityDto<TPrimaryKey>(
          id: fromJsonTPrimaryKey(json['id']),
          isDeleted: json['isDeleted'] as bool,
          deleterId: json['deleterId'] as String?,
          deletionTime: json['deletionTime'] == null
              ? null
              : DateTime.parse(json['deletionTime'] as String),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          lastModificationTime: json['lastModificationTime'] == null
              ? null
              : DateTime.parse(json['lastModificationTime'] as String),
          lastModifierId: json['lastModifierId'] as String?,
          extraProperties: json['extraProperties'] as Map<String, dynamic>?,
        );

Map<String, dynamic> _$ExtensibleFullAuditedEntityDtoToJson<TPrimaryKey>(
  ExtensibleFullAuditedEntityDto<TPrimaryKey> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
      'id': toJsonTPrimaryKey(instance.id),
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
      'lastModifierId': instance.lastModifierId,
      'isDeleted': instance.isDeleted,
      'deleterId': instance.deleterId,
      'deletionTime': instance.deletionTime?.toIso8601String(),
    };

ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
    _$ExtensibleFullAuditedEntityWithUserDtoFromJson<TPrimaryKey, TUserDto>(
  Map<String, dynamic> json,
  TPrimaryKey Function(Object? json) fromJsonTPrimaryKey,
  TUserDto Function(Object? json) fromJsonTUserDto,
) =>
        ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>(
          id: fromJsonTPrimaryKey(json['id']),
          creator: fromJsonTUserDto(json['creator']),
          lastModifier: fromJsonTUserDto(json['lastModifier']),
          deleter: fromJsonTUserDto(json['deleter']),
          isDeleted: json['isDeleted'] as bool,
          deleterId: json['deleterId'] as String?,
          deletionTime: json['deletionTime'] == null
              ? null
              : DateTime.parse(json['deletionTime'] as String),
          creationTime: DateTime.parse(json['creationTime'] as String),
          creatorId: json['creatorId'] as String?,
          lastModificationTime: json['lastModificationTime'] == null
              ? null
              : DateTime.parse(json['lastModificationTime'] as String),
          lastModifierId: json['lastModifierId'] as String?,
          extraProperties: json['extraProperties'] as Map<String, dynamic>?,
        );

Map<String, dynamic>
    _$ExtensibleFullAuditedEntityWithUserDtoToJson<TPrimaryKey, TUserDto>(
  ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> instance,
  Object? Function(TPrimaryKey value) toJsonTPrimaryKey,
  Object? Function(TUserDto value) toJsonTUserDto,
) =>
        <String, dynamic>{
          'extraProperties': instance.extraProperties,
          'id': toJsonTPrimaryKey(instance.id),
          'creationTime': instance.creationTime.toIso8601String(),
          'creatorId': instance.creatorId,
          'lastModificationTime':
              instance.lastModificationTime?.toIso8601String(),
          'lastModifierId': instance.lastModifierId,
          'isDeleted': instance.isDeleted,
          'deleterId': instance.deleterId,
          'deletionTime': instance.deletionTime?.toIso8601String(),
          'creator': toJsonTUserDto(instance.creator),
          'lastModifier': toJsonTUserDto(instance.lastModifier),
          'deleter': toJsonTUserDto(instance.deleter),
        };

ListResultDto<T> _$ListResultDtoFromJson<T>(
  Map<String, dynamic> json,
  T Function(Object? json) fromJsonT,
) =>
    ListResultDto<T>(
      items: (json['items'] as List<dynamic>).map(fromJsonT).toList(),
    );

Map<String, dynamic> _$ListResultDtoToJson<T>(
  ListResultDto<T> instance,
  Object? Function(T value) toJsonT,
) =>
    <String, dynamic>{
      'items': instance.items.map(toJsonT).toList(),
    };

LimitedResultRequestDto _$LimitedResultRequestDtoFromJson(
        Map<String, dynamic> json) =>
    LimitedResultRequestDto(
      maxResultCount: json['maxResultCount'] as int?,
    );

Map<String, dynamic> _$LimitedResultRequestDtoToJson(
        LimitedResultRequestDto instance) =>
    <String, dynamic>{
      'maxResultCount': instance.maxResultCount,
    };

ExtensibleLimitedResultRequestDto _$ExtensibleLimitedResultRequestDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensibleLimitedResultRequestDto(
      maxResultCount: json['maxResultCount'] as int?,
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ExtensibleLimitedResultRequestDtoToJson(
        ExtensibleLimitedResultRequestDto instance) =>
    <String, dynamic>{
      'maxResultCount': instance.maxResultCount,
      'extraProperties': instance.extraProperties,
    };

ExtensibleListResultDto<T> _$ExtensibleListResultDtoFromJson<T>(
  Map<String, dynamic> json,
  T Function(Object? json) fromJsonT,
) =>
    ExtensibleListResultDto<T>(
      items: (json['items'] as List<dynamic>).map(fromJsonT).toList(),
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ExtensibleListResultDtoToJson<T>(
  ExtensibleListResultDto<T> instance,
  Object? Function(T value) toJsonT,
) =>
    <String, dynamic>{
      'items': instance.items.map(toJsonT).toList(),
      'extraProperties': instance.extraProperties,
    };

PagedResultDto<T> _$PagedResultDtoFromJson<T>(
  Map<String, dynamic> json,
  T Function(Object? json) fromJsonT,
) =>
    PagedResultDto<T>(
      totalCount: json['totalCount'] as int,
      items: (json['items'] as List<dynamic>).map(fromJsonT).toList(),
    );

Map<String, dynamic> _$PagedResultDtoToJson<T>(
  PagedResultDto<T> instance,
  Object? Function(T value) toJsonT,
) =>
    <String, dynamic>{
      'items': instance.items.map(toJsonT).toList(),
      'totalCount': instance.totalCount,
    };

ExtensiblePagedResultDto<T> _$ExtensiblePagedResultDtoFromJson<T>(
  Map<String, dynamic> json,
  T Function(Object? json) fromJsonT,
) =>
    ExtensiblePagedResultDto<T>(
      totalCount: json['totalCount'] as int,
      items: (json['items'] as List<dynamic>).map(fromJsonT).toList(),
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ExtensiblePagedResultDtoToJson<T>(
  ExtensiblePagedResultDto<T> instance,
  Object? Function(T value) toJsonT,
) =>
    <String, dynamic>{
      'items': instance.items.map(toJsonT).toList(),
      'totalCount': instance.totalCount,
      'extraProperties': instance.extraProperties,
    };

SortedResultRequest _$SortedResultRequestFromJson(Map<String, dynamic> json) =>
    SortedResultRequest(
      sorting: json['sorting'] as String?,
    );

Map<String, dynamic> _$SortedResultRequestToJson(
        SortedResultRequest instance) =>
    <String, dynamic>{
      'sorting': instance.sorting,
    };

PagedResultRequestDto _$PagedResultRequestDtoFromJson(
        Map<String, dynamic> json) =>
    PagedResultRequestDto(
      skipCount: json['skipCount'] as int?,
      maxResultCount: json['maxResultCount'] as int?,
    );

Map<String, dynamic> _$PagedResultRequestDtoToJson(
        PagedResultRequestDto instance) =>
    <String, dynamic>{
      'maxResultCount': instance.maxResultCount,
      'skipCount': instance.skipCount,
    };

PagedAndSortedResultRequestDto _$PagedAndSortedResultRequestDtoFromJson(
        Map<String, dynamic> json) =>
    PagedAndSortedResultRequestDto(
      skipCount: json['skipCount'] as int?,
      maxResultCount: json['maxResultCount'] as int?,
      sorting: json['sorting'] as String?,
    );

Map<String, dynamic> _$PagedAndSortedResultRequestDtoToJson(
        PagedAndSortedResultRequestDto instance) =>
    <String, dynamic>{
      'maxResultCount': instance.maxResultCount,
      'skipCount': instance.skipCount,
      'sorting': instance.sorting,
    };

ExtensiblePagedAndSortedResultRequestDto
    _$ExtensiblePagedAndSortedResultRequestDtoFromJson(
            Map<String, dynamic> json) =>
        ExtensiblePagedAndSortedResultRequestDto(
          skipCount: json['skipCount'] as int?,
          maxResultCount: json['maxResultCount'] as int?,
          sorting: json['sorting'] as String?,
          extraProperties: json['extraProperties'] as Map<String, dynamic>?,
        );

Map<String, dynamic> _$ExtensiblePagedAndSortedResultRequestDtoToJson(
        ExtensiblePagedAndSortedResultRequestDto instance) =>
    <String, dynamic>{
      'maxResultCount': instance.maxResultCount,
      'skipCount': instance.skipCount,
      'sorting': instance.sorting,
      'extraProperties': instance.extraProperties,
    };

ExtensiblePagedResultRequestDto _$ExtensiblePagedResultRequestDtoFromJson(
        Map<String, dynamic> json) =>
    ExtensiblePagedResultRequestDto(
      skipCount: json['skipCount'] as int?,
      maxResultCount: json['maxResultCount'] as int?,
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ExtensiblePagedResultRequestDtoToJson(
        ExtensiblePagedResultRequestDto instance) =>
    <String, dynamic>{
      'maxResultCount': instance.maxResultCount,
      'skipCount': instance.skipCount,
      'extraProperties': instance.extraProperties,
    };
