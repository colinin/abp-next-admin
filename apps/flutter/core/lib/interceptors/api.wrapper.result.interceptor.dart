import 'package:core/models/wrapper.dart';
import 'package:dio/dio.dart';

class WrapperResultInterceptor extends Interceptor {
  @override
  void onResponse(Response response, ResponseInterceptorHandler handler) {
    if ('true' == response.headers.value('_abpwrapresult')) {
      if (response.data == null) {
        handler.reject(DioException(
          requestOptions: response.requestOptions,
          response: response,
          type: DioExceptionType.badResponse,
          message: '请求出错, 请稍后再试!'));
          return;
      }
      var result = Wrapper.fromJson(response.data!);
      if ('0' == result.code) {
        response.data = result;
        handler.resolve(response);
        return;
      }

      handler.reject(DioException(
        requestOptions: response.requestOptions,
        response: response,
        type: DioExceptionType.badResponse,
        message: result.details ?? result.message));

      return;
    }
    handler.next(response);
  }
}