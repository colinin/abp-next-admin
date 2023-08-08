import 'package:core/services/environment.service.dart';
import 'package:core/services/service.base.dart';
import 'package:core/utils/index.dart';
import 'package:platforms/modes/state.dart';
import 'package:platforms/modes/menu.dto.dart';
import 'package:platforms/proxy/favorite.menu.service.dart';

class FavoriteMenuStateService extends ServiceBase {
  FavoriteMenuStateService(super.injector);

  final InternalStore<FavoriteMenuState> _state = InternalStore<FavoriteMenuState>(state: FavoriteMenuState());

  EnvironmentService get _environmentService => resolve<EnvironmentService>();
  FavoriteMenuService get _favoriteMenuService => resolve<FavoriteMenuService>();

  @override
  void onInit() {
    super.onInit();
    refreshState();
  }

  Future<void> refreshState() async {
    var environment = _environmentService.getEnvironment();
    var framework = environment.application.framework ?? 'flutter';
    var result = await _favoriteMenuService.getMyFavoriteMenuList(framework);
    _state.patch((state) => state.menus = result.items);
  }

  List<UserFavoriteMenuDto> getFavoriteMenus() {
    return _state.state.menus;
  }

  Stream<List<UserFavoriteMenuDto>> getFavoriteMenus$() {
    return _state.sliceState((state) => state.menus);
  }
}