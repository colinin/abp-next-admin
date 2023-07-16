import 'package:get/get.dart';

import 'controller.dart';

class CenterBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => CenterController());
  }
}