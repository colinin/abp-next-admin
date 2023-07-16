import 'package:core/services/session.service.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class AuthorizationMiddleware extends GetMiddleware {
  AuthorizationMiddleware(this.loginRoute);
  final String loginRoute;
  @override
  RouteSettings? redirect(String? route) {
    if (!SessionService.to.isAuthenticated) {
      return RouteSettings(name: loginRoute);
    }
    return super.redirect(route);
  }
}