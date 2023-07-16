import 'package:dio/dio.dart';
import 'package:get/get.dart' hide Response;

class RestService implements Dio {
  RestService({
    required this.dio,
  });
  late Dio dio;

  static RestService get to => Get.find();
  
  @override
  HttpClientAdapter get httpClientAdapter => dio.httpClientAdapter;

  @override
  set httpClientAdapter(adapter) => dio.httpClientAdapter = adapter;
  
  @override
  BaseOptions get options => dio.options;
  
  @override
  set options(opt) => dio.options = opt;

  @override
  Transformer get transformer => dio.transformer;

  @override
  set transformer(trans) => dio.transformer = trans;

  @override
  Interceptors get interceptors => dio.interceptors;
  
  @override
  void close({bool force = false}) => dio.close(force: force);
  
  @override
  Future<Response<T>> fetch<T>(RequestOptions requestOptions) => dio.fetch<T>(requestOptions);
  
  @override
  Future<Response<T>> get<T>(
    String path, 
    {
      Object? data, 
      Map<String, dynamic>? queryParameters, 
      Options? options, 
      CancelToken? cancelToken, 
      ProgressCallback? onReceiveProgress,
    }) => dio.get<T>(path,
      data: data,
      queryParameters: queryParameters,
      options: options,
      cancelToken: cancelToken,
      onReceiveProgress: onReceiveProgress);
  
  @override
  Future<Response<T>> getUri<T>(
    Uri uri, {
    Object? data,
    Options? options,
    CancelToken? cancelToken,
    ProgressCallback? onReceiveProgress,
  }) => dio.getUri<T>(uri,
      data: data,
      options: options,
      cancelToken: cancelToken,
      onReceiveProgress: onReceiveProgress);

  @override
  Future<Response<T>> post<T>(
    String path, {
    Object? data,
    Map<String, dynamic>? queryParameters,
    Options? options,
    CancelToken? cancelToken,
    ProgressCallback? onSendProgress,
    ProgressCallback? onReceiveProgress,
  }) => dio.post<T>(path,
      data: data,
      queryParameters: queryParameters,
      options: options,
      cancelToken: cancelToken,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);

  @override
  Future<Response<T>> postUri<T>(
    Uri uri, {
    Object? data,
    Options? options,
    CancelToken? cancelToken,
    ProgressCallback? onSendProgress,
    ProgressCallback? onReceiveProgress,
  }) => dio.postUri<T>(uri,
      data: data,
      options: options,
      cancelToken: cancelToken,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);

  @override
  Future<Response<T>> put<T>(
    String path, {
    Object? data,
    Map<String, dynamic>? queryParameters,
    Options? options,
    CancelToken? cancelToken,
    ProgressCallback? onSendProgress,
    ProgressCallback? onReceiveProgress,
  }) => dio.put<T>(path,
      data: data,
      queryParameters: queryParameters,
      options: options,
      cancelToken: cancelToken,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);

  @override
  Future<Response<T>> putUri<T>(
    Uri uri, {
    Object? data,
    Options? options,
    CancelToken? cancelToken,
    ProgressCallback? onSendProgress,
    ProgressCallback? onReceiveProgress,
  }) => dio.putUri<T>(uri,
      data: data,
      options: options,
      cancelToken: cancelToken,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);

  @override
  Future<Response<T>> head<T>(
    String path, {
    Object? data,
    Map<String, dynamic>? queryParameters,
    Options? options,
    CancelToken? cancelToken,
  }) => dio.head<T>(path,
      data: data,
      queryParameters: queryParameters,
      options: options,
      cancelToken: cancelToken);

  @override
  Future<Response<T>> headUri<T>(
    Uri uri, {
    Object? data,
    Options? options,
    CancelToken? cancelToken,
  }) => dio.headUri<T>(uri,
      data: data,
      options: options,
      cancelToken: cancelToken);
  
  @override
  Future<Response<T>> delete<T>(
    String path, 
    {
      Object? data, 
      Map<String, dynamic>? queryParameters, 
      Options? options, 
      CancelToken? cancelToken,
    }) => dio.delete<T>(path,
      data: data,
      queryParameters: queryParameters,
      options: options,
      cancelToken: cancelToken);
  
  @override
  Future<Response<T>> deleteUri<T>(
    Uri uri, 
    {
      Object? data, 
      Options? options, 
      CancelToken? cancelToken,
    }) => dio.deleteUri<T>(uri,
      data: data,
      options: options,
      cancelToken: cancelToken);
  
  @override
  Future<Response> download(
    String urlPath, savePath, 
    {
      ProgressCallback? onReceiveProgress, 
      Map<String, dynamic>? queryParameters, 
      CancelToken? cancelToken, 
      bool deleteOnError = true, 
      String lengthHeader = Headers.contentLengthHeader, 
      Object? data, 
      Options? options,
    }) => dio.download(urlPath, savePath,
      onReceiveProgress: onReceiveProgress,
      queryParameters: queryParameters,
      cancelToken: cancelToken,
      deleteOnError: deleteOnError,
      lengthHeader: lengthHeader,
      data: data,
      options: options);
  
  @override
  Future<Response> downloadUri(
    Uri uri, savePath, 
    {
      ProgressCallback? onReceiveProgress, 
      CancelToken? cancelToken, 
      bool deleteOnError = true, 
      String lengthHeader = Headers.contentLengthHeader, 
      Object? data, 
      Options? options,
    }) => dio.downloadUri(uri, savePath,
      onReceiveProgress: onReceiveProgress,
      cancelToken: cancelToken,
      deleteOnError: deleteOnError,
      lengthHeader: lengthHeader,
      data: data,
      options: options);
  
    @override
    Future<Response<T>> patch<T>(
      String path, 
      {
        Object? data, 
        Map<String, dynamic>? queryParameters, 
        Options? options, 
        CancelToken? cancelToken, 
        ProgressCallback? onSendProgress, 
        ProgressCallback? onReceiveProgress,
      }) => dio.patch<T>(path,
      data: data,
      queryParameters: queryParameters,
      options: options,
      cancelToken: cancelToken,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);
  
    @override
    Future<Response<T>> patchUri<T>(
      Uri uri, 
      {
        Object? data, 
        Options? options, 
        CancelToken? cancelToken, 
        ProgressCallback? onSendProgress, 
        ProgressCallback? onReceiveProgress,
      }) => dio.patchUri<T>(uri,
      data: data,
      options: options,
      cancelToken: cancelToken,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);
  
    @override
    Future<Response<T>> request<T>(
      String path, 
      {
        Object? data, 
        Map<String, dynamic>? queryParameters, 
        CancelToken? cancelToken, 
        Options? options, 
        ProgressCallback? onSendProgress, 
        ProgressCallback? onReceiveProgress,
      }) => dio.request<T>(path,
      data: data,
      queryParameters: queryParameters,
      cancelToken: cancelToken,
      options: options,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);
  
    @override
    Future<Response<T>> requestUri<T>(
      Uri uri,
      {
        Object? data, 
        CancelToken? cancelToken, 
        Options? options, 
        ProgressCallback? onSendProgress, 
        ProgressCallback? onReceiveProgress,
      }) => dio.requestUri<T>(uri,
      data: data,
      cancelToken: cancelToken,
      options: options,
      onSendProgress: onSendProgress,
      onReceiveProgress: onReceiveProgress);
}
