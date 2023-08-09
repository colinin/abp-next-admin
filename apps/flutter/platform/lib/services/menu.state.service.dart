import 'package:core/services/environment.service.dart';
import 'package:core/services/service.base.dart';
import 'package:core/services/session.service.dart';
import 'package:core/utils/index.dart';
import 'package:platforms/modes/state.dart';
import 'package:platforms/modes/menu.dto.dart';
import 'package:platforms/proxy/index.dart';

class MenuStateService extends ServiceBase {
  MenuStateService(super.injector);

  final InternalStore<MenuState> _state = InternalStore<MenuState>(state: MenuState());

  EnvironmentService get _environmentService => resolve<EnvironmentService>();
  SessionService get _sessionService => resolve<SessionService>();
  MenuService get _menuService => resolve<MenuService>();

  @override
  void onInit() {
    super.onInit();
    _initState();
  }

  void _initState() {
    _sessionService.getToken$()
      .listen((token) {
        _state.patch((state) => state.menus = []);
        if (token != null) {
          refreshState();
        }
      });
  }

  Future<void> refreshState() async {
    var environment = _environmentService.getEnvironment();
    var framework = environment.application.framework ?? 'abp-flutter';
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