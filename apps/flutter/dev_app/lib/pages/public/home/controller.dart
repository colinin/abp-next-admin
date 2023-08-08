import 'package:core/models/common.dart';
import 'package:get/get.dart';
import 'package:core/dependency/index.dart';
import 'package:platforms/services/index.dart';

import 'state.dart';

class HomeController extends GetxController {
  MenuStateService get _menuStateService => injector.get<MenuStateService>();
  FavoriteMenuStateService get _favoriteMenuStateService => injector.get<FavoriteMenuStateService>();

  final Rx<HomeState> _state = Rx<HomeState>(HomeState());
  HomeState get state => _state.value;

  @override
  void onInit() {
    super.onInit();
    _menuStateService.getMyMenus$()
      .listen((menus) { 
        _state.update((val) { 
          val?.menus = menus;
        });
      });
    _favoriteMenuStateService.getFavoriteMenus$()
      .listen((menus) { 
        _state.update((val) { 
          val?.favoriteMenus = menus;
        });
      });
  }

  void redirectToRoute(String route) {
    Get.toNamed(route);
  }

  void onMenuExpanded(Menu menu) {
    _state.update((val) {
      val?.activedMenu = menu.name;
    });
  }
}