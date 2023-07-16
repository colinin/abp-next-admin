import 'package:get/get.dart';
import 'controller.dart';

class NotifierManageBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => NotifierManageController());
  }
}