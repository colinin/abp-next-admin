import 'package:core/dependency/injector.dart';
import 'package:core/models/environment.dart';
import 'package:core/proxy/volo/abp/localization/models.dart';
import 'package:core/utils/index.dart';

import 'service.base.dart';

class EnvironmentService extends ServiceBase {
  EnvironmentService(super._injector);
  
  static EnvironmentService get to => Injector.instance.get<EnvironmentService>();

  final InternalStore<Environment> _store = InternalStore<Environment>(state: Environment.empty());

  Stream<Environment> get createOnUpdateStream {
    return _store.sliceUpdate((state) => state);
  }

  setState(Environment environment) {
    _store.set(environment);
  }

  Stream<Environment> getEnvironment$() {
    return _store.sliceState((state) => state);
  }

  Environment getEnvironment() {
    return _store.state;
  }

  Stream<String> getApiUrl$(String? key) {
    return _store.sliceState((state) => state.remoteServices).map((services) => services.getApiUrl(key));
  }
  String getApiUrl(String? key) {
    return _store.state.remoteServices.getApiUrl(key);
  }

  Stream<String> getAuthority$() {
    return _store.sliceState((state) => state.auth).map((auth) => auth.getAuthority());
  }

  String getAuthority() {
    return _store.state.auth.getAuthority();
  }

  List<LanguageInfo> getSupportedLocales() {
    return _store.state.localization.supportedLocales ?? [];
  }
}