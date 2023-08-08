import 'package:core/models/common.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_app_environment/flutter_app_environment.dart';

import 'package:core/services/application.initial.dart';

class FlutterApplicationInitialService extends ApplicationInitialService {
  @override
  Future<Application> initialApp() async {
    var envType = EnvironmentType.development;
    if (kReleaseMode) {
      envType = EnvironmentType.production;
    } else if (kProfileMode) {
      envType = EnvironmentType.test;
    }

    await Environment.initFromJson<Application>(
      environmentType: envType,
      fromJson: Application.fromJson,
    );

    return Environment<Application>.instance().config;
  }
}