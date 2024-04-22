import 'package:dio/dio.dart';

class RequestOptions {
  RequestOptions({
    CancelToken? cancelToken,
    this.sendTimeout,
    this.receiveTimeout,
    this.headers,
  }) {
    this.cancelToken = cancelToken ?? CancelToken();
  }
  late CancelToken? cancelToken;
  late Duration? sendTimeout;
  late Duration? receiveTimeout;
  late Map<String, dynamic>? headers;

  void cancel() {
    cancelToken?.cancel();
  }
}