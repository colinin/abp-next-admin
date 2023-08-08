import 'package:core/models/auth.dart';
import 'package:get/get.dart';

class CenterState {
  CenterState({
    required this.isAuthenticated,
    this.token,
    this.profile,
  });
  Token? token;
  UserProfile? profile;
  bool isAuthenticated;

  String get userName {
    return !isAuthenticated ? 'Label:Anonymous'.tr : profile!.userName;
  }

  String get phoneNumber {
    return !isAuthenticated ? '' : profile!.phoneNumber ?? 'Label:PhoneNumberNotBound'.tr;
  }
}