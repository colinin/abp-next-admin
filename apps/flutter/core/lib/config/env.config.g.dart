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
      tenantKey: json['tenantKey'] as String? ?? '__tenant',
      defaultLanguage: json['defaultLanguage'] as String? ?? 'en',
      notifications: json['notifications'] == null
          ? null
          : NotificationConfig.fromJson(
              json['notifications'] as Map<String, dynamic>),
    );

NotificationConfig _$NotificationConfigFromJson(Map<String, dynamic> json) =>
    NotificationConfig(
      android: json['android'] == null
          ? null
          : AndroidNotification.fromJson(
              json['android'] as Map<String, dynamic>),
      darwin: json['darwin'] == null
          ? null
          : DarwinNotification.fromJson(json['darwin'] as Map<String, dynamic>),
      linux: json['linux'] == null
          ? null
          : LinuxNotification.fromJson(json['linux'] as Map<String, dynamic>),
    );

LinuxNotification _$LinuxNotificationFromJson(Map<String, dynamic> json) =>
    LinuxNotification(
      defaultActionName: json['defaultActionName'] as String,
    );

AndroidNotification _$AndroidNotificationFromJson(Map<String, dynamic> json) =>
    AndroidNotification(
      channelId: json['channelId'] as String,
      channelName: json['channelName'] as String,
      channelDescription: json['channelDescription'] as String?,
    );

DarwinNotification _$DarwinNotificationFromJson(Map<String, dynamic> json) =>
    DarwinNotification();
