import 'package:core/dependency/index.dart';
import 'package:core/models/auth.dart';
import 'package:core/services/index.dart';
import 'package:get/get.dart';
import 'state.dart';

class UserInfoController extends GetxController {
  SessionService get sessionService => injector.get();

  final Rx<UserInfoState> _state = Rx<UserInfoState>(UserInfoState());
  UserInfoState get state => _state.value;
  Token? get token => sessionService.token;

  @override
  void onInit() {
    super.onInit();
    _state.update((val) {
      val!.profile = sessionService.profile;
    });
  }
}