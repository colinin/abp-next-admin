import 'package:core/models/auth.dart';
import 'package:flutter/material.dart';

class LoginState {
  LoginState({
    required this.loading,
    required this.showPassword,
    required this.portalProviders,
  });
  bool showPassword;
  bool loading;
  List<PortalLoginProvider> portalProviders;

  late TextEditingController username = TextEditingController();
  late TextEditingController password = TextEditingController();
}
