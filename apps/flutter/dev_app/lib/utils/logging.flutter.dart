import 'package:core/abstracts/logging.dart';
import 'package:core/models/environment.dart';
import 'package:flutter_logs/flutter_logs.dart';

class FlutterLogging implements ILogger {
  FlutterLogging(this._environment);
  final Environment _environment;

  @override
  void all(message, [error, StackTrace? stackTrace]) {
    FlutterLogs.logThis(
      tag: _environment.application.name,
      logMessage: message,
      level: LogLevel.INFO,
      error: error,
      errorMessage: stackTrace?.toString() ?? '',
    );
  }

  @override
  void debug(message, [error, StackTrace? stackTrace]) {
    FlutterLogs.logThis(
      tag: _environment.application.name,
      logMessage: message,
      level: LogLevel.INFO,
      error: error,
      errorMessage: stackTrace?.toString() ?? '',
    );
  }

  @override
  void error(message, [error, StackTrace? stackTrace]) {
    FlutterLogs.logThis(
      tag: _environment.application.name,
      logMessage: message,
      level: LogLevel.ERROR,
      error: error,
      errorMessage: stackTrace?.toString() ?? '',
    );
  }

  @override
  void info(message, [error, StackTrace? stackTrace]) {
    FlutterLogs.logThis(
      tag: _environment.application.name,
      logMessage: message,
      level: LogLevel.INFO,
      error: error,
      errorMessage: stackTrace?.toString() ?? '',
    );
  }

  @override
  void trace(message, [error, StackTrace? stackTrace]) {
    FlutterLogs.logThis(
      tag: _environment.application.name,
      logMessage: message,
      level: LogLevel.INFO,
      error: error,
      errorMessage: stackTrace?.toString() ?? '',
    );
  }

  @override
  void warn(message, [error, StackTrace? stackTrace]) {
    FlutterLogs.logThis(
      tag: _environment.application.name,
      logMessage: message,
      level: LogLevel.WARNING,
      error: error,
      errorMessage: stackTrace?.toString() ?? '',
    );
  }
}