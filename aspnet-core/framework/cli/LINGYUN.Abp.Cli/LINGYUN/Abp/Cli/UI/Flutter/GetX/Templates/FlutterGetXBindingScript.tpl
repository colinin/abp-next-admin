import 'package:get/get.dart';

import 'controller.dart';

class {{ model.application }}Binding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => {{ model.application }}Controller());
  }
}