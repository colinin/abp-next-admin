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
  });
  String clientId;
  String? clientSecret;
  String authority;
  String baseUrl;
  String? uploadFilesUrl;
  String? staticFilesUrl;
  String? tenantKey;
  String? defaultLanguage;

  factory EnvConfig.fromJson(Map<String, dynamic> json) => _$EnvConfigFromJson(json);
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

    Environment.current = envlib.Environment.instance().config;
  }

  static late EnvConfig current;
}
