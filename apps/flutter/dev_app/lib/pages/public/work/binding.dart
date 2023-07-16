import 'package:get/get.dart';

import 'controller.dart';

class WorkBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => WorkController());
  }
}