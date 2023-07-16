import 'package:get/get.dart';

import 'index.dart';
import 'route.name.dart';

class PublicRoute {
  static List<GetPage> routes = [
    GetPage(
      name: PublicRoutes.home,
      page: () => const HomePage(),
      binding: HomeBinding(),
    ),
    GetPage(
      name: PublicRoutes.work,
      page: () => const WorkPage(),
      binding: WorkBinding(),
    ),
    GetPage(
      name: PublicRoutes.center,
      page: () => const CenterPage(),
      binding: CenterBinding(),
    ),
  ];
}