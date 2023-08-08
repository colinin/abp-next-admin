import 'package:platforms/modes/menu.dto.dart';

class FavoriteMenuState {
  FavoriteMenuState({
    this.menus = const [],
  });
  List<UserFavoriteMenuDto> menus;
}

class MenuState {
  MenuState({
    this.menus = const [],
  });
  List<MenuDto> menus;
}
