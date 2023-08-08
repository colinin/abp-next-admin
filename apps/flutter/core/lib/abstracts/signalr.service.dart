import 'dart:async';

import 'package:core/models/common.dart';
import 'package:core/utils/logging.dart';

abstract class SignalrService {

  Future<void>? start();

  Future<void> stop();

  StreamSubscription<Exception?> onClose(Function(Exception?) listen, { bool Function(Exception?)? filter});

  StreamSubscription<Exception?> onReconnecting(Function(Exception?) listen, { bool Function(Exception?)? filter});

  StreamSubscription<String?> onReconnected(Function(String?) listen, { bool Function(String?)? filter});

  Stream<SignalrMessage> subscribe(String methodName);

  void unsubscribe(String methodName);

  Future<void> send(String methodName, { List<Object>? args });

  Future<dynamic> invoke(String methodName, { List<Object>? args });
}

class NullSignalrService extends SignalrService {
  @override
  Future invoke(String methodName, {List<Object>? args}) {
    logger.warn('signalr - invoke method not implemented!');
    return Future.value();
  }

  @override
  StreamSubscription<Exception?> onClose(Function(Exception? p1) listen, {bool Function(Exception? p1)? filter}) {
    logger.warn('signalr - onClose method not implemented!');
    Exception? nullValue;
    return Stream.value(nullValue).listen(logger.debug);
  }

  @override
  StreamSubscription<String?> onReconnected(Function(String? p1) listen, {bool Function(String? p1)? filter}) {
    logger.warn('signalr - onReconnected method not implemented!');
    String? nullValue;
    return Stream.value(nullValue).listen(logger.debug);
  }

  @override
  StreamSubscription<Exception?> onReconnecting(Function(Exception? p1) listen, {bool Function(Exception? p1)? filter}) {
    logger.warn('signalr - onReconnecting method not implemented!');
    Exception? nullValue;
    return Stream.value(nullValue).listen(logger.debug);
  }

  @override
  Future<void> send(String methodName, {List<Object>? args}) {
    logger.warn('signalr - send method not implemented!');
    return Future.value();
  }

  @override
  Future<void>? start() {
    logger.warn('signalr - start method not implemented!');
    return Future.value();
  }

  @override
  Future<void> stop() {
    logger.warn('signalr - stop method not implemented!');
    return Future.value();
  }

  @override
  Stream<SignalrMessage> subscribe(String methodName) {
    logger.warn('signalr - subscribe method not implemented!');
    return Stream.value(SignalrMessage(methodName, []));
  }

  @override
  void unsubscribe(String methodName) {
    logger.warn('signalr - unsubscribe method not implemented!');
  }

}