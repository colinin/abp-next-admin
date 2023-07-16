import 'package:get/get.dart';
import 'controller.dart';

class SystemSettingsBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => SystemSettingsController());
  }
}