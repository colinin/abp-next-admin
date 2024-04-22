import 'index.dart';
import 'package:get/get.dart';

import 'route.name.dart';

class SystemRoute {
  static List<GetPage> routes = [
    GetPage(
      name: SystemRoutes.settings,
      page: () => const SystemSettingsPage(),
      binding: SystemSettingsBinding(),
    ),
  ];
}