import 'package:core/services/error.reporter.service.dart';
import 'package:dio/dio.dart';

class ExceptionInterceptor extends Interceptor {
  ExceptionInterceptor(this._errorReporterService);

  final ErrorReporterService _errorReporterService;

  @override
  void onError(DioException err, ErrorInterceptorHandler handler) {
    _errorReporterService.reportError(err);
    return handler.next(err);
  }
}