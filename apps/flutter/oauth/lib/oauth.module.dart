import 'package:core/modularity/index.dart';
import 'package:core/services/auth.service.dart';
import 'package:oauth/services/index.dart';
import 'package:oauth_dio/oauth_dio.dart';

class OAuthModule extends Module {
  @override
  void configureServices() {
    lazyInject<AuthService>((injector) => OAuthService(injector));
    lazyInject<OAuthStorage>((injector) => OAuthStorageService(injector));
  }
}