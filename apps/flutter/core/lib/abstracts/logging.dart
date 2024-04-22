abstract class ILogger {
  
  void debug(dynamic message, [dynamic error, StackTrace? stackTrace]);

  void info(dynamic message, [dynamic error, StackTrace? stackTrace]);

  void warn(dynamic message, [dynamic error, StackTrace? stackTrace]);

  void error(dynamic message, [dynamic error, StackTrace? stackTrace]);

  void trace(dynamic message, [dynamic error, StackTrace? stackTrace]);

  void all(dynamic message, [dynamic error, StackTrace? stackTrace]);
}