import 'package:core/dependency/index.dart';
import 'package:core/services/subscription.service.dart';
import 'package:notifications/tokens/index.dart';
import 'package:get/get.dart';

import 'controller.dart';

class MainBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => MainController());
    Get.lazyPut(() => SubscriptionService(Injector.instance), tag: NotificationTokens.consumer);
  }
}