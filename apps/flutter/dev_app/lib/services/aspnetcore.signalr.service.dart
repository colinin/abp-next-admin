import 'dart:async';

import 'package:core/abstracts/signalr.service.dart';
import 'package:core/models/common.dart';
import 'package:core/services/session.service.dart';
import 'package:core/utils/logging.dart';
import 'package:rxdart/rxdart.dart';
import 'package:signalr_netcore/signalr_client.dart';

class AspNetCoreSignalrService extends SignalrService {
  AspNetCoreSignalrService(String url,
    {
      bool autoStart = false,
      bool automaticReconnect = true,
      List<int>? retryDelays,
      Future<String> Function()? accessTokenFactory,
    }) {
    _hubConnection = _createSignalR(url,
      autoStart: autoStart,
      automaticReconnect: automaticReconnect,
      retryDelays: retryDelays,
      accessTokenFactory: accessTokenFactory ?? () {
        var token = SessionService.to.token;
        if (token != null) return Future.value(token.accessToken);
        return Future.value('');
      });
    _initHubEvents();
  }
  late HubConnection _hubConnection;

  final BehaviorSubject<Exception?> _onClose = BehaviorSubject<Exception?>();
  final BehaviorSubject<Exception?> _onReconnecting = BehaviorSubject<Exception?>();
  final BehaviorSubject<String?> _onReconnected = BehaviorSubject<String?>();
  final BehaviorSubject<SignalrMessage> _onReceived = BehaviorSubject<SignalrMessage>();

  static HubConnection _createSignalR(
    String url,
    {
      bool autoStart = false,
      bool automaticReconnect = true,
      List<int>? retryDelays,
      Future<String> Function()? accessTokenFactory,
    }
  ) {
    var httpConnectionOptions = HttpConnectionOptions(
      httpClient: WebSupportingHttpClient(logger),
      accessTokenFactory: accessTokenFactory,
      logger: logger,
      logMessageContent: true);
    var hubConnectionBuilder = HubConnectionBuilder()
      .withUrl(url, options: httpConnectionOptions);
    if (automaticReconnect) {
      hubConnectionBuilder.withAutomaticReconnect(
        retryDelays: retryDelays,
      );
    }
    return hubConnectionBuilder.build();
  }

  void _initHubEvents() {
    _hubConnection.onclose(({error}) {
      _onClose.add(error);
    });
    _hubConnection.onreconnected(({connectionId}) {
      _onReconnected.add(connectionId);
    });
    _hubConnection.onreconnecting(({error}) {
      _onReconnecting.add(error);
    });
  }

  @override
  Future<void>? start() {
    return _hubConnection.start();
  }

  @override
  Future<void> stop() {
    return _hubConnection.stop();
  }

  @override
  StreamSubscription<Exception?> onClose(Function(Exception?) listen, { bool Function(Exception?)? filter}) {
    return _onClose
      .where((event) => filter != null ? filter(event) : true)
      .listen(listen);
  }

  @override
  StreamSubscription<Exception?> onReconnecting(Function(Exception?) listen, { bool Function(Exception?)? filter}) {
    return _onReconnecting
      .where((event) => filter != null ? filter(event) : true)
      .listen(listen);
  }

  @override
  StreamSubscription<String?> onReconnected(Function(String?) listen, { bool Function(String?)? filter}) {
    return _onReconnected
      .where((event) => filter != null ? filter(event) : true)
      .listen(listen);
  }

  @override
  Stream<SignalrMessage> subscribe(String methodName) {
    _hubConnection.on(methodName, (data) {
      if (data == null) return;
      _onReceived.add(SignalrMessage(methodName, data));
    });
    return _onReceived.where((message) => message.method == methodName);
  }

  @override
  void unsubscribe(String methodName,) {
    _hubConnection.off(methodName);
  }

  @override
  Future<void> send(String methodName, { List<Object>? args }) {
    return _hubConnection.send(methodName, args: args);
  }

  @override
  Future<dynamic> invoke(String methodName, { List<Object>? args }) {
    return _hubConnection.invoke(methodName,  args: args);
  }
}