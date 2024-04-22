import 'package:core/dependency/index.dart';
import 'package:get/get.dart';

abstract class ServiceBase extends GetxService {
  ServiceBase(this._injector);
  final Injector _injector;
  T resolve<T>({ String? tag}) => _injector.get<T>(tag: tag);
}