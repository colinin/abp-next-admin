import 'package:core/services/environment.service.dart';
import 'package:core/services/session.service.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:dio/dio.dart';

class AppendHeaderInterceptor extends Interceptor {
  @override
  void onRequest(RequestOptions options, RequestInterceptorHandler handler) {
    var session = SessionService.to.state;
    if (!session.language.isNullOrWhiteSpace()) {
      options.headers['Accept-Language'] = session.language;
    }
    if (session.tenant != null && session.tenant!.isAvailable == true) {
      var environment = EnvironmentService.to.getEnvironment();
      options.headers[environment.tenant.tenantKey ?? '__tenant'] = session.tenant!.id;
    }
    return handler.next(options);
  }
}