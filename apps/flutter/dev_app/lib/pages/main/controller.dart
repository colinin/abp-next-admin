import 'package:get/get.dart';
import 'package:core/config/index.dart';
import 'package:core/services/config.state.service.dart';
import 'package:core/services/session.service.dart';
import 'package:core/services/signalr.service.dart';
import 'package:core/services/subscription.service.dart';
import 'package:core/tokens/index.dart';
import 'package:core/utils/index.dart';
import 'package:notifications/models/notification.dart';

class MainController extends GetxController {
  final RxInt _pageIndex = RxInt(0);
  int get currentIndex => _pageIndex.value;

  SessionService get sessionService => Get.find();
  ConfigStateService get configStateService => Get.find();
  SubscriptionService get subscriptionService => Get.find(tag: NotificationTokens.consumer);
  SignalrService get signalrService => Get.find(tag: NotificationTokens.producer);

  @override
  void onInit() async {
    super.onInit();
    subscriptionService.addOne(signalrService.onClose(logger.debug));
    subscriptionService.addOne(signalrService.onReconnected(logger.debug));
    subscriptionService.addOne(signalrService.onReconnecting(logger.debug));
    subscriptionService.subscribe(
      signalrService.subscribe(NotificationTokens.receiver),
      (message) {
        var notification = NotificationInfo.fromJson(message.data.first as dynamic);
        logger.debug(notification);
      },
    );
    sessionService.getToken$()
      .listen((token) async {
        if (token == null) {
          await signalrService.stop();
        } else {
          await signalrService.start();
        }
      });
    if (sessionService.currentLanguage.isNullOrWhiteSpace()) {
      sessionService.setLanguage(Environment.current.defaultLanguage ?? 'en');
    }
  }

  @override
  void onClose() {
    subscriptionService.closeAll()
      .whenComplete(() => signalrService.stop());
    super.onClose();
  }

  void setCurrentIndex(int index) {
    _pageIndex.value = index;
  }
}