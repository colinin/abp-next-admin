import 'package:get/get.dart';
import 'package:core/middlewares/index.dart';

import 'index.dart';
import 'route.name.dart';

class AccountRoute {
  static List<GetPage> routes = [
    GetPage(
      name: AccountRoutes.login,
      page: () => const LoginPage(),
      binding: LoginBinding(),
    ),
    GetPage(
      name: AccountRoutes.profile,
      page: () => const UserInfoPage(),
      binding: UserInfoBinding(),
      middlewares: [AuthorizationMiddleware(AccountRoutes.login)]
    ),
  ];
}