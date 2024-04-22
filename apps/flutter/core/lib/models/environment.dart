import 'package:core/proxy/volo/abp/localization/models.dart';
import 'package:json_annotation/json_annotation.dart';

part 'environment.g.dart';

@JsonSerializable()
class Environment {
  Environment({
    required this.application,
    required this.remoteServices,
    required this.auth,
    required this.tenant,
    required this.localization,
    required this.remoteEnvironment,
    this.notifications,
  });
  ApplicationConfig application;
  RemoteServiceConfig remoteServices;
  AuthConfig auth;
  TenantConfig tenant;
  LocalizationConfig localization;
  NotificationConfig? notifications;
  RemoteEnvironmentConfig remoteEnvironment;

  factory Environment.empty() {
    return Environment(
      application: ApplicationConfig(name: ''),
      remoteServices: RemoteServiceConfig(services: {}),
      auth: AuthConfig(authority: '', clientId: ''),
      tenant: TenantConfig(tenantKey: ''),
      localization: LocalizationConfig(defaultLanguage: ''),
      remoteEnvironment: RemoteEnvironmentConfig(url: 'url'),
    );
  }

  factory Environment.fromJson(Map<String, dynamic> json) => _$EnvironmentFromJson(json);
  Map<String, dynamic> toJson() => _$EnvironmentToJson(this);

  Environment copyWith({
    ApplicationConfig? application,
    RemoteServiceConfig? remoteServices,
    AuthConfig? auth,
    TenantConfig? tenant,
    LocalizationConfig? localization,
    NotificationConfig? notifications,
    RemoteEnvironmentConfig? remoteEnvironment,
  }) {
    return Environment(
      application: application ?? this.application,
      remoteServices: remoteServices ?? this.remoteServices,
      auth: auth ?? this.auth,
      tenant: tenant ?? this.tenant,
      localization: localization ?? this.localization,
      notifications: notifications ?? this.notifications,
      remoteEnvironment: remoteEnvironment ?? this.remoteEnvironment,
    );
  }
}

@JsonEnum()
enum MergeStrategy {
  @JsonValue('deepmerge')
  deepmerge,
  @JsonValue('overwrite')
  overwrite;
}

@JsonSerializable()
class RemoteEnvironmentConfig {
  RemoteEnvironmentConfig({
    required this.url,
    this.strategy = MergeStrategy.deepmerge,
    this.method = 'GET',
    this.headers,
  });
  String url;
  MergeStrategy? strategy;
  String? method;
  Map<String, String>? headers;

  factory RemoteEnvironmentConfig.fromJson(Map<String, dynamic> json) => _$RemoteEnvironmentConfigFromJson(json);
  Map<String, dynamic> toJson() => _$RemoteEnvironmentConfigToJson(this);

  RemoteEnvironmentConfig copyWith({
    String? url,
    MergeStrategy? strategy,
    String? method,
    Map<String, String>? headers,
  }) {
    return RemoteEnvironmentConfig(
      url: url ?? this.url,
      strategy: strategy ?? this.strategy,
      method: method ?? this.method,
      headers: headers ?? this.headers,
    );
  }
}

@JsonSerializable()
class ApplicationConfig {
  ApplicationConfig({
    required this.name,
    this.framework = 'flutter',
  });
  String name;
  String? framework;

  factory ApplicationConfig.fromJson(Map<String, dynamic> json) => _$ApplicationConfigFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationConfigToJson(this);

  ApplicationConfig copyWith({
    String? name,
  }) {
    return ApplicationConfig(
      name: name ?? this.name,
    );
  }
}

@JsonSerializable()
class AuthConfig {
  AuthConfig({
    required this.authority,
    required this.clientId,
    this.clientSecret,
  });
  String clientId;
  String? clientSecret;
  String authority;

  factory AuthConfig.fromJson(Map<String, dynamic> json) => _$AuthConfigFromJson(json);
  Map<String, dynamic> toJson() => _$AuthConfigToJson(this);

  AuthConfig copyWith({
    String? clientId,
    String? clientSecret,
    String? authority,
  }) {
    return AuthConfig(
      authority: authority ?? this.authority,
      clientId: clientId ?? this.clientId,
      clientSecret: clientSecret ?? this.clientSecret,
    );
  }

  String getAuthority() {
    return _mapToAuthority(authority);
  }

  String _mapToAuthority(String authority) {
    return authority.endsWith('/') ? authority : '$authority/';
  }
}

@JsonSerializable()
class TenantConfig {
  TenantConfig({
    this.tenantKey,
  });
  String? tenantKey;

  factory TenantConfig.fromJson(Map<String, dynamic> json) => _$TenantConfigFromJson(json);
  Map<String, dynamic> toJson() => _$TenantConfigToJson(this);

  TenantConfig copyWith({
    String? tenantKey,
  }) {
    return TenantConfig(
      tenantKey: tenantKey ?? this.tenantKey,
    );
  }
}

@JsonSerializable()
class LocalizationConfig {
  LocalizationConfig({
    this.defaultLanguage,
    this.useLocalResources = true,
    this.supportedLocales = const [],
    this.translationFiles = const {},
  });
  String? defaultLanguage;
  bool? useLocalResources;
  List<LanguageInfo>? supportedLocales;
  Map<String, List<String>>? translationFiles;

  factory LocalizationConfig.fromJson(Map<String, dynamic> json) => _$LocalizationConfigFromJson(json);
  Map<String, dynamic> toJson() => _$LocalizationConfigToJson(this);

  LocalizationConfig copyWith({
    String? defaultLanguage,
    bool? useLocalResources,
  }) {
    return LocalizationConfig(
      defaultLanguage: defaultLanguage ?? this.defaultLanguage,
      useLocalResources: useLocalResources ?? this.useLocalResources,
    );
  }
}

class RemoteServiceConfig {
  RemoteServiceConfig({
    required Map<String, RemoteService> services,
  }) {
    _services = services;
  }
  late Map<String, RemoteService> _services;

  RemoteService get def => this['default']!;
  RemoteService? operator [](String key) => _services[key];
  void operator []=(String key, RemoteService service) => _services[key] = service;

  RemoteServiceConfig copyWith({
    Map<String, RemoteService>? services,
  }) {
    return RemoteServiceConfig(
      services: services ?? _services,
    );
  }

  String getApiUrl(String? key) {
    return _mapToApiUrl(key)(this);
  }

  String Function(RemoteServiceConfig) _mapToApiUrl([String? key]) {
    return (RemoteServiceConfig services) {
      if (key != null) {
        return services[key]?.url ?? services.def.url;
      }
      return services.def.url;
    };
  }
}

@JsonSerializable()
class RemoteService {
  RemoteService({
    required this.url,
    this.rootNamespace,
  });
  String url;
  String? rootNamespace;

  factory RemoteService.fromJson(Map<String, dynamic> json) => _$RemoteServiceFromJson(json);
  Map<String, dynamic> toJson() => _$RemoteServiceToJson(this);

  RemoteService copyWith({
    String? url,
    String? rootNamespace,
  }) {
    return RemoteService(
      url: url ?? this.url,
      rootNamespace: rootNamespace ?? this.rootNamespace,
    );
  }
}

@JsonSerializable()
class NotificationConfig {
  NotificationConfig({
    this.serverUrl,
    this.android,
    this.darwin,
    this.linux,
  });
  String? serverUrl;
  AndroidNotification? android;
  DarwinNotification? darwin;
  LinuxNotification? linux;

  factory NotificationConfig.fromJson(Map<String, dynamic> json) => _$NotificationConfigFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationConfigToJson(this);

  NotificationConfig copyWith({
    AndroidNotification? android,
    DarwinNotification? darwin,
    LinuxNotification? linux,
  }) {
    return NotificationConfig(
      android: android ?? this.android,
      darwin: darwin ?? this.darwin,
      linux: linux ?? this.linux,
    );
  }
}

@JsonSerializable()
class LinuxNotification {
  LinuxNotification({
    required this.defaultActionName,
  });
  String defaultActionName;

  factory LinuxNotification.fromJson(Map<String, dynamic> json) => _$LinuxNotificationFromJson(json);
  Map<String, dynamic> toJson() => _$LinuxNotificationToJson(this);

  LinuxNotification copyWith({
    String? defaultActionName,
  }) {
    return LinuxNotification(
      defaultActionName: defaultActionName ?? this.defaultActionName,
    );
  }
}

@JsonSerializable()
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
  Map<String, dynamic> toJson() => _$AndroidNotificationToJson(this);

  AndroidNotification copyWith({
    String? channelId,
    String? channelName,
    String? channelDescription,
  }) {
    return AndroidNotification(
      channelId: channelId ?? this.channelId,
      channelName: channelName ?? this.channelName,
      channelDescription: channelDescription ?? this.channelDescription,
    );
  }
}

@JsonSerializable()
class DarwinNotification {
  DarwinNotification();

  factory DarwinNotification.fromJson(Map<String, dynamic> json) => _$DarwinNotificationFromJson(json);
  Map<String, dynamic> toJson() => _$DarwinNotificationToJson(this);

  DarwinNotification copyWith() {
    return DarwinNotification(
    );
  }
}