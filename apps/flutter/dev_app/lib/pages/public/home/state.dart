import 'package:core/models/common.dart';
import 'package:notifications/models/common.dart';
import 'package:platforms/modes/menu.dto.dart';

class HomeState {
  HomeState({
    this.activedMenu,
    this.menus = const [],
    this.favoriteMenus = const [],
    this.notifications = const [],
  });
  String? activedMenu;
  List<MenuDto> menus;
  List<UserFavoriteMenuDto> favoriteMenus;
  List<NotificationPaylod> notifications;

  List<Menu> getMenus() => _buildTreeRecursive(menus, null, 0);

  List<Menu> _buildTreeRecursive(List<MenuDto> treeMenus, String? parentId, int level) {
    List<Menu> results = [];
    var tempList = treeMenus.where((menu) => menu.parentId == parentId).toList();
    for (int i = 0; i < tempList.length; i++) {
      var menu = Menu(
        id: tempList[i].id.hashCode,
        path: tempList[i].path,
        name: tempList[i].name,
        displayName: tempList[i].displayName,
        description: tempList[i].description,
        redirect: tempList[i].redirect,
        meta: tempList[i].meta,
        level: level + 1
      );
      menu.children = _buildTreeRecursive(treeMenus, tempList[i].id, menu.level);
      results.add(menu);
    }
   return results;
  }
}