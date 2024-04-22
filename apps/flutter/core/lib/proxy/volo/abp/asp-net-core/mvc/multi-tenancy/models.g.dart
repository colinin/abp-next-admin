// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'models.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

FindTenantResultDto _$FindTenantResultDtoFromJson(Map<String, dynamic> json) =>
    FindTenantResultDto(
      tenantId: json['tenantId'] as String?,
      name: json['name'] as String?,
      success: json['success'] as bool,
      isActive: json['isActive'] as bool,
    );

Map<String, dynamic> _$FindTenantResultDtoToJson(
        FindTenantResultDto instance) =>
    <String, dynamic>{
      'success': instance.success,
      'tenantId': instance.tenantId,
      'name': instance.name,
      'isActive': instance.isActive,
    };

CurrentTenantDto _$CurrentTenantDtoFromJson(Map<String, dynamic> json) =>
    CurrentTenantDto(
      id: json['id'] as String?,
      name: json['name'] as String?,
      isAvailable: json['isAvailable'] as bool?,
    );

Map<String, dynamic> _$CurrentTenantDtoToJson(CurrentTenantDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'isAvailable': instance.isAvailable,
    };

MultiTenancyInfoDto _$MultiTenancyInfoDtoFromJson(Map<String, dynamic> json) =>
    MultiTenancyInfoDto(
      isEnabled: json['isEnabled'] as bool,
    );

Map<String, dynamic> _$MultiTenancyInfoDtoToJson(
        MultiTenancyInfoDto instance) =>
    <String, dynamic>{
      'isEnabled': instance.isEnabled,
    };
