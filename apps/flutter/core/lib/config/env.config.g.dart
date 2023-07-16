// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'env.config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

EnvConfig _$EnvConfigFromJson(Map<String, dynamic> json) => EnvConfig(
      clientId: json['clientId'] as String,
      clientSecret: json['clientSecret'] as String?,
      authority: json['authority'] as String,
      baseUrl: json['baseUrl'] as String,
      uploadFilesUrl: json['uploadFilesUrl'] as String?,
      staticFilesUrl: json['staticFilesUrl'] as String?,
      tenantKey: json['tenantKey'] as String?,
      defaultLanguage: json['defaultLanguage'] as String?,
    );
