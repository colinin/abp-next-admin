import 'package:get/get.dart';
import 'controller.dart';

class CenterSettingsBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut(() => CenterSettingsController());
  }
}