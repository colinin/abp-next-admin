import 'package:core/modularity/module.dart';
import 'package:platforms/proxy/index.dart';
import 'package:platforms/services/index.dart';

class PlatformModule extends Module {
  @override
  void configureServices() {
    lazyInject<MenuService>((injector) => MenuService(injector));
    lazyInject<FavoriteMenuService>((injector) => FavoriteMenuService(injector));

    lazyInject<MenuStateService>((injector) => MenuStateService(injector));
    lazyInject<FavoriteMenuStateService>((injector) => FavoriteMenuStateService(injector));
  }
}