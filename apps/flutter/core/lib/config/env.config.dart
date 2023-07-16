import 'package:json_annotation/json_annotation.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_app_environment/flutter_app_environment.dart' as envlib;

part 'env.config.g.dart';

@JsonSerializable(createToJson: false)
class EnvConfig {
  EnvConfig({
    required this.clientId,
    this.clientSecret,
    required this.authority,
    required this.baseUrl,
    this.uploadFilesUrl,
    this.staticFilesUrl,
    this.tenantKey = '__tenant',
    this.defaultLanguage = 'en',
    this.notifications,
  });
  String clientId;
  String? clientSecret;
  String authority;
  String baseUrl;
  String? uploadFilesUrl;
  String? staticFilesUrl;
  String? tenantKey;
  String? defaultLanguage;
  NotificationConfig? notifications;

  factory EnvConfig.fromJson(Map<String, dynamic> json) => _$EnvConfigFromJson(json);
}

@JsonSerializable(createToJson: false)
class NotificationConfig {
  NotificationConfig({
    this.android,
    this.darwin,
    this.linux,
  });
  AndroidNotification? android;
  DarwinNotification? darwin;
  LinuxNotification? linux;

  factory NotificationConfig.fromJson(Map<String, dynamic> json) => _$NotificationConfigFromJson(json);
}

@JsonSerializable(createToJson: false)
class LinuxNotification {
  LinuxNotification({
    required this.defaultActionName,
  });
  String defaultActionName;

  factory LinuxNotification.fromJson(Map<String, dynamic> json) => _$LinuxNotificationFromJson(json);
}

@JsonSerializable(createToJson: false)
class AndroidNotification {
  AndroidNotification({
    required this.channelId,
    required this.channelName,
    this.channelDescription,
  });
  String channelId;
  String channelName;
  String? channelDescription;

  factory AndroidNotification.fromJson(Map<String, dynamic> json) => _$AndroidNotificationFromJson(json);
}

@JsonSerializable(createToJson: false)
class DarwinNotification {
  DarwinNotification();

  factory DarwinNotification.fromJson(Map<String, dynamic> json) => _$DarwinNotificationFromJson(json);
}

enum Env {
  development('DEV', 'Development'),
  profile('PROF', 'Profile'),
  production('PROD', 'Production');

  final String name;
  final String value;
  const Env(this.name, this.value);
  
}

class Environment {
  static Future<void> initAsync() async {
    var envType = envlib.EnvironmentType.development;
    if (kReleaseMode) {
      envType = envlib.EnvironmentType.production;
    } else if (kProfileMode) {
      envType = envlib.EnvironmentType.test;
    }

    await envlib.Environment.initFromJson<EnvConfig>(
      environmentType: envType,
      fromJson: EnvConfig.fromJson,
    );

    Environment.current = envlib.Environment<EnvConfig>.instance().config;
  }

  static late EnvConfig current;
}
