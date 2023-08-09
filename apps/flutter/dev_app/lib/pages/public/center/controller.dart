import 'package:account/pages/route.name.dart';
import 'package:core/models/auth.dart';
import 'package:core/dependency/index.dart';
import 'package:core/services/session.service.dart';
import 'package:dev_app/pages/center/route.name.dart';
import 'package:get/get.dart';
import 'package:notifications/pages/route.name.dart';

import 'state.dart';

class CenterController extends GetxController {
  SessionService get sessionService => injector.get();

  final Rx<CenterState> _state = Rx<CenterState>(CenterState(isAuthenticated: false));
  CenterState get state => _state.value;

  bool get isAuthenticated => state.isAuthenticated;
  UserProfile? get profile => state.profile;
  Token? get token => state.token;

  @override
  void onInit() {
    super.onInit();
    sessionService.getProfile$()
      .listen((profile) {
        _state.update((val) {
          val?.profile = profile;
          val?.isAuthenticated = sessionService.isAuthenticated;
        });
      });
    sessionService.getToken$()
      .listen((token) {
        _state.update((val) {
          val?.token = token;
        });
      });
  }

  void onClickProfile() {
    if (!state.isAuthenticated) {
      return redirectToRoute(AccountRoutes.login);
    }
    redirectToRoute(AccountRoutes.profile);
  }

  void onClickFeedback() {
    redirectToRoute('/feedback');
  }

  void onClickHelp() {
    redirectToRoute('/help');
  }

  void onClickInfo() {
    redirectToRoute('/info');
  }

  void onClickMessage() {
    redirectToRoute(NotificationRoutes.notifies);
  }

  void onSettings() {
    redirectToRoute(CenterRoutes.settings);
  }

  void redirectToRoute(String route) {
    Get.toNamed(route);
  }

  void changeLocale(String locale) {
    sessionService.setLanguage(locale);
  }
}