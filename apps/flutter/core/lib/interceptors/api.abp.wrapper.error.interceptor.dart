import 'package:core/models/abp.error.dart';
import 'package:dio/dio.dart';

class AbpWrapperRemoteServiceErrorInterceptor extends Interceptor {
  @override
  void onError(DioException err, ErrorInterceptorHandler handler) {
    if (err.response == null) return handler.next(err);
    var abpErrorMap = err.response?.headers['_abperrorformat'];
    if (abpErrorMap != null && abpErrorMap.contains('true')) {
      var errorJson = err.response!.data['error'];
      if (errorJson == null && err.response!.data?.type == 'application/json') {
        // 读取流
      }
      else {
        var remoteServiceErrorInfo = RemoteServiceErrorInfo.fromJson(errorJson);
        var errorMessage = remoteServiceErrorInfo.message;
        if (remoteServiceErrorInfo.validationErrors?.isEmpty == false) {
          errorMessage += remoteServiceErrorInfo.validationErrors!.map((v) => v.message).join('\n');
        }
        var cloneError = err.copyWith(message: errorMessage);
        return handler.next(cloneError);
      }
    }
    handler.next(err);
  }
}