import 'package:core/services/notification.send.service.dart';
import 'package:get/get.dart';
import 'package:core/config/index.dart';
import 'package:core/services/session.service.dart';
import 'package:core/services/signalr.service.dart';
import 'package:core/services/subscription.service.dart';
import 'package:core/tokens/index.dart';
import 'package:core/utils/index.dart';
import 'package:notifications/models/index.dart';

class MainController extends GetxController {
  final RxInt _pageIndex = RxInt(0);
  int get currentIndex => _pageIndex.value;

  SessionService get _sessionService => Get.find();
  SubscriptionService get _subscriptionService => Get.find(tag: NotificationTokens.consumer);
  SignalrService get _signalrService => Get.find(tag: NotificationTokens.producer);
  NotificationSendService get _notificationSendService => Get.find();

  @override
  void onInit() async {
    super.onInit();
    _subscriptionService.addOne(_signalrService.onClose(logger.debug));
    _subscriptionService.addOne(_signalrService.onReconnected(logger.debug));
    _subscriptionService.addOne(_signalrService.onReconnecting(logger.debug));
    // 在SignalR Hub之上再次订阅，用于全局通知启用按钮来管理
    _subscriptionService.subscribe(
      // 订阅SignalR Hub
      _signalrService.subscribe(NotificationTokens.receiver),
      (message) async {
        for (var data in message.data) {
          if (data == null) continue;
          // 解析通知数据
          var notification = NotificationInfo.fromJson(data as dynamic);
          // 格式化为移动端可识别通知数据
          var payload = NotificationPaylod.fromData(notification.data);
          // 发布本地通知
          await _notificationSendService.send(
            payload.title,
            payload.body,
            payload.payload,
          );
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
      _sessionService.setLanguage(Environment.current.defaultLanguage ?? 'en');
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