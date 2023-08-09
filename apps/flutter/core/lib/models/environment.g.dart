// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'environment.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Environment _$EnvironmentFromJson(Map<String, dynamic> json) => Environment(
      application: ApplicationConfig.fromJson(
          json['application'] as Map<String, dynamic>),
      remoteServices: RemoteServiceConfig(
          services: (json['remoteServices'] as Map<String, dynamic>)
            .map((key, value) => MapEntry(key, RemoteService.fromJson(value as Map<String, dynamic>)))),
      auth: AuthConfig.fromJson(json['auth'] as Map<String, dynamic>),
      tenant: TenantConfig.fromJson(json['tenant'] as Map<String, dynamic>),
      localization: LocalizationConfig.fromJson(
          json['localization'] as Map<String, dynamic>),
      remoteEnvironment: RemoteEnvironmentConfig.fromJson(
          json['remoteEnvironment'] as Map<String, dynamic>),
      notifications: json['notifications'] == null
          ? null
          : NotificationConfig.fromJson(
              json['notifications'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$EnvironmentToJson(Environment instance) =>
    <String, dynamic>{
      'application': instance.application,
      'remoteServices': instance.remoteServices,
      'auth': instance.auth,
      'tenant': instance.tenant,
      'localization': instance.localization,
      'notifications': instance.notifications,
      'remoteEnvironment': instance.remoteEnvironment,
    };

RemoteEnvironmentConfig _$RemoteEnvironmentConfigFromJson(
        Map<String, dynamic> json) =>
    RemoteEnvironmentConfig(
      url: json['url'] as String,
      strategy: $enumDecodeNullable(_$MergeStrategyEnumMap, json['strategy']) ??
          MergeStrategy.deepmerge,
      method: json['method'] as String? ?? 'GET',
      headers: (json['headers'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as String),
      ),
    );

Map<String, dynamic> _$RemoteEnvironmentConfigToJson(
        RemoteEnvironmentConfig instance) =>
    <String, dynamic>{
      'url': instance.url,
      'strategy': _$MergeStrategyEnumMap[instance.strategy],
      'method': instance.method,
      'headers': instance.headers,
    };

const _$MergeStrategyEnumMap = {
  MergeStrategy.deepmerge: 'deepmerge',
  MergeStrategy.overwrite: 'overwrite',
};

ApplicationConfig _$ApplicationConfigFromJson(Map<String, dynamic> json) =>
    ApplicationConfig(
      name: json['name'] as String,
      framework: json['framework'] as String?,
    );

Map<String, dynamic> _$ApplicationConfigToJson(ApplicationConfig instance) =>
    <String, dynamic>{
      'name': instance.name,
      'framework': instance.framework,
    };

AuthConfig _$AuthConfigFromJson(Map<String, dynamic> json) => AuthConfig(
      authority: json['authority'] as String,
      clientId: json['clientId'] as String,
      clientSecret: json['clientSecret'] as String?,
    );

Map<String, dynamic> _$AuthConfigToJson(AuthConfig instance) =>
    <String, dynamic>{
      'clientId': instance.clientId,
      'clientSecret': instance.clientSecret,
      'authority': instance.authority,
    };

TenantConfig _$TenantConfigFromJson(Map<String, dynamic> json) => TenantConfig(
      tenantKey: json['tenantKey'] as String?,
    );

Map<String, dynamic> _$TenantConfigToJson(TenantConfig instance) =>
    <String, dynamic>{
      'tenantKey': instance.tenantKey,
    };

LocalizationConfig _$LocalizationConfigFromJson(Map<String, dynamic> json) =>
    LocalizationConfig(
      defaultLanguage: json['defaultLanguage'] as String?,
      useLocalResources: json['useLocalResources'] as bool?,
      supportedLocales: json['supportedLocales'] != null
        ? (json['supportedLocales'] as List<dynamic>).map((e) => LanguageInfo.fromJson(e)).toList()
        : null,
      translationFiles: json['translationFiles'] != null
        ? (json['translationFiles'] as Map<String, dynamic>)
          .map((key, value) => MapEntry(key, (value as List<dynamic>)
            .map((e) => e as String).toList()))
        : null,
    );

Map<String, dynamic> _$LocalizationConfigToJson(LocalizationConfig instance) =>
    <String, dynamic>{
      'defaultLanguage': instance.defaultLanguage,
      'useLocalResources': instance.useLocalResources,
      'supportedLocales': instance.supportedLocales,
      'translationFiles': instance.translationFiles,
    };

RemoteService _$RemoteServiceFromJson(Map<String, dynamic> json) =>
    RemoteService(
      url: json['url'] as String,
      rootNamespace: json['rootNamespace'] as String?,
    );

Map<String, dynamic> _$RemoteServiceToJson(RemoteService instance) =>
    <String, dynamic>{
      'url': instance.url,
      'rootNamespace': instance.rootNamespace,
    };

NotificationConfig _$NotificationConfigFromJson(Map<String, dynamic> json) =>
    NotificationConfig(
      serverUrl: json['serverUrl'] as String?,
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

Map<String, dynamic> _$NotificationConfigToJson(NotificationConfig instance) =>
    <String, dynamic>{
      'serverUrl': instance.serverUrl,
      'android': instance.android,
      'darwin': instance.darwin,
      'linux': instance.linux,
    };

LinuxNotification _$LinuxNotificationFromJson(Map<String, dynamic> json) =>
    LinuxNotification(
      defaultActionName: json['defaultActionName'] as String,
    );

Map<String, dynamic> _$LinuxNotificationToJson(LinuxNotification instance) =>
    <String, dynamic>{
      'defaultActionName': instance.defaultActionName,
    };

AndroidNotification _$AndroidNotificationFromJson(Map<String, dynamic> json) =>
    AndroidNotification(
      channelId: json['channelId'] as String,
      channelName: json['channelName'] as String,
      channelDescription: json['channelDescription'] as String?,
    );

Map<String, dynamic> _$AndroidNotificationToJson(
        AndroidNotification instance) =>
    <String, dynamic>{
      'channelId': instance.channelId,
      'channelName': instance.channelName,
      'channelDescription': instance.channelDescription,
    };

DarwinNotification _$DarwinNotificationFromJson(Map<String, dynamic> json) =>
    DarwinNotification();

Map<String, dynamic> _$DarwinNotificationToJson(DarwinNotification instance) =>
    <String, dynamic>{};
