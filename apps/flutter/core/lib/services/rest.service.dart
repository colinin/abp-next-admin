import 'dart:async';

import 'package:core/dependency/injector.dart';
import 'package:core/models/request.dart';
import 'package:dio/dio.dart' hide RequestOptions;

import 'environment.service.dart';

class HttpMethod {
  // ignore: constant_identifier_names
  static const String GET = "GET";
  // ignore: constant_identifier_names
  static const String POST = "POST";
  // ignore: constant_identifier_names
  static const String PATCH = "PATCH";
  // ignore: constant_identifier_names
  static const String PUT = "PUT";
  // ignore: constant_identifier_names
  static const String DELETE = "DELETE";
  // ignore: constant_identifier_names
  static const String OPTIONS = "OPTIONS";
  // ignore: constant_identifier_names
  static const String HEAD = "HEAD";
}

class RestService {
  RestService(
    this.dio,
    this._environmentService);
  late Dio dio;
  late final EnvironmentService _environmentService;

  static RestService get to => injector.get<RestService>();

  Future<T> request<T>(
    {
      required String url,
      required String method,
      T Function(Response<dynamic> response)? transformer,
      RequestOptions? requestOptions,
      String? apiName,
      Object? data, 
      Map<String, dynamic>? queryParameters, 
      CancelToken? cancelToken, 
      Options? options, 
      ProgressCallback? onSendProgress, 
      ProgressCallback? onReceiveProgress,
    }) {
    options ??= options?.copyWith(
        method: method,
        sendTimeout: requestOptions?.sendTimeout,
        receiveTimeout: requestOptions?.receiveTimeout,
        headers: requestOptions?.headers,
      ) 
      ?? Options(
        method: method,
        sendTimeout: requestOptions?.sendTimeout,
        receiveTimeout: requestOptions?.receiveTimeout,
        headers: requestOptions?.headers,
      );

    var apiUrl = _environmentService.getApiUrl(apiName);
    url = removeDuplicateSlashes(apiUrl + url);

    return dio.request(url,
      data: data,
      queryParameters: queryParameters,
      cancelToken: cancelToken ?? requestOptions?.cancelToken,
      options: options,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress)
    .then((response) => transformer != null ? transformer(response) : null as T);
  }

  String removeDuplicateSlashes(String url) {
    return url.replaceAll(RegExp('/([^:]/)/+/g'), '\$1');
  }
}
