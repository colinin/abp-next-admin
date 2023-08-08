import 'package:platforms/modes/menu.dto.dart';

class HomeState {
  HomeState({
    this.menus = const [],
    this.favoriteMenus = const [],
  });
  List<MenuDto> menus;
  List<UserFavoriteMenuDto> favoriteMenus;
}