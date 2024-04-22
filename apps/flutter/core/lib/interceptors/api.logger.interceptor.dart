import 'package:core/utils/logging.dart';
import 'package:dio/dio.dart';

class LoggerInterceptor extends Interceptor {
  @override
  void onRequest(RequestOptions options, RequestInterceptorHandler handler) {
    logger.debug('网络请求开始: ${options.uri.toString()}');
    return handler.next(options);
  }

  @override
  void onResponse(Response response, ResponseInterceptorHandler handler) {
    logger.debug('网络请求结束, status: ${response.statusCode}, message: ${response.statusMessage}');
    return handler.next(response);
  }

  @override
  void onError(DioException err, ErrorInterceptorHandler handler) {
    logger.error('网络请求错误', err, err.stackTrace);
    return handler.next(err);
  }
}
