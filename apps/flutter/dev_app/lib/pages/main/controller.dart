import 'package:core/dependency/index.dart';
import 'package:core/abstracts/signalr.service.dart';
import 'package:core/services/environment.service.dart';
import 'package:dev_app/handlers/index.dart';
import 'package:get/get.dart';
import 'package:core/services/session.service.dart';
import 'package:core/services/subscription.service.dart';
import 'package:core/utils/index.dart';
import 'package:notifications/models/index.dart';
import 'package:notifications/services/notification.state.service.dart';
import 'package:notifications/tokens/index.dart';

class MainController extends GetxController {
  final RxInt _pageIndex = RxInt(0);
  int get currentIndex => _pageIndex.value;

  SessionService get _sessionService => injector.get();
  SubscriptionService get _subscriptionService => injector.get(tag: NotificationTokens.consumer);
  SignalrService get _signalrService => injector.get(tag: NotificationTokens.producer);
  NotificationStateService get _notificationStateService => injector.get();
  EnvironmentService get _environmentService => injector.get();
  ErrorHandler get _errorHandler => injector.get();

  @override
  void onInit() async {
    super.onInit();
    _subscriptionService.addOne(_errorHandler.listenToRestError());
    _subscriptionService.addOne(_signalrService.onClose(logger.debug));
    _subscriptionService.addOne(_signalrService.onReconnected(logger.debug));
    _subscriptionService.addOne(_signalrService.onReconnecting(logger.debug));
    // 在SignalR Hub之上再次订阅，用于全局通知启用按钮来管理
    _subscriptionService.subscribe(
      // 订阅SignalR Hub
      _signalrService.subscribe(NotificationTokens.receiver),
      next: (message) async {
        for (var data in message.data) {
          if (data == null) continue;
          // 解析通知数据
          var notification = NotificationInfo.fromJson(data as dynamic);
          _notificationStateService.addNotification(notification);
        }
      },
    );
    _sessionService.getToken$()
      .listen((token) async {
        if (token == null) {
          await _signalrService.stop();
        } else {
          await _signalrService.start();
        }
      });
    if (_sessionService.currentLanguage.isNullOrWhiteSpace()) {
      var environment = _environmentService.getEnvironment();
      _sessionService.setLanguage(environment.localization.defaultLanguage ?? 'en');
    }
  }

  @override
  void onClose() {
    _subscriptionService.closeAll()
      .whenComplete(() => _signalrService.stop());
    super.onClose();
  }

  void setCurrentIndex(int index) {
    _pageIndex.value = index;
  }
}