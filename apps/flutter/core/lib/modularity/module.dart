import 'package:get/get.dart';

import '../dependency/index.dart';
import 'module.key.dart';

abstract class Module {
  late bool isInitlized= false;

  Injector get injector => Injector.instance;
  List<Module> get dependencies => const [];
  List<GetPage> get routes => const [];

  Future<void> configureServicesAsync() {
    return Future.sync(() => configureServices());
  }

  void configureServices() {

  }

  T get<T>({String? tag}) => injector.get<T>(tag: tag);

  T inject<T>(
    T dependency,
    {
      String? tag,
      bool permanent = false,
      InjectorBuilderFactory<T>? builder,
    }) => injector.inject(dependency, tag: tag, permanent: permanent, builder: builder);

  void create<T>(
    InjectorBuilderFactory<T> builder,
    {
      String? tag,
      bool permanent = true,
    }
  ) => injector.create(builder, tag: tag, permanent: permanent);

  void lazyInject<T>(
    InjectorBuilderFactory<T> builder,
    {
      String? tag,
      bool fenix = false,
    }) => injector.lazyInject(builder, tag: tag, fenix: fenix);

  Future<T> injectAsync<T>(
    AsyncInjectorBuilderFactory<T> builder,
    {
      String? tag, 
      bool permanent = false,
    }) => injector.injectAsync(builder, tag: tag, permanent: permanent);

  bool isInjected({String? tag}) => injector.isInjected(tag: tag);

  Future<Map<ModuleKey, List<GetPage>>> initAsync() async {
    var routeMap = <ModuleKey, List<GetPage>>{};
    if (isInitlized) return routeMap;
    for (var dependency in dependencies) {
      var childrenRoute = await dependency.initAsync();
      routeMap.addAll(childrenRoute);
    }
    await configureServicesAsync();
    routeMap.putIfAbsent(ModuleKey(toString()), () => routes);
    isInitlized = true;
    return routeMap;
  }

  Map<ModuleKey, List<GetPage>> init() {
    var routeMap = <ModuleKey, List<GetPage>>{};
    if (isInitlized) return routeMap;
    for (var dependency in dependencies) {
      var childrenRoute = dependency.init();
      routeMap.addAll(childrenRoute);
    }
    configureServices();
    routeMap.putIfAbsent(ModuleKey(toString()), () => routes);
    isInitlized = true;
    return routeMap;
  }

  List<GetPage> getRoutes() {
    List<GetPage> findRoutes = [];
    for (var dependency in dependencies) {
      findRoutes.addAll(dependency.getRoutes());
      
    }
    findRoutes.addAll(routes);
    return findRoutes;
  }
}