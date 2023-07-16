import 'package:account/models/auth.dart';
import 'package:account/models/common.dart';
import 'package:account/pages/login/widget/portal_form.dart';
import 'package:core/index.dart';
import 'package:get/get.dart';
import 'state.dart';

class LoginController extends GetxController {
  AuthService get authService => Get.find();
  SessionService get sessionService => Get.find();

  final Rx<LoginState> _state = Rx<LoginState>(LoginState(
    loading: false,
    showPassword: false,
    portalProviders: [],
  ));
  LoginState get state => _state.value;

  void changePwdVisiable() {
    _state.update((val) {
      val?.showPassword = !val.showPassword;
    });
  }

  Future<void> login() async {
    await portalLogin(null);
  }

  void showPortalLoginDialog() {
    Get.defaultDialog(
      title: 'Lable:LoginToPortal'.tr,
      content: Obx(() => PortalForm(
          portalProviders: state.portalProviders,
          onSelected: portalLogin,
        ),
      ),
      barrierDismissible: false,
    );
  }

  Future<void> portalLogin(PortalLoginProvider? provider) async {
    _state.update((val) {
      val?.loading = true;
    });
    try {
      var token = await authService.portal(PortalLoginParams(
        enterpriseId: provider?.id,
        username: state.username.text,
        password: state.password.text));
      sessionService.refreshToken(token);
      state.username.clear();
      state.password.clear();
      Get.back(closeOverlays: true);
    } on PortalLoginException catch (error) {
      _state.update((val) {
        val!.portalProviders = error.providers;
      });
      showPortalLoginDialog();
    }
    finally {
      _state.update((val) {
        val?.loading = false;
      });
    }
  }
}