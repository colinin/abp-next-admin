import 'package:get/get.dart';

import 'injector.builder.dart';

final Injector injector = Injector.instance;

class Injector {
  factory Injector() => _getInstance ??= const Injector._();

  const Injector._();

  static Injector? _getInstance;

  static Injector get instance => Injector();

  T get<T>({ String? tag }) => Get.find(tag: tag);

  T inject<T>(
    T dependency,
    {
      String? tag,
      bool permanent = false,
      InjectorBuilderFactory<T>? builder,
    }) => Get.put(dependency, tag: tag, permanent: permanent, builder: () {
      return builder != null ? builder(get()) : dependency;
    });

  void create<T>(
    InjectorBuilderFactory<T> builder,
    {
      String? tag,
      bool permanent = true,
    }
  ) => Get.create(() => builder(get()), tag: tag, permanent: permanent);

  void lazyInject<T>(
    InjectorBuilderFactory<T> builder,
    {
      String? tag,
      bool fenix = false,
    }) => Get.lazyPut(() => builder(get()), tag: tag, fenix: fenix);

  Future<T> injectAsync<T>(
    AsyncInjectorBuilderFactory<T> builder,
    {
      String? tag, 
      bool permanent = false,
    }) => Get.putAsync(() => builder(get()), tag: tag, permanent: permanent);

  bool isInjected({String? tag}) => Get.isRegistered(tag: tag);
}