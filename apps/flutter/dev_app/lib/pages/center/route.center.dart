import 'package:get/get.dart';

import 'index.dart';
import 'route.name.dart';

class CenterRoute {
  static List<GetPage> routes = [
    GetPage(
      name: CenterRoutes.settings,
      page: () => const CenterSettingsPage(),
      binding: CenterSettingsBinding(),
    ),
  ];
}