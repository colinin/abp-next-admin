import 'package:core/config/index.dart';
import 'package:core/services/session.service.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:dio/dio.dart';

class AppendHeaderInterceptor extends Interceptor {
  @override
  void onRequest(RequestOptions options, RequestInterceptorHandler handler) {
    var language = SessionService.to.currentLanguage;
    if (!language.isNullOrWhiteSpace()) {
      options.headers['Accept-Language'] = language;
    }
    var tenant = SessionService.to.tenant;
    if (tenant != null && tenant.isAvailable == true) {
      options.headers[Environment.current.tenantKey ?? '__tenant'] = tenant.id;
    }
    return handler.next(options);
  }
}