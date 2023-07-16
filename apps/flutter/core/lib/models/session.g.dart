// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'session.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Session _$SessionFromJson(Map<String, dynamic> json) => Session(
      language: json['language'] as String?,
      profile: json['profile'] == null
          ? null
          : UserProfile.fromJson(json['profile'] as Map<String, dynamic>),
      tenant: json['tenant'] == null
          ? null
          : CurrentTenantDto.fromJson(json['tenant'] as Map<String, dynamic>),
      token: json['token'] == null
          ? null
          : Token.fromJson(json['token'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$SessionToJson(Session instance) => <String, dynamic>{
      'language': instance.language,
      'profile': instance.profile,
      'tenant': instance.tenant,
      'token': instance.token,
    };
