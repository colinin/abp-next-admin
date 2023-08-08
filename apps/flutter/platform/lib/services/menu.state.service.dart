import 'package:core/services/environment.service.dart';
import 'package:core/services/service.base.dart';
import 'package:core/utils/index.dart';
import 'package:platforms/modes/state.dart';
import 'package:platforms/modes/menu.dto.dart';
import 'package:platforms/proxy/index.dart';

class MenuStateService extends ServiceBase {
  MenuStateService(super.injector);

  final InternalStore<MenuState> _state = InternalStore<MenuState>(state: MenuState());

  EnvironmentService get _environmentService => resolve<EnvironmentService>();
  MenuService get _menuService => resolve<MenuService>();

  @override
  void onInit() {
    super.onInit();
    refreshState();
  }

  Future<void> refreshState() async {
    var environment = _environmentService.getEnvironment();
    var framework = environment.application.framework ?? 'flutter';
    var result = await _menuService.getCurrentUserMenuList(framework);
    _state.patch((state) => state.menus = result.items);
  }

  List<MenuDto> getMyMenus() {
    return _state.state.menus;
  }

  Stream<List<MenuDto>> getMyMenus$() {
    return _state.sliceState((state) => state.menus);
  }
}