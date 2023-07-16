import 'dart:async';

import 'package:core/abstracts/logging.dart';
import 'package:logging/logging.dart';

class LoggingLogger extends ILogger implements Logger {
  LoggingLogger(String name, { Level? level }) {
    Logger.root.level = level ?? Level.ALL;
    Logger.root.onRecord.listen((record) {
      // ignore: avoid_print
      print('${record.level.name}: ${record.time}: ${record.message}');
    });
    _innerLogger = Logger(name);
  }
  late Logger _innerLogger;

  factory LoggingLogger.create(String name, { Level? level }) {
    return LoggingLogger(name, level: level);
  }
  
  @override
  void debug(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.shout(message, error, stackTrace);
  }

  @override
  void info(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.fine(message, error, stackTrace);
  }

  @override
  void warn(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.warning(message, error, stackTrace);
  }

  @override
  void error(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.severe(message, error, stackTrace);
  }

  @override
  void trace(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.finer(message, error, stackTrace);
  }

  @override
  void all(dynamic message, [dynamic error, StackTrace? stackTrace]) {
    _innerLogger.log(Level.ALL, message, error, stackTrace);
  }
  
  @override
  Level get level => _innerLogger.level;
  
  @override
  set level(l) => _innerLogger.level = l;
  
  @override
  Map<String, Logger> get children => _innerLogger.children;
  
  @override
  void clearListeners() {
    _innerLogger.clearListeners();
  }
  
  @override
  void config(Object? message, [Object? error, StackTrace? stackTrace]) {
    _innerLogger.config(message, error, stackTrace);
  }
  
  @override
  void fine(Object? message, [Object? error, StackTrace? stackTrace]) {
    _innerLogger.fine(message, error, stackTrace);
  }
  
  @override
  void finer(Object? message, [Object? error, StackTrace? stackTrace]) {
    _innerLogger.finer(message, error, stackTrace);
  }
  
  @override
  void finest(Object? message, [Object? error, StackTrace? stackTrace]) {
    _innerLogger.finest(message, error, stackTrace);
  }
  
  @override
  String get fullName => _innerLogger.fullName;
  
  @override
  bool isLoggable(Level value) => _innerLogger.isLoggable(value);
  
  @override
  String get name => _innerLogger.name;
  
  @override
  Stream<Level?> get onLevelChanged => _innerLogger.onLevelChanged;
  
  @override
  Stream<LogRecord> get onRecord => _innerLogger.onRecord;
  
  @override
  Logger? get parent => _innerLogger.parent;
  
  @override
  void severe(Object? message, [Object? error, StackTrace? stackTrace]) {
    _innerLogger.severe(message, error, stackTrace);
  }
  
  @override
  void shout(Object? message, [Object? error, StackTrace? stackTrace]) {
    _innerLogger.shout(message, error, stackTrace);
  }
  
  @override
  void warning(Object? message, [Object? error, StackTrace? stackTrace]) {
    _innerLogger.warning(message, error, stackTrace);
  }

  @override
  void log(Level logLevel, Object? message, [Object? error, StackTrace? stackTrace, Zone? zone]) {
    _innerLogger.log(logLevel, message, error, stackTrace);
  }
}