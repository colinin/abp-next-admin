
import 'package:core/models/oauth.dart';
import 'package:core/services/auth.service.dart';
import 'package:core/services/rest.service.dart';
import 'package:core/services/session.service.dart';
import 'package:core/tokens/http.token.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:dio/dio.dart';

class ApiAuthorizationInterceptor extends Interceptor {
  @override
  void onRequest(RequestOptions options, RequestInterceptorHandler handler) {
    if (!options.extra.containsKey(HttpTokens.ignoreToken)) {
      var token = SessionService.to.token;
      if (token != null && token.accessToken.isNullOrWhiteSpace() == false) {
        var accessToken = token.accessToken;
        var scheme = token.tokenType ?? 'Bearer';
        options.headers['Authorization'] = '$scheme $accessToken';
      }
    }
    return handler.next(options);
  }

  @override
  void onError(DioException err, ErrorInterceptorHandler handler) {
    // 需要处理401错误
    if (err.response?.statusCode == 401) {
      var token = SessionService.to.token;
      if (token == null || token.refreshToken.isNullOrWhiteSpace() == true)
      {
        return handler.next(err);
      }
      var authService = AuthService.to;
      authService.refreshToken(RefreshTokenParams(token.refreshToken!))
        .then((value) {
          //SessionService.to.setToken(value);
          //err.requestOptions
          RestService.to.dio.request(
            err.requestOptions.path,
            data: err.requestOptions.data,
            queryParameters: err.requestOptions.queryParameters,
            cancelToken: err.requestOptions.cancelToken,
            onSendProgress: err.requestOptions.onSendProgress,
            onReceiveProgress: err.requestOptions.onReceiveProgress,
            options: Options(
              method: err.requestOptions.method,
              sendTimeout: err.requestOptions.sendTimeout,
              receiveTimeout: err.requestOptions.receiveTimeout,
              extra: err.requestOptions.extra,
              headers: err.requestOptions.headers,
              responseType: err.requestOptions.responseType,
              contentType: err.requestOptions.contentType,
              validateStatus: err.requestOptions.validateStatus,
              receiveDataWhenStatusError: err.requestOptions.receiveDataWhenStatusError,
              followRedirects: err.requestOptions.followRedirects,
              maxRedirects: err.requestOptions.maxRedirects,
              persistentConnection: err.requestOptions.persistentConnection,
              requestEncoder: err.requestOptions.requestEncoder,
              responseDecoder: err.requestOptions.responseDecoder,
              listFormat: err.requestOptions.listFormat,
            )
          ).then((value) {
            return handler.resolve(value);
          }).catchError((_) {
            handler.next(err);
          });
        });
    } else {
      return handler.next(err);
    }
  }
}