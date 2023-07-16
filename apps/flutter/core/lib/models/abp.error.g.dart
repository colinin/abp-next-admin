// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'abp.error.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RemoteServiceErrorInfo _$RemoteServiceErrorInfoFromJson(
        Map<String, dynamic> json) =>
    RemoteServiceErrorInfo(
      code: json['code'] as String,
      message: json['message'] as String,
      details: json['details'] as String?,
      data: (json['data'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as String),
      ),
      validationErrors: (json['validationErrors'] as List<dynamic>?)
          ?.map((e) => RemoteServiceValidationErrorInfo.fromJson(
              e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$RemoteServiceErrorInfoToJson(
        RemoteServiceErrorInfo instance) =>
    <String, dynamic>{
      'code': instance.code,
      'message': instance.message,
      'details': instance.details,
      'data': instance.data,
      'validationErrors': instance.validationErrors,
    };

RemoteServiceValidationErrorInfo _$RemoteServiceValidationErrorInfoFromJson(
        Map<String, dynamic> json) =>
    RemoteServiceValidationErrorInfo(
      message: json['message'] as String,
      members:
          (json['members'] as List<dynamic>?)?.map((e) => e as String).toList(),
    );

Map<String, dynamic> _$RemoteServiceValidationErrorInfoToJson(
        RemoteServiceValidationErrorInfo instance) =>
    <String, dynamic>{
      'message': instance.message,
      'members': instance.members,
    };
