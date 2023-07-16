import 'package:core/services/subscription.service.dart';
import 'package:core/tokens/index.dart';
import 'package:get/get.dart';

import 'controller.dart';

class MainBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => MainController());
    Get.lazyPut(() => SubscriptionService(), tag: NotificationTokens.consumer);
  }
}