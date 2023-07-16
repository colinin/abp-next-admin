import 'package:core/abstracts/logging.dart';
import 'package:logger/logger.dart';

class LoggerLogger extends ILogger {
  final _innerLogger = Logger();
  
  @override
  void debug(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.d(message, error, stackTrace);
  }

  @override
  void info(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.i(message, error, stackTrace);
  }

  @override
  void warn(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.w(message, error, stackTrace);
  }

  @override
  void error(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.e(message, error, stackTrace);
  }

  @override
  void trace(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.wtf(message, error, stackTrace);
  }

  @override
  void all(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.v(message, error, stackTrace);
  }
}