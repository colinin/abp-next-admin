import 'package:dev_app/utils/loading.dart';
import 'package:core/tokens/http.token.dart';
import 'package:dio/dio.dart';
import 'package:get/get.dart';

class ExceptionInterceptor extends Interceptor {
  ExceptionInterceptor();

  @override
  void onError(DioException err, ErrorInterceptorHandler handler) {
    if (err.requestOptions.extra.containsKey(HttpTokens.ignoreError)) {
      return handler.next(err);
    }
    var errorMessage = err.message ?? '网络请求失败';
    if (err.response != null) {
      if (err.response!.statusCode == 401) {
        errorMessage = '您的会话已超时, 请重新登录';
        _showErrorToast(errorMessage);
        Get.toNamed('/account/login');
        return;
      }

      if (err.response?.data?['error_description'] != null) {
        errorMessage = err.response!.data['error_description'];
        _showErrorToast(errorMessage);
        return handler.next(err);
      }
    }
    
    _showErrorToast(errorMessage);
    return handler.next(err);
  }

  void _showErrorToast(String errorMessage) {
    Loading.toast(errorMessage);
  }
}