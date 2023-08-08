import 'package:get/get.dart';

import 'index.dart';
import 'route.name.dart';

class NotificationRoute {
  static List<GetPage> routes = [
    GetPage(
      name: NotificationRoutes.manageNotifier,
      page: () => const NotifierManagePage(),
      binding: NotifierManageBinding(),
    ),
    GetPage(
      name: NotificationRoutes.notifies,
      page: () => const NotificationsPage(),
      binding: NotificationsBinding(),
    ),
  ];
}