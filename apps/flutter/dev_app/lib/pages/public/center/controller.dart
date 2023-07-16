import 'package:core/models/auth.dart';
import 'package:core/services/session.service.dart';
import 'package:get/get.dart';

import 'state.dart';

class CenterController extends GetxController {
  SessionService get sessionService => Get.find();

  final Rx<CenterState> _state = Rx<CenterState>(CenterState(isAuthenticated: false));
  CenterState get state => _state.value;

  bool get isAuthenticated => state.isAuthenticated;
  UserProfile? get profile => state.profile;
  Token? get token => state.token;

  @override
  void onInit() {
    super.onInit();
    sessionService.getProfile$()
      .listen((event) {
        _state.update((val) {
          val?.profile = event;
          val?.isAuthenticated = sessionService.isAuthenticated;
        });
      });
    sessionService.getToken$()
      .listen((event) {
        _state.update((val) {
          val?.token = event;
        });
      });
  }

  String get userName {
    return !isAuthenticated ? 'Label:Anonymous'.tr : profile!.userName;
  }

  String get phoneNumber {
    return !isAuthenticated ? '' : profile!.phoneNumber ?? 'Label:PhoneNumberNotBound'.tr;
  }

  void redirectToRoute(String route) {
    Get.toNamed(route);
  }

  void changeLocale(String locale) {
    sessionService.setLanguage(locale);
  }
}