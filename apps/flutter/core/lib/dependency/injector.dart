import 'package:get/get.dart';

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
      InstanceBuilderCallback<T>? builder,
    }) => Get.put(dependency, tag: tag, permanent: permanent, builder: builder);

  void create<T>(
    InstanceBuilderCallback<T> builder,
    {
      String? tag,
      bool permanent = true,
    }
  ) => Get.create(builder, tag: tag, permanent: permanent);

  void lazyInject<T>(
    InstanceBuilderCallback<T> builder,
    {
      String? tag,
      bool fenix = false,
    }) => Get.lazyPut(builder, tag: tag, fenix: fenix);

  Future<T> injectAsync<T>(
    AsyncInstanceBuilderCallback<T> builder,
    {
      String? tag, 
      bool permanent = false,
    }) => Get.putAsync(builder, tag: tag, permanent: permanent);

  bool isInjected({String? tag}) => Get.isRegistered(tag: tag);
}