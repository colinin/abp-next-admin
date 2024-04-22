import 'package:core/dependency/index.dart';
import 'package:get/get.dart';

abstract class BaseWidget<Bloc extends GetLifeCycleBase?> extends GetWidget<Bloc> {
  const BaseWidget({super.key});

  Bloc get bloc => Injector.instance.get(tag: tag);
}