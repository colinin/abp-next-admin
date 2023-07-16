import 'package:get/get.dart';

abstract class ServiceBase extends GetxService {
  T find<T>({ String? tag}) => Get.find<T>(tag: tag);
}