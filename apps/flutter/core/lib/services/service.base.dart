import 'package:get/get.dart';

abstract class ServiceBase extends GetxService {
  T find<T>() => Get.find<T>();
}