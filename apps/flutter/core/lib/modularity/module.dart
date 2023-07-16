import 'package:get/get.dart';

import 'module.key.dart';

abstract class Module {
  static final Map<Module, bool> _initModuleMap = {};
  List<Module> get dependencies => const [];

  List<GetPage> get routes => const [];

  Future<void> configureServicesAsync() {
    return Future.sync(() => configureServices());
  }

  void configureServices() {

  }

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

  Future<Map<ModuleKey, List<GetPage>>> initAsync() async {
    final routeMap = <ModuleKey, List<GetPage>>{};
    for (var dependency in dependencies) {
      if (_initModuleMap.containsKey(dependency)) {
        continue;
      }
      await dependency.configureServicesAsync();
      var childrenRoute = await dependency.initAsync();
      routeMap.addAll(childrenRoute);
      _initModuleMap[dependency] = true;
    }
    await configureServicesAsync();
    routeMap.assign(ModuleKey(toString()), routes);
    _initModuleMap[this] = true;
    return routeMap;
  }

  Map<ModuleKey, List<GetPage>> init() {
    final routeMap = <ModuleKey, List<GetPage>>{};
    for (var dependency in dependencies) {
      if (_initModuleMap.containsKey(dependency)) {
        continue;
      }
      dependency.configureServices();
      var childrenRoute = dependency.init();
      routeMap.addAll(childrenRoute);
      _initModuleMap[dependency] = true;
    }
    configureServices();
    routeMap.assign(ModuleKey(toString()), routes);
    _initModuleMap[this] = true;
    return routeMap;
  }

  List<GetPage> getRoutes() {
    List<GetPage> findRoutes = [];
    findRoutes.addAll(routes);
    for (var dependency in dependencies) {
      findRoutes.addAll(dependency.routes);
    }

    return findRoutes;
  }
}