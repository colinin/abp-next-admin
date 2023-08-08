import 'package:core/dependency/injector.dart';
import 'package:core/models/environment.dart';
import 'package:core/services/index.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:dio/dio.dart';

class EnvironmentUtils {
  static Future<void> mergeRemoteEnvironment(Injector injector, Environment environment) {
    var restService = injector.get<RestService>();
    var environmentService = injector.get<EnvironmentService>();

    var url = environment.remoteEnvironment.url;
    var method = environment.remoteEnvironment.method ?? 'GET';
    var headers = environment.remoteEnvironment.headers ?? {};
    if (url.isNullOrWhiteSpace()) {
      return Future.value();
    }

    return restService.request(
      method: method,
      url: url,
      options: Options(
        headers: headers,
      ),
      transformer: (res) {
        if (res.data == null) return;
        var remoteEnv = Environment.fromJson(res.data);
        environmentService.setState(
          _mergeEnvironments(
            environment, 
            remoteEnv, 
            environment.remoteEnvironment
          )
        );
      }
    );
  }

  static Environment _mergeEnvironments(
    Environment localEnv,
    Environment remoteEnv,
    RemoteEnvironmentConfig remoteEnvConfig) 
  {
    switch (remoteEnvConfig.strategy) {
      case MergeStrategy.deepmerge:
        return localEnv.copyWith(
          auth: remoteEnv.auth,
          tenant: remoteEnv.tenant,
          application: remoteEnv.application,
          localization: remoteEnv.localization,
          notifications: remoteEnv.notifications,
          remoteServices: remoteEnv.remoteServices,
          remoteEnvironment: remoteEnv.remoteEnvironment,
        );
      case MergeStrategy.overwrite:
      default: return remoteEnv;
    }
  }
}